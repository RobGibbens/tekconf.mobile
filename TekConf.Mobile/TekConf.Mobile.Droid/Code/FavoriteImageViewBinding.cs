using System;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace TekConf.Mobile.Droid.Code
{
	public class FavoriteImageViewBinding : MvxAndroidTargetBinding
	{
		protected MvxImageView ImageView
		{
			get
			{
				return (MvxImageView)Target;
			}
		}

		private bool _currentValue;

		public FavoriteImageViewBinding(MvxImageView imageView) : base(imageView)
		{
			imageView.Click += ImageViewOnClick;
		}

		private void ImageViewOnClick(object sender, EventArgs eventArgs)
		{
			_currentValue = !_currentValue;
			SetButtonBackground();
			FireValueChanged(_currentValue);
		}

		protected override void SetValueImpl(object target, object value)
		{
			var boolValue = (bool)value;
			_currentValue = boolValue;
			SetButtonBackground();
		}

		private void SetButtonBackground()
		{
			var imageView = ImageView;
			if (imageView == null)
				return;

			if (_currentValue)
			{
				imageView.SetBackgroundResource(Resource.Drawable.check);
			}
			else
			{
				imageView.SetBackgroundResource(Resource.Drawable.staro);
			}
		}

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				var button = ImageView;
				if (button != null)
				{
					button.Click -= ImageViewOnClick;
				}
			}
			base.Dispose(isDisposing);
		}

		public override Type TargetType
		{
			get { return typeof(bool); }
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.TwoWay; }
		}
	}
}