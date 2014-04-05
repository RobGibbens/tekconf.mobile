using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Fragging;
using TekConf.Mobile.Core.ViewModels;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Binding.BindingContext;

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

		private BindableProgress _conferencesLoadingProgress;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			_conferencesLoadingProgress = new BindableProgress(this, "Loading Conferences");

			var set = this.CreateBindingSet<ConferenceDetailTabView, ConferenceDetailTabViewModel>();

			set.Bind(_conferencesLoadingProgress)
				.For(p => p.Visible)
				.To(vm => ((ConferenceDetailViewModel)vm.ConferenceDetailViewModel).IsConferenceLoading)
//				.To(vm => ((ConferenceDetailSessionsViewModel)vm.ConferenceDetailSessionsViewModel).AreSessionsLoading)
//				.To(vm => ((ConferenceDetailScheduleViewModel)vm.ConferenceDetailScheduleViewModel).AreSessionsLoading)
				;
			//set.Bind(_scheduleLoadingProgress).For(p => p.Visible).To(vm => ((ConferencesScheduleViewModel)vm.ConferencesScheduleViewModel).AreConferencesLoading);
			set.Apply();
		}
	}
}