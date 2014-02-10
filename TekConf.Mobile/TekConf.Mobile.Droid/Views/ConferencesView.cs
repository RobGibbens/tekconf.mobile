using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using Android.Widget;

namespace TekConf.Mobile.Droid.Views
{
	[Activity (Label = "Conferences", Icon = "@drawable/Icon")]
	public class ConferencesView : MvxActivity
	{
		private BindableProgress _bindableProgress;
		private MvxSubscriptionToken _conferencesLoadedToken;
		private IMvxMessenger _messenger;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_bindableProgress = new BindableProgress(this);
			_messenger = Mvx.Resolve<IMvxMessenger> ();
			_conferencesLoadedToken = _messenger.SubscribeOnMainThread<ConferencesLoaded> (OnConferencesLoaded);

			SetContentView (Resource.Layout.ConferencesView);

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel> ();
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.AreConferencesLoading);
			set.Apply ();
		}

		void OnConferencesLoaded (ConferencesLoaded obj)
		{
			var vm = this.ViewModel as ConferencesViewModel;
			var gridview = FindViewById<GridView> (Resource.Id.gridview);
			gridview.FastScrollEnabled = true;
			gridview.Adapter = new ConferenceListAdapter (this, vm.Conferences);
		}
	}

}