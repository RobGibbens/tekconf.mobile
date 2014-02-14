using System;
using System.Threading.Tasks;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferenceDetailViewModel : BaseSubTabViewModel
	{
		public event ConferenceDetailChangedEventHandler Changed;

		private readonly SQLiteAsyncConnection _sqLiteConnection;
		public ConferenceDetailViewModel(SQLiteAsyncConnection sqLiteConnection)
		{
			_sqLiteConnection = sqLiteConnection;
			
		}
		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}
		public async void Init(int id)
		{
			await LoadConferencesAsync(id);
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

		public async Task LoadConferencesAsync(int id)
		{
			this.IsConferenceLoading = true;

			Conference conference = await LoadConferenceFromLocalAsync(id);

			this.Conference = conference;

			this.IsConferenceLoading = false;
			OnChanged(EventArgs.Empty);
		}


		private async Task<Conference> LoadConferenceFromLocalAsync(int id)
		{
			var conference = await _sqLiteConnection.Table<Conference>().Where(c => c.Id == id).FirstOrDefaultAsync();

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