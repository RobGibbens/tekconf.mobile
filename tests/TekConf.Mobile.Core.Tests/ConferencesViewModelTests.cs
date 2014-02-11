using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Should;
using SQLite.Net.Async;
using TekConf.Mobile.Core.Tests.Extensions;
using TekConf.Mobile.Core.ViewModels;
using Xunit;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Tests
{
    public class ConferencesViewModelTests
    {
        readonly HttpClient _httpClient;
        private readonly IMvxJsonConverter _jsonConverter;
		private readonly IMvxMessenger _messenger;
        public ConferencesViewModelTests()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _jsonConverter = new MvxJsonConverter();
			_messenger = new MvxMessengerHub ();
        }

        [Fact]
        public async Task GetConferences()
        {
			var viewModel = new ConferencesViewModel(_httpClient, _jsonConverter, null);
            viewModel.Conferences.ShouldBeNull();
			await viewModel.LoadConferences(LoadRequest.Load);
            viewModel.Conferences.ShouldNotBeNull();
        }

        [Fact]
        public async Task LoadingConferencesNotifies()
        {
            var viewModel = new ConferencesViewModel(_httpClient, _jsonConverter, null);
            var propertyChanged = false;
            
            viewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                propertyChanged = true;
            };
            
			await viewModel.LoadConferences(LoadRequest.Load);

            propertyChanged.ShouldBeTrue();
        }
    }
}
