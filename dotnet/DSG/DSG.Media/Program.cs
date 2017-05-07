using DSG.Media.Task;

namespace DSG.Media
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = "D:\\DRIVE\\MS CS UCU\\DataScienceGame\\data\\extra_infos.json";
            var outputFile = "D:\\DRIVE\\MS CS UCU\\DataScienceGame\\data\\features\\all_media_info.csv";
            var task = new TrackPropertiesDownloaderTask();
            task.Run(inputFile, outputFile, 6);
        }
    }
}
