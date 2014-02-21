using System.Threading.Tasks;
using System.Collections.Generic;

namespace TekConf.Mobile.Core
{
	public interface IDatabaseService
	{
		Task<List<Conference>> LoadConferencesFromLocalAsync();
		Task DeleteAllConferencesAsync();

		Task SaveAllConferencesAsync (List<Conference> conferences);
		Task SaveConferenceAsync (Conference conference);
		Task SaveSessionAsync(Session session);

		Task<Conference> LoadConferenceAsync(int conferenceId);
		Task<List<Session>> LoadSessionsAsync(int conferenceId);
		Task<List<Session>> LoadFavoriteSessionsAsync(int conferenceId);
	}
}