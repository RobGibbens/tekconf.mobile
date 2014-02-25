using System;
using Android.Content;
using Android.App;
using AndroidHUD;

namespace TekConf.Mobile.Droid
{
	public class BindableProgress
	{

		private Activity _parent;

		public BindableProgress(Activity parent)
		{
			_parent = parent;
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
					AndHUD.Shared.Show(_parent, "Loading", -1, MaskType.Clear);
				}
				else
				{
					AndHUD.Shared.Dismiss ();
				}
			}
		}

	}
}

