using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;

namespace TekConf.Mobile.Droid.Code
{
	public static class Extensions
	{
		public static MobileServiceAuthenticationProvider AsMobileServiceProvider(this Core.Services.MobileServiceAuthenticationProvider provider)
		{
			return (MobileServiceAuthenticationProvider) ((int) provider);
		}
	}
}
