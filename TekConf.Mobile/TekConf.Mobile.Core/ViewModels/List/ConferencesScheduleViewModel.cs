using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using SQLite.Net.Async;
using System;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferencesScheduleViewModel : BaseSubTabViewModel
	{
		private readonly HttpClient _httpClient;
		private readonly IMvxJsonConverter _jsonConverter;
		private readonly SQLiteAsyncConnection _sqLiteConnection;
		public event ChangedEventHandler Changed;
		readonly IRemoteConferenceService _conferenceService;
		readonly IDatabaseService _databaseService;

		public ConferencesScheduleViewModel(HttpClient httpClient, 
			IMvxJsonConverter jsonConverter, 
			SQLiteAsyncConnection sqLiteConnection,
			IRemoteConferenceService conferenceService, 
			IDatabaseService databaseService)
		{
			_httpClient = httpClient;
			_jsonConverter = jsonConverter;
			_sqLiteConnection = sqLiteConnection;
			_databaseService = databaseService;
			_conferenceService = conferenceService;

			this.Conferences = Enumerable.Empty<ScheduledConference>();
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}

		public async void Init()
		{
			this.Conferences = Enumerable.Empty<ScheduledConference>();
			CreateDatabase();
			await LoadConferencesAsync(LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
			this.Conferences = Enumerable.Empty<ScheduledConference>();
			CreateDatabase();
			await LoadConferencesAsync(LoadRequest.Refresh);
		}

		public async Task SearchAsync(string query)
		{
			var conferences = await _databaseService.SearchScheduledConferences (query);
			this.Conferences = conferences;
		}

		public async Task SortByDateAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderByDescending(x => x.Start); });
		}

		public async Task SortByNameAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderBy(x => x.Name); });
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
			var conferenceTask = _sqLiteConnection.CreateTableAsync<ScheduledConference>();
			Task.WaitAll(conferenceTask);
		}

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			this.AreConferencesLoading = true;

			List<ScheduledConference> conferences = await LoadConferencesFromLocalAsync();
			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				conferences = await LoadConferencesFromRemoteAsync();
			}

			this.Conferences = conferences;

			this.AreConferencesLoading = false;
			OnChanged(EventArgs.Empty);
		}

		private async Task<List<ScheduledConference>> LoadConferencesFromLocalAsync()
		{
			var conferences = await _sqLiteConnection.Table<ScheduledConference>().OrderByDescending(x => x.Start).ToListAsync();

			return conferences;
		}

		private async Task<List<ScheduledConference>> LoadConferencesFromRemoteAsync()
		{
			string url = string.Format(TekConfApi.BaseUrl + "/conferences/schedules?userName={0}", "robgibbens");

			var deleteTask = _sqLiteConnection.DeleteAllAsync<ScheduledConference>();
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

			return conferences.OrderByDescending(c => c.Start).ToList();
		}

		private Task<List<ScheduledConference>> DeserializeConferenceListAsync(Stream result)
		{
			return Task.Factory.StartNew(() =>
				{
					var reader = new StreamReader(result);
					string json = reader.ReadToEnd();
					var conferences = _jsonConverter.DeserializeObject<List<ScheduledConference>>(json);

					return conferences.OrderByDescending(c => c.Start).ToList();
				});
		}


		private IList<ScheduledConference> _conferences;
		public IEnumerable<ScheduledConference> Conferences
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
