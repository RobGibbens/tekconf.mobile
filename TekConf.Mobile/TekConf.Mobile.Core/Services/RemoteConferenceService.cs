﻿using System;
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

		public async Task<List<ConferenceDto>> LoadScheduledConferencesAsync()
		{
			string url = string.Format(TekConfApi.BaseUrl + "/conferences/schedules?userName={0}", "robgibbens");

			var json = await _httpClient.GetStringAsync(url);
			var conferences = await TaskEx.Run(() => _jsonConverter.DeserializeObject<List<ConferenceDto>>(json));

			return conferences.OrderBy(c => c.Start).ToList();
		}
	}
}