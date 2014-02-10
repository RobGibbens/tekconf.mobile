using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using TekConf.Mobile.Core;
using System.Linq;
using Android.Graphics.Drawables;
using UrlImageViewHelper;
using Android.Util;
using Android.Runtime;

namespace TekConf.Mobile.Droid
{

	public class SquareImage : ImageView 
	{
		public SquareImage (Context context) : base(context)
		{
		}

		public SquareImage (Context context, IAttributeSet attrs) : base(context, attrs)
		{
			
		}

		public SquareImage (Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{

		}

		public SquareImage (IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension (MeasuredWidth, MeasuredWidth);
		}
	}
}