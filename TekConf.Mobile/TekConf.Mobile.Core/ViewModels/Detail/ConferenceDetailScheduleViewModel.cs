using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferenceDetailScheduleViewModel : BaseSubTabViewModel
	{
		private readonly IDatabaseService _databaseService;

		private int _conferenceId;

		public ConferenceDetailScheduleViewModel(IDatabaseService databaseService)
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

			var sessions = await _databaseService.LoadFavoriteSessionsAsync(_conferenceId);

			this.Sessions = sessions;

			this.AreSessionsLoading = false;
		}

		public async Task RefreshAsync()
		{
			await LoadSessionsAsync(LoadRequest.Refresh);
		}

		public async Task SearchAsync(string query)
		{
			var sessions = await _databaseService.SearchSessionsAsync (_conferenceId, query);
			this.Sessions = sessions;
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


		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand<Conference>(conference => ShowViewModel<SettingsDetailTabViewModel>());
			}
		}
	}
}