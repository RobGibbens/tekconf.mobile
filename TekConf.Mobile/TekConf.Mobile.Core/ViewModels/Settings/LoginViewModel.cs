using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Mobile.Core.Models;
using TekConf.Mobile.Core.Services;

namespace TekConf.Mobile.Core.ViewModels
{
	public class LoginViewModel : BaseSubTabViewModel
	{
		private readonly IAuthenticationService _authenticationService;

		public LoginViewModel(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
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
			await _authenticationService.Login(provider);
		}

	}
}