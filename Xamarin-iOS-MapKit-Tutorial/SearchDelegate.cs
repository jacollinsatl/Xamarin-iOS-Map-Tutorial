using System;
using UIKit;
using MapKit;
using Foundation;
using System.Linq;

namespace XamariniOSMapKitTutorial
{
	public class SearchDelegate : UISearchDisplayDelegate
	{
		public SearchDelegate ()
		{
		}

		public override bool ShouldReloadForSearchString(UISearchDisplayController controller, string forSearchString)
		{
			var searchRequest = new MKLocalSearchRequest ();
			searchRequest.NaturalLanguageQuery = forSearchString;

			var localSearch = new MKLocalSearch (searchRequest);
			localSearch.Start (delegate (MKLocalSearchResponse response, NSError error) {
				if(response != null && error == null) {
					Console.WriteLine (response);
					((SearchSource)controller.SearchResultsSource).mapItems = response.MapItems.ToList();
					controller.SearchResultsTableView.ReloadData();
				} else {
					Console.WriteLine (error);
				}
			});

			return false;
		}
	}
}

