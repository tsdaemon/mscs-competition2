using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Deezer.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public string Picture { get; set; }

        public string PictureSmall { get; set; }

        public string PictureMedium { get; set; }

        public string PictureBig { get; set; }

        public string PictureXl { get; set; }

        public int NbAlbum { get; set; }

        public int NbFans { get; set; }

        public bool Radio { get; set; }

        public string Tracklist { get; set; }
    }
}
