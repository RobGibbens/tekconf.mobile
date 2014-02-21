using System;
using Android.App;
using Android.OS;

using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Fragging;
using TekConf.Mobile.Core.ViewModels;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class ConferenceDetailTabView : MvxTabsFragmentActivity
	{
		public ConferenceDetailTabViewModel ConferenceDetailTabViewModel
		{
			get { return (ConferenceDetailTabViewModel)base.ViewModel; }
		}

		public ConferenceDetailTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.ConferenceActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_conference_search).ActionView;
			searchView.SearchClick += OnSearchClicked;

			return true;
		}

		private void OnSearchClicked(object sender, EventArgs e)
		{
			var x = "";
		}


		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferenceDetailTabViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
					case "Search":
						//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
						break;

					case "Refresh":
						//Task.Factory.StartNew(() => vm.Refresh().Wait());
						break;
					case "Settings":
						//vm.ShowSettingsCommand.Execute (null);
						break;
					case "Sort By Date":
						Task.Factory.StartNew(() => vm.SortByDate().Wait());
						break;
					case "Sort By Name":
						Task.Factory.StartNew(() => vm.SortByName().Wait());
						break;
				}
			}

			return false;
		}

		protected override void AddTabs(Bundle args)
		{
			AddTab<ConferenceDetailFragment>("ConferencesDetailTab", "Detail", args, ConferenceDetailTabViewModel.ConferenceDetail);

			AddTab<ConferenceDetailSessionsFragment>("ConferenceDetailSessionsTab", "Sessions", args, ConferenceDetailTabViewModel.ConferenceDetailSessions);

			AddTab<ConferenceDetailScheduleFragment>("ConferenceDetailScheduleTab", "Schedule", args, ConferenceDetailTabViewModel.ConferenceDetailSessions);
			//AddTab<ConferenceDetailLocationFragment>("ConferenceDetailLocationTab", "Map", args, ConferenceDetailTabViewModel.ConferenceDetailLocation);

		}
	}
}