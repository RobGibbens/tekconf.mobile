using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Messages
{
	public class ConferencesLoaded : MvxMessage
	{
		public ConferencesLoaded(object sender) : base(sender)
		{
		}
	}
}

