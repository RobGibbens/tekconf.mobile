using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;

namespace TekConf.Mobile.Core.ViewModels
{

	public class ConferencesViewModel 
		: MvxViewModel
    {
		private readonly HttpClient _httpClient;
		private readonly IMvxJsonConverter _jsonConverter;

		public ConferencesViewModel (HttpClient httpClient, IMvxJsonConverter jsonConverter)
		{
			this._jsonConverter = jsonConverter;
			_httpClient = httpClient;
		}

		public async Task LoadConferences()
		{
			var url = TekConfApi.BaseUrl + "/conferences";

			var json = await _httpClient.GetStringAsync (url);
			var conferences = _jsonConverter.DeserializeObject<List<Conference>> (json);

		}
    }
}
