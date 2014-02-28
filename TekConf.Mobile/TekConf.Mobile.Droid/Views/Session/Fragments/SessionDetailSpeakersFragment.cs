using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Widget;
using System;
using TekConf.Mobile.Core.ViewModels;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	public class SessionDetailSpeakersFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.SessionDetailSpeakersView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			var viewModel = this.DataContext as SessionDetailSpeakersViewModel;

			inflater.Inflate(Resource.Menu.SessionDetailSpeakersActionItems, menu);

			var searchView = (SearchView)menu.FindItem(Resource.Id.menu_session_speakers_search).ActionView;

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
					case "Add to Schedule":
						//Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
						break;
				}
			}

			return false;
		}
	}
}