using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using TekConf.Mobile.Core.Dtos;
using AutoMapper;

namespace TekConf.Mobile.Core.ViewModels
{
	//public delegate void ChangedEventHandler(object sender, EventArgs e);
	public class ConferencesViewModel : BaseSubTabViewModel
	{
		public event ChangedEventHandler Changed;
		readonly IRemoteConferenceService _conferenceService;
		readonly IDatabaseService _databaseService;

		public ConferencesViewModel(IRemoteConferenceService conferenceService, IDatabaseService databaseService)
		{
			_databaseService = databaseService;
			_conferenceService = conferenceService;
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
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

			this.AreConferencesLoading = true;

			await LoadConferencesAsync(LoadRequest.Load);

			this.AreConferencesLoading = false;

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

		private bool _areConferencesLoading;
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

		private async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			this.AreConferencesLoading = true;

			List<Conference> conferences = await _databaseService.LoadConferencesFromLocalAsync();
			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				await _databaseService.DeleteAllConferencesAsync ();
				var conferenceDtos = await _conferenceService.LoadConferencesFromRemoteAsync ();

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

				conferences = await _databaseService.LoadConferencesFromLocalAsync();
			}

			this.Conferences = conferences;

			this.AreConferencesLoading = false;
			OnChanged(EventArgs.Empty);
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