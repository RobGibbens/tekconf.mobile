using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace TekConf.Mobile.Droid.Views
{
	public class Tab1Fragment : MvxFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
																			Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ConferencesView, null);
		}
	}

}