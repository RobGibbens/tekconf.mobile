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

namespace TekConf.Mobile.Droid
{

	public class TekConfGridView : MvxGridView, AbsListView.IOnScrollListener
	{
		public event Action<int> OverScrolled;
		public event Action OverScrollCanceled;

		public TekConfGridView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			Initialize ();
		}

		public TekConfGridView (Context context, IAttributeSet attrs, IMvxAdapter adapter) : base (context, attrs, adapter)
		{
			Initialize ();
		}

		void Initialize ()
		{
			SetOnScrollListener (this);
		}

		protected override bool OverScrollBy (int deltaX, int deltaY, int scrollX, int scrollY, int scrollRangeX, int scrollRangeY, int maxOverScrollX, int maxOverScrollY, bool isTouchEvent)
		{
			if (OverScrolled != null)
				OverScrolled (deltaY);
			return base.OverScrollBy (deltaX, deltaY, scrollX, scrollY, scrollRangeX, scrollRangeY, maxOverScrollX, maxOverScrollY, isTouchEvent);
		}

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
			if (OverScrollCanceled != null)
				OverScrollCanceled ();
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			if (OverScrollCanceled != null
				&& (scrollState == ScrollState.Idle || scrollState == ScrollState.Fling))
				OverScrollCanceled ();
		}
	}
}