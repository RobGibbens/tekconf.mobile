using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using TekConf.Mobile.Core.ViewModels;
using Android.Widget;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	public class ConferencesScheduleFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ConferencesScheduleView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
		}

		//		public override void OnCreateOptionsMenu (IMenu menu)
		//		{
		//			base.OnCreateOptionsMenu (menu, inflater);
		//		}
		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			var conferencesScheduleViewModel = this.DataContext as ConferencesScheduleViewModel;

			//var conferencesViewModel = tabViewModel.ConferencesViewModel as ConferencesViewModel;

			inflater.Inflate(Resource.Menu.ConferencesListActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_search).ActionView;

			searchView.Close += async (sender, e) => 
			{
				await conferencesScheduleViewModel.LoadConferencesAsync(LoadRequest.Load);
				searchView.ClearFocus();
				//((InputMethodManager)GetSystemService(InputMethodService)).ShowSoftInput(searchView, ShowFlags.Implicit);
			};
			searchView.QueryTextSubmit += async (sender, e) => {

				await conferencesScheduleViewModel.Search(e.Query);

				searchView.ClearFocus();
				//((InputMethodManager)GetSystemService(InputMethodService)).ShowSoftInput(searchView, ShowFlags.Implicit);
			};
		}


		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferencesScheduleViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
				case "Search":
					//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
					break;

				case "Refresh":
					Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
					break;
				case "Settings":
					//vm.ShowSettingsCommand.Execute (null);
					break;
				case "Sort By Date":
					Task.Factory.StartNew(() => vm.SortByDateAsync().Wait());
					break;
				case "Sort By Name":
					Task.Factory.StartNew(() => vm.SortByNameAsync().Wait());
					break;
				}
			}

			return false;
		}
	}
}