using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Fragging;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class ConferencesTabView : MvxTabsFragmentActivity
	{
		public ConferencesTabViewModel ConferencesTabViewModel
		{
			get { return (ConferencesTabViewModel)base.ViewModel; }
		}

		public ConferencesTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
		}

		protected override void AddTabs(Bundle args)
		{
			AddTab<ConferencesListFragment>("ConferencesTab", "Conferences", args, ConferencesTabViewModel.ConferencesViewModel);

			AddTab<ConferencesScheduleFragment>("ConferencesScheduleTab", "Schedule", args, ConferencesTabViewModel.ConferencesScheduleViewModel);
		}
	}
}