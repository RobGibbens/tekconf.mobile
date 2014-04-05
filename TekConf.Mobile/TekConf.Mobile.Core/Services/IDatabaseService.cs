using System.Threading.Tasks;
using System.Collections.Generic;

namespace TekConf.Mobile.Core
{
	public interface IDatabaseService
	{
		Task<List<Conference>> LoadConferencesAsync();
		Task<List<Conference>> LoadScheduledConferencesAsync();
		Task<Conference> LoadConferenceAsync(int conferenceId);
		Task<Conference> LoadConferenceAsync(string slug);
		Task<Session> LoadSessionAsync(int sessionId);
		Task<Session> LoadSessionAsync(string slug);
		Task<List<Session>> LoadSessionsAsync(int conferenceId);
		Task<List<Session>> LoadFavoriteSessionsAsync(int conferenceId);

		Task<List<Speaker>> LoadSpeakersAsync (int sessionId);

		Task DeleteAllConferencesAsync();
		Task DeleteAllScheduledConferencesAsync();

		Task SaveAllConferencesAsync (List<Conference> conferences);

		Task SaveConferenceAsync (Conference conference);
		Task SaveSessionAsync(Session session);
		Task SaveSpeakerAsync(Speaker speaker);

		Task<List<Session>> SearchScheduledSessionsAsync (int id, string query);
		Task<List<Session>> SearchSessionsAsync (int id, string query);
		Task<List<Conference>> SearchConferencesAsync (string query);
		Task<List<Conference>> SearchScheduledConferences (string query);

		Task SaveCurrentUserAsync (User user);
		Task<User> LoadCurrentUserAsync ();
	}
}