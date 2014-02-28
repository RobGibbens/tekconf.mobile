using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Fragging;
using Android.OS;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class SessionDetailTabView : MvxTabsFragmentActivity
	{
		public SessionDetailTabViewModel SessionDetailTabViewModel
		{
			get { return (SessionDetailTabViewModel)base.ViewModel; }
		}

		public SessionDetailTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
		}

		protected override void AddTabs(Bundle args)
		{
			AddTab<SessionDetailFragment>("SessionDetailTab", "Detail", args, SessionDetailTabViewModel.SessionDetailViewModel);

			AddTab<SessionDetailSpeakersFragment>("SessionDetailSpeakersTab", "Speakers", args, SessionDetailTabViewModel.SessionDetailSpeakersViewModel);
		}
	}
}

