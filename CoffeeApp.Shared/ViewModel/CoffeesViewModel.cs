using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.ComponentModel;
using System.Linq;
using MvvmHelpers;

using Plugin.Connectivity;
using Acr.UserDialogs;

namespace CoffeeApp.Logic
{
    public class CoffeesViewModel : INotifyPropertyChanged
    {
        const string SearchQueryUrl = "https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&location={1},{2}&radius=1000&key={3}";

        public ObservableRangeCollection<Coffee> Places { get; } = new ObservableRangeCollection<Coffee>();

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }


        public async Task SearchForPlaces(double lat, double lng, string query)
        {
            if (IsBusy)
                return;

            
            try
            {
                IsBusy = true;

                var requestUri = string.Format(SearchQueryUrl,
                    query,
                    lat.ToString(CultureInfo.InvariantCulture),
                    lng.ToString(CultureInfo.InvariantCulture),
                    Keys.GoogleAPIKey);

                var client = new HttpClient();
                var result = await client.GetStringAsync(requestUri);
                var queryObject = JsonConvert.DeserializeObject<SearchQueryResult>(result);

                if (queryObject?.Places != null)
                {
                    Places.ReplaceRange(queryObject.Places.Select(p => new Coffee
                    {
                        Latitude = p.Latitude,
                        Longitude = p.Longitude,
                        Name = p.Name,
                        Stars = (float)p.Rating
                    }));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to query" + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public async Task LoadCoffees()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var coffees = await AzureService.Instance.GetCoffees();
                Places.ReplaceRange(coffees);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to load" + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SaveCoffee(string name, double lat, double lng)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await AzureService.Instance.AddCoffee(name, lat, lng);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add" + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
