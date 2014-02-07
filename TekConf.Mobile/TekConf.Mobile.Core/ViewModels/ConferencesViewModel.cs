using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;

namespace TekConf.Mobile.Core.ViewModels
{

    public class ConferencesViewModel
        : MvxViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMvxJsonConverter _jsonConverter;
        private IList<Conference> _conferences;


        public ConferencesViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter)
        {
            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
        }

        public async void Init()
        {
            await LoadConferences();

        }
        public async Task LoadConferences()
        {
            const string url = TekConfApi.BaseUrl + "/conferences";

            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

            var result = await response.Content.ReadAsStreamAsync();
            var conferences = await DeserializeConferenceList(result);

            this.Conferences = conferences;
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

        public IList<Conference> Conferences
        {
            get
            {
                return _conferences;
            }
            set
            {
                if (_conferences != value)
                {
                    _conferences = value;
                    RaisePropertyChanged(() => Conferences);
                }
            }
        }
    }
}
