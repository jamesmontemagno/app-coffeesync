using System;

using UIKit;
using System.Linq;
using MapKit;
using CoreLocation;
using CoffeeApp.Logic;
using System.Threading.Tasks;

namespace CoffeeApp
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        CoffeesViewModel viewModel;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

           
            viewModel = new CoffeesViewModel();


            ButtonSearch.TouchUpInside += async (sender, args) =>
            {
                ButtonSearch.Enabled = false;

                var center = MyMap.Region.Center;
				ActivityIsLoading.StartAnimating();

                await UIView.AnimateAsync(1.0,
                    () => MyMap.Alpha = 0);

                MyMap.RemoveAnnotations(MyMap.Annotations);

                var places = await viewModel.SearchForPlaces(center.Latitude, center.Longitude, TextFieldQuery.Text);

                foreach(var item in places)
                {
                    MyMap.AddAnnotation(new MKPointAnnotation
                    {
                        Title = item.Name,
                        Coordinate = new CLLocationCoordinate2D(item.Latitude, item.Longitude)
                    });

                }

                await UIView.AnimateAsync(1.0,
                    () => MyMap.Alpha = 1);

                ButtonSearch.Enabled = true;
				ActivityIsLoading.StopAnimating();
            };

			ButtonSave.TouchUpInside += async (sender, e) =>
			{
				ActivityIsLoading.StartAnimating();
				var selected = MyMap.SelectedAnnotations.FirstOrDefault();

				if (selected == null)
					return;
				
				ButtonSave.Enabled = false;

				await AzureService.Instance.AddCoffee(selected.GetTitle(), selected.Coordinate.Latitude, selected.Coordinate.Longitude);

				new UIAlertView("Saved!", "Item has been saved", null, "OK").Show();

				ButtonSave.Enabled = true;
				ActivityIsLoading.StopAnimating();
			};

            ButtonLoad.TouchUpInside += async (sender, args) =>
            {
				ActivityIsLoading.StartAnimating();
                ButtonLoad.Enabled = false;
                MyMap.RemoveAnnotations(MyMap.Annotations);

                var places = await AzureService.Instance.GetCoffees();

                foreach (var item in places)
                {
                    MyMap.AddAnnotation(new MKPointAnnotation
                    {
                        Title = item.Name,
                        Coordinate = new CLLocationCoordinate2D(item.Latitude, item.Longitude)
                    });

                }
                ButtonLoad.Enabled = true;
				ActivityIsLoading.StopAnimating();
            };
            

           
            ZoomToSeattle();
         }


        void ZoomToSeattle()
        {

            var coords = new CLLocationCoordinate2D(47.6062, -122.3321);
            var span = new MKCoordinateSpan(DistanceUtils.MilesToLatitudeDegrees(20),
                DistanceUtils.MilesToLongitudeDegrees(20, coords.Latitude));

            MyMap.Region = new MKCoordinateRegion(coords, span);
        }

       

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public async Task<string> GetNameForLocation(double latitude, double longitude)
        {

            var coder = new CLGeocoder();

            var locations = await coder.ReverseGeocodeLocationAsync(new CLLocation
                (latitude, longitude));

            var name = locations?.FirstOrDefault()?.Name ?? "unknown";
            

            return name;
        }

    }
}