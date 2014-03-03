using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
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
		}

		private MvxCommand _loginWithTwitterCommand;
		public ICommand LoginWithTwitterCommand
		{
			get
			{

				_loginWithTwitterCommand = _loginWithTwitterCommand ?? new MvxCommand(async () => await Login(MobileServiceAuthenticationProvider.Twitter));
				return _loginWithTwitterCommand;
			}
		}


		private MvxCommand _loginWithFacebookCommand;
		public ICommand LoginWithFacebookCommand
		{
			get
			{

				_loginWithFacebookCommand = _loginWithFacebookCommand ?? new MvxCommand(async () => await Login(MobileServiceAuthenticationProvider.Facebook));
				return _loginWithFacebookCommand;
			}
		}
		private async Task Login(MobileServiceAuthenticationProvider provider)
		{
			await _authenticationService.Login(provider);
		}

	}
}