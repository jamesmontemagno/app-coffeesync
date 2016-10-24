using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
#if __IOS__
using CoreLocation;
#elif __ANDROID__
using Android.Content;
using Android.Locations;
#endif

namespace CoffeeApp.Logic
{
    public class CoffeesViewModel
    {
        const string SearchQueryUrl = "https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&location={1},{2}&radius=1000&key={3}";


        public bool IsBusy { get; set; }

        public async Task<List<Place>> SearchForPlaces(double lat, double lng, string query)
        {
            
            IsBusy = true;
           
            var requestUri = string.Format(SearchQueryUrl,
                query,
                lat.ToString(CultureInfo.InvariantCulture),
                lng.ToString(CultureInfo.InvariantCulture),
                Keys.GoogleAPIKey);
           
            try
            {
                var client = new HttpClient();
                var result = await client.GetStringAsync(requestUri);
                var queryObject = JsonConvert.DeserializeObject<SearchQueryResult>(result);

                return queryObject?.Places;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to query" + ex);
            }
            finally
            {
                IsBusy = false;
            }

            return null;
        }

    }
}
