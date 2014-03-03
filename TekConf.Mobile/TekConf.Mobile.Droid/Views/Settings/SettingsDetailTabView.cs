using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Fragging;
using Android.OS;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.Droid.Views
{
	[Activity]
	public class SettingsDetailTabView : MvxTabsFragmentActivity
	{
		public SettingsDetailTabViewModel SettingsDetailTabViewModel
		{
			get { return (SettingsDetailTabViewModel)base.ViewModel; }
		}

		public SettingsDetailTabView() : base(Resource.Layout.Page_TabView, Resource.Id.actualtabcontent)
		{
		}

		protected override void AddTabs(Bundle args)
		{
			AddTab<LoginFragment>("LoginTab", "Login", args, SettingsDetailTabViewModel.LoginViewModel);
		}
	}
}

