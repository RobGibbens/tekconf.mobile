using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TekConf.Mobile.Core.Dtos;

namespace TekConf.Mobile.Core
{
	public interface IRemoteConferenceService
	{
		Task<List<ConferenceDto>> LoadConferencesFromRemoteAsync();
	}
}