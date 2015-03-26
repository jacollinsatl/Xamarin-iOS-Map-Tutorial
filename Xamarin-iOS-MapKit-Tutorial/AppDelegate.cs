using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using CoreLocation;

namespace XamariniOSMapKitTutorial
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		private UINavigationController _navController;
		private UIViewController _rootViewController;
		private CLLocationManager _locationManager;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// Create a CLLocationManager object
			_locationManager = new CLLocationManager ();

			// Check the iOS version
			if (UIDevice.CurrentDevice.CheckSystemVersion (8,0))
				_locationManager.RequestWhenInUseAuthorization ();
			
			// If you have defined a root view controller, set it here:
			// window.RootViewController = myViewController;

			_navController = new UINavigationController ();
			_rootViewController = new MapViewController ();

			// Push the view controller onto the navigation controller and show the window
			_navController.PushViewController(_rootViewController, false);
			window.RootViewController = _navController;
			window.MakeKeyAndVisible (); 
			
			return true;
		}
	}
}

