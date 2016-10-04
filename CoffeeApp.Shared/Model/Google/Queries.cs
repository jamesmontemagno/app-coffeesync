using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoffeeApp
{
	
	public class SearchQueryResult
	{
		[JsonProperty(PropertyName = "next_page_token")]
		public string NextPageToken { get; set; } = string.Empty;
		[JsonProperty(PropertyName = "results")]
		public List<Place> Places { get; set; } = new List<Place>();
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; } = string.Empty;
	}

	
	public class DetailQueryResult
	{
		[JsonProperty(PropertyName = "result")]
		public Place Place { get; set; } = new Place();

		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; } = string.Empty;
	}
}

