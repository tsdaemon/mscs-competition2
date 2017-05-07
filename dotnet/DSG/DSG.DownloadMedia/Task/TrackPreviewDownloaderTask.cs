using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DSC.Core.Readers;
using DSC.Core.Time;
using DSG.Deezer.Models;

namespace DSG.DownloadMedia.Task
{
    public class TrackPreviewDownloaderTask
    {
        bool stopping = false;
        public void Run(string mediaPropertiesCsvPath, string previewsDirectory, int streams, int? clusterSplit = null, int? clusters = null)
        {
            Console.WriteLine($"TrackPreviewDownloaderTask started. Output: {previewsDirectory}. Looking for already downloded files...");
            var downloadedIds = GetDownloaded(previewsDirectory).ToDictionary(i => i);

            Console.WriteLine($"Already downloaded: {downloadedIds.Count}. Reading tracks file {mediaPropertiesCsvPath} for urls...");
            var tracks = CsvReader.ReadCsv<Track>(mediaPropertiesCsvPath)
                .Where(t => !downloadedIds.ContainsKey(t.Id))
                .ToArray();

            if (clusterSplit.HasValue && clusters.HasValue)
            {
                var allStreams = streams * clusters;
                var ourBatchesStart = clusterSplit * streams;
                var ourBatchesEnd = (clusterSplit + 1) * streams;
                Console.WriteLine(
                    $"Processing cluster {clusterSplit} from {clusters} clusters. Batches from {ourBatchesStart} to {ourBatchesEnd}.");
                tracks = tracks.Where(i => i.Id % allStreams >= ourBatchesStart && i.Id % allStreams < ourBatchesEnd)
                        .ToArray();
            }

            Console.WriteLine($"Read {tracks.Length} downloaded files. Preparing work items...");
            var timeEstimator = new GeneralTimeEstimator(tracks.Length);
            var workItems = GetGrouping(tracks, streams, clusterSplit, clusters)
                    .Select<IGrouping<int, Track>, Action>(batch => () => ProcessBatch(batch, timeEstimator, previewsDirectory))
                    .ToArray();

            Console.WriteLine($"Prepared {streams} batches. Processing in parallel...");
            var thread = new Thread(() => Parallel.Invoke(workItems));
            thread.Start();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Processing started. Press `i` to output status and time estimation, `s` to stop.  DO NOT STOP BY CLOSING THE WINDOW, DOWNLOADED FILES MIGHT BE CORRUPTED!");
            Console.BackgroundColor = ConsoleColor.Black;
            while (thread.IsAlive)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'i')
                {
                    OutputStatus(timeEstimator);
                }
                if (key.KeyChar == 's')
                {
                    stopping = true;
                    Thread.Sleep(2000);
                }
            }
            if(stopping) Console.WriteLine("Stopped");
            else Console.WriteLine("Done!");
            timeEstimator.Dispose();
        }

        private IEnumerable<IGrouping<int, Track>> GetGrouping(IEnumerable<Track> items, int streams, int? clusterSplit, int? clusters)
        {
            if (clusterSplit.HasValue && clusters.HasValue)
            {
                var allStreams = streams * clusters;
                return items.GroupBy(i => i.Id % allStreams.Value);
            }
            else
            {
                return items.GroupBy(i => i.Id % streams);
            }
        }

        private IEnumerable<int> GetDownloaded(string previewsDirectory)
        {
            return Directory.EnumerateFiles(previewsDirectory, "*.mp3")
                .Select(Path.GetFileNameWithoutExtension)
                .Select(int.Parse);
        }

        private void OutputStatus(ITimeEstimator timeEstimator)
        {
            var status = timeEstimator.EstimateLeftTime();
            Console.WriteLine(
                $"Processed {status.Item2} of {status.Item3}. Time left: {TimeSpan.FromMilliseconds(status.Item1)}");
        }

        private void ProcessBatch(IGrouping<int, Track> batch, ITimeEstimator estimator, string saveTo)
        {
            Console.WriteLine($"Started batch {batch.Key}.");
            foreach (var item in batch)
            {
                if (stopping)
                {
                    Console.WriteLine($"Batch {batch.Key} stopped.");
                    break;
                }
                estimator.ExecuteItem(() => ProcessItem(item, saveTo));
            }
        }

        private void ProcessItem(Track item, string saveTo)
        {
            if (string.IsNullOrEmpty(item.Preview) || !Uri.IsWellFormedUriString(item.Preview, UriKind.Absolute)) return;

            var url = item.Preview;
            string fileName = $"{saveTo}/{item.Id}.mp3";

            // Create a new WebClient instance.
            using (var webClient = new WebClient())
            {
                // Download the Web resource and save it into the current filesystem folder.
                webClient.DownloadFile(url, fileName);
            }
        }
    }
}
