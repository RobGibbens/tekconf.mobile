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
			Vm1 = Mvx.IocConstruct<ConferencesViewModel>();
			Vm2 = Mvx.IocConstruct<ConferencesScheduleViewModel>();
		}

		public async void Init()
		{
			((ConferencesViewModel)Vm1).Init();
			((ConferencesScheduleViewModel)Vm2).Init();
		}

		public async Task Refresh()
		{

			var conferencesTask = ((ConferencesViewModel)Vm1).RefreshAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)Vm2).RefreshAsync();

			Task.WaitAll (conferencesTask, scheduleTask);
		}

		public async Task SortByDate()
		{

			var conferencesTask = ((ConferencesViewModel)Vm1).SortByDateAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)Vm2).SortByDateAsync();

			Task.WaitAll(conferencesTask, scheduleTask);
		}

		public async Task SortByName()
		{

			var conferencesTask = ((ConferencesViewModel)Vm1).SortByNameAsync();
			var scheduleTask = ((ConferencesScheduleViewModel)Vm2).SortByNameAsync();

			Task.WaitAll(conferencesTask, scheduleTask);
		}


		public BaseViewModel Vm1 { get; set; }
		public BaseViewModel Vm2 { get; set; }
		public BaseViewModel Vm3 { get; set; }
	}
}