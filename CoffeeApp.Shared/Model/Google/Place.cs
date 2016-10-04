using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoffeeApp
{

    public class Place
    {
        [JsonProperty(PropertyName = "address_components")]
        public List<AddressComponent> AddressComponents { get; set; } = new List<AddressComponent>();

        [JsonProperty(PropertyName = "adr_address")]
        public string Address { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "formatted_address")]
        public string AddressFormatted { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "formatted_phone_number")]
        public string PhoneNumberFormatted { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; } = new Geometry();

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "international_phone_number")]
        public string InternationalPhoneNumber { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "opening_hours")]
        public OpeningHours OpeningHours { get; set; } = new OpeningHours();

        [JsonProperty(PropertyName = "photos")]
        public List<Photo> Photos { get; set; } = new List<Photo>();

        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; } = 0;

        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "types")]
        public List<string> Types { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "vicinity")]
        public string Vicinity { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "price_level")]
        public int? PriceLevel { get; set; } = 0;

        [JsonProperty(PropertyName = "reviews")]
        public List<Review> Reviews { get; set; } = new List<Review>();

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "user_ratings_total")]
        public int UserRatingsCount { get; set; } = 0;

        [JsonProperty(PropertyName = "utc_offset")]
        public int UTCOffset { get; set; } = 0;

        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "permanently_closed")]
        public bool PermanentlyClosed { get; set; } = false;

        [JsonIgnore]
        public bool HasImage
        {
            get
            {
                return Photos != null && Photos.Count > 0;
            }
        }

        [JsonIgnore]
        public bool HasReviews
        {
            get
            {
                return Reviews != null && Reviews.Count > 0;
            }
        }

        [JsonIgnore]
        public string MainImage
        {
            get
            {
                if (!HasImage)
                    return Icon;

                return Photos[0].ImageUrlLarge;
            }
        }

        [JsonIgnore]
        public double Latitude => Geometry?.Location?.Latitude ?? 0;

        [JsonIgnore]
        public double Longitude => Geometry?.Location?.Longitude ?? 0;

    }
}