using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Widget;
using System;
using TekConf.Mobile.Core.ViewModels;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	public class ConferenceDetailSessionsFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = this.BindingInflate(Resource.Layout.ConferenceDetailSessionsView, null);

			var listView = view.FindViewById<MvxListView>(Resource.Id.sessionsListView);
			var emptyView = view.FindViewById<TekConfTextView>(Resource.Id.emptySessions);
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

			var vm = this.DataContext as ConferenceDetailSessionsViewModel;
			if (vm != null)
			{
				Task.Run(async () => await vm.LoadSessionsAsync (LoadRequest.Load));
			}
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			var viewModel = this.DataContext as ConferenceDetailSessionsViewModel;

			inflater.Inflate(Resource.Menu.ConferenceActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_conference_search).ActionView;

			var textChangedLastTime = DateTime.Now;

			searchView.QueryTextChange += async (object sender, SearchView.QueryTextChangeEventArgs e) => 
			{
				var changedSpan = DateTime.Now.Subtract(textChangedLastTime);
				if (changedSpan.TotalMilliseconds > 200)
				{
					await viewModel.SearchAsync(e.NewText);
				}
				textChangedLastTime = DateTime.Now;
			};

			searchView.Close += async (sender, e) => 
			{
				await viewModel.LoadSessionsAsync(LoadRequest.Load);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed();
			};

			searchView.QueryTextSubmit += async (sender, e) => {
				await viewModel.SearchAsync(e.Query);
				searchView.ClearFocus();
				searchView.OnActionViewCollapsed();

			};
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferenceDetailSessionsViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
				case "Refresh":
					Task.Run (async () => await vm.RefreshAsync ());
						break;
					case "Settings":
						vm.ShowSettingsCommand.Execute (null);
						break;
					case "Sort By Date":
					Task.Run(async () => await vm.SortByDateAsync());
						break;
					case "Sort By Name":
					Task.Run(async () => await vm.SortByTitleAsync());
						break;
				}
			}

			return false;
		}
	}
}