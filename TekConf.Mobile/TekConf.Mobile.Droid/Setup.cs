using System;
using System.IO;
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
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using TekConf.Mobile.Core;

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
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var sqLiteConnection = CreateSqLiteConnection();

		    Mvx.RegisterSingleton(sqLiteConnection);

			return new Core.App();
		}

	    private static SQLiteAsyncConnection CreateSqLiteConnection()
	    {
	        const string sqliteFilename = "tekconf.db3";
	        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
	        var path = Path.Combine(documentsPath, sqliteFilename);

            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => 
                new SQLiteConnectionWithLock(new SQLitePlatformAndroid(), 
                    new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));
            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);
            return asyncConnection;
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