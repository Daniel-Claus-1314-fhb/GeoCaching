﻿using GeoCacheingFinder.Domain;
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
        /// <param name="code"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<GeoCacheModel> findCacheByCodeAsync(String code, String latitude, String longitude)
        {
            latitude = latitude.Replace(",", ".").Trim();
            longitude = longitude.Replace(",", ".").Trim();

            return await findGeoCacheDetailsAsync(code, latitude, longitude);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private async Task<GeoCacheModel> findGeoCacheDetailsAsync(String code, String latitude, String longitude)
        {
            GeoCacheModel geoCacheModel = new GeoCacheModel();

            if (code != null && code.Length > 0)
            {
                String credentialsParam = _apiCredentials.GetString("ConsumerKey");
                String cacheCodeParam = _uriResource.GetString("CacheCodeParam") + code;
                String myLocationParam = _uriResource.GetString("MyLocationParam") + latitude + "|" + longitude;
                String fieldsParam = _uriResource.GetString("FullFields");

                using (HttpClient client = new HttpClient())
                {
                    prepareHttpClient(client);
                    String preparedRequestUri = _uriResource.GetString("GeoCacheDetailsUri") + "?" + credentialsParam + "&" + cacheCodeParam + "&" + myLocationParam + "&" + fieldsParam;

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(preparedRequestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        String responseString = await response.Content.ReadAsStringAsync();
                        geoCacheModel = new GeoCacheModel(responseString);
                    }
                }
            }
            return geoCacheModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchOptionViewModel"></param>
        /// <returns></returns>
        public async Task<List<GeoCacheModel>> searchNearestCachesAsync(SearchOptionViewModel searchOptionViewModel)
        {
            String latitude = searchOptionViewModel.Latitude.Replace(",", ".").Trim();
            String longitude = searchOptionViewModel.Longitude.Replace(",", ".").Trim();
            String radius = searchOptionViewModel.Radius.ToString();

            // get request the codes of the geocaches which are in a certain radius to the given geo position.
            List<String> geoCacheCodes = await searchNearestCacheCodesAsync(latitude, longitude, radius);

            // get request the details for the received geocache codes
            List<GeoCacheModel> gcModels = await findGeoCacheDetailsAsync(geoCacheCodes, latitude, longitude);
            return gcModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private async Task<List<String>> searchNearestCacheCodesAsync(String latitude, String longitude, String radius )
        {
            List<String> geoCacheCodes = new List<String>();

            String credentialsParam = _apiCredentials.GetString("ConsumerKey");
            String myLocationParam = _uriResource.GetString("CenterParam") + latitude + "|" + longitude;
            String radiusParam = _uriResource.GetString("RadiusParam") + radius;

            using (HttpClient client = new HttpClient())
            {
                prepareHttpClient(client);
                String preparedRequestPath = _uriResource.GetString("NearestGeoCachesUri") + "?" + credentialsParam + "&" + myLocationParam + "&" + radiusParam;

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(preparedRequestPath);
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    geoCacheCodes = mapToCodeList(responseString);
                }
            }

            return geoCacheCodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoCacheCodes"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private async Task<List<GeoCacheModel>> findGeoCacheDetailsAsync(List<String> geoCacheCodes, String latitude, String longitude)
        {
            List<GeoCacheModel> geoCacheModels = new List<GeoCacheModel>();

            if (geoCacheCodes != null && geoCacheCodes.Count > 0)
            {
                String credentialsParam = _apiCredentials.GetString("ConsumerKey");
                String cacheCodesParam = _uriResource.GetString("CacheCodesParam") + transformCodes(geoCacheCodes);
                String myLocationParam = _uriResource.GetString("MyLocationParam") + latitude + "|" + longitude;
                String fieldsParam = _uriResource.GetString("SimpleFields");
                            
                using (HttpClient client = new HttpClient())
                {
                    prepareHttpClient(client);
                    String preparedRequestUri = _uriResource.GetString("GeoCachesDetailsUri") + "?" + credentialsParam + "&" + cacheCodesParam + "&" + myLocationParam + "&" + fieldsParam;

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(preparedRequestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        String responseString = await response.Content.ReadAsStringAsync();
                        geoCacheModels = mapToList(responseString);
                    }
                }
            }
            return geoCacheModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoCacheCodes"></param>
        /// <returns></returns>
        private string transformCodes(List<string> geoCacheCodes)
        {
            String strValue = "";
            foreach (String geoCacheCode in geoCacheCodes)
            {
                strValue = strValue + "|" + geoCacheCode;
            }
            return strValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void prepareHttpClient(HttpClient client) 
        {
            client.BaseAddress = new Uri(_uriResource.GetString("BaseUri"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private List<GeoCacheModel> mapToList(String jsonString)
        {
            List<GeoCacheModel> geoCacheModels = new List<GeoCacheModel>();
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (IJsonValue jsonValue in jsonObject.Values) {
                geoCacheModels.Add(new GeoCacheModel(jsonValue));
            }
            return geoCacheModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private List<String> mapToCodeList(String jsonString)
        {
            List<String> codes = new List<String>();
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray("results", new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.String)
                {
                    codes.Add(jsonValue.GetString());
                }
            }
            return codes;
        }
    }
}
