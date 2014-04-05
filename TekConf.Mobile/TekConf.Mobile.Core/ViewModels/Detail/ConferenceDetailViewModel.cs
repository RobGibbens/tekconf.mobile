using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;

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

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			this.IsConferenceLoading = true;

			var conference = await LoadConferenceFromLocalAsync();

			this.Conference = conference;

			this.IsConferenceLoading = false;
		}
			
		private async Task<Conference> LoadConferenceFromLocalAsync()
		{
			var conference = await _databaseService.LoadConferenceAsync (_conferenceId);

			return conference;
		}

		public async Task RefreshAsync()
		{
			await LoadConferencesAsync(LoadRequest.Refresh);
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

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand<Conference>(conference => ShowViewModel<SettingsDetailTabViewModel>());
			}
		}
	}
}