using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;
using Moq;
using Ploeh.AutoFixture;
using Should;
using TekConf.Mobile.Core;
using TekConf.Mobile.Core.Dtos;
using TekConf.Mobile.Core.ViewModels;
using Xunit;

namespace Specs.Core
{
	[Trait ("A user logs in", "")]
	public class UserLogsIn
	{
		private IFixture _fixture;

		public UserLogsIn ()
		{
			_fixture = new Fixture ();
		}

		[Fact (DisplayName = "Current conferences should be retrieved")]
		public async Task Current_conferences_retrieved ()
		{
			var conferenceService = new Mock<IRemoteConferenceService> ();
			var conferenceDtos = _fixture.CreateMany<ConferenceDto> (50).ToList ();
			var conferences = _fixture.CreateMany<Conference> (50).ToList ();
			conferenceService.Setup (x => x.LoadConferencesAsync ("")).Returns (Task.FromResult (conferenceDtos));
				
			var databaseService = new Mock<IDatabaseService> ();
			var pq = new Queue<List<Conference>> (new[] { 
				null, 
				conferences
			});

			databaseService.Setup (x => x.LoadConferencesAsync ()).Returns (Task.FromResult (pq.Dequeue ()));
			var messenger = new Mock<IMvxMessenger> ();
			var viewModel = new ConferencesViewModel (conferenceService.Object, databaseService.Object, messenger.Object);
			viewModel.Init ();
			await viewModel.LoadConferencesAsync (LoadRequest.Load);
			viewModel.Conferences.ShouldNotBeNull ();
		}

		[Fact (DisplayName = "Scheduled conferences should be retrieved")]
		public void Scheduled_conferences_retrieved ()
		{
			throw new NotImplementedException ();
		}
	}
}
