using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using TekConf.Mobile.Core.ViewModels;
using Android.Widget;
using System.Threading.Tasks;
using System;

namespace TekConf.Mobile.Droid.Views
{
	public class ConferencesScheduleFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			
			var view = this.BindingInflate(Resource.Layout.ConferencesScheduleView, null);

			var listView = view.FindViewById<MvxGridView>(Resource.Id.schedulesGridview);
			var emptyView = view.FindViewById<TekConfTextView>(Resource.Id.emptyConferencesSchedule);
			if (listView != null && emptyView != null)
			{
				listView.EmptyView = emptyView;
			}

			return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			var conferencesScheduleViewModel = this.DataContext as ConferencesScheduleViewModel;

			inflater.Inflate(Resource.Menu.ConferencesListActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_search).ActionView;

			var textChangedLastTime = DateTime.Now;
			searchView.QueryTextChange += async (object sender, SearchView.QueryTextChangeEventArgs e) => 
			{
				var changedSpan = DateTime.Now.Subtract(textChangedLastTime);
				if (changedSpan.TotalMilliseconds > 200)
				{
					await conferencesScheduleViewModel.SearchAsync(e.NewText);
				}
				textChangedLastTime = DateTime.Now;
			};

			searchView.Close += async (sender, e) => 
			{
				await conferencesScheduleViewModel.LoadConferencesAsync(LoadRequest.Load);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed();
			};

			searchView.QueryTextSubmit += async (sender, e) => {
				await conferencesScheduleViewModel.SearchAsync(e.Query);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed(); 
			};
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferencesScheduleViewModel;
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