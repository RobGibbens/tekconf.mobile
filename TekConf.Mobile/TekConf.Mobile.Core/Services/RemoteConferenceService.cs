using System;
using System.Net.Http;
using Cirrious.CrossCore.Platform;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TekConf.Mobile.Core.Dtos;

namespace TekConf.Mobile.Core
{
	public class ConferenceService : IRemoteConferenceService
	{
		private readonly HttpClient _httpClient;
		private readonly IMvxJsonConverter _jsonConverter;
		private readonly string AddSessionToScheduleUrl = TekConfApi.BaseUrl + "/conferences/{1}/schedule?userName={0}&conferenceSlug={1}&sessionSlug={2}&format=json";

		public ConferenceService (HttpClient httpClient, IMvxJsonConverter jsonConverter)
		{
			_httpClient = httpClient;
			_jsonConverter = jsonConverter;
		}

		public async Task<List<ConferenceDto>> LoadConferencesAsync()
		{
			const string url = TekConfApi.BaseUrl + "/conferences"; //?showPastConferences=true

			var json = await _httpClient.GetStringAsync(url);
			var conferences = await TaskEx.Run(() => _jsonConverter.DeserializeObject<List<ConferenceDto>>(json));

			return conferences.OrderBy(c => c.Start).ToList();
		}

		public async Task<List<ConferenceDto>> LoadScheduledConferencesAsync(string userName)
		{
			string url = string.Format(TekConfApi.BaseUrl + "/conferences/schedules?userName={0}", userName);

			var json = await _httpClient.GetStringAsync(url);
			var conferences = await TaskEx.Run(() => _jsonConverter.DeserializeObject<List<ConferenceDto>>(json));

			return conferences.OrderBy(c => c.Start).ToList();
		}

		public async Task AddSessionToScheduleAsync(string userName, string conferenceSlug, string sessionSlug)
		{
			string uri = string.Format(AddSessionToScheduleUrl, userName, conferenceSlug, sessionSlug);
			var response = await _httpClient.PostAsync (uri, null);
		}
	}
}