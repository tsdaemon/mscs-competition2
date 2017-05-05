using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Deezer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TitleShort { get; set; }

        public string TitleVersion { get; set; }

        public string Isrc { get; set; }

        public string Url { get; set; }

        public int Position { get; set; }

        public int DiskNumber { get; set; }

        public int Rank { get; set; }

        public bool ExplicitLyrics { get; set; }

        public string Preview { get; set; }

        public float Bpm { get; set; }

        public float Gain { get; set; }

        public string[] AvailableCountries { get; set; }

        public Artist Artist { get; set; }

        public Album Album { get; set; }
    }
}
