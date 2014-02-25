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

		public async Task<List<Conference>> LoadConferencesAsync()
		{
			var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();
			return conferences;
		}

		public async Task<List<ScheduledConference>> LoadScheduledConferencesAsync()
		{
			var conferences = await _sqLiteConnection.Table<ScheduledConference>().OrderByDescending(x => x.Start).ToListAsync();
			return conferences;
		}

		public async Task DeleteAllConferencesAsync ()
		{
			await _sqLiteConnection.DeleteAllAsync<Session> ();
			await _sqLiteConnection.DeleteAllAsync<Conference>();
		}

		public async Task DeleteAllScheduledConferencesAsync ()
		{
			await _sqLiteConnection.DeleteAllAsync<ScheduledConference> ();
		}

		public async Task SaveConferenceAsync (Conference conference)
		{
			await _sqLiteConnection.InsertAsync (conference);
		}

		public async Task SaveScheduledConferenceAsync (ScheduledConference conference)
		{
			await _sqLiteConnection.InsertAsync (conference);
		}

		public async Task SaveAllConferencesAsync (List<Conference> conferences)
		{
			await _sqLiteConnection.InsertAllAsync(conferences);
		}

		public async Task SaveAllScheduledConferencesAsync (List<ScheduledConference> conferences)
		{
			await _sqLiteConnection.InsertAllAsync(conferences);
		}

		public async Task SaveSessionAsync (Session session)
		{
			await _sqLiteConnection.InsertAsync(session);
		}

		public async Task<Conference> LoadConferenceAsync(int conferenceId)
		{
			var conference = await _sqLiteConnection.Table<Conference>()
														.Where(c => c.Id == conferenceId)
														.FirstOrDefaultAsync();
			return conference;
		}

		public async Task<List<Session>> LoadSessionsAsync(int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session>()
															.Where(s => s.ConferenceId == conferenceId)
															.OrderBy(s => s.Start)
															.ToListAsync();
			return sessions;
		}

		public async Task<List<Session>> SearchSessionsAsync (int conferenceId, string query)
		{
			var sessions = await _sqLiteConnection.Table<Session>()
				.Where(s => s.ConferenceId == conferenceId)
				.Where(s => 
						s.Description.Contains(query)
						|| s.Room.Contains(query)
						|| s.Title.Contains(query)
				)
				.OrderBy(s => s.Start)
				.ToListAsync();

			return sessions;
		}

		public async Task<List<Session>> SearchScheduledSessionsAsync (int conferenceId, string query)
		{
			var sessions = await _sqLiteConnection.Table<Session>()
				.Where(s => s.ConferenceId == conferenceId)
				.Where(s => 
					s.Description.Contains(query)
					|| s.Room.Contains(query)
					|| s.Title.Contains(query)
				)
				.OrderBy(s => s.Start)
				.ToListAsync();

			return sessions;
		}

		public async Task<List<Conference>> SearchConferencesAsync (string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return await LoadConferencesAsync ();
			}

			var conferences = await _sqLiteConnection.Table<Conference>()
				.Where(c => 
					c.Description.Contains(query) 
					|| c.City.Contains(query)
					|| c.Name.Contains(query)
					|| c.State.Contains(query)
				).ToListAsync();

			return conferences;
		}

		public async Task<List<ScheduledConference>> SearchScheduledConferences (string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				//return await LoadScheduledConferencesFromLocalAsync ();
			}

			var conferences = await _sqLiteConnection.Table<ScheduledConference>()
				.Where(c => 
					c.Description.Contains(query) 
					|| c.City.Contains(query)
					|| c.Name.Contains(query)
					|| c.State.Contains(query)
				).ToListAsync();

			return conferences;
		}

		public async Task<List<Session>> LoadFavoriteSessionsAsync(int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session>()
																.Where(s => s.ConferenceId == conferenceId)
																.Where(s => s.IsAddedToSchedule)
																.OrderBy(s => s.Start)
																.ToListAsync();

			return sessions;
		}

		public void CreateDatabase()
		{
			var conferenceTask = _sqLiteConnection.CreateTableAsync<Conference>();
			var sessionTask = _sqLiteConnection.CreateTableAsync<Session>();
			var scheduledConferenceTask = _sqLiteConnection.CreateTableAsync<ScheduledConference> ();

			Task.WaitAll(conferenceTask, sessionTask, scheduledConferenceTask);
		}
	}

}
