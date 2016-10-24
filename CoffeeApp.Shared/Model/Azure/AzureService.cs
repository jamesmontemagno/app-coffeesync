using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeApp
{
    public class AzureService
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Coffee> coffeeTable;
        public static bool UseAuth { get; set; } = false;

        static AzureService instance;
        public static AzureService Instance => instance ?? (instance = new AzureService());

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://motzcoffee.azurewebsites.net";


            //Create our client
            Client = new MobileServiceClient(appUrl);

            var path = "syncstore.db";
            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<Coffee>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            coffeeTable = Client.GetSyncTable<Coffee>();
        }


        public async Task<IEnumerable<Coffee>> GetCoffees()
        {
            await Initialize();
            await SyncCoffee();
            return await coffeeTable.OrderBy(c=>c.Name).ToEnumerableAsync();
        }

        public async Task<Coffee> AddCoffee(string name, double lat, double lng)
        {
            await Initialize();

            //create and insert coffee
            var coffee = new Coffee
            {
                Name = name,
                Latitude = lat,
                Longitude = lng
            };

            await coffeeTable.InsertAsync(coffee);

            //Synchronize coffee
            await SyncCoffee();

            return coffee;
        }

        public async Task SyncCoffee()
        {
            try
            {
                //pull down all latest changes and then push current coffees up
                await Client.SyncContext.PushAsync();
                await coffeeTable.PullAsync("allCoffees", coffeeTable.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
            }

        }


    }
}
