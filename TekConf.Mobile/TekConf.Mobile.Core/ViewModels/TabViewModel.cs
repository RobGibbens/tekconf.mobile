using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using SQLite.Net.Async;

namespace TekConf.Mobile.Core.ViewModels
{

	public delegate void ChangedEventHandler(object sender, EventArgs e);

	public class TabViewModel : BaseViewModel
	{
		private readonly HttpClient _httpClient;
		private readonly IMvxJsonConverter _jsonConverter;
		private readonly SQLiteAsyncConnection _sqLiteConnection;
		public event ChangedEventHandler Changed;

		public TabViewModel(HttpClient httpClient, IMvxJsonConverter jsonConverter, SQLiteAsyncConnection sqLiteConnection)
		{
			_httpClient = httpClient;
			_jsonConverter = jsonConverter;
			_sqLiteConnection = sqLiteConnection;

			Vm1 = Mvx.IocConstruct<ConferencesViewModel>();
			//Vm2 = Mvx.IocConstruct<SecondTabViewModel>();
			//Vm3 = Mvx.IocConstruct<ThirdTabViewModel>();
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}

		public async void Init()
		{
			Vm1.Init(null);
			CreateDatabase();
			await LoadConferences(LoadRequest.Load);

		}
		public void CreateDatabase()
		{
			var conferenceTask = _sqLiteConnection.CreateTableAsync<Conference>();
			Task.WaitAll(conferenceTask);
		}

		public async Task LoadConferences(LoadRequest loadRequest)
		{
			((ConferencesViewModel)Vm1).AreConferencesLoading = true;

			List<Conference> conferences = await LoadConferencesFromLocal();
			if (!conferences.Any() || loadRequest == LoadRequest.Refresh)
			{
				conferences = await LoadConferencesFromRemote();
			}

			((ConferencesViewModel)Vm1).Conferences = conferences;

			((ConferencesViewModel)Vm1).AreConferencesLoading = false;
			OnChanged(EventArgs.Empty);
		}

		public async Task Refresh()
		{
			((ConferencesViewModel)Vm1).Conferences = Enumerable.Empty<Conference>();
			CreateDatabase();
			await LoadConferences(LoadRequest.Refresh);
		}
		private async Task<List<Conference>> LoadConferencesFromLocal()
		{
			var conferences = await _sqLiteConnection.Table<Conference>().OrderBy(x => x.Start).ToListAsync();

			return conferences;
		}

		private async Task<List<Conference>> LoadConferencesFromRemote()
		{
			const string url = TekConfApi.BaseUrl + "/conferences";

			var deleteTask = _sqLiteConnection.DeleteAllAsync<Conference>();
			var httpCallTask = _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

			Task.WaitAll(deleteTask, httpCallTask);

			var response = httpCallTask.Result;

			var result = await response.Content.ReadAsStreamAsync();
			var conferences = await DeserializeConferenceList(result);
			foreach (var conference in conferences)
			{
				if (string.IsNullOrWhiteSpace(conference.ImageUrlSquare))
				{
					conference.ImageUrlSquare = conference.ImageUrl;
				}
			}
			await _sqLiteConnection.InsertAllAsync(conferences);

			return conferences;
		}

		private Task<List<Conference>> DeserializeConferenceList(Stream result)
		{
			return Task.Factory.StartNew(() =>
			{
				var reader = new StreamReader(result);
				string json = reader.ReadToEnd();
				var conferences = _jsonConverter.DeserializeObject<List<Conference>>(json);

				return conferences.OrderBy(c => c.Start).ToList();
			});
		}

		public BaseViewModel Vm1 { get; set; }
		public BaseViewModel Vm2 { get; set; }
		public BaseViewModel Vm3 { get; set; }
	}
}