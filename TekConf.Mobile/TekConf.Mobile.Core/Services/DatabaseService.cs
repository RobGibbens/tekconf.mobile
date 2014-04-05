using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite.Net.Async;
using System.Linq;

namespace TekConf.Mobile.Core
{
	public class DatabaseService : IDatabaseService
	{
		private readonly SQLiteAsyncConnection _sqLiteConnection;

		public DatabaseService (SQLiteAsyncConnection sqLiteConnection)
		{
			_sqLiteConnection = sqLiteConnection;
			TaskEx.Run (() => CreateDatabase ()).Wait ();
		}

		public async Task<List<Conference>> LoadConferencesAsync ()
		{
			var newDate = DateTime.Now.AddDays (-3);
			//var scheduledConferences = await _sqLiteConnection.Table<Conference> ().Where (x => x.End < newDate).Where(x => x.IsAddedToSchedule).OrderBy (x => x.Start).ToListAsync ();
			var conferences = await _sqLiteConnection.Table<Conference> ().Where (x => x.End >= newDate).OrderBy (x => x.Start).ToListAsync ();
			//return conferences.Union(scheduledConferences).ToList();
			return conferences;
		}

		public async Task<List<Conference>> LoadScheduledConferencesAsync ()
		{
			var conferences = await _sqLiteConnection.Table<Conference> ().Where (x => x.IsAddedToSchedule).OrderByDescending (x => x.Start).ToListAsync ();
			return conferences;
		}

		public async Task DeleteAllConferencesAsync ()
		{
			await _sqLiteConnection.DeleteAllAsync<Session> ();
			await _sqLiteConnection.DeleteAllAsync<Conference> ();
		}

		public async Task DeleteAllScheduledConferencesAsync ()
		{
			var scheduledConferences = await LoadScheduledConferencesAsync ();
			foreach (var conference in scheduledConferences) {
				await _sqLiteConnection.DeleteAsync (conference);
			}
		}

		public async Task SaveConferenceAsync (Conference conference)
		{
			var existingConference = await LoadConferenceAsync (conference.Slug);
			if (existingConference == null) {
				await _sqLiteConnection.InsertAsync (conference);
			} else {
				conference.Id = existingConference.Id;
				await _sqLiteConnection.UpdateAsync (conference);
			}
		}

		public async Task SaveAllConferencesAsync (List<Conference> conferences)
		{
			foreach (var conference in conferences)
			{
				await SaveConferenceAsync (conference);
			}
		}

		public async Task SaveSessionAsync (Session session)
		{
			var existingSession = await _sqLiteConnection.Table<Session> ()
							.Where (s => s.ConferenceId == session.ConferenceId)
							.Where (s => s.Slug == session.Slug)
							.FirstOrDefaultAsync ();

			if (existingSession == null) {
				await _sqLiteConnection.InsertAsync (session);
			} else {
				session.Id = existingSession.Id;
				await _sqLiteConnection.UpdateAsync (session);
			}
		}

		public async Task SaveSpeakerAsync (Speaker speaker)
		{
			var existingSpeaker = await _sqLiteConnection.Table<Speaker> ()
				.Where (s => s.Slug == speaker.Slug)
				.Where (s => s.SessionId == speaker.SessionId)
				.FirstOrDefaultAsync ();

			if (existingSpeaker == null) {
				await _sqLiteConnection.InsertAsync (speaker);
			} else {
				speaker.Id = existingSpeaker.Id;
				await _sqLiteConnection.UpdateAsync (speaker);
			}
		}

		public async Task<Conference> LoadConferenceAsync (int conferenceId)
		{
			var conference = await _sqLiteConnection.Table<Conference> ()
														.Where (c => c.Id == conferenceId)
														.FirstOrDefaultAsync ();
			return conference;
		}

		public async Task<Conference> LoadConferenceAsync (string slug)
		{
			var conference = await _sqLiteConnection.Table<Conference> ()
				.Where (c => c.Slug == slug)
														.FirstOrDefaultAsync ();
			return conference;
		}

