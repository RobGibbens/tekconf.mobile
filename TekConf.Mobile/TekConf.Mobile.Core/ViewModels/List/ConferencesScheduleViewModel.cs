using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite.Net.Async;
using Cirrious.MvvmCross.Plugins.Messenger;
using AutoMapper;
using TekConf.Mobile.Core.Dtos;

namespace TekConf.Mobile.Core.ViewModels
{
	public class ConferencesScheduleViewModel : BaseSubTabViewModel
	{
		private readonly IRemoteConferenceService _conferenceService;
		private readonly IDatabaseService _databaseService;
		private readonly IMvxMessenger _messenger;
		private User _currentUser;

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

			Mapper.CreateMap<ConferenceDto, Conference>()
				.ForMember(x => x.StreetNumber, src => src.MapFrom(x => x.Address.StreetNumber))
				.ForMember(x => x.StreetName, src => src.MapFrom(x => x.Address.StreetName))
				.ForMember(x => x.City, src => src.MapFrom(x => x.Address.City))
				.ForMember(x => x.State, src => src.MapFrom(x => x.Address.State))
				.ForMember(x => x.PostalArea, src => src.MapFrom(x => x.Address.PostalArea))
				.ForMember(x => x.Country, src => src.MapFrom(x => x.Address.Country))
				.ForMember(c => c.Latitude, opt => opt.ResolveUsing<ConferencesViewModel.LatitudeResolver>())
				.ForMember(c => c.Longitude, opt => opt.ResolveUsing<ConferencesViewModel.LongitudeResolver>());

				await LoadConferencesAsync (LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
			await LoadConferencesAsync(LoadRequest.Refresh);
		}

		public async Task SearchAsync(string query)
		{
			var conferences = await _databaseService.SearchScheduledConferences(query);
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

		public async Task LoadConferencesAsync(LoadRequest loadRequest)
		{
			this.AreConferencesLoading = true;

			_currentUser = await _databaseService.LoadCurrentUserAsync ();
			if (_currentUser != null) {

				var scheduledConferences = await _databaseService.LoadScheduledConferencesAsync ();

				if (!scheduledConferences.Any () || loadRequest == LoadRequest.Refresh) {
					await _databaseService.DeleteAllScheduledConferencesAsync ();
					var scheduledConferenceDtos = await _conferenceService.LoadScheduledConferencesAsync (_currentUser.UserName);

					foreach (var scheduledConferenceDto in scheduledConferenceDtos) {
						var dto = scheduledConferenceDto;
						var scheduledConference = await TaskEx.Run (() => Mapper.Map<Conference> (dto));
						
						await _databaseService.SaveConferenceAsync (scheduledConference);

						//foreach (var sessionDto in scheduledConferenceDto.Sessions)
						//{
						//	SessionDto dto1 = sessionDto;
						//	var session = await TaskEx.Run(() => Mapper.Map<Session>(dto1));
						//	session.ConferenceId = scheduledConference.Id;
						//	await _databaseService.SaveSessionAsync(session);
						//}
					}

					scheduledConferences = await _databaseService.LoadScheduledConferencesAsync ();
				}

				this.Conferences = scheduledConferences;
			}

			this.AreConferencesLoading = false;
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

		public ICommand ShowDetailCommand
		{
			get
			{
				return new MvxCommand<Conference>(scheduledConference =>
				{
						var dbConference = TaskEx.Run(() => _databaseService.LoadConferenceAsync(scheduledConference.Slug)).Result;

						ShowViewModel<ConferenceDetailTabViewModel>(new { id = dbConference.Id });
					}
				);
			}
		}

		public ICommand ShowSettingsCommand
		{
			get
			{
				return new MvxCommand<Conference>(conference => ShowViewModel<SettingsDetailTabViewModel>());
			}
		}
	}
}
