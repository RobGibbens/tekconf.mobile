using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace TekConf.Mobile.Core.ViewModels
{

	public delegate void ChangedEventHandler(object sender, EventArgs e);

	public class ConferencesTabViewModel : BaseViewModel
	{
		public ConferencesTabViewModel()
		{
			ConferencesViewModel = Mvx.IocConstruct<ConferencesViewModel>();
			ConferencesScheduleViewModel = Mvx.IocConstruct<ConferencesScheduleViewModel>();
		}

		public async void Init()
		{
			((ConferencesViewModel)ConferencesViewModel).Init();
			((ConferencesScheduleViewModel)ConferencesScheduleViewModel).Init();
		}

		public async Task Refresh()
		{

			var conferencesTask = ((ConferencesViewModel)ConferencesViewModel).RefreshAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)ConferencesScheduleViewModel).RefreshAsync();

			Task.WaitAll (conferencesTask, scheduleTask);
		}

		public async Task SortByDate()
		{

			var conferencesTask = ((ConferencesViewModel)ConferencesViewModel).SortByDateAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)ConferencesScheduleViewModel).SortByDateAsync();

			Task.WaitAll(conferencesTask, scheduleTask);
		}

		public async Task SortByName()
		{

			var conferencesTask = ((ConferencesViewModel)ConferencesViewModel).SortByNameAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)ConferencesScheduleViewModel).SortByNameAsync();

			Task.WaitAll(conferencesTask, scheduleTask);
		}


		public BaseViewModel ConferencesViewModel { get; set; }
		public BaseViewModel ConferencesScheduleViewModel { get; set; }
		public BaseViewModel Vm3 { get; set; }
	}
}