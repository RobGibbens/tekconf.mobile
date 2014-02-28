using System.Threading.Tasks;
using System.Collections.Generic;

namespace TekConf.Mobile.Core
{
	public interface IDatabaseService
	{
		Task<List<Conference>> LoadConferencesAsync();
		Task<List<ScheduledConference>> LoadScheduledConferencesAsync();
		Task<Conference> LoadConferenceAsync(int conferenceId);
		Task<Session> LoadSessionAsync(int sessionId);
		Task<List<Session>> LoadSessionsAsync(int conferenceId);
		Task<List<Session>> LoadFavoriteSessionsAsync(int conferenceId);

		Task DeleteAllConferencesAsync();
		Task DeleteAllScheduledConferencesAsync();

		Task SaveAllConferencesAsync (List<Conference> conferences);
		Task SaveAllScheduledConferencesAsync (List<ScheduledConference> conferences);

		Task SaveConferenceAsync (Conference conference);
		Task SaveScheduledConferenceAsync (ScheduledConference conference);
		Task SaveSessionAsync(Session session);

		Task<List<Session>> SearchScheduledSessionsAsync (int id, string query);
		Task<List<Session>> SearchSessionsAsync (int id, string query);
		Task<List<Conference>> SearchConferencesAsync (string query);
		Task<List<ScheduledConference>> SearchScheduledConferences (string query);
	}
}