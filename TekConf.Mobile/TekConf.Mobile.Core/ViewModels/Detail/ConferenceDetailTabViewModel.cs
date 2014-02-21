using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace TekConf.Mobile.Core.ViewModels
{

	public delegate void ConferenceDetailChangedEventHandler(object sender, EventArgs e);

	public class ConferenceDetailTabViewModel : BaseViewModel
	{
		public ConferenceDetailTabViewModel()
		{
			ConferenceDetail = Mvx.IocConstruct<ConferenceDetailViewModel>();
			ConferenceDetailSessions = Mvx.IocConstruct<ConferenceDetailSessionsViewModel>();
			ConferenceDetailSchedule = Mvx.IocConstruct<ConferenceDetailScheduleViewModel>();
			ConferenceDetailLocation = Mvx.IocConstruct<ConferenceDetailLocationViewModel>();
		}

		public async void Init(int id)
		{
			((ConferenceDetailViewModel)ConferenceDetail).Init(id);
			((ConferenceDetailSessionsViewModel)ConferenceDetailSessions).Init(id);
			((ConferenceDetailScheduleViewModel)ConferenceDetailSchedule).Init();
			((ConferenceDetailLocationViewModel)ConferenceDetailLocation).Init();
		}

		//public async Task Refresh()
		//{
		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).RefreshAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)ConferenceDetailSessions).RefreshAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		public async Task SortByDate()
		{
			await ((ConferenceDetailSessionsViewModel)ConferenceDetailSessions).SortByDateAsync();
			//var scheduleTask = ((ConferenceDetailScheduleViewModel)ConferenceDetailSchedule).SortByDateAsync();

			//Task.WaitAll(sessionsTask, scheduleTask);
		}

		public async Task SortByName()
		{
			var sessionsTask = ((ConferenceDetailSessionsViewModel)ConferenceDetailSessions).SortByTitleAsync();
			//var scheduleTask = ((ConferenceDetailScheduleViewModel)ConferenceDetailSchedule).SortByTitleAsync();

			//Task.WaitAll(sessionsTask, scheduleTask);
		}


		public BaseViewModel ConferenceDetail { get; set; }
		public BaseViewModel ConferenceDetailSessions { get; set; }
		public BaseViewModel ConferenceDetailSchedule { get; set; }
		public BaseViewModel ConferenceDetailLocation { get; set; }

	}
}