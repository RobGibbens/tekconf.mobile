using System;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.CrossCore.Core;
using System.Net.Http;
using ModernHttpClient;
using System.Threading.Tasks;
using System.IO;

namespace TekConf.Mobile.Droid
{
	public class MvxFastFileDownloadRequest
	{
		public MvxFastFileDownloadRequest(string url, string downloadPath)
		{
			Url = url;
			DownloadPath = downloadPath;
		}

		public string DownloadPath { get; private set; }
		public string Url { get; private set; }

		public event EventHandler<MvxFileDownloadedEventArgs> DownloadComplete;
		public event EventHandler<MvxValueEventArgs<Exception>> DownloadFailed;

		HttpClient CreateClient()
		{
			return new HttpClient(new OkHttpNetworkHandler());
		}

		public void Start()
		{
			var client = CreateClient();

			Task<HttpResponseMessage> result;

			result = client.GetAsync(Url);

			result.ContinueWith(res =>  {

				var httpResult = res.Result;
				httpResult.EnsureSuccessStatusCode(); 
				httpResult.Content.ReadAsStreamAsync()
					.ContinueWith(HandleSuccess, 
						TaskContinuationOptions.NotOnFaulted)
					.ContinueWith(ae => FireDownloadFailed(ae.Exception), 
						TaskContinuationOptions.OnlyOnFaulted);

			}).ContinueWith(ae => FireDownloadFailed(ae.Exception), TaskContinuationOptions.OnlyOnFaulted);

		}

		private void HandleSuccess(Task<Stream> result)
		{
			try
			{
				var fileService = MvxFileStoreHelper.SafeGetFileStore();
				var tempFilePath = DownloadPath + ".tmp";

				using (result.Result)
				{
					fileService.WriteFile(tempFilePath,
						(fileStream) =>
						{
							result.Result.CopyTo(fileStream);
						});
				}
				fileService.TryMove(tempFilePath, DownloadPath, true);
			}
			catch (Exception exception)
			{
				FireDownloadFailed(exception);
				return;
			}

			FireDownloadComplete();
		}

		private void FireDownloadFailed(Exception exception)
		{
			var handler = DownloadFailed;
			if (handler != null)
				handler(this, new MvxValueEventArgs<Exception>(exception));
		}

		private void FireDownloadComplete()
		{
			var handler = DownloadComplete;
			if (handler != null)
				handler(this, new MvxFileDownloadedEventArgs(Url, DownloadPath));
		}
	}
}

