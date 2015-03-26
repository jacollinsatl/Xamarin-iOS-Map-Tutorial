using System;

namespace XamariniOSMapKitTutorial
{
	public class DataModel
	{
		private string _title = string.Empty;
		private double _latitude;
		private double _longitude;

		public DataModel (string title, double latitude, double longitude)
		{
			Title = title;
			Latitude = latitude;
			Longitude = longitude;
		}

		public string Title {
			get { return _title; }
			set { _title = value; }
		}

		public double Latitude {
			get { return _latitude; }
			set { _latitude = value; }
		}

		public double Longitude {
			get { return _longitude; }
			set { _longitude = value; }
		}
	}
}

