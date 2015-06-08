using GeoCacheingFinder.Domain.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace GeoCacheingFinder.Service
{
    class OptionStorageService
    {
        private const String SearchOptionKey = "searchOption";
        private const String ContainerKey = "Option";

        private ApplicationDataContainer _container = null;

        /// <summary>
        /// Constructor init localsettings and container
        /// </summary>
        public OptionStorageService()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Containers.ContainsKey(SearchOptionKey))
            {
                _container = localSettings.Containers[SearchOptionKey];
            }
            else
            {
                _container = localSettings.CreateContainer(SearchOptionKey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }
        }

        public void SaveSearchOptionToSetting(SearchOptionViewModel searchOptionViewModel)
        {
            if (searchOptionViewModel != null && _container != null)
            {
                String searchOptionJsonString = JsonConvert.SerializeObject(searchOptionViewModel);
                _container.Values[SearchOptionKey] = searchOptionJsonString;
            }
            
        }

        public SearchOptionViewModel LoadSearchOptionFromSetting()
        {
            SearchOptionViewModel LoadSearchOptionViewModel = new SearchOptionViewModel();
            if (_container != null && _container.Values.ContainsKey(SearchOptionKey))
            {
                String searchOptionJsonString = (String) _container.Values[SearchOptionKey];
                if (searchOptionJsonString != null && searchOptionJsonString.Length > 0)
                    LoadSearchOptionViewModel = JsonConvert.DeserializeObject<SearchOptionViewModel>(searchOptionJsonString);
            }
            return LoadSearchOptionViewModel;
        }
    }
}
