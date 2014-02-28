using System;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SessionDetailViewModel : BaseSubTabViewModel
	{
		private int _sessionId;

		IDatabaseService _databaseService;

		public SessionDetailViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
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