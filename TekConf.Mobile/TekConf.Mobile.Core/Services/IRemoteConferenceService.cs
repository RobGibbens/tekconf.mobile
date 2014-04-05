using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TekConf.Mobile.Core.Dtos;

namespace TekConf.Mobile.Core
{
	public interface IRemoteConferenceService
	{
		Task<List<ConferenceDto>> LoadConferencesAsync(string userName);
		Task<List<ConferenceDto>> LoadScheduledConferencesAsync(string userName);
		Task AddSessionToScheduleAsync (string userName, string conferenceSlug, string sessionSlug);
	}
}