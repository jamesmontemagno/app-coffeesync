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




            var coords = new CLLocationCoordinate2D(47.6062, -122.3321);
            var span = new MKCoordinateSpan(DistanceUtils.MilesToLatitudeDegrees(20),
                DistanceUtils.MilesToLongitudeDegrees(20, coords.Latitude));

            //MyMap.Region = new MKCoordinateRegion(coords, span);
        }


       
       

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}