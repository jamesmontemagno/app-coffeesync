using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoffeeApp
{
	
	public class OpeningHours
	{
		[JsonProperty (PropertyName = "open_now")]
		public bool IsOpen { get; set; }

		[JsonProperty (PropertyName = "weekday_text")]
		public List<string> WeekdayText { get; set; } = new List<string>();

		[JsonProperty (PropertyName = "periods")]
		public List<Period> Periods { get; set; } = new List<Period>();
	}

	
	public class Close
	{
		[JsonProperty (PropertyName = "day")]
		public int Day { get; set; } = 0;

		[JsonProperty (PropertyName = "time")]
		public string Time { get; set; }  = string.Empty;
	}

	
	public class Open
	{
		[JsonProperty (PropertyName = "day")]
		public int Day { get; set; } = 0;

		[JsonProperty (PropertyName = "time")]
		public string Time { get; set; } = string.Empty;
	}

	
	public class Period
	{
		[JsonProperty (PropertyName = "close")]
		public Close Close { get; set; } = new Close();

		[JsonProperty (PropertyName = "open")]
		public Open Open { get; set; } = new Open();
	}
}

