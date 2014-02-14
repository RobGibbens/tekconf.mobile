using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using SQLite.Net.Async;
using System;

namespace TekConf.Mobile.Core.ViewModels
{
	//public delegate void ChangedEventHandler(object sender, EventArgs e);
	public class ConferencesViewModel : BaseSubTabViewModel
	{
		private readonly HttpClient _httpClient;
		private readonly IMvxJsonConverter _jsonConverter;
		private readonly SQLiteAsyncConnection _sqLiteConnection;
		public event ChangedEventHandler Changed;

		public ConferencesViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter, SQLiteAsyncConnection sqLiteConnection)
		{
			_httpClient = httpClient;
			_jsonConverter = jsonConverter;
			_sqLiteConnection = sqLiteConnection;
			this.Conferences = Enumerable.Empty<Conference>();
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}

		public async void Init()
		{
			this.Conferences = Enumerable.Empty<Conference>();
			CreateDatabase();
			await LoadConferencesAsync(LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
			this.Conferences = Enumerable.Empty<Conference>();
			CreateDatabase();
			await LoadConferencesAsync(LoadRequest.Refresh);
		}

		public async Task SortByDateAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderBy(x => x.Start); });
		}

		public async Task SortByNameAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderBy(x => x.Name); });
		}

		public ICommand ShowDetailCommand
		{
			get
			{
				return new MvxCommand<Conference>(conference => 
					ShowViewModel<ConferenceDetailTabViewModel>(new { id = conference.Id })
				);
			}
		}

		private bool _areConferencesLoading;
		public bool AreConferencesLoading
		{
			get { return _areConferencesLoading; }
			set
			{
				if (_areConferencesLoading != value)
				{
					_areConferencesLoading = value;
					RaisePropertyChanged(() => AreConferencesLoading);
				}
			}
		}

		public void CreateDatabase()
		{
			var conferenceTask = _sqLiteConnection.CreateTableAsync<Conference>();
			Task.WaitAll(conferenceTask);
		}

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			this.AreConferencesLoading = true;

			List<Conference> conferences = await LoadConferencesFromLocalAsync();
			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				conferences = await LoadConferencesFromRemoteAsync();
			}

			this.Conferences = conferences;

			this.AreConferencesLoading = false;
			OnChanged(EventArgs.Empty);
		}

		private async Task<List<Conference>> LoadConferencesFromLocalAsync()
		{
			var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();

			return conferences;
		}

		private async Task<List<Conference>> LoadConferencesFromRemoteAsync()
		{
			const string url = TekConfApi.BaseUrl + "/conferences";

			var deleteTask = _sqLiteConnection.DeleteAllAsync<Conference>();
			var httpCallTask = _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

			Task.WaitAll(deleteTask, httpCallTask);

			var response = httpCallTask.Result;

			var result = await response.Content.ReadAsStreamAsync();
			var conferences = await DeserializeConferenceListAsync(result);
			foreach (var conference in conferences)
			{
				if (string.IsNullOrWhiteSpace(conference.ImageUrlSquare))
				{
					conference.ImageUrlSquare = conference.ImageUrl;
				}
			}
			await _sqLiteConnection.InsertAllAsync(conferences);
			conferences = await _sqLiteConnection.Table<Conference>().ToListAsync();
			return conferences;
		}

		private Task<List<Conference>> DeserializeConferenceListAsync(Stream result)
		{
			return Task.Factory.StartNew(() =>
			{
				var reader = new StreamReader(result);
				string json = reader.ReadToEnd();
					var conferences = _jsonConverter.DeserializeObject<List<Conference>>(json);

				return conferences.OrderBy(c => c.Start).ToList();
			});
		}


		private IList<Conference> _conferences;
		public IEnumerable<Conference> Conferences
		{
			get
			{
				return _conferences;
			}
			set
			{
				if (_conferences != value)
				{
					_conferences = value.ToList();
					RaisePropertyChanged(() => Conferences);
				}
			}
		}
	}
}
