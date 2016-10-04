using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoffeeApp
{
	
	public class Aspect
	{
		[JsonProperty (PropertyName = "rating")]
		public int Rating { get; set; } = 0;

		[JsonProperty (PropertyName = "type")]
		public string Type { get; set; } = string.Empty;
	}

	
	public class Review
	{
		[JsonProperty (PropertyName = "aspects")]
		public List<Aspect> Aspects { get; set; } = new List<Aspect>();

		[JsonProperty (PropertyName = "author_name")]
		public string AuthorName { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "author_url")]
		public string AuthorUrl { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "language")]
		public string Language { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "rating")]
		public int Rating { get; set; } = 0;

		[JsonProperty (PropertyName = "text")]
		public string Text { get; set; } = string.Empty;

		[JsonProperty (PropertyName = "time")]
		public double Time { get; set; } = DateTime.Now.Ticks;
	}
}

