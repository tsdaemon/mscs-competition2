using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace DSG.Media.Models
{
    [DelimitedRecord(",")]
    public class MediaModelExtended
    {
        public int MediaId { get; set; }
        public string MediaTitle { get; set; }
        public string MediaTitleShort { get; set; }
        public string MediaTitleVersion { get; set; }
        public string MediaIsrc { get; set; }
        public string MediaUrl { get; set; }
        public int MediaPosition { get; set; }
        public int MediaDiskNumber { get; set; }
        public int MediaRank { get; set; }
        public bool MediaExplicitLyrics { get; set; }
        public string MediaPreview { get; set; }
        public float MediaBpm { get; set; }
        public float MediaGain { get; set; }

        public string MediaAvailableCountriesExp { get; set; }

        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumUpc { get; set; }
        public string AlbumLink { get; set; }
        public string AlbumCover { get; set; }
        public string AlbumCoverSmall { get; set; }
        public string AlbumCoverMedium { get; set; }
        public string AlbumCoverBig { get; set; }
        public string AlbumCoverXl { get; set; }

        public string AlbumGenresExp { get; set; }

        public string AlbumLabel { get; set; }
        public int AlbumNbTracks { get; set; }
        public int AlbumDuration { get; set; }
        public int AlbumFans { get; set; }
        public int AlbumRating { get; set; }
        public string AlbumRecordType { get; set; }
        public bool AlbumAvailable { get; set; }

        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string ArtistLink { get; set; }
        public string ArtistPicture { get; set; }
        public string ArtistPictureSmall { get; set; }
        public string ArtistPictureMedium { get; set; }
        public string ArtistPictureBig { get; set; }
        public string ArtistPictureXl { get; set; }
        public int ArtistNbAlbum { get; set; }
        public int ArtistNbFans { get; set; }
        public bool ArtistRadio { get; set; }
        public string ArtistTracklist { get; set; }
    }
}
