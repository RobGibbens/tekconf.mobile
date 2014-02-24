using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Widget;
using System;
using TekConf.Mobile.Core.ViewModels;
using System.Threading.Tasks;
using Android.Views.InputMethods;

namespace TekConf.Mobile.Droid.Views
{
	public class ConferenceDetailScheduleFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ConferenceDetailScheduleView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
		}
		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate(Resource.Menu.ConferenceDetailSessionsActionItems, menu);
			var viewModel = this.DataContext as ConferenceDetailScheduleViewModel;
			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_conference_detail_sessions_search).ActionView;
			searchView.QueryTextSubmit += async (sender, e) => {

				await viewModel.SearchAsync(e.Query);

				searchView.ClearFocus();
				//((InputMethodManager)GetSystemService(InputMethodService)).ShowSoftInput(searchView, ShowFlags.Implicit);
			};
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var vm = this.DataContext as ConferenceDetailScheduleViewModel;
			if (vm != null)
			{
				switch (item.ToString())
				{
					case "Refresh":
						//Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
						break;
					case "Settings":
						//vm.ShowSettingsCommand.Execute (null);
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