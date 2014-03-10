using System.Threading.Tasks;

namespace TekConf.Mobile.Core.Services
{
	public interface IAuthenticationService
	{
		Task<MobileUser> Login(MobileServiceAuthenticationProvider provider);
	}
}