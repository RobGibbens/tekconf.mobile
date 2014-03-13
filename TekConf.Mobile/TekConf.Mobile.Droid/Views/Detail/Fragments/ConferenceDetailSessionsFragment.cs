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
			var emptyView = view.FindViewById<TextView>(Resource.Id.emptySessions);
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
						//Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
						break;
					case "Settings":
						vm.ShowSettingsCommand.Execute (null);
						break;
					case "Sort By Date":
						Task.Factory.StartNew(() => vm.SortByDateAsync().Wait());
						break;
					case "Sort By Name":
						Task.Factory.StartNew(() => vm.SortByTitleAsync().Wait());
						break;
				}
			}

			return false;
		}
	}
}