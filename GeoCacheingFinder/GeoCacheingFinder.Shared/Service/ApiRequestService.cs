using GeoCacheingFinder.Domain;
using GeoCacheingFinder.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;

namespace GeoCacheingFinder.Service
{
    class ApiRequestService
    {
        ResourceLoader _uriResource;
        ResourceLoader _apiCredentials;

        public ApiRequestService()
        {
            _uriResource = ResourceLoader.GetForCurrentView("URIsOpencache");
            _apiCredentials = ResourceLoader.GetForCurrentView("ApiCredentials");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<GeoCacheModel>> searchNearestCachesAsync(SearchOptionViewModel searchOptionViewModel)
        {
            // get request the codes of the geocaches which are in a certain radius to the given geo position.
            GeoCacheCodesModel gcCodes = await searchNearestCacheCodesAsync(searchOptionViewModel);

            // get request the details for the received geocache codes
            List<GeoCacheModel> gcModels = await searchGeoCacheDetailsAsync(gcCodes, searchOptionViewModel);
            return gcModels;
        }

        private async Task<GeoCacheCodesModel> searchNearestCacheCodesAsync(SearchOptionViewModel searchOptionViewModel)
        {
            GeoCacheCodesModel gcCodes = new GeoCacheCodesModel();
            String latitude = searchOptionViewModel.Latitude.Replace(",",".").Trim();
            String longitude = searchOptionViewModel.Longitude.Replace(",", ".").Trim();
            int radius = searchOptionViewModel.Radius;

            using (HttpClient client = new HttpClient())
            {
                prepareHttpClient(client);

                String preparedRequestPath = _uriResource.GetString("NearestGeoCachesUri") + "?" 
                    + _apiCredentials.GetString("ConsumerKey")
                    + "&center=" + latitude + "|" + longitude + "&radius=" + radius;

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(preparedRequestPath);
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    gcCodes = new GeoCacheCodesModel(responseString);
                }
            }

            return gcCodes;
        }

        private async Task<List<GeoCacheModel>> searchGeoCacheDetailsAsync(GeoCacheCodesModel geoCacheCodes, SearchOptionViewModel searchOptionViewModel)
        {
            List<GeoCacheModel> gcModels = new List<GeoCacheModel>();
            String latitude = searchOptionViewModel.Latitude.Replace(",", ".").Trim();
            String longitude = searchOptionViewModel.Longitude.Replace(",", ".").Trim();
            String fields = "code|name|location|type|status|distance";

            if (geoCacheCodes.Codes != null && geoCacheCodes.Codes.Count > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    prepareHttpClient(client);

                    String preparedRequestUri = _uriResource.GetString("GeoCachesDetailsUri")
                        + "?" + _apiCredentials.GetString("ConsumerKey")
                        + "&cache_codes=" + geoCacheCodes.ToString()
                        + "&my_location=" + latitude + "|" + longitude
                        + "&fields=" + fields;

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(preparedRequestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        String responseString = await response.Content.ReadAsStringAsync();
                        gcModels = mapTo(responseString);
                    }
                }
            }

            return gcModels;
        }

        private void prepareHttpClient(HttpClient client) 
        {
            client.BaseAddress = new Uri(_uriResource.GetString("BaseUri"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private List<GeoCacheModel> mapTo(String jsonString)
        {
            List<GeoCacheModel> geoCacheModels = new List<GeoCacheModel>();

            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (IJsonValue jsonValue in jsonObject.Values) {
                geoCacheModels.Add(new GeoCacheModel(jsonValue));
            }

            return geoCacheModels;
        }
    }
}
