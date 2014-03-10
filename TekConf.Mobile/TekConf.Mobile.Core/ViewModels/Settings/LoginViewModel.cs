using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Mobile.Core.Models;
using TekConf.Mobile.Core.Services;
using System.Net.Http;
using System.Threading;
using System;
using Cirrious.CrossCore.Platform;

namespace TekConf.Mobile.Core.ViewModels
{
	public class LoginViewModel : BaseSubTabViewModel
	{
		private readonly IAuthenticationService _authenticationService;

		private HttpClient _httpClient;

		private IMvxJsonConverter _jsonConverter;

		public LoginViewModel(IAuthenticationService authenticationService, IMvxJsonConverter jsonConverter, HttpClient httpClient)
		{
			_jsonConverter = jsonConverter;
			_authenticationService = authenticationService;
			_httpClient = httpClient;
		}

		public void Init()
		{
			this.LoginProviders = new List<LoginProvider>
			{
				new LoginProvider { ProviderType = MobileServiceAuthenticationProvider.Twitter, ImageName = "twitter.png" },
				new LoginProvider { ProviderType = MobileServiceAuthenticationProvider.Facebook, ImageName = "facebookLogin.png"},
				new LoginProvider { ProviderType = MobileServiceAuthenticationProvider.Google, ImageName = "Google.png"},
				new LoginProvider { ProviderType = MobileServiceAuthenticationProvider.MicrosoftAccount, ImageName = "Microsoft.png"},
			};
		}

		private IList<LoginProvider> _loginProviders;
		public IEnumerable<LoginProvider> LoginProviders
		{
			get
			{
				return _loginProviders;
			}
			set
			{
				if (_loginProviders != value)
				{
					_loginProviders = value.ToList();
					RaisePropertyChanged(() => LoginProviders);
				}
			}
		}

		private MvxCommand<LoginProvider> _loginCommand;
		public ICommand LoginCommand
		{
			get
			{
				_loginCommand = _loginCommand ?? new MvxCommand<LoginProvider>(async (provider) => await Login(provider.ProviderType));
				return _loginCommand;
			}
		}

		private async Task Login(MobileServiceAuthenticationProvider provider)
		{
			var user = await _authenticationService.Login(provider);
			var userName = await GetIsOauthUserRegistered (user.UserId);
		}


		private async Task<string> GetIsOauthUserRegistered(string providerId)
		{
			string tekConfName = "";
			try
			{
				//var token = new CancellationToken();

				string providerName = "";
				string userName = "";
				if (providerId.ToLower().Contains("twitter"))
				{
					providerName = "twitter";
					userName = providerId.ToLower().Replace("twitter:", "");
				}
				else if (providerId.ToLower().Contains("facebook"))
				{
					providerName = "facebook";
					userName = providerId.ToLower().Replace("facebook:", "");
				}
				else if (providerId.ToLower().Contains("google"))
				{
					providerName = "google";
					userName = providerId.ToLower().Replace("google:", "");
				}

				var uri = string.Format(TekConfWeb.BaseUrl + "/account/IsOAuthUserRegistered?providerName={0}&userId={1}", providerName, userName);

				var json = await _httpClient.GetStringAsync(uri);
				var tekConfUser = await TaskEx.Run(() => _jsonConverter.DeserializeObject<TekConfUser>(json) );
				tekConfName = tekConfUser.UserName;
			}
			catch (Exception exception)
			{

			}

			return tekConfName;
		}



	}
}