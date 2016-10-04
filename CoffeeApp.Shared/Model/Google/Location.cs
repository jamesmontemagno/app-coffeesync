using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoffeeApp
{
	public class AddressComponent
	{
		[JsonProperty (PropertyName =  "long_name")]
		public string LongName { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "short_name")]
		public string ShortName { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "types")]
		public List<string> Types { get; set; } = new List<string>();
	}

	public class Location
	{
		[JsonProperty (PropertyName = "lat")]
		public double Latitude { get; set; } = 0;

		[JsonProperty (PropertyName = "lng")]
		public double Longitude { get; set; } = 0;
	}
	public class Geometry
	{
		[JsonProperty (PropertyName = "location")]
		public Location Location { get; set; } = new Location();
	}
}

