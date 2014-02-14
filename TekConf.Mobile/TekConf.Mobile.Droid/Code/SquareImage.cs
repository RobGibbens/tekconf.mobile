using Android.Content;
using Android.Util;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace TekConf.Mobile.Droid
{

	public class SquareImage : MvxImageView 
	{
		public SquareImage (Context context): base(context)
		{
		}

		public SquareImage (Context context, IAttributeSet attrs) : base(context, attrs)
		{
			
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension (MeasuredWidth, MeasuredWidth);
		}
	}
}