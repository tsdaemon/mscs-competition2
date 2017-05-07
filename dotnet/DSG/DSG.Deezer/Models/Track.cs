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

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Title;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string TitleShort;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string TitleVersion;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Isrc;

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

        //public Artist Artist;

        //public Album Album;
    }
}
