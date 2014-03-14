using System.Threading.Tasks;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SettingsDetailViewModel : BaseSubTabViewModel
	{
		IDatabaseService _databaseService;

		public SettingsDetailViewModel(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		public async void Init()
		{
			await LoadUser();
		}

		private async Task LoadUser()
		{
			var user = await _databaseService.LoadCurrentUserAsync();
			if (user == null)
			{
				this.UserName = "Not logged in.";
			}
			else
			{
				this.UserName = "Logged in as " + user.UserName;
			}
		}
		private string _userName;
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				if (_userName != value)
				{
					_userName = value;
					RaisePropertyChanged(() => UserName);
				}
			}
		}
	}
}