using GeoCacheingFinder.Domain;
using GeoCacheingFinder.Domain.NavigationModel;
using GeoCacheingFinder.Domain.ViewModel;
using GeoCacheingFinder.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkID=390556 dokumentiert.

namespace GeoCacheingFinder.Geo
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private CancellationTokenSource _cts;
        private ApiRequestService _apiRequestService;
        private GeoCacheViewModel _geoCacheViewModel;
        private GeoCacheModel _geoCacheModel;
        private GeoLocationService _geoLocationService;
        private CacheStorageService _cacheStorageService;
        private OptionStorageService _optionStorageService;
        private SearchOptionViewModel _searchOptionViewModel;

        public DetailPage()
        {
            this.InitializeComponent();
            this._apiRequestService = new ApiRequestService();
            this._geoCacheViewModel = new GeoCacheViewModel();
            this._geoLocationService = new GeoLocationService();
            this._cacheStorageService = new CacheStorageService();
            this._optionStorageService = new OptionStorageService();
            this._searchOptionViewModel = new SearchOptionViewModel();
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            _searchOptionViewModel = _optionStorageService.LoadSearchOptionFromSetting();

            if (e.Parameter != null)
            {
                ProgressBar.Visibility = Visibility.Visible;
                DetailPageParamModel paramModel = (DetailPageParamModel)e.Parameter;

                if (paramModel.IsFavorite)
                    _geoCacheModel = await _cacheStorageService.FindGeoCacheByCodeFormFavoriteAsync(paramModel.Code);
                else
                    _geoCacheModel = await FindGeoCacheDetailsAsync(paramModel.Code, _searchOptionViewModel.Latitude, _searchOptionViewModel.Longitude);

                SetDetailViewState(_geoCacheModel.IsFavorite);
                _geoCacheViewModel = new GeoCacheViewModel(_geoCacheModel);
                DetailView.DataContext = _geoCacheViewModel;

                ProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SetDetailViewState(bool isFavoriteState)
        {
            if (isFavoriteState)
            {
                SaveButton.IsEnabled = false;
                DeleteButton.IsEnabled = true;
            }
            else
            {
                SaveButton.IsEnabled = true;
                DeleteButton.IsEnabled = false;
            }
        }

        private async Task<GeoCacheModel> FindGeoCacheDetailsAsync(String code, String latitude, String longitude)
        {
            GeoCacheModel geoCacheModel = await this._apiRequestService.findCacheByCodeAsync(code, latitude, longitude);
            return geoCacheModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            base.OnNavigatedFrom(e);
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;

            if (frame == null)
            {
                return;
            }

            if (frame != null && frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _geoCacheModel.IsFavorite = true;
            _cacheStorageService.AddGeoCacheToFavoriteAsync(_geoCacheModel);
            SetDetailViewState(_geoCacheModel.IsFavorite);
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _geoCacheModel.IsFavorite = false;
            _cacheStorageService.DeleteGeoCacheFromFavoriteAsync(_geoCacheModel);
            SetDetailViewState(_geoCacheModel.IsFavorite);
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            RefreshButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Visible;

            if (_geoCacheViewModel != null && _geoCacheViewModel.Code != null)
            {
                // Carry out the operation
                _cts = new CancellationTokenSource();
                Geoposition pos = await _geoLocationService.FindGeoLocation((uint)_searchOptionViewModel.GPSAccuracy, _cts);
                _cts = null;

                if (pos != null)
                {
                    String latitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Latitude);
                    String longitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Longitude);
                    _geoCacheModel = await FindGeoCacheDetailsAsync(_geoCacheViewModel.Code, latitude, longitude);
                    _cacheStorageService.UpdateGeoCacheInFavoriteAsync(_geoCacheModel);
                    _geoCacheViewModel = new GeoCacheViewModel(_geoCacheModel);
                    DetailView.DataContext = _geoCacheViewModel;
                }

                if (_geoCacheModel != null)
                {
                    _cacheStorageService.UpdateGeoCacheInFavoriteAsync(_geoCacheModel);
                }
            }


            RefreshButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void AbbrechenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }
    }
}
