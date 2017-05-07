using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSC.Core;
using DSC.Core.Readers;
using DSC.Core.Time;
using DSC.Core.Writers;
using DSG.Deezer;
using DSG.Deezer.Models;

namespace DSG.Media.Task
{
    public class TrackPropertiesDownloaderTask
    {
        private DeezerClient _deezerClient;
        private AsyncSaverQueue<Track> _asyncSaverQueue;

        public TrackPropertiesDownloaderTask()
        {
            _deezerClient = new DeezerClient();
        }

        public void Run(string mediaJsonPath, string mediaPropertiesCsvPath, int streams)
        {
            Console.WriteLine($"TrackPropertiesDownloaderTask started. Output: {mediaPropertiesCsvPath}. Initializing saver...");
            var csvFile = new CsvFileWriter<Track>(mediaPropertiesCsvPath);
            _asyncSaverQueue = new AsyncSaverQueue<Track>(csvFile);

            Console.WriteLine($"Saver intialized. Reading already processed indexes...");
            var alreadyProcessed = GetAlreadyProcessed(mediaPropertiesCsvPath).ToDictionary(i => i, i => i);

            Console.WriteLine($"Already processed: {alreadyProcessed.Count}. Reading mediadump {mediaJsonPath}...");
            var medias = JsonReaderInner.ReadMongoDump<MediaModel>(mediaJsonPath)
                .Where(m => !alreadyProcessed.ContainsKey(m.MediaId))
                .ToArray();

            Console.WriteLine($"Read {medias.Length} media items. Preparing work items...");
            var timeEstimator = new GeneralTimeEstimator(medias.Length);
            var workItems = medias
                    .GroupBy(x => x.MediaId % streams)
                    .Select<IGrouping<int, MediaModel>, Action>(batch => () => ProcessBatch(batch, timeEstimator))
                    .ToArray();

            Console.WriteLine($"Prepared {streams} work items. Processing in parallel...");
            var thread = new Thread(() => Parallel.Invoke(workItems));
            thread.Start();
            Console.WriteLine("Processing started. Press i to output status and time estimation");
            while (thread.IsAlive)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'i')
                {
                    OutputStatus(timeEstimator);
                }
            }
            Console.WriteLine("Done!");
            timeEstimator.Dispose();
        }

        private IEnumerable<int> GetAlreadyProcessed(string mediaPropertiesCsvPath)
        {
            if(File.Exists(mediaPropertiesCsvPath))
                using (var rdr = new StreamReader(File.OpenRead(mediaPropertiesCsvPath)))
                {
                    rdr.ReadLine();
                    while (!rdr.EndOfStream)
                    {
                        var id = rdr.ReadLine().Split(new []{','}, StringSplitOptions.RemoveEmptyEntries)[0];
                        yield return int.Parse(id);
                    }

                }
        }

        private void OutputStatus(ITimeEstimator timeEstimator)
        {
            var status = timeEstimator.EstimateLeftTime();
            Console.WriteLine(
                $"Processed {status.Item2} of {status.Item3}. Time left: {TimeSpan.FromMilliseconds(status.Item1)}");
        }

        private void ProcessBatch(IGrouping<int, MediaModel> batch, ITimeEstimator estimator)
        {
            Console.WriteLine($"Processing batch {batch.Key}...");
            foreach (var item in batch)
            {
                estimator.ExecuteItem(() => ProcessItem(item));
            }
        }

        private void ProcessItem(MediaModel item)
        {
            var track = _deezerClient.GetTrack(item.MediaId);
            if (track == null) return;
            if (string.IsNullOrEmpty(track.Preview) && track.Alternative != null)
            {
                track.Preview = track.Alternative.Preview;
            }
            
            _asyncSaverQueue.Save(track);
        }
    }
}
