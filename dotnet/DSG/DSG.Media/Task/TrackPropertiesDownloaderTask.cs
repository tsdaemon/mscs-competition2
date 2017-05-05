using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSC.Core;
using DSC.Core.Readers;
using DSG.Deezer;
using DSG.Media.Models;

namespace DSG.Media.Task
{
    public class TrackPropertiesDownloaderTask
    {
        private DeezerClient _deezerClient;
        private Dictionary<int, MediaModelExtended> results;

        public TrackPropertiesDownloaderTask()
        {
            _deezerClient = new DeezerClient();
        }

        public void Run(string mediaJsonPath, string mediaPropertiesCsvPath)
        {
            Console.WriteLine($"TrackPropertiesDownloaderTask started. Input: {mediaJsonPath}, output: ")
            var medias = JsonReaderInner.ReadMongoDump<MediaModel>(mediaJsonPath).ToArray();
            results = new 
        }
    }
}
