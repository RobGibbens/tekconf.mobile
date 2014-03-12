using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using TekConf.Mobile.Core.ViewModels;
using Android.Widget;
using Android.InputMethodServices;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using System;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace TekConf.Mobile.Droid.Views
{
	public class ConferencesListFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ConferencesView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
			var listView = this.Activity.FindViewById<MvxListView> (Resource.Id.gridview);
			var emptyView = this.Activity.FindViewById<TextView> (Resource.Id.emptyConferences);
			listView.EmptyView = emptyView;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			var conferencesViewModel = this.DataContext as ConferencesViewModel;

			inflater.Inflate(Resource.Menu.ConferencesListActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_search).ActionView;

			var textChangedLastTime = DateTime.Now;

			searchView.QueryTextChange += async (object sender, SearchView.QueryTextChangeEventArgs e) => 
			{
				var changedSpan = DateTime.Now.Subtract(textChangedLastTime);
				if (changedSpan.TotalMilliseconds > 200)
				{
					await conferencesViewModel.SearchAsync(e.NewText);
				}
				textChangedLastTime = DateTime.Now;
			};

			searchView.Close += async (sender, e) => 
			{
				await conferencesViewModel.LoadConferencesAsync(LoadRequest.Load);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed();
			};

			searchView.QueryTextSubmit += async (sender, e) => {
				await conferencesViewModel.SearchAsync(e.Query);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed();
			};
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferencesViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
				case "Refresh":
					Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
					break;
				case "Settings":
					vm.ShowSettingsCommand.Execute (null);
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