using System;
using Android.App;
using Android.Widget;
using Android.OS;

using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;

using CoffeeApp.Logic;
using System.Collections.Generic;

namespace CoffeeApp.Droid
{
    [Activity(Label = "Coffee App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback
    {

        GoogleMap map;
        MapFragment mapFragment;
        Handler handler;

        CoffeesViewModel viewModel;
        EditText query;
        Button searchButton, saveButton, loadButton;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            viewModel = new CoffeesViewModel();

            handler = new Handler();
            
            //load map
            mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            //Find all items on the UI
            query = FindViewById<EditText>(Resource.Id.editText1);
            searchButton = FindViewById<Button>(Resource.Id.button_search);


            loadButton = FindViewById<Button>(Resource.Id.button_load);
            saveButton = FindViewById<Button>(Resource.Id.button_save);

            //normal click handlers here.
            loadButton.Click += Load_Click;

            saveButton.Click += Save_Click;

            //Click events can be done in line with lambdas
            searchButton.Click += async (sender, args) =>
            {
                searchButton.Enabled = false;

                var center = map.CameraPosition.Target;

                var places = await viewModel.SearchForPlaces(center.Latitude, center.Longitude, query.Text);

                map.Clear();

                //load existing coffees from azure
                if(coffees != null)
                {
                    foreach (var coffee in coffees)
                        AddMarker(coffee.Latitude, coffee.Longitude, coffee.Name);
                }

                //load new places
                foreach(var place in places)
                    AddMarker(place.Latitude, place.Longitude, place.Name);

                searchButton.Enabled = true;
            };
           
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            if (selectedMarker == null)
                return;

            await AzureService.Instance.AddCoffee(selectedMarker.Title, selectedMarker.Position.Latitude, selectedMarker.Position.Longitude);
            Toast.MakeText(this, "Saved!!!!", ToastLength.Short).Show();
        }

        private async void Load_Click(object sender, EventArgs e)
        {
            if (map == null)
                return;

            map.Clear();
            coffees = await AzureService.Instance.GetCoffees();
            foreach (var item in coffees)
                AddMarker(item.Latitude, item.Longitude, item.Name);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            if (googleMap == null)
                return;

            map = googleMap;

            map.MarkerClick += Map_MarkerClick;

            handler.PostDelayed(ZoomToSeattle, 1000);
        }

        private void Map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            if (selectedMarker != null)
                selectedMarker.HideInfoWindow();

            selectedMarker = e.Marker;
            selectedMarker.ShowInfoWindow();
        }

        IEnumerable<Coffee> coffees;
        private Marker selectedMarker;

        void AddMarker(double lat, double lng, string title)
        {
            if (map == null)
                return;


            var marker = new MarkerOptions()
                .SetPosition(new LatLng(lat, lng))
                .SetTitle(title);
            map.AddMarker(marker);

        }


        void ZoomToSeattle()
        {
           
            var location = new LatLng(47.6062, -122.3321);
            var builder = CameraPosition.InvokeBuilder()
                .Target(location)
                .Zoom(14);

            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(builder.Build());
            RunOnUiThread(()=>map?.MoveCamera(cameraUpdate));
        }
    }
}

