using System;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace TekConf.Mobile.Core.ViewModels
{
	public class SessionDetailTabViewModel : BaseViewModel
	{
		public SessionDetailTabViewModel()
		{
			SessionDetailViewModel = Mvx.IocConstruct<SessionDetailViewModel>();
			SessionDetailSpeakersViewModel = Mvx.IocConstruct<SessionDetailSpeakersViewModel>();
		}

		public async void Init(int id)
		{
			((SessionDetailViewModel)SessionDetailViewModel).Init(id);
			((SessionDetailSpeakersViewModel)SessionDetailSpeakersViewModel).Init(id);
		}

		public BaseViewModel SessionDetailViewModel { get; set; }
		public BaseViewModel SessionDetailSpeakersViewModel { get; set; }
	}
}