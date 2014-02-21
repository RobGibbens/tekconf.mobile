using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core
{
	public interface IDatabaseService
	{
		Task<List<Conference>> LoadConferencesFromLocalAsync();
		Task DeleteAllConferencesAsync();

		Task SaveAllConferencesAsync (List<Conference> conferences);
		Task SaveConferenceAsync (Conference conference);
		Task SaveSessionAsync(Session session);
	}


}
