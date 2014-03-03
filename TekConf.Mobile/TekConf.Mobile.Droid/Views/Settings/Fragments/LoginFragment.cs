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
	public class LoginFragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.LoginView, null);
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//HasOptionsMenu = true;
		}
		//public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		//{
		//	inflater.Inflate(Resource.Menu.LoginActionItems, menu);
		//}

		//public override bool OnOptionsItemSelected(IMenuItem item)
		//{
		//	var vm = this.DataContext as LoginViewModel;
		//	if (vm != null)
		//	{
		//		switch (item.ToString())
		//		{
		//			case "Add to Schedule":
		//				//Task.Factory.StartNew(() => vm.RefreshAsync().Wait());
		//				break;
		//		}
		//	}

		//	return false;
		//}
	}
}