using GeoCacheingFinder.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace GeoCacheingFinder.Service
{
    public class CacheStorageService
    {
        private const String FavoriteKey = "favoriteList";
        private const String ContainerKey = "GeoCacheContainer";

        private ApplicationDataContainer _container = null;

        /// <summary>
        /// Constructor init localsettings and container
        /// </summary>
        public CacheStorageService()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Containers.ContainsKey(FavoriteKey))
            {
                _container = localSettings.Containers[FavoriteKey];
            }
            else
            {
                _container = localSettings.CreateContainer(FavoriteKey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }
        }

        /// <summary>
        /// Delivers all geo caches which are stored as favorite.
        /// </summary>
        /// <returns></returns>
        public List<GeoCacheModel> findAllCachesFormFavorite()
        {
            List<GeoCacheModel> storedGeoCacheModels = new List<GeoCacheModel>();

            if (_container != null)
            {
                if (_container.Values.ContainsKey(FavoriteKey))
                {
                    storedGeoCacheModels = (List<GeoCacheModel>)_container.Values[FavoriteKey];
                }
            }
            return storedGeoCacheModels;
        }

        public GeoCacheModel findCacheByCodeFormFavorite(String code)
        {
            GeoCacheModel storedGeoCacheModel = null;

            if (code != null)
            {
                List<GeoCacheModel> storedGeoCacheModels = new List<GeoCacheModel>();
                if (_container != null)
                {
                    if (_container.Values.ContainsKey(FavoriteKey))
                    {
                        storedGeoCacheModels = (List<GeoCacheModel>)_container.Values[FavoriteKey];
                    }
                }

                foreach (GeoCacheModel geoCacheModel in storedGeoCacheModels)
                {
                    if (geoCacheModel.Code.Contains(code))
                    {
                        storedGeoCacheModel = geoCacheModel;
                    }
                }
            }
            return storedGeoCacheModel;
        }

        /// <summary>
        /// Adds an certain geo cache from the favorite geo cache list.
        /// </summary>
        /// <param name="geoCacheModel"></param>
        public void AddCacheToFavorite(GeoCacheModel geoCacheModel)
        {
            if (geoCacheModel != null)
            {
                if (_container != null)
                {
                    List<GeoCacheModel> storedGeoCacheModels;
                    if (_container.Values.ContainsKey(FavoriteKey))
                    {
                        storedGeoCacheModels = (List<GeoCacheModel>)_container.Values[FavoriteKey];
                    }
                    else
                    {
                        storedGeoCacheModels = new List<GeoCacheModel>();
                    }
                    storedGeoCacheModels.Add(geoCacheModel);
                    _container.Values[FavoriteKey] = storedGeoCacheModels;
                }
            }
        }

        /// <summary>
        /// Removes an certain geo cache from the favorite geo cache list.
        /// </summary>
        /// <param name="geoCacheModel"></param>
        public void removeCacheToFavorite(GeoCacheModel geoCacheModel)
        {
            if (geoCacheModel != null)
            {
                if (_container != null)
                {
                    List<GeoCacheModel> storedGeoCacheModels;
                    if (_container.Values.ContainsKey(FavoriteKey))
                    {
                        storedGeoCacheModels = (List<GeoCacheModel>)_container.Values[FavoriteKey];
                    }
                    else
                    {
                        storedGeoCacheModels = new List<GeoCacheModel>();
                    }

                    if (storedGeoCacheModels.Count > 0 && storedGeoCacheModels.Contains(geoCacheModel))
                    {
                        storedGeoCacheModels.Remove(geoCacheModel);
                        _container.Values[FavoriteKey] = storedGeoCacheModels;
                    }
                }
            }
        }
    }
}