		public async Task<Session> LoadSessionAsync (int sessionId)
		{
			var session = await _sqLiteConnection.Table<Session> ()
				.Where (s => s.Id == sessionId)
				.FirstOrDefaultAsync ();

			return session;
		}

		public async Task<Session> LoadSessionAsync (string sessionSlug)
		{
			var session = await _sqLiteConnection.Table<Session> ()
				.Where (s => s.Slug == sessionSlug)
				.FirstOrDefaultAsync ();

			return session;
		}

		public async Task<List<Session>> LoadSessionsAsync (int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session> ()
															.Where (s => s.ConferenceId == conferenceId)
															.OrderBy (s => s.Start)
															.ToListAsync ();

			var sdsd = sessions.Where (x => x.IsAddedToSchedule).ToList ();
			return sessions;
		}

		public async Task<List<Speaker>> LoadSpeakersAsync (int sessionId)
		{
			var sessions = await _sqLiteConnection.Table<Speaker> ()
				.Where (s => s.SessionId == sessionId)
				.OrderBy (s => s.LastName)
				.ToListAsync ();
			return sessions;
		}

		public async Task<List<Session>> SearchSessionsAsync (int conferenceId, string query)
		{
			var sessions = await _sqLiteConnection.Table<Session> ()
				.Where (s => s.ConferenceId == conferenceId)
				.Where (s => 
						s.Description.Contains (query)
			               || s.Room.Contains (query)
			               || s.Title.Contains (query)
			               )
				.OrderBy (s => s.Start)
				.ToListAsync ();

			return sessions;
		}

		public async Task<List<Session>> SearchScheduledSessionsAsync (int conferenceId, string query)
		{
			var sessions = await _sqLiteConnection.Table<Session> ()
				.Where (s => s.ConferenceId == conferenceId)
				.Where (s => 
					s.Description.Contains (query)
			               || s.Room.Contains (query)
			               || s.Title.Contains (query)
			               )
				.OrderBy (s => s.Start)
				.ToListAsync ();

			return sessions;
		}

		public async Task<List<Conference>> SearchConferencesAsync (string query)
		{
			if (string.IsNullOrWhiteSpace (query)) {
				return await LoadConferencesAsync ();
			}

			var conferences = await _sqLiteConnection.Table<Conference> ()
				.Where (c => 
					c.Description.Contains (query)
			                  || c.City.Contains (query)
			                  || c.Name.Contains (query)
			                  || c.State.Contains (query)
			                  ).ToListAsync ();

			return conferences;
		}

		public async Task<List<Conference>> SearchScheduledConferences (string query)
		{
			if (string.IsNullOrWhiteSpace (query)) {
				//return await LoadScheduledConferencesFromLocalAsync ();
			}

			var conferences = await _sqLiteConnection.Table<Conference> ()
				.Where (x => x.IsAddedToSchedule)
				.Where (c => 
					c.Description.Contains (query)
			                  || c.City.Contains (query)
			                  || c.Name.Contains (query)
			                  || c.State.Contains (query)
			                  ).ToListAsync ();

			return conferences;
		}

		public async Task<List<Session>> LoadFavoriteSessionsAsync (int conferenceId)
		{
			var sessions = await _sqLiteConnection.Table<Session> ()
																.Where (s => s.ConferenceId == conferenceId)
																.Where (s => s.IsAddedToSchedule)
																.OrderBy (s => s.Start)
																.ToListAsync ();

			return sessions;
		}

		public async Task SaveCurrentUserAsync (User user)
		{
			await _sqLiteConnection.InsertAsync (user);
		}

		public async Task<User> LoadCurrentUserAsync ()
		{
			var user = await _sqLiteConnection.Table<User> ().FirstOrDefaultAsync ();

			return user;
		}

		public async Task CreateDatabase ()
		{
			await _sqLiteConnection.CreateTableAsync<Conference> ();
			await _sqLiteConnection.CreateTableAsync<Session> ();
			await _sqLiteConnection.CreateTableAsync<Speaker> ();
			await _sqLiteConnection.CreateTableAsync<User> ();

			//Task.WaitAll(conferenceTask, sessionTask, scheduledConferenceTask);
		}
	}
}
