using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Fragging;
using Android.OS;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

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

		private BindableProgress _bindableProgress;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			_bindableProgress = new BindableProgress(this, "Updating");
			var set = this.CreateBindingSet<SessionDetailTabView, SessionDetailTabViewModel>();

			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => ((SessionDetailViewModel)vm.SessionDetailViewModel).IsFavoritingSession);
			set.Apply();
		}
	}
}

