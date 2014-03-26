using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Fragging;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.CrossCore;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class ConferencesTabView : MvxTabsFragmentActivity
	{
		readonly IMvxMessenger _messenger;
		private BindableProgress _bindableProgress;
		private MvxSubscriptionToken _conferencesLoadingToken;
		private MvxSubscriptionToken _conferencesLoadedToken;

		public ConferencesTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
			_messenger = Mvx.Resolve<IMvxMessenger>();
			//WireUpLoadingEvents();
		}

		public ConferencesTabViewModel ConferencesTabViewModel
		{
			get { return (ConferencesTabViewModel)base.ViewModel; }
		}

		protected override void AddTabs(Bundle args)
		{
			AddTab<ConferencesListFragment>("ConferencesTab", "Conferences", args, ConferencesTabViewModel.ConferencesViewModel);
			AddTab<ConferencesScheduleFragment>("ConferencesScheduleTab", "Schedule", args, ConferencesTabViewModel.ConferencesScheduleViewModel);
		}
			
		protected override void OnResume()
		{
			base.OnResume();
			//WireUpLoadingEvents();
		}

		//private void WireUpLoadingEvents()
		//{
		//	if (_bindableProgress == null)
		//		_bindableProgress = new BindableProgress(this);

		//	if (_conferencesLoadingToken == null)
		//	_conferencesLoadingToken = _messenger.SubscribeOnMainThread<ConferencesLoading>(OnConferencesLoading);

		//	if (_conferencesLoadedToken == null)
		//	_conferencesLoadedToken = _messenger.SubscribeOnMainThread<ConferencesLoaded>(OnConferencesLoaded);
		//}

		protected override void OnPause()
		{
			//_messenger.Unsubscribe<ConferencesLoading>(_conferencesLoadingToken);
			//_conferencesLoadingToken = null;

			//_messenger.Unsubscribe<ConferencesLoaded>(_conferencesLoadedToken);
			//_conferencesLoadedToken = null;

			base.OnPause();
		}

		private void OnConferencesLoading(ConferencesLoading message)
		{
			_bindableProgress.Visible = true;
		}

		private void OnConferencesLoaded(ConferencesLoaded message)
		{
			_bindableProgress.Visible = false;
		}
	}
}