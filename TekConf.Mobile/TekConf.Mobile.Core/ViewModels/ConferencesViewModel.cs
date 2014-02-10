using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using System.Threading;

namespace TekConf.Mobile.Core.ViewModels
{

    public class ConferencesViewModel
        : MvxViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMvxJsonConverter _jsonConverter;

		private readonly IMvxMessenger _messenger;

		public ConferencesViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter, IMvxMessenger messenger)
        {
			_messenger = messenger;
            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
        }

        public async void Init()
		{
			_messenger.Publish<ConferencesLoading> (new ConferencesLoading (this));

			this.Conferences = Enumerable.Empty<Conference> ();

            await LoadConferences();

        }
        public async Task LoadConferences()
        {
            const string url = TekConfApi.BaseUrl + "/conferences";

			InvokeOnMainThread (() => {
				_messenger.Publish<ConferencesLoading> (new ConferencesLoading (this));
			});

            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

            var result = await response.Content.ReadAsStreamAsync();
            var conferences = await DeserializeConferenceList(result);

            this.Conferences = conferences;

			InvokeOnMainThread (() => {
				_messenger.Publish<ConferencesLoaded> (new ConferencesLoaded (this));
			});
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
