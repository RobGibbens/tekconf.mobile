using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Net.Http;
using ModernHttpClient;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using System.Net.Http.Headers;
namespace TekConf.Mobile.Droid
{
	public class Setup : MvxAndroidSetup
	{
		public Setup(Context applicationContext) : base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
            MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;

			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
			Mvx.RegisterSingleton<HttpClient>(httpClient);

			return new Core.App();
		}
		
		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}

		protected override void InitializeLastChance ()
		{
			Mvx.RegisterSingleton<IMvxHttpFileDownloader>(() => new MvxFastHttpFileDownloader());
			base.InitializeLastChance ();
		}
	}
}