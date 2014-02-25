using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using TekConf.Mobile.Core.Dtos;
using AutoMapper;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferencesViewModel : BaseSubTabViewModel
	{
		readonly IRemoteConferenceService _conferenceService;
		readonly IDatabaseService _databaseService;

		IMvxMessenger _messenger;

		public ConferencesViewModel(IRemoteConferenceService conferenceService, IDatabaseService databaseService, IMvxMessenger messenger)
		{
			_messenger = messenger;
			_databaseService = databaseService;
			_conferenceService = conferenceService;
		}

		public async void Init()
		{
			Mapper.CreateMap<ConferenceDto, Conference> ()
				.ForMember(x => x.StreetNumber, src => src.MapFrom(x => x.Address.StreetNumber))
				.ForMember(x => x.StreetName, src => src.MapFrom(x => x.Address.StreetName))
				.ForMember(x => x.City, src => src.MapFrom(x => x.Address.City))
				.ForMember(x => x.State, src => src.MapFrom(x => x.Address.State))
				.ForMember(x => x.PostalArea, src => src.MapFrom(x => x.Address.PostalArea))
				.ForMember(x => x.Country, src => src.MapFrom(x => x.Address.Country))
				.ForMember(c => c.Latitude, opt => opt.ResolveUsing<LatitudeResolver>())
				.ForMember(c => c.Longitude, opt => opt.ResolveUsing<LongitudeResolver>());

			Mapper.CreateMap<SessionDto, Session> ();

			await LoadConferencesAsync(LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
			await LoadConferencesAsync(LoadRequest.Refresh);
		}

		public async Task SortByDateAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderBy(x => x.Start); });
		}

		public async Task SortByNameAsync()
		{
			await TaskEx.Run(() => { this.Conferences = this.Conferences.OrderBy(x => x.Name); });
		}

		public ICommand ShowDetailCommand
		{
			get
			{
				return new MvxCommand<Conference>(conference => 
					ShowViewModel<ConferenceDetailTabViewModel>(new { id = conference.Id })
				);
			}
		}

		public async Task SearchAsync(string query)
		{
			var conferences = await _databaseService.SearchConferencesAsync (query);
			this.Conferences = conferences;
		}

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			_messenger.Publish (new ConferencesLoading (this));

			List<Conference> conferences = await _databaseService.LoadConferencesAsync();

			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				await _databaseService.DeleteAllConferencesAsync ();
				var conferenceDtos = await _conferenceService.LoadConferencesAsync ();

				foreach (var conferenceDto in conferenceDtos)
				{
					ConferenceDto dto = conferenceDto;
					var conference = await TaskEx.Run(() => Mapper.Map<Conference>(dto));
					await _databaseService.SaveConferenceAsync (conference);

					foreach (var sessionDto in conferenceDto.Sessions)
					{
						SessionDto dto1 = sessionDto;
						var session = await TaskEx.Run(() => Mapper.Map<Session>(dto1));
						session.ConferenceId = conference.Id;
						await _databaseService.SaveSessionAsync (session);
					}
				}

				conferences = await _databaseService.LoadConferencesAsync();
			}

			this.Conferences = conferences;

			_messenger.Publish (new ConferencesLoaded (this));
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

		public class LatitudeResolver : ValueResolver<ConferenceDto, double>
		{
			protected override double ResolveCore(ConferenceDto source)
			{
				if (source != null && source.Position != null && source.Position.Length == 2)
				{
					return source.Position[1];
				}
				else
				{
					return 0.0;
				}
			}
		}

		public class LongitudeResolver : ValueResolver<ConferenceDto, double>
		{
			protected override double ResolveCore(ConferenceDto source)
			{
				if (source != null && source.Position != null && source.Position.Length == 2)
				{
					return source.Position[0];
				}
				else
				{
					return 0.0;
				}
			}
		}
	}
}