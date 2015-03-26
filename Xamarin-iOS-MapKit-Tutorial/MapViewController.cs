
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;
using MapKit;
using CoreLocation;

namespace XamariniOSMapKitTutorial
{
	public partial class MapViewController : DialogViewController
	{
		private MKMapView _mapView;
		private List<DataModel> _dataList;
		private UIButton _annotationDetailButton; // Add this here to avoid the GC

		protected string annotationIdentifier = "AnnotationIdentifier";

		public MapViewController () : base (UITableViewStyle.Grouped, null)
		{
			initializeMap ();
			Root = new RootElement ("Map View") {};

			_dataList = createAnnotationLocations ();
			addAnnotations ();
		} // end constructor

		// Initialize map and user location
		private void initializeMap() 
		{
			// Create a new MKMapView and add it to the view
			_mapView = new MKMapView (View.Bounds);
			_mapView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			View.AddSubview (_mapView);

			// Show the users location
			_mapView.ShowsUserLocation = true;

			// Add an event handler for when the user moves and the location updates
			_mapView.DidUpdateUserLocation += (sender, e) => {
				if(_mapView.UserLocation != null) {
					CLLocationCoordinate2D coords = _mapView.UserLocation.Coordinate;
					MKCoordinateSpan span = new MKCoordinateSpan(kmToLatitudeDegrees(5), kmToLongitudeDegrees(5, coords.Latitude));
					_mapView.Region = new MKCoordinateRegion(coords, span);
				}
			};

			// If the user denies location permission, or the devices GPS/location is unavailable
			if (!_mapView.UserLocationVisible) {
				CLLocationCoordinate2D coords = new CLLocationCoordinate2D (37.4057900, -122.0781260); // Set default coordinates to someplace close to Google HQ
				MKCoordinateSpan span = new MKCoordinateSpan(kmToLatitudeDegrees(5), kmToLongitudeDegrees(5, coords.Latitude));
				_mapView.Region = new MKCoordinateRegion (coords, span);
			}

			// Add delegate for annotations to be able to customize the annotation
			_mapView.GetViewForAnnotation = GetViewForAnnotation;
		} // end initializeMap

		// Add all annotations to map
		private void addAnnotations()
		{
			foreach (var annotationLocation in _dataList) {
				var annotation = new AnnotationModel (new CLLocationCoordinate2D (annotationLocation.Latitude, annotationLocation.Longitude), annotationLocation.Title);
				_mapView.AddAnnotation (annotation);
			}
		} // end addAnnotations

		// Create a list for annotations
		private List<DataModel> createAnnotationLocations()
		{
			List<DataModel> dataList = new List<DataModel> () {
				new DataModel ("Google HQ", 37.4224760, -122.0842500),
				new DataModel ("Taco Bell", 37.4082550, -122.0776200),
				new DataModel ("Google MTV", 37.3974740, -122.0848690)
			};

			return dataList;
		} // end createAnnotationLocations

		// Calculate the latitude degrees from a given km distance
		private double kmToLatitudeDegrees(double kms)
		{
			double earthRadius = 6371.0; 
			double radiansToDegrees = 180.0 / Math.PI;

			return (kms / earthRadius) * radiansToDegrees;
		} // end kmToLatitutdeDegrees

		// Calculate the longitude degrees from a given km distance and at a certain latitude
		private double kmToLongitudeDegrees(double kms, double atLatitude)
		{
			double earthRadius = 6371.0; // in kms
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (kms / radiusAtLatitude) * radiansToDegrees;
		} // End kmToLongitudeDegrees

		// Customize the annotation view
		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = mapView.DequeueReusableAnnotation (annotationIdentifier);

			// Set current location and location of annotation
			CLLocationCoordinate2D currentLocation = mapView.UserLocation.Coordinate;
			CLLocationCoordinate2D annotationLocation = annotation.Coordinate;

			// We don't want a special annotation for the user location
			if (currentLocation.Latitude == annotationLocation.Latitude && currentLocation.Longitude == annotationLocation.Longitude)
				return null;

			if (annotationView == null)
				annotationView = new MKPinAnnotationView (annotation, annotationIdentifier);
			else
				annotationView.Annotation = annotation;

			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = false; // Set to true if you want to animate the pin dropping
			(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Red;
			annotationView.SetSelected (true, false);

			_annotationDetailButton = UIButton.FromType (UIButtonType.DetailDisclosure);
			_annotationDetailButton.TouchUpInside += (sender, e) => {
				new UIAlertView("Yay!", "You successfully clicked the detail button!", null, "OK", null).Show();
			};

			annotationView.RightCalloutAccessoryView = _annotationDetailButton;

			// Annotation icon may be specified like this, in case you want it.
			// annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("example.png"));

			return annotationView;
		} // end GetViewForAnnotation
	}
}
