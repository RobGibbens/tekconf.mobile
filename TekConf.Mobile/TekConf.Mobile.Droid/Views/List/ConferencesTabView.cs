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
	public class ConferencesTabView : MvxTabsFragmentActivity
	{
		public ConferencesTabViewModel ConferencesTabViewModel
		{
			get { return (ConferencesTabViewModel)base.ViewModel; }
		}

		public ConferencesTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.ConferencesListActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_search).ActionView;
			searchView.SearchClick += OnSearchClicked;
			
			return true;
		}

		private void OnSearchClicked(object sender, EventArgs e)
		{
			var x = "";
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferencesTabViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
					case "Search":
						//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
						break;

					case "Refresh":
						Task.Factory.StartNew(() => vm.Refresh().Wait());
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
			AddTab<ConferencesListFragment>("ConferencesTab", "Conferences", args, ConferencesTabViewModel.Vm1);

			AddTab<ConferencesScheduleFragment>("ConferencesScheduleTab", "Schedule", args, ConferencesTabViewModel.Vm2);
		}
	}
}