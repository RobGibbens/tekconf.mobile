using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using SQLite.Net.Async;
using TekConf.Mobile.Core.Messages;
using System.Threading;
using SQLite;

namespace TekConf.Mobile.Core.ViewModels
{

    public class ConferencesViewModel
        : MvxViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMvxJsonConverter _jsonConverter;
        private readonly SQLiteAsyncConnection _sqLiteConnection;

        public ConferencesViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter, SQLiteAsyncConnection sqLiteConnection)
        {
            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
		    _sqLiteConnection = sqLiteConnection;
        }

        public async void Init()
		{
			this.Conferences = Enumerable.Empty<Conference> ();
            CreateDatabase();
            await LoadConferences();
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
        public async Task LoadConferences()
        {
			this.AreConferencesLoading = true;
            
            List<Conference> conferences = await LoadConferencesFromLocal();
            if (!conferences.Any())
            {
                conferences = await LoadConferencesFromRemote();
            }

            this.Conferences = conferences;
            
			this.AreConferencesLoading = false;
        }

        private async Task<List<Conference>> LoadConferencesFromLocal()
        {
            var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();

            return conferences;
        }
        private async Task<List<Conference>> LoadConferencesFromRemote()
        {
            const string url = TekConfApi.BaseUrl + "/conferences";

            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

            var result = await response.Content.ReadAsStreamAsync();
            var conferences = await DeserializeConferenceList(result);

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
