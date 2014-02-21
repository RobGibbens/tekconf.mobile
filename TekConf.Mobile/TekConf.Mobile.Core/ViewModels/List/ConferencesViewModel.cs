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

			this.Conferences = Enumerable.Empty<Conference>();
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}

		public async void Init()
		{
			this.Conferences = Enumerable.Empty<Conference>();

			Mapper.CreateMap<ConferenceDto, Conference> ();
			Mapper.CreateMap<SessionDto, Session> ();

			await LoadConferencesAsync(LoadRequest.Load);
		}

		public async Task RefreshAsync()
		{
			this.Conferences = Enumerable.Empty<Conference>();

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
					var conference = Mapper.Map<Conference> (conferenceDto);
					await _databaseService.SaveConferenceAsync (conference);

					foreach (var sessionDto in conferenceDto.Sessions)
					{
						var session = Mapper.Map<Session> (sessionDto);
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

		private async Task<List<Conference>> MapConferences(List<ConferenceDto> conferenceDtos)
		{
			var conferences = await TaskEx.Run (() => Mapper.Map<List<Conference>> (conferenceDtos));

			return conferences;
		}

		private async Task<List<Session>> MapSessions(List<ConferenceDto> dtos)
		{
			var entities = await TaskEx.Run (() => Mapper.Map<List<Session>> (dtos));

			return entities;
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
