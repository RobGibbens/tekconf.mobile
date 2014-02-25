using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using SQLite.Net.Async;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using AutoMapper;
using TekConf.Mobile.Core.Dtos;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferencesScheduleViewModel : BaseSubTabViewModel
	{
		private readonly IRemoteConferenceService _conferenceService;
		private readonly IDatabaseService _databaseService;
		private readonly IMvxMessenger _messenger;

		public ConferencesScheduleViewModel(
			SQLiteAsyncConnection sqLiteConnection,
			IRemoteConferenceService conferenceService, 
			IDatabaseService databaseService,
			IMvxMessenger messenger)
		{
			_messenger = messenger;
			_databaseService = databaseService;
			_conferenceService = conferenceService;
		}

		public async void Init()
		{

			Mapper.CreateMap<ConferenceDto, ScheduledConference> ()
				.ForMember(x => x.StreetNumber, src => src.MapFrom(x => x.Address.StreetNumber))
				.ForMember(x => x.StreetName, src => src.MapFrom(x => x.Address.StreetName))
				.ForMember(x => x.City, src => src.MapFrom(x => x.Address.City))
				.ForMember(x => x.State, src => src.MapFrom(x => x.Address.State))
				.ForMember(x => x.PostalArea, src => src.MapFrom(x => x.Address.PostalArea))
				.ForMember(x => x.Country, src => src.MapFrom(x => x.Address.Country))
				.ForMember(c => c.Latitude, opt => opt.ResolveUsing<TekConf.Mobile.Core.ViewModels.ConferencesViewModel.LatitudeResolver>())
				.ForMember(c => c.Longitude, opt => opt.ResolveUsing<TekConf.Mobile.Core.ViewModels.ConferencesViewModel.LongitudeResolver>());

			await LoadConferencesAsync(LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
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

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			_messenger.Publish (new ConferencesLoading (this));

			List<ScheduledConference> scheduledConferences = await _databaseService.LoadScheduledConferencesAsync();

			if (!scheduledConferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				await _databaseService.DeleteAllScheduledConferencesAsync ();
				var scheduledConferenceDtos = await _conferenceService.LoadScheduledConferencesAsync ();

				foreach (var scheduledConferenceDto in scheduledConferenceDtos)
				{
					var dto = scheduledConferenceDto;
					var scheduledConference = await TaskEx.Run(() => Mapper.Map<ScheduledConference>(dto));
					await _databaseService.SaveScheduledConferenceAsync (scheduledConference);

					//					foreach (var sessionDto in scheduledConferenceDto.Sessions)
					//					{
					//						SessionDto dto1 = sessionDto;
					//						var session = await TaskEx.Run(() => Mapper.Map<Session>(dto1));
					//						session.ConferenceId = scheduledConference.Id;
					//						await _databaseService.SaveSessionAsync (session);
					//					}
				}

				scheduledConferences = await _databaseService.LoadScheduledConferencesAsync();
			}

			this.Conferences = scheduledConferences;

			_messenger.Publish (new ConferencesLoaded (this));
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
