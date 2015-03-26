using System;
using MapKit;
using CoreLocation;

namespace XamariniOSMapKitTutorial
{
	public class AnnotationModel : MKAnnotation
	{
		private string _title;
		private string _subtitle;

		public AnnotationModel (CLLocationCoordinate2D coordinate, string title, string subtitle = "")
		{
			this.Coords = coordinate;
			_title = title;
			_subtitle = subtitle;
		}

		public override string Title { get { return _title; } }
		public override string Subtitle { get { return _subtitle; } }

		public CLLocationCoordinate2D Coords;
		public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }
	}
}

