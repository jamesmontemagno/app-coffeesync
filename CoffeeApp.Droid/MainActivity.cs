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
using Acr.UserDialogs;
using Android.Support.V7.App;

namespace CoffeeApp.Droid
{
    [Activity(Label = "Coffee App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {

        GoogleMap map;
        MapFragment mapFragment;
        Handler handler;

        CoffeesViewModel viewModel;
        EditText query;
        Button searchButton, saveButton, loadButton;
        ProgressBar progress;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            
            SetSupportActionBar(toolbar);
            

            viewModel = new CoffeesViewModel();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Places.CollectionChanged += Places_CollectionChanged;


            //Setup UI here
            query = FindViewById<EditText>(Resource.Id.edittext_query);
            searchButton = FindViewById<Button>(Resource.Id.button_search);
            progress = FindViewById<ProgressBar>(Resource.Id.progressbar_loading);

            loadButton = FindViewById<Button>(Resource.Id.button_load);
            saveButton = FindViewById<Button>(Resource.Id.button_save);

            //normal click handlers here.
            loadButton.Click += LoadButton_Click;
            saveButton.Click += SaveButton_Click;

            //Click events can be done in line with lambdas
            searchButton.Click += async (sender, args) =>
            {
                if (map == null)
                    return;

                var center = map.CameraPosition.Target;

                await viewModel.SearchForPlaces(center.Latitude, center.Longitude, query.Text);

            };



            //load map
            mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            UserDialogs.Init(this);
        }

        

        private void Places_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                map.Clear();

                foreach (var place in viewModel.Places)
                {

                    var color = BitmapDescriptorFactory.HueGreen;
                    if (place.Stars < 3.5)
                        color = BitmapDescriptorFactory.HueRed;
                    else if (place.Stars < 4.3)
                        color = BitmapDescriptorFactory.HueMagenta;

                    var marker = new MarkerOptions()
                        .SetPosition(new LatLng(place.Latitude, place.Longitude))
                        .SetTitle(place.Name)
                        .SetSnippet($"{place.Stars} / 5 stars")
                        .SetIcon(BitmapDescriptorFactory.DefaultMarker(color));



                    map.AddMarker(marker);
                }
            });
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel.IsBusy)
            {
                searchButton.Enabled = false;
                progress.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                searchButton.Enabled = true;
                progress.Visibility = Android.Views.ViewStates.Invisible;
            }
        }

        #region Map Ready Code
        private Marker selectedMarker;
        public void OnMapReady(GoogleMap googleMap)
        {
            if (googleMap == null)
                return;

            map = googleMap;

            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.ZoomGesturesEnabled = true;

            map.MarkerClick += Map_MarkerClick;
            handler = new Handler();
            handler.PostDelayed(ZoomToLocation, 1000);
        }

        private void Map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            if (selectedMarker != null)
                selectedMarker.HideInfoWindow();

            selectedMarker = e.Marker;
            selectedMarker.ShowInfoWindow();
        }
        void ZoomToLocation()
        {

            var location = new LatLng(App.StartLat, App.StartLong);
            var builder = CameraPosition.InvokeBuilder()
                .Target(location)
                .Zoom(14);

            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(builder.Build());
            RunOnUiThread(() => map?.MoveCamera(cameraUpdate));
        }
        #endregion   

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (selectedMarker == null)
                return;

            await viewModel.SaveCoffee(selectedMarker.Title, selectedMarker.Position.Latitude, selectedMarker.Position.Longitude);
        }


        private async void LoadButton_Click(object sender, EventArgs e)
        {
            await viewModel.LoadCoffees();
        }

          
        
    }
}

