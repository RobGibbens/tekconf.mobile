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
		private BindableProgress _bindableProgress;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBarOverlay);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (new Color (r: 129, g: 153, b: 77)));
			_bindableProgress = new BindableProgress (this);

			SetContentView (Resource.Layout.ConferencesView);

//			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinner1);
//
//			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
//			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.sorts_array, Android.Resource.Layout.SimpleSpinnerItem);
//
//			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
//			spinner.Adapter = adapter;

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel> ();
			set.Bind (_bindableProgress).For (b => b.Visible).To (vm => vm.AreConferencesLoading);
			set.Apply ();

		}



	}
}