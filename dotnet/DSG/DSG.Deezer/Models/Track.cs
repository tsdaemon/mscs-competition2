using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace DSG.Deezer.Models
{
    [DelimitedRecord(",")]
    public class Track
    {
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int Id;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Title;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string TitleShort;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string TitleVersion;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Isrc;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Link;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int Duration;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int TrackPosition;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int DiskNumber;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int Rank;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public bool ExplicitLyrics;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Preview;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float Bpm;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float Gain;

        [FieldHidden]
        public Track Alternative;

        //public Artist Artist;

        //public Album Album;
    }
}
