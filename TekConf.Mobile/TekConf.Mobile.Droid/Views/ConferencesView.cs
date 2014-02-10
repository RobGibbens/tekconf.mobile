using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Droid.Views
{
	[Activity (Label = "Conferences", Icon = "@drawable/Icon")]
	public class ConferencesView : MvxActivity
	{
		private BindableProgress _bindableProgress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_bindableProgress = new BindableProgress(this);

			SetContentView (Resource.Layout.ConferencesView);

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel> ();
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.AreConferencesLoading);
			set.Apply ();

            ActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(r: 129, g: 153, b: 77)));
		}
	}
}