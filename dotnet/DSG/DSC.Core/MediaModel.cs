using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DSC.Core
{
    public class MediaModel
    {
        [JsonProperty(PropertyName = "media_id")]
        public int MediaId { get; set; }

        [JsonProperty(PropertyName = "sng_title")]
        public string SongTitle { get; set; }

        [JsonProperty(PropertyName = "alb_title")]
        public string AlbumTitle { get; set; }

        [JsonProperty(PropertyName = "art_name")]
        public string ArtistName { get; set; }
    }
}
