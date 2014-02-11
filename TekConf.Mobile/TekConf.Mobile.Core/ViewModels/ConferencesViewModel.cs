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
	public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class ConferencesViewModel : MvxViewModel
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
        }

		protected virtual void OnChanged(EventArgs e) 
		{
			if (Changed != null)
				Changed(this, e);
		}

        public async void Init()
		{
			this.Conferences = Enumerable.Empty<Conference> ();
            CreateDatabase();
			await LoadConferences(LoadRequest.Load);
        }

		public async Task Refresh()
		{
			this.Conferences = Enumerable.Empty<Conference> ();
			CreateDatabase();
			await LoadConferences(LoadRequest.Refresh);
		}

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

		public async Task LoadConferences(LoadRequest loadRequest)
        {
			this.AreConferencesLoading = true;
            
            List<Conference> conferences = await LoadConferencesFromLocal();
			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
            {
                conferences = await LoadConferencesFromRemote();
            }

            this.Conferences = conferences;
            
			this.AreConferencesLoading = false;
			OnChanged(EventArgs.Empty);
        }

        private async Task<List<Conference>> LoadConferencesFromLocal()
        {
            var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();

            return conferences;
        }

        private async Task<List<Conference>> LoadConferencesFromRemote()
        {
            const string url = TekConfApi.BaseUrl + "/conferences";
         
            var deleteTask = _sqLiteConnection.DeleteAllAsync<Conference>();
            var httpCallTask = _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

            Task.WaitAll(deleteTask, httpCallTask);
            
            var response = httpCallTask.Result;

            var result = await response.Content.ReadAsStreamAsync();
            var conferences = await DeserializeConferenceList(result);
            foreach (var conference in conferences)
            {
                if (string.IsNullOrWhiteSpace(conference.ImageUrlSquare))
                {
                    conference.ImageUrlSquare = conference.ImageUrl;
                }
            }
            await _sqLiteConnection.InsertAllAsync(conferences);

            return conferences;
        }

        private Task<List<Conference>> DeserializeConferenceList(Stream result)
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
        private bool _areConferencesLoading;

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
