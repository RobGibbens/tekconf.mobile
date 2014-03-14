using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SettingsDetailTabViewModel : BaseViewModel
	{
		public SettingsDetailTabViewModel()
		{
			LoginViewModel = Mvx.IocConstruct<LoginViewModel>();
			SettingsDetailViewModel = Mvx.IocConstruct<SettingsDetailViewModel>();

		}

		public async void Init(int id)
		{
			((LoginViewModel)LoginViewModel).Init();
			((SettingsDetailViewModel)SettingsDetailViewModel).Init();

		}

		public BaseViewModel LoginViewModel { get; set; }
		public BaseViewModel SettingsDetailViewModel { get; set; }

	}
}