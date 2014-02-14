using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core.ViewModels
{

	public delegate void ChangedEventHandler(object sender, EventArgs e);

	public class TabViewModel : BaseViewModel
	{
		public TabViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter, SQLiteAsyncConnection sqLiteConnection)
		{
			Vm1 = Mvx.IocConstruct<ConferencesViewModel>();
			Vm2 = Mvx.IocConstruct<ConferencesScheduleViewModel>();
			//Vm3 = Mvx.IocConstruct<ThirdTabViewModel>();
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


		public BaseViewModel Vm1 { get; set; }
		public BaseViewModel Vm2 { get; set; }
		public BaseViewModel Vm3 { get; set; }
	}
}