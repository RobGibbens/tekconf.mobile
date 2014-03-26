using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Linq;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SessionDetailViewModel : BaseSubTabViewModel
	{
		private int _sessionId;

		IDatabaseService _databaseService;

		IRemoteConferenceService _remoteConferenceService;

		public SessionDetailViewModel(IDatabaseService databaseService, IRemoteConferenceService remoteConferenceService)
		{
			_databaseService = databaseService;
			_remoteConferenceService = remoteConferenceService;
		}

		public async void Init(int id)
		{
			_sessionId = id;
			await LoadSessionAsync();
		}

		private bool _isSessionLoading;
		public bool IsSessionLoading
		{
			get { return _isSessionLoading; }
			set
			{
				if (_isSessionLoading != value)
				{
					_isSessionLoading = value;
					RaisePropertyChanged(() => IsSessionLoading);
				}
			}
		}

		private async Task LoadSessionAsync()
		{
			this.IsSessionLoading = true;

			Session session = await LoadSessionFromLocalAsync();

			this.Session = session;

			this.IsSessionLoading = false;
		}

		private async Task<Session> LoadSessionFromLocalAsync()
		{
			var session = await _databaseService.LoadSessionAsync (_sessionId);

			return session;
		}

		public async Task ToggleFavoriteAsync()
		{
			var existingConference = await _databaseService.LoadConferenceAsync (this.Session.ConferenceId);
			var existingScheduledConference = await _databaseService.LoadScheduledConferenceAsync (existingConference.Name);
			if (existingScheduledConference == null)
			{
				existingScheduledConference = new ScheduledConference (existingConference);
				await _databaseService.SaveScheduledConferenceAsync (existingScheduledConference);
			}

			var existingSessions = await _databaseService.LoadFavoriteSessionsAsync (existingScheduledConference.Id);
			if (existingSessions == null)
			{
				existingSessions = new List<Session> ();
			}

			var session = existingSessions.FirstOrDefault (s => s.Title == this.Session.Title);
			if (session == null)
			{
				this.Session.IsAddedToSchedule = true;
				var user = await _databaseService.LoadCurrentUserAsync ();
				if (user != null) {
					var userName = user.UserName;
					var conferenceSlug = existingScheduledConference.Slug;
					var sessionSlug = this.Session.Slug;
					await _remoteConferenceService.AddSessionToScheduleAsync (userName, conferenceSlug, sessionSlug);
				}
			}
			else
			{
				this.Session.IsAddedToSchedule = false;
			}

			await _databaseService.SaveSessionAsync (this.Session);


		}

		private Session _session;
		public Session Session
		{
			get
			{
				return _session;
			}
			set
			{
				if (_session != value)
				{
					_session = value;
					RaisePropertyChanged(() => Session);
				}
			}
		}
	}
}