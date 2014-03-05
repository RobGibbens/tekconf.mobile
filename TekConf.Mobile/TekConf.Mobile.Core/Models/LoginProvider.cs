using TekConf.Mobile.Core.Services;

namespace TekConf.Mobile.Core.Models
{
	public class LoginProvider
	{
		public MobileServiceAuthenticationProvider ProviderType { get; set; }
		public string ImageName { get; set; }
	}
}