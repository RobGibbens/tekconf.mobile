using System.IO;
using System.Linq;
using System.Net.Http;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using SQLite.Net.Async;
using TekConf.Mobile.Core.Messages;
using System.Threading;
using SQLite;

namespace TekConf.Mobile.Core.ViewModels
{
	public enum LoadRequest
	{
		Load,
		Refresh
	}
    
}
