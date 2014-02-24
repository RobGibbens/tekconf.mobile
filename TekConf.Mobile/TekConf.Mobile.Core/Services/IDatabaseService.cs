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
		Task<List<Session>> SearchScheduledSessionsAsync (int id, string query);
		Task<List<Session>> SearchSessionsAsync (int id, string query);

		Task<List<Conference>> SearchConferencesAsync (string query);
		Task<List<ScheduledConference>> SearchScheduledConferences (string query);
	}
}