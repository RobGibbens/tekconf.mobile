using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.Droid.Views
{
    [Activity(Label = "Conferences")]
    public class ConferencesView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ConferencesView);

            var conferencesList = FindViewById<MvxListView>(Resource.Id.conferencesList);
            conferencesList.FastScrollEnabled = true;

            var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel>();
            set.Apply();
        }
    }
}