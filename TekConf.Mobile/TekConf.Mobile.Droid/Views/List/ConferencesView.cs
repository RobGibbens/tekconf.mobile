using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using Android.Views;

namespace TekConf.Mobile.Droid.Views
{
	[Activity (Label = "Conferences", Icon = "@drawable/Icon")]
	[IntentFilter(new string[] { "android.intent.action.SEARCH" })]
	[MetaData(("android.app.searchable"), Resource = "@xml/searchable")]
	public class ConferencesView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBarOverlay);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (new Color (r: 129, g: 153, b: 77)));

			SetContentView (Resource.Layout.ConferencesView);
		}
	}
}