namespace DSG.Deezer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Upc { get; set; }

        public string Link { get; set; }

        public string Cover { get; set; }

        public string CoverSmall { get; set; }

        public string CoverMedium { get; set; }

        public string CoverBig { get; set; }

        public string CoverXl { get; set; }

        public string[] Genres { get; set; }

        public string Label { get; set; }

        public int NbTracks { get; set; }

        public int Duration { get; set; }

        public int Fans { get; set; }

        public int Rating { get; set; }

        public string RecordType { get; set; }

        public bool Available { get; set; }

        public string Tracklist { get; set; }
    }
}
