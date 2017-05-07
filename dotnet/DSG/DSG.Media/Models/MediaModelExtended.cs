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
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int MediaId;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaTitle;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaTitleShort;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaTitleVersion;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaIsrc;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaLink;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int MediaDuration;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int MediaTrackPosition;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int MediaDiskNumber;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int MediaRank;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public bool MediaExplicitLyrics;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaPreview;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float MediaBpm;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float MediaGain;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string MediaAvailableCountriesExp;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int AlbumId;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumTitle;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumUpc;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumLink;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumCover;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumCoverSmall;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumCoverMedium;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumCoverBig;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumCoverXl;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumGenresExp;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumLabel;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int AlbumNbTracks;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int AlbumDuration;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int AlbumFans;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int AlbumRating;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string AlbumRecordType;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int ArtistId;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistName;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistLink;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistPicture;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistPictureSmall;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistPictureMedium;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistPictureBig;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistPictureXl;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int ArtistNbAlbum;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int ArtistNbFans;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public bool ArtistRadio;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string ArtistTracklist;
    }

}
