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
		}

		public async void Init(int id)
		{
			((ConferenceDetailViewModel)ConferenceDetail).Init(id);
			((ConferenceDetailSessionsViewModel)ConferenceDetailSessions).Init();
			((ConferenceDetailScheduleViewModel)ConferenceDetailSchedule).Init();
		}

		//public async Task Refresh()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).RefreshAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)ConferenceDetailSessions).RefreshAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		//public async Task SortByDate()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).SortByDateAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)ConferenceDetailSessions).SortByDateAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		//public async Task SortByName()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).SortByNameAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)ConferenceDetailSessions).SortByNameAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}


		public BaseViewModel ConferenceDetail { get; set; }
		public BaseViewModel ConferenceDetailSessions { get; set; }
		public BaseViewModel ConferenceDetailSchedule { get; set; }

	}
}