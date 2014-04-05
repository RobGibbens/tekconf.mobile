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
	public class ConferenceDetailFragment : MvxFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			return this.BindingInflate (Resource.Layout.ConferenceDetailView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;

			var vm = this.DataContext as ConferenceDetailViewModel;
			if (vm != null)
			{
				Task.Run(async () => await vm.LoadConferencesAsync (LoadRequest.Load));
			}
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.ConferenceDetailActionItems, menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			var vm = this.DataContext as ConferenceDetailViewModel;
			if (vm != null) {
				switch (item.ToString ()) {
				case "Refresh":
					Task.Run (async () => await vm.RefreshAsync ());
					break;
				case "Settings":
					vm.ShowSettingsCommand.Execute (null);
					break;
				}
			}

			return false;
		}
	}
}