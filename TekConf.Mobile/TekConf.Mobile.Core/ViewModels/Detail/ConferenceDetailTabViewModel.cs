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
			//Vm2 = Mvx.IocConstruct<ConferenceDetailSessionsViewModel>();
			//Vm3 = Mvx.IocConstruct<ConferenceDetailScheduleViewModel>();
		}

		public async void Init(int id)
		{
			((ConferenceDetailViewModel)ConferenceDetail).Init(id);
			//((ConferencesScheduleViewModel)Vm2).Init();
		}

		//public async Task Refresh()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).RefreshAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)Vm2).RefreshAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		//public async Task SortByDate()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).SortByDateAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)Vm2).SortByDateAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}

		//public async Task SortByName()
		//{

		//	var conferencesTask = ((ConferenceDetailViewModel)ConferenceDetail).SortByNameAsync();
		//	var scheduleTask = ((ConferencesScheduleViewModel)Vm2).SortByNameAsync();

		//	Task.WaitAll(conferencesTask, scheduleTask);
		//}


		public BaseViewModel ConferenceDetail { get; set; }
		public BaseViewModel Vm2 { get; set; }
		public BaseViewModel Vm3 { get; set; }

	}
}