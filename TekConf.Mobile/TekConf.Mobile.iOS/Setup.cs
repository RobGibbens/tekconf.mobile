using MonoTouch.UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using System.Net.Http;
using Cirrious.MvvmCross.Binding;
using System.Net.Http.Headers;
using System;
using Cirrious.CrossCore;
using SQLite.Net.Async;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using ModernHttpClient;

namespace TekConf.Mobile.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;

			var httpClient = new HttpClient(new AFNetworkHandler());
			httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
			Mvx.RegisterSingleton<HttpClient>(httpClient);
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

			var sqLiteConnection = CreateSqLiteConnection();

			Mvx.RegisterSingleton(sqLiteConnection);

			return new Core.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		private static SQLiteAsyncConnection CreateSqLiteConnection()
		{
			const string sqliteFilename = "tekconf.db3";
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, sqliteFilename);

			var connectionFactory = new Func<SQLiteConnectionWithLock>(() => 
				new SQLiteConnectionWithLock(new SQLitePlatformIOS(), 
					new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));
			var asyncConnection = new SQLiteAsyncConnection(connectionFactory);
			return asyncConnection;
		}
	}
}