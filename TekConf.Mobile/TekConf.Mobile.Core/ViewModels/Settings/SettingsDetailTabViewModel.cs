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
		}

		public async void Init(int id)
		{
			((LoginViewModel)LoginViewModel).Init();
		}

		public BaseViewModel LoginViewModel { get; set; }
	}
}