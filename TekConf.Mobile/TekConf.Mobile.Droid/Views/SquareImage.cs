using System;
using Android.Widget;
using Android.Content;
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