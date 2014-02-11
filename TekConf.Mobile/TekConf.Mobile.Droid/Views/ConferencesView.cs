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
	public class ConferencesView : MvxActivity
	{
		private BindableProgress _bindableProgress;
		LinearLayout _loadingBars;
		ProgressBar _bar1;
		ProgressBar _bar2;
		TextView _swipeText;
		bool _setup = false;
		int _accumulatedDeltaY = 0;
		ObjectAnimator _bar1Fade, _bar2Fade;

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
			_loadingBars = FindViewById<LinearLayout> (Resource.Id.loadingBars);
			_bar1 = FindViewById<ProgressBar> (Resource.Id.loadingBar1);
			_bar2 = FindViewById<ProgressBar> (Resource.Id.loadingBar2);
			_swipeText = FindViewById<TextView> (Resource.Id.swipeToRefreshText);

			// Remove progress bar background
			foreach (var p in new[] { _bar1, _bar2 }) {
				var layer = p.ProgressDrawable as LayerDrawable;
				if (layer != null)
					layer.SetDrawableByLayerId (Android.Resource.Id.Background,
						new ColorDrawable (Color.Transparent));
			}

			list.OverScrolled += deltaY => {
				ShowSwipeDown ();

				var vm = DataContext as ConferencesViewModel;
				if (vm != null) {
					Task.Factory.StartNew (() => vm.Refresh ().Wait()); 
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
			if (!_setup) {
				ActionBar.Hide ();
				if (_bar1Fade != null) {
					_bar1Fade.Cancel ();
					_bar1Fade = null;
				}
				if (_bar2Fade != null) {
					_bar2Fade.Cancel ();
					_bar2Fade = null;
				}
				_loadingBars.Visibility = ViewStates.Visible;
				_swipeText.TranslationY = -(ActionBar.Height + _swipeText.Height + 4);
				_swipeText.Visibility = ViewStates.Visible;
				_swipeText.Animate ().TranslationY (0).SetStartDelay (50).Start ();
				_accumulatedDeltaY = 0;
				_setup = true;
			}
		}

		private void HideSwipeDown ()
		{
			ActionBar.Show ();
			_swipeText.Visibility = ViewStates.Invisible;
			_bar1Fade = ObjectAnimator.OfInt (_bar1, "progress", _bar1.Progress, 0);
			_bar1Fade.SetDuration (250);
			_bar1Fade.Start ();
			_bar2Fade = ObjectAnimator.OfInt (_bar2, "progress", _bar2.Progress, 0);
			_bar2Fade.SetDuration (250);
			_bar2Fade.Start ();
			_bar2Fade.AnimationEnd += (sender, e) => _loadingBars.Visibility = ViewStates.Gone;
			_setup = false;
		}
	}
}