using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace DSC.Core.Writers
{
    public class CsvFileWriter<T> : ICanSave<T>, IDisposable where T : class
    {
        private readonly string _fileName;
        private FileHelperEngine<T> _fileHelperEngine;
        private StreamWriter _writer;
        private bool _initialized;

        public CsvFileWriter(string fileName)
        {
            _fileName = fileName;
            _fileHelperEngine = new FileHelperEngine<T>(Encoding.UTF8);
            _fileHelperEngine.BeforeReadRecord += (sender, args) =>
                    args.RecordLine = args.RecordLine.Replace("\"", "'");
        }

        public void Init()
        {
            var exists = File.Exists(_fileName);
            _writer = File.AppendText(_fileName);
            //_writer.AutoFlush = true;
            if (!exists)
            {
                var header = _fileHelperEngine.GetFileHeader();
                _writer.WriteLine(header);
            }
            _initialized = true;
        }

        public void SaveOne(T item)
        {
            if (!_initialized) Init();

            var line = _fileHelperEngine.WriteString(new[] {item});
            _writer.Write(line);
        }

        public void SaveMany(IEnumerable<T> items)
        {
            if (!_initialized) Init();

            var lines = _fileHelperEngine.WriteString(items);
            _writer.Write(lines);
        }

        public void Dispose()
        {
            _writer.Close();
        }
    }
}
