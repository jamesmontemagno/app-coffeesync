using System;

using UIKit;
using System.Linq;
using MapKit;
using CoreLocation;
using CoffeeApp.Logic;
using System.Threading.Tasks;
using Foundation;

namespace CoffeeApp
{
    public partial class ViewController : UIViewController, IMKMapViewDelegate
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        CoffeesViewModel viewModel;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MyMap.WeakDelegate = this;

            ButtonLoad.Hidden = true;
            ButtonSave.Hidden = true;
           
            viewModel = new CoffeesViewModel();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Places.CollectionChanged += Places_CollectionChanged;

            ButtonSearch.TouchUpInside += ButtonSearch_TouchUpInside;
            ButtonLoad.TouchUpInside += ButtonLoad_TouchUpInside;
            ButtonSave.TouchUpInside += ButtonSave_TouchUpInside;
            
            var coords = new CLLocationCoordinate2D(App.StartLat, App.StartLong);
            var span = new MKCoordinateSpan(DistanceUtils.MilesToLatitudeDegrees(10),
                DistanceUtils.MilesToLongitudeDegrees(10, coords.Latitude));

            MyMap.Region = new MKCoordinateRegion(coords, span);
        }

       

        private async void ButtonSearch_TouchUpInside(object sender, EventArgs e)
        {
            await viewModel.SearchForPlaces(
                MyMap.Region.Center.Latitude,
                MyMap.Region.Center.Longitude,
                TextFieldQuery.Text);
        }
        
        private void Places_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MyMap.RemoveAnnotations(MyMap.Annotations);
            MyMap.AddAnnotations(viewModel.Places.Select(place => new MKPointAnnotation
            {
                Coordinate = new CLLocationCoordinate2D(place.Latitude, place.Longitude),
                Title = place.Name,
                Subtitle = $"{place.Stars} / 5 stars",
            }).ToArray());
        }

        [Export("mapView:viewForAnnotation:")]
        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var pinView = mapView.DequeueReusableAnnotation("colorpin") as MKPinAnnotationView;
            if(pinView == null)
            {
                pinView = new MKPinAnnotationView(annotation, "colorpin")
                {
                    CanShowCallout = true,
                };
            }
            else
            {
                pinView.Annotation = annotation;
            }
            var color = MKPinAnnotationColor.Green;

            var place = viewModel.Places.First(p => p.Latitude == annotation.Coordinate.Latitude && p.Longitude == annotation.Coordinate.Longitude && p.Name == annotation.GetTitle());
            if (place.Stars < 3.5)
                color = MKPinAnnotationColor.Red;
            else if (place.Stars < 4.3)
                color = MKPinAnnotationColor.Purple;

            pinView.PinColor = color;

            return pinView;
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel.IsBusy)
            {
                ButtonSearch.Enabled = false;
                ProgressBar.StartAnimating();
            }
            else
            {
                ButtonSearch.Enabled = true;
                ProgressBar.StopAnimating();
            }
        }

        private async void ButtonSave_TouchUpInside(object sender, EventArgs e)
        {
            if (MyMap.SelectedAnnotations.Length == 0)
                return;

            var item = MyMap.SelectedAnnotations[0];
            await viewModel.SaveCoffee(item.GetTitle(), item.Coordinate.Latitude, item.Coordinate.Longitude);
        }

        private async void ButtonLoad_TouchUpInside(object sender, EventArgs e)
        {
            await viewModel.LoadCoffees();
        }
    }
}