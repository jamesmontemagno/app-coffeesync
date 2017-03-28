// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace CoffeeApp
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonLoad { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView MyMap { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView ProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TextFieldQuery { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ButtonLoad != null) {
                ButtonLoad.Dispose ();
                ButtonLoad = null;
            }

            if (ButtonSave != null) {
                ButtonSave.Dispose ();
                ButtonSave = null;
            }

            if (ButtonSearch != null) {
                ButtonSearch.Dispose ();
                ButtonSearch = null;
            }

            if (MyMap != null) {
                MyMap.Dispose ();
                MyMap = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose ();
                ProgressBar = null;
            }

            if (TextFieldQuery != null) {
                TextFieldQuery.Dispose ();
                TextFieldQuery = null;
            }
        }
    }
}