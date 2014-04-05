using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Fragging;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class ConferencesTabView : MvxTabsFragmentActivity
	{

		public ConferencesTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{

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

		private BindableProgress _conferencesLoadingProgress;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			_conferencesLoadingProgress = new BindableProgress(this, "Loading Conferences");

			var set = this.CreateBindingSet<ConferencesTabView, ConferencesTabViewModel>();

			set.Bind(_conferencesLoadingProgress)
				.For(p => p.Visible)
				.To(vm => ((ConferencesViewModel)vm.ConferencesViewModel).AreConferencesLoading);
			//set.Bind(_scheduleLoadingProgress).For(p => p.Visible).To(vm => ((ConferencesScheduleViewModel)vm.ConferencesScheduleViewModel).AreConferencesLoading);
			set.Apply();
		}

	}
}