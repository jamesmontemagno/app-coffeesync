Coffee Sync - Search and save favorite coffee shops
=========================

Simple application to go out and search and save favorite coffee shops on iOS and Android with Xamarin

Setup azure backend:

Blog: https://blog.xamarin.com/getting-started-azure-mobile-apps-easy-tables/

Update URL into AzureService.cs file.


Maps & Places (special note)
----

To use the Google Maps API on Android you must generate an **API key** and add it to your Android project. See the Xamarin doc on [obtaining a Google Maps API key](http://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key/). After following those instructions, paste the **API key** in the `Properties/AndroidManifest.xml` file (view source and find/update the following element):

    <meta-data android:name="com.google.android.maps.v2.API_KEY" android:value="APIKeyGoesHere" />


**Places Rest API**
In addition to this you must toggle on `Places API` under Services in your Google API Console. Then you can create a new “Simple API Access Key” that can be used in the `CoffeesViewModel.cs` file.
### License
The MIT License (MIT)

Copyright (c) 2014 James Montemagno / Refractored LLC
