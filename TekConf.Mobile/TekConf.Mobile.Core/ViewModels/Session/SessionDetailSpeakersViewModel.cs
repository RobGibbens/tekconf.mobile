﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SessionDetailSpeakersViewModel : BaseSubTabViewModel
	{
		private readonly IDatabaseService _databaseService;
		private int _conferenceId;

		public SessionDetailSpeakersViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		public async void Init(int id)
		{
			_conferenceId = id;

			await LoadSessionsAsync(LoadRequest.Load);
		}

		public async Task LoadSessionsAsync(LoadRequest loadRequest)
		{
			this.AreSessionsLoading = true;

			var sessions = await _databaseService.LoadSessionsAsync(_conferenceId);

			this.Sessions = sessions;

			this.AreSessionsLoading = false;
		}

		public async Task SearchAsync(string query)
		{
			var sessions = await _databaseService.SearchSessionsAsync (_conferenceId, query);
			this.Sessions = sessions;
		}

		public async Task RefreshAsync()
		{
			this.Sessions = Enumerable.Empty<Session>();

			await LoadSessionsAsync(LoadRequest.Refresh);
		}

		public async Task SortByDateAsync()
		{
			await TaskEx.Run(() => { this.Sessions = this.Sessions.OrderBy(x => x.Start); });
		}

		public async Task SortByTitleAsync()
		{
			await TaskEx.Run(() => { this.Sessions = this.Sessions.OrderBy(x => x.Title); });
		}

		private IList<Session> _sessions;
		public IEnumerable<Session> Sessions
		{
			get
			{
				return _sessions;
			}
			set
			{
				if (_sessions != value)
				{
					_sessions = value.ToList();
					RaisePropertyChanged(() => Sessions);
				}
			}
		}

		private bool _areSessionsLoading;
		public bool AreSessionsLoading
		{
			get { return _areSessionsLoading; }
			set
			{
				if (_areSessionsLoading != value)
				{
					_areSessionsLoading = value;
					RaisePropertyChanged(() => AreSessionsLoading);
				}
			}
		}
	}
}