using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DSC.Core.Readers
{
    public class JsonReaderInner
    {
        public static IEnumerable<T> ReadMongoDump<T>(string fileName) where T:new()
        {
            using (var rs = new StreamReader(File.OpenRead(fileName)))
            {
                while (rs.EndOfStream)
                {
                    var line = rs.ReadLine();
                    yield return JsonConvert.DeserializeObject<T>(line);
                }
            }
        }

        public static T DeserializeDeezer<T>(string str) where T : new()
        {
            var resolver = new DefaultContractResolver {NamingStrategy = new SnakeCaseNamingStrategy()};
            return JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings { ContractResolver = resolver });
        }
    }
}
