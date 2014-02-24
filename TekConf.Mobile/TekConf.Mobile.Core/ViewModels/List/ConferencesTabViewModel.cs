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

		public BaseViewModel ConferencesViewModel { get; set; }
		public BaseViewModel ConferencesScheduleViewModel { get; set; }
		public BaseViewModel Vm3 { get; set; }
	}
}