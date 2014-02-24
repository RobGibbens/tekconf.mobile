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



		protected override void AddTabs(Bundle args)
		{
			AddTab<ConferenceDetailFragment>("ConferencesDetailTab", "Detail", args, ConferenceDetailTabViewModel.ConferenceDetailViewModel);

			AddTab<ConferenceDetailSessionsFragment>("ConferenceDetailSessionsTab", "Sessions", args, ConferenceDetailTabViewModel.ConferenceDetailSessionsViewModel);

			AddTab<ConferenceDetailScheduleFragment>("ConferenceDetailScheduleTab", "Schedule", args, ConferenceDetailTabViewModel.ConferenceDetailScheduleViewModel);
			//AddTab<ConferenceDetailLocationFragment>("ConferenceDetailLocationTab", "Map", args, ConferenceDetailTabViewModel.ConferenceDetailLocation);

		}
	}
}