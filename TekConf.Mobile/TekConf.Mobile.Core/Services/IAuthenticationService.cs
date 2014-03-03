using System.Threading.Tasks;

namespace TekConf.Mobile.Core.Services
{
	public interface IAuthenticationService
	{
		Task Login(MobileServiceAuthenticationProvider provider);
	}
}