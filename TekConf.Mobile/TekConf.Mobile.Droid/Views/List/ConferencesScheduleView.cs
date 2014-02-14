using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using Android.Views;
using Android.Widget;
using Android.Animation;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	[Activity (Label = "Conferences", Icon = "@drawable/Icon")]
	public class ConferencesScheduleView : MvxActivity
	{
		private BindableProgress _bindableProgress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBarOverlay);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (Color.Transparent));

			_bindableProgress = new BindableProgress (this);

			SetContentView (Resource.Layout.ConferencesScheduleView);
			var set = this.CreateBindingSet<ConferencesScheduleView, ConferencesScheduleViewModel> ();
			set.Bind (_bindableProgress).For (b => b.Visible).To (vm => vm.AreConferencesLoading);
			set.Apply ();

			ActionBar.SetBackgroundDrawable (new ColorDrawable (new Color (r: 129, g: 153, b: 77)));
		}

//		public override bool OnCreateOptionsMenu (IMenu menu)
//		{
//			MenuInflater.Inflate (Resource.Menu.ConferencesListActionItems, menu);
//			return true;
//		}

//		public override bool OnOptionsItemSelected (IMenuItem item)
//		{
//			var vm = this.DataContext as ConferencesScheduleViewModel;
//			if (vm != null) {
//				switch (item.ToString ()) {
//				case "Search":
//					//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
//					break;
//
//				case "Refresh":
//					//Task.Factory.StartNew (() => {
//						//vm.Refresh ();
//					//});
//					break;
//				case "Settings":
//					//vm.ShowSettingsCommand.Execute (null);
//					break;
//				}
//			}
//
//			return false;
//		}


	}
}