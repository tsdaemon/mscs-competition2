using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace DSC.Core.Writers
{
    public class CsvFile<T> : ICanSave<T>, IDisposable where T : class
    {
        private readonly string _fileName;
        private FileHelperEngine<T> _fileHelperEngine;
        private StreamWriter _writer;
        private bool _initialized;

        public CsvFile(string fileName)
        {
            _fileName = fileName;
            _fileHelperEngine = new FileHelperEngine<T>(Encoding.UTF8);
            _fileHelperEngine.BeforeReadRecord += (sender, args) =>
                    args.RecordLine = args.RecordLine.Replace(@"""", "'");
        }

        public async Task Init()
        {
            var exists = File.Exists(_fileName);
            _writer = new StreamWriter(File.OpenWrite(_fileName));
            if (!exists)
            {
                var header = _fileHelperEngine.GetFileHeader();
                await _writer.WriteLineAsync(header);
            }
            _initialized = true;
        }

        public async Task SaveOneAsync(T item)
        {
            if (!_initialized) await Init();

            var line = _fileHelperEngine.WriteString(new[] {item});
            await _writer.WriteAsync(line);
        }

        public async Task SaveManyAsync(IEnumerable<T> items)
        {
            if (!_initialized) await Init();

            var lines = _fileHelperEngine.WriteString(items);
            await _writer.WriteLineAsync(lines);
        }

        public void Dispose()
        {
            _writer.Close();
        }
    }
}
