using DSG.DownloadMedia.Task;

namespace DSG.DownloadMedia
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = new TrackPreviewDownloaderTask();

            int? cluster = null;
            if(args.Length > 3) cluster = int.Parse(args[3]);
            int? clusters = null;
            if (args.Length > 4) clusters = int.Parse(args[4]);

            task.Run(args[0], args[1], int.Parse(args[2]), cluster, clusters);
        }
    }
}
