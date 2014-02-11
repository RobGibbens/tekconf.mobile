using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Cirrious.MvvmCross.Binding.Droid.Views;
using System;
using Android.Content;
using Android.Util;
using System.Threading.Tasks;

namespace TekConf.Mobile.Droid.Views
{
	[Activity (Label = "Conferences", Icon = "@drawable/Icon")]
	public class ConferencesView : MvxActivity
	{
		private BindableProgress _bindableProgress;
		LinearLayout loadingBars;
		ProgressBar bar1;
		ProgressBar bar2;
		TextView swipeText;
		bool setup = false;
		int accumulatedDeltaY = 0;
		ObjectAnimator bar1Fade, bar2Fade;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBarOverlay);
			ActionBar.SetBackgroundDrawable (new ColorDrawable (Color.Transparent));

			_bindableProgress = new BindableProgress (this);

			SetContentView (Resource.Layout.ConferencesView);
			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel> ();
			set.Bind (_bindableProgress).For (b => b.Visible).To (vm => vm.AreConferencesLoading);
			set.Apply ();

			ActionBar.SetBackgroundDrawable (new ColorDrawable (new Color (r: 129, g: 153, b: 77)));
			var list = FindViewById<TekConfGridView> (Resource.Id.gridview);
			loadingBars = FindViewById<LinearLayout> (Resource.Id.loadingBars);
			bar1 = FindViewById<ProgressBar> (Resource.Id.loadingBar1);
			bar2 = FindViewById<ProgressBar> (Resource.Id.loadingBar2);
			swipeText = FindViewById<TextView> (Resource.Id.swipeToRefreshText);

			// Remove progress bar background
			foreach (var p in new[] { bar1, bar2 }) {
				var layer = p.ProgressDrawable as LayerDrawable;
				if (layer != null)
					layer.SetDrawableByLayerId (Android.Resource.Id.Background,
						new ColorDrawable (Color.Transparent));
			}

			list.OverScrolled += deltaY => {
				ShowSwipeDown ();

				var vm = DataContext as ConferencesViewModel;
				if (vm != null) {
					Task.Factory.StartNew (() => { 
						vm.Refresh ();
					}); 
				}

				HideSwipeDown ();
			};
			list.OverScrollCanceled += HideSwipeDown;

		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.ConferencesListActionItems, menu);
			return true;
		}
		//		protected override void OnViewModelSet ()
		//		{
		//			base.OnViewModelSet ();
		//			var vm = DataContext as ConferencesViewModel;
		//			var gridview = FindViewById<TekConfGridView> (Resource.Id.gridview);
		//
		//			vm.Changed += (object sender, EventArgs e) => {
		//				Typeface tf = Typeface.CreateFromAsset(this.Assets, "fonts/fontawesome.ttf");
		//				var myTextView = FindViewById<TextView> (Resource.Id.mytextview);
		//				myTextView.SetTypeface (tf, TypefaceStyle.Normal);
		//			};
		//		}
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			var vm = this.DataContext as ConferencesViewModel;
			if (vm != null) {
				switch (item.ToString ()) {
				case "Search":
					//TODO vm.ShowSessionsCommand.Execute(vm.Conference.slug);
					break;

				case "Refresh":
					Task.Factory.StartNew (() => {
						vm.Refresh ();
					});
					break;
				case "Settings":
					//vm.ShowSettingsCommand.Execute (null);
					break;
				}
			}

			return false;
		}

		private void ShowSwipeDown ()
		{
			if (!setup) {
				ActionBar.Hide ();
				if (bar1Fade != null) {
					bar1Fade.Cancel ();
					bar1Fade = null;
				}
				if (bar2Fade != null) {
					bar2Fade.Cancel ();
					bar2Fade = null;
				}
				loadingBars.Visibility = ViewStates.Visible;
				swipeText.TranslationY = -(ActionBar.Height + swipeText.Height + 4);
				swipeText.Visibility = ViewStates.Visible;
				swipeText.Animate ().TranslationY (0).SetStartDelay (50).Start ();
				accumulatedDeltaY = 0;
				setup = true;
			}
		}

		private void HideSwipeDown ()
		{
			ActionBar.Show ();
			swipeText.Visibility = ViewStates.Invisible;
			bar1Fade = ObjectAnimator.OfInt (bar1, "progress", bar1.Progress, 0);
			bar1Fade.SetDuration (250);
			bar1Fade.Start ();
			bar2Fade = ObjectAnimator.OfInt (bar2, "progress", bar2.Progress, 0);
			bar2Fade.SetDuration (250);
			bar2Fade.Start ();
			bar2Fade.AnimationEnd += (sender, e) => loadingBars.Visibility = ViewStates.Gone;
			setup = false;
		}
	}
}