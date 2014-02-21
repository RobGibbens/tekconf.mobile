using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core
{

	public class DatabaseService : IDatabaseService
	{
		private readonly SQLiteAsyncConnection _sqLiteConnection;

		public DatabaseService (SQLiteAsyncConnection sqLiteConnection)
		{
			_sqLiteConnection = sqLiteConnection;
			CreateDatabase ();
		}

		public async Task<List<Conference>> LoadConferencesFromLocalAsync()
		{
			var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();

			return conferences;
		}

		public async Task DeleteAllConferencesAsync ()
		{
			await _sqLiteConnection.DeleteAllAsync<Session> ();
			await _sqLiteConnection.DeleteAllAsync<Conference>();
		}

		public async Task SaveConferenceAsync (Conference conference)
		{
			await _sqLiteConnection.InsertAsync (conference);
		}

		public async Task SaveAllConferencesAsync (List<Conference> conferences)
		{
			await _sqLiteConnection.InsertAllAsync(conferences);
		}

		public async Task SaveSessionAsync (Session session)
		{
			await _sqLiteConnection.InsertAsync(session);
		}

		public async Task<Conference> LoadConferenceAsync(int conferenceId)
		{
			var conference = await _sqLiteConnection.Table<Conference>().Where(c => c.Id == conferenceId).FirstOrDefaultAsync();

			return conference;
		}

		public async Task<List<Session>> LoadSessionsAsync(int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session>().Where(s => s.ConferenceId == conferenceId).OrderBy(s => s.Start).ToListAsync();

			return sessions;
		}

		public void CreateDatabase()
		{
			var conferenceTask = _sqLiteConnection.CreateTableAsync<Conference>();
			var sessionTask = _sqLiteConnection.CreateTableAsync<Session>();

			Task.WaitAll(conferenceTask, sessionTask);
		}
	}

}
