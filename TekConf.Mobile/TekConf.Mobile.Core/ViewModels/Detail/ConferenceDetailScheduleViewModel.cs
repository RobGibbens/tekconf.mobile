using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferenceDetailScheduleViewModel : BaseSubTabViewModel
	{
		private readonly IDatabaseService _databaseService;
		public event ChangedEventHandler SessionsChanged;

		public ConferenceDetailScheduleViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		public async void Init(int id)
		{
			//this.Sessions = Enumerable.Empty<Session>();

			await LoadSessionsAsync(id, LoadRequest.Load);
		}

		protected virtual void OnSessionsChanged(EventArgs e)
		{
			if (SessionsChanged != null)
				SessionsChanged(this, e);
		}
		private async Task LoadSessionsAsync(int id, LoadRequest loadRequest)
		{
			this.AreSessionsLoading = true;

			var sessions = await _databaseService.LoadFavoriteSessionsAsync(id);

			this.Sessions = sessions;

			this.AreSessionsLoading = false;
			OnSessionsChanged(EventArgs.Empty);
		}

		public async Task RefreshAsync(int id)
		{
			//this.Sessions = Enumerable.Empty<Session>();

			await LoadSessionsAsync(id, LoadRequest.Refresh);
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