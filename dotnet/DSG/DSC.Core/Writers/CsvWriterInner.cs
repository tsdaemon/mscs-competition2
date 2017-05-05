using System.Collections.Generic;
using System.IO;
using FileHelpers;

namespace DSC.Core.Writers
{
    public class CsvWriterInner
    {
        public static void WriteCsv<T>(string fileName, IEnumerable<T> collection) where T : class, new()
        {
            var engine = new FileHelperEngine<T>();
            engine.WriteFile(fileName, collection);
        }
    }
}
