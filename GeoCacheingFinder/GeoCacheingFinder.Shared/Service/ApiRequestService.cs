using GeoCacheingFinder.Domain;
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
        public async Task<List<GeoCacheModel>> searchNearestCachesAsync()
        {
            // get request the codes of the geocaches which are in a certain radius to the given geo position.
            GeoCacheCodes gcCodes = await searchNearestCacheCodesAsync();

            // get request the details for the received geocache codes
            List<GeoCacheModel> gcModels = await searchGeoCacheDetailsAsync(gcCodes);
            return gcModels;
        }

        private async Task<GeoCacheCodes> searchNearestCacheCodesAsync()
        {
            GeoCacheCodes gcCodes = new GeoCacheCodes();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uriResource.GetString("BaseUri"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                String preparedRequestPath = _uriResource.GetString("NearestGeoCachesUri") + "?" 
                    + _apiCredentials.GetString("ConsumerKey")
                    + "&center=52.3871|13.0993&radius=10";

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(preparedRequestPath);
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    gcCodes = new GeoCacheCodes(responseString);
                }
            }

            return gcCodes;
        }

        private async Task<List<GeoCacheModel>> searchGeoCacheDetailsAsync(GeoCacheCodes geoCacheCodes)
        {
            List<GeoCacheModel> gcModels = new List<GeoCacheModel>();
            String preparedCodes = geoCacheCodes.ToString();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uriResource.GetString("BaseUri"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("caches/geocaches?consumer_key=&cache_codes=" + preparedCodes);
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    gcModels = mapTo(responseString);
                }
            }

            return gcModels;
        }

        private List<GeoCacheModel> mapTo(String jsonString)
        {
            List<GeoCacheModel> geoCacheModels = new List<GeoCacheModel>();

            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (IJsonValue jsonValue in jsonObject.Values) {
                // {"code":"OCC95C","name":"Neuendorfer Triftwege","location":"52.391367|13.095367","type":"Traditional","status":"Available"}

                JsonObject jsonObject2 = jsonValue.GetObject();

                GeoCacheModel gcModel = new GeoCacheModel();
                gcModel.Code = jsonObject2.GetNamedString("code", "");
                gcModel.Name = jsonObject2.GetNamedString("name", "");
                gcModel.Location = jsonObject2.GetNamedString("location", "");
                gcModel.Type = jsonObject2.GetNamedString("type", "");
                gcModel.Status = jsonObject2.GetNamedString("status", "");

                geoCacheModels.Add(gcModel);
            }

            return geoCacheModels;
        }
    }
}
