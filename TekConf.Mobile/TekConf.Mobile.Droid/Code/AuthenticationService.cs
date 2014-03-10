using System;
using System.Threading.Tasks;
using Android.App;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Microsoft.WindowsAzure.MobileServices;
using TekConf.Mobile.Core.Services;
using System.Threading;

namespace TekConf.Mobile.Droid.Code
{
	public class AuthenticationService : IAuthenticationService
	{
		private MobileServiceClient _client;
		private MobileServiceUser _user;
		private IMvxAndroidCurrentTopActivity _currentActivity;
		private Core.Services.MobileServiceAuthenticationProvider _provider;

		public async Task<MobileUser> Login(Core.Services.MobileServiceAuthenticationProvider provider)
		{
			_provider = provider;
			_currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();

			CurrentPlatform.Init();

			_client = new MobileServiceClient(
				"https://tekconfauth.azure-mobile.net/",
				"NeMPYjchPdsFKlUqDdyAJYZtdrOPiJ11");

			await Authenticate();
			return new MobileUser { Token = _user.MobileServiceAuthenticationToken, UserId = _user.UserId };
		}

		void CreateAndShowDialog(Exception exception, String title)
		{
			CreateAndShowDialog(exception.Message, title);
		}

		void CreateAndShowDialog(string message, string title)
		{
			var builder = new AlertDialog.Builder(_currentActivity.Activity);

			builder.SetMessage(message);
			builder.SetTitle(title);
			builder.Create().Show();
		}

		private async Task Authenticate()
		{
			try
			{
				_user = await _client.LoginAsync(_currentActivity.Activity, _provider.AsMobileServiceProvider());


				//CreateAndShowDialog(string.Format("you are now logged in - {0}", _user.UserId), "Logged in!");
			}
			catch (Exception ex)
			{
				CreateAndShowDialog(ex, "Authentication failed");
			}
		}









	}
}