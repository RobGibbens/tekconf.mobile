using System;
using Android.Content;
using Android.App;
using AndroidHUD;

namespace TekConf.Mobile.Droid
{
	public class BindableProgress
	{

		private Activity _parent;
		private string _message;
		public BindableProgress(Activity parent) : this(parent, "Loading")
		{
		}

		public BindableProgress(Activity parent, string message)
		{
			_parent = parent;
			_message = message;
		}

		public bool Visible
		{
			get { 
				return false; //TODO
			}
			set
			{
				if (value)
				{
					AndHUD.Shared.Show(_parent, _message, -1, MaskType.Clear);
				}
				else
				{
					AndHUD.Shared.Dismiss ();
				}
			}
		}

	}
}

