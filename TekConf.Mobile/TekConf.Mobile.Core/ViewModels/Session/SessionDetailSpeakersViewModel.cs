using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SessionDetailSpeakersViewModel : BaseSubTabViewModel
	{
		private readonly IDatabaseService _databaseService;
		private int _sessionId;

		public SessionDetailSpeakersViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		public async void Init(int id)
		{
			_sessionId = id;

			await LoadSessionSpeakersAsync(LoadRequest.Load);
		}

		public async Task LoadSessionSpeakersAsync(LoadRequest loadRequest)
		{
			this.AreSpeakersLoading = true;

			var sessions = await _databaseService.LoadSpeakersAsync(_sessionId);

			this.Speakers = sessions;

			this.AreSpeakersLoading = false;
		}

		public async Task SearchAsync(string query)
		{
			//var speakers = await _databaseService.SearchSpeakersAsync (_sessionId, query);
			//this.Speakers = speakers;
		}

		public async Task RefreshAsync()
		{
			this.Speakers = Enumerable.Empty<Speaker>();

			await LoadSessionSpeakersAsync(LoadRequest.Refresh);
		}

		private IList<Speaker> _speakers;
		public IEnumerable<Speaker> Speakers
		{
			get
			{
				return _speakers;
			}
			set
			{
				if (_speakers != value)
				{
					_speakers = value.ToList();
					RaisePropertyChanged(() => Speakers);
				}
			}
		}

		private bool _areSessionsLoading;
		public bool AreSpeakersLoading
		{
			get { return _areSessionsLoading; }
			set
			{
				if (_areSessionsLoading != value)
				{
					_areSessionsLoading = value;
					RaisePropertyChanged(() => AreSpeakersLoading);
				}
			}
		}
	}
}