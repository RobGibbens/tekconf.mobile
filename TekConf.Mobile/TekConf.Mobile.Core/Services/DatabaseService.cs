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
			TaskEx.Run (() => CreateDatabase ()).Wait();
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

		public async Task SaveScheduledConferenceAsync (ScheduledConference scheduledConference)
		{
			var id = await _sqLiteConnection.InsertAsync (scheduledConference);
		}

		public async Task SaveAllConferencesAsync (List<Conference> conferences)
		{
			await _sqLiteConnection.InsertAllAsync(conferences);
		}

		public async Task SaveAllScheduledConferencesAsync (List<ScheduledConference> scheduledConferences)
		{
			await _sqLiteConnection.InsertAllAsync(scheduledConferences);
		}

		public async Task SaveSessionAsync (Session session)
		{
			if (session.Id != 0)
			{
				var existingSession = await _sqLiteConnection.Table<Session> ().Where (s => s.Id == session.Id).FirstOrDefaultAsync();
				if (existingSession == null) {
					await _sqLiteConnection.InsertAsync (session);
				} else {
					await _sqLiteConnection.UpdateAsync (session);
				}
			}
			else {
				await _sqLiteConnection.InsertAsync (session);
			}
		}

		public async Task SaveSpeakerAsync (Speaker speaker)
		{
			await _sqLiteConnection.InsertAsync(speaker);
		}

		public async Task<Conference> LoadConferenceAsync(int conferenceId)
		{
			var conference = await _sqLiteConnection.Table<Conference>()
														.Where(c => c.Id == conferenceId)
														.FirstOrDefaultAsync();
			return conference;
		}

		public async Task<Conference> LoadConferenceAsync(string name)
		{
			var conference = await _sqLiteConnection.Table<Conference>()
														.Where(c => c.Name == name)
														.FirstOrDefaultAsync();
			return conference;
		}

		public async Task<ScheduledConference> LoadScheduledConferenceAsync(string name)
		{
			var conference = await _sqLiteConnection.Table<ScheduledConference>()
														.Where(c => c.Name == name)
														.FirstOrDefaultAsync();
			return conference;
		}

		public async Task<Session> LoadSessionAsync(int sessionId)
		{
			var session = await _sqLiteConnection.Table<Session>()
				.Where(s => s.Id == sessionId)
				.FirstOrDefaultAsync();

			return session;
		}

		public async Task<List<Session>> LoadSessionsAsync(int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session>()
															.Where(s => s.ConferenceId == conferenceId)
															.OrderBy(s => s.Start)
															.ToListAsync();
			return sessions;
		}

		public async Task<List<Speaker>> LoadSpeakersAsync(int sessionId)
		{
			var sessions = await _sqLiteConnection.Table<Speaker>()
				.Where(s => s.SessionId == sessionId)
				.OrderBy(s => s.LastName)
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

		public async Task SaveCurrentUserAsync (User user)
		{
			await _sqLiteConnection.InsertAsync(user);
		}

		public async Task<User> LoadCurrentUserAsync ()
		{
			var user = await _sqLiteConnection.Table<User> ().FirstOrDefaultAsync ();

			return user;
		}

		public async Task CreateDatabase()
		{
			await _sqLiteConnection.CreateTableAsync<Conference>();
			await _sqLiteConnection.CreateTableAsync<Session>();
			await _sqLiteConnection.CreateTableAsync<ScheduledConference> ();
			await _sqLiteConnection.CreateTableAsync<Speaker> ();
			await _sqLiteConnection.CreateTableAsync<User>();

			//Task.WaitAll(conferenceTask, sessionTask, scheduledConferenceTask);
		}
	}

}
