﻿using System;
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
using TekConf.Mobile.Core.Tests.Extensions;
using TekConf.Mobile.Core.ViewModels;
using Xunit;

namespace TekConf.Mobile.Core.Tests
{
    public class ConferencesViewModelTests
    {
        readonly HttpClient _httpClient;
        private readonly IMvxJsonConverter _jsonConverter;
        public ConferencesViewModelTests()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _jsonConverter = new MvxJsonConverter();
        }
        [Fact]
        public async Task GetConferences()
        {
            var viewModel = new ConferencesViewModel(_httpClient, _jsonConverter);
            viewModel.Conferences.ShouldBeNull();
            await viewModel.LoadConferences();
            viewModel.Conferences.ShouldNotBeNull();
        }

        [Fact]
        public async Task LoadingConferencesNotifies()
        {
            var viewModel = new ConferencesViewModel(_httpClient, _jsonConverter);
            var propertyChanged = false;
            
            viewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                propertyChanged = true;
            };
            
            await viewModel.LoadConferences();

            propertyChanged.ShouldBeTrue();
        }
    }
}
