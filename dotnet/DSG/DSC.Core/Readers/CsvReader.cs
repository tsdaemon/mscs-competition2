using System.Collections.Generic;
using System.Data;
using System.Text;
using FileHelpers;
using System;
using System.IO;
using System.Linq;

namespace DSC.Core.Readers
{
    public static class CsvReader
    {
        public static IEnumerable<T> ReadCsv<T>(string fileName) where T : class, new()
        {
            var engine = new FileHelperEngine<T>(Encoding.UTF8)
            {
                //ErrorMode = ErrorMode.IgnoreAndContinue
            };
            return engine.ReadFile(fileName);
        }

        public static DataTable ReadCsvDT<T>(string fileName) where T : class, new()
        {
            var engine = new FileHelperEngine<T>(Encoding.UTF8);
            //engine.ErrorMode = ErrorMode.IgnoreAndContinue;
            return engine.ReadFileAsDT(fileName);
        }

        public static IEnumerable<Tuple<int, string>> FuckingReadFuckingUrlsCsv(string fileName)
        {
            return File.ReadAllLines(fileName)
                .Skip(1)
                .Select(s => s.Split(','))
                .Select(s => new Tuple<int, string>(int.Parse(s[0]), s[1]));
        }
    }
}
