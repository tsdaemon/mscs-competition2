using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace DSC.Core.Readers
{
    public static class CsvReader
    {
        public static IEnumerable<T> ReadCsv<T>(string fileName) where T : class, new()
        {
            var engine = new FileHelperEngine<T>(Encoding.UTF8);
            engine.ErrorMode = ErrorMode.IgnoreAndContinue;
            return engine.ReadFile(fileName);
        }
    }
}
