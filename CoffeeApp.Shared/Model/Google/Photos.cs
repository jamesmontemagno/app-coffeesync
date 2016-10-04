using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace CoffeeApp
{

    public class Photo
    {
        const string RegularImageUrl = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=500&photoreference={0}&key={1}";
        const string LargeImageUrl = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=800&photoreference={0}&key={1}";

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; } = 0;

        [JsonProperty(PropertyName = "photo_reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; } = 0;

        [JsonIgnore]
        public string ImageUrl
        {
            get
            {
                return string.Format(RegularImageUrl, Reference, Keys.GoogleAPIKey);
            }
        }

        [JsonIgnore]
        public string ImageUrlLarge
        {
            get
            {
                return string.Format(LargeImageUrl, Reference, Keys.GoogleAPIKey);
            }
        }
    }
}

