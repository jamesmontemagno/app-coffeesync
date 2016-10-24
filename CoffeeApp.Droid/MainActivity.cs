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

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            viewModel = new CoffeesViewModel();

            handler = new Handler();
            
            //load map
            mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            mapFragment.GetMapAsync(this);


           
        }


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
            RunOnUiThread(() => map?.MoveCamera(cameraUpdate));
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

    }
}

