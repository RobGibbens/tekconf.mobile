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
			ConferenceDetailViewModel = Mvx.IocConstruct<ConferenceDetailViewModel>();
			ConferenceDetailSessionsViewModel = Mvx.IocConstruct<ConferenceDetailSessionsViewModel>();
			ConferenceDetailScheduleViewModel = Mvx.IocConstruct<ConferenceDetailScheduleViewModel>();
			ConferenceDetailLocation = Mvx.IocConstruct<ConferenceDetailLocationViewModel>();
		}

		public async void Init(int id)
		{
			((ConferenceDetailViewModel)ConferenceDetailViewModel).Init(id);
			((ConferenceDetailSessionsViewModel)ConferenceDetailSessionsViewModel).Init(id);
			((ConferenceDetailScheduleViewModel)ConferenceDetailScheduleViewModel).Init(id);
			((ConferenceDetailLocationViewModel)ConferenceDetailLocation).Init();
		}

		//public async Task Refresh()
		//{
		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetailViewModel).RefreshAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)ConferenceDetailSessionsViewModel).RefreshAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		public async Task SortByDate()
		{
			var sessionsTask = ((ConferenceDetailSessionsViewModel)ConferenceDetailSessionsViewModel).SortByDateAsync();
			var scheduleTask = ((ConferenceDetailScheduleViewModel)ConferenceDetailScheduleViewModel).SortByDateAsync();

			Task.WaitAll(sessionsTask, scheduleTask);
		}

		public async Task SortByName()
		{
			var sessionsTask = ((ConferenceDetailSessionsViewModel)ConferenceDetailSessionsViewModel).SortByTitleAsync();
			var scheduleTask = ((ConferenceDetailScheduleViewModel)ConferenceDetailScheduleViewModel).SortByTitleAsync();

			Task.WaitAll(sessionsTask, scheduleTask);
		}

		public BaseViewModel ConferenceDetailViewModel { get; set; }
		public BaseViewModel ConferenceDetailSessionsViewModel { get; set; }
		public BaseViewModel ConferenceDetailScheduleViewModel { get; set; }
		public BaseViewModel ConferenceDetailLocation { get; set; }

	}
}