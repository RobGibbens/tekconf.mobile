using System;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferenceDetailViewModel : BaseSubTabViewModel
	{
		private int _conferenceId;

		IDatabaseService _databaseService;

		public ConferenceDetailViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		public async void Init(int id)
		{
			_conferenceId = id;
			await LoadConferencesAsync();
		}

		private bool _isConferenceLoading;
		public bool IsConferenceLoading
		{
			get { return _isConferenceLoading; }
			set
			{
				if (_isConferenceLoading != value)
				{
					_isConferenceLoading = value;
					RaisePropertyChanged(() => IsConferenceLoading);
				}
			}
		}

		public async Task LoadConferencesAsync()
		{
			this.IsConferenceLoading = true;

			Conference conference = await LoadConferenceFromLocalAsync();

			this.Conference = conference;

			this.IsConferenceLoading = false;
		}
			
		private async Task<Conference> LoadConferenceFromLocalAsync()
		{
			var conference = await _databaseService.LoadConferenceAsync (_conferenceId);

			return conference;
		}

		private Conference _conference;
		public Conference Conference
		{
			get
			{
				return _conference;
			}
			set
			{
				if (_conference != value)
				{
					_conference = value;
					RaisePropertyChanged(() => Conference);
				}
			}
		}
	}
}