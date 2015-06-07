using GeoCacheingFinder.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GeoCacheingFinder.Service
{
    public class CacheStorageService
    {
        private const string FileName = "GeoCacheFavorite.txt";
        
        /// <summary>
        /// Delivers all geo caches which are stored as favorite.
        /// </summary>
        /// <returns></returns>
        public async Task<List<GeoCacheModel>> FindAllGeoCachesFormFavoriteAsync()
        {
            List<GeoCacheModel> loadGeoCacheModels = new List<GeoCacheModel>();
            
            String loadGeoCachesJsonString = await ReadFromFileAsync(FileName);

            if (loadGeoCachesJsonString != null && loadGeoCachesJsonString.Length > 0)
            {
                loadGeoCacheModels = JsonConvert.DeserializeObject<List<GeoCacheModel>>(loadGeoCachesJsonString);
            }
            return loadGeoCacheModels;
        }

        public async Task<GeoCacheModel> FindGeoCacheByCodeFormFavoriteAsync(String code)
        {
            GeoCacheModel loadGeoCacheModel = null;

            if (code != null)
            {
                List<GeoCacheModel> loadGeoCacheModels = await FindAllGeoCachesFormFavoriteAsync();

                foreach (GeoCacheModel geoCacheModel in loadGeoCacheModels)
                {
                    if (geoCacheModel.Code.Contains(code))
                    {
                        loadGeoCacheModel = geoCacheModel;
                        break;
                    }
                }
            }
            return loadGeoCacheModel;
        }

        /// <summary>
        /// Adds an certain geo cache from the favorite geo cache list.
        /// </summary>
        /// <param name="geoCacheModel"></param>
        public async void AddGeoCacheToFavoriteAsync(GeoCacheModel addGeoCacheModel)
        {
            if (addGeoCacheModel != null)
            {
                bool hasUpdated = false;
                List<GeoCacheModel> loadGeoCacheModels = await FindAllGeoCachesFormFavoriteAsync();

                foreach (GeoCacheModel loadGeoCacheModel in loadGeoCacheModels)
                {
                    if (loadGeoCacheModel.Code == addGeoCacheModel.Code) 
                    {
                        int index = loadGeoCacheModels.IndexOf(loadGeoCacheModel);
                        loadGeoCacheModels.Remove(loadGeoCacheModel);
                        loadGeoCacheModels.Insert(index, addGeoCacheModel);
                        hasUpdated = true;
                        break;
                    }
                }

                if (!hasUpdated)
                {
                    loadGeoCacheModels.Add(addGeoCacheModel);
                }
                String saveGeoCachesJsonString = JsonConvert.SerializeObject(loadGeoCacheModels);
                WriteToFileAsync(FileName, saveGeoCachesJsonString);
            }
        }

        public async void UpdateGeoCacheInFavoriteAsync(GeoCacheModel updateGeoCacheModel)
        {
            if (updateGeoCacheModel != null)
            {
                bool hasUpdated = false;
                List<GeoCacheModel> loadGeoCacheModels = await FindAllGeoCachesFormFavoriteAsync();

                foreach (GeoCacheModel loadGeoCacheModel in loadGeoCacheModels)
                {
                    if (loadGeoCacheModel.Code == updateGeoCacheModel.Code) 
                    {
                        int index = loadGeoCacheModels.IndexOf(loadGeoCacheModel);
                        loadGeoCacheModels.Remove(loadGeoCacheModel);
                        loadGeoCacheModels.Insert(index, updateGeoCacheModel);
                        hasUpdated = true;
                        break;
                    }
                }

                if (hasUpdated)
                {
                    String saveGeoCachesJsonString = JsonConvert.SerializeObject(loadGeoCacheModels);
                    WriteToFileAsync(FileName, saveGeoCachesJsonString);
                }
            }
        }

        /// <summary>
        /// Removes an certain geo cache from the favorite geo cache list.
        /// </summary>
        /// <param name="geoCacheModel"></param>
        public async void DeleteGeoCacheFromFavoriteAsync(GeoCacheModel deleteGeoCacheModel)
        {
            if (deleteGeoCacheModel != null)
            {
                List<GeoCacheModel> loadGeoCacheModels = await FindAllGeoCachesFormFavoriteAsync();

                foreach (GeoCacheModel loadGeoCacheModel in loadGeoCacheModels)
                {
                    if (loadGeoCacheModel.Code == deleteGeoCacheModel.Code)
                    {
                        int index = loadGeoCacheModels.IndexOf(loadGeoCacheModel);
                        loadGeoCacheModels.Remove(loadGeoCacheModel);
                        break;
                    }
                }

                String saveGeoCachesJsonString = JsonConvert.SerializeObject(loadGeoCacheModels);
                WriteToFileAsync(FileName, saveGeoCachesJsonString);
                
            }
        }

        /// <summary>
        /// clears the list of favorites
        /// </summary>
        public void DeleteAllGeoCachesFromFavoriteAsync()
        {
            List<GeoCacheModel> loadGeoCacheModels = new List<GeoCacheModel>();
            
            String saveGeoCachesJsonString = JsonConvert.SerializeObject(loadGeoCacheModels);
            WriteToFileAsync(FileName, saveGeoCachesJsonString);            
        }

        private async void WriteToFileAsync(String fileName, String jsonString)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            // Write data to a file
            StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, jsonString);
        }


        private async Task<String> ReadFromFileAsync(String fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            String jsonString = null;

            try
            {
                StorageFile file = await localFolder.GetFileAsync(fileName);
                jsonString = await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {

            }
            return jsonString;
        }
    }
}
