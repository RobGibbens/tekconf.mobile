using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Android.Content;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Net.Http;
using ModernHttpClient;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using System.Net.Http.Headers;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinAndroid;
using TekConf.Mobile.Core.Services;
using TekConf.Mobile.Droid.Code;

namespace TekConf.Mobile.Droid
{
	public class Setup : MvxAndroidSetup
	{
		public Setup(Context applicationContext)
			: base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;

			var httpClient = new HttpClient(new OkHttpNetworkHandler());
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Mvx.RegisterSingleton<HttpClient>(httpClient);

			var sqLiteConnection = CreateSqLiteConnection();

			Mvx.RegisterSingleton(sqLiteConnection);
			Mvx.RegisterType(typeof(IAuthenticationService), typeof(AuthenticationService));

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

		protected override void InitializeLastChance()
		{
			Mvx.RegisterSingleton<IMvxHttpFileDownloader>(() => new MvxFastHttpFileDownloader());
			base.InitializeLastChance();
		}

		//protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
		//{
		//	base.FillTargetFactories(registry);

		//	registry.RegisterFactory(new MvxCustomBindingFactory<MvxImageView>("IsAddedToSchedule", (imageView) => new FavoriteImageViewBinding(imageView)));
		//}

		protected override List<Type> ValueConverterHolders
		{
			get
			{
				return new List<Type>
				{
					typeof(Converters)
				};
			}
		}

	}

	public class SessionScheduleStateConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string image = "";
			if (DateTime.Now.Millisecond % 2 == 0)
			{
				image = "sessionstates/notadded.png";
			}
			else
			{
				image = "sessionstates/added.png";
			}
			Thread.Sleep(2);
			//var image = string.Format("sessionstates/{0}.png", ((SessionScheduleState)value).ToString().ToLower());
			return image;
		}
	}

	public class Converters
	{
		public readonly SessionScheduleStateConverter SessionScheduleState = new SessionScheduleStateConverter();
	}
}