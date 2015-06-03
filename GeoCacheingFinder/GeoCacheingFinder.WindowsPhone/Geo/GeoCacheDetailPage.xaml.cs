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
    public sealed partial class GeoCacheDetailPage : Page
    {
        private CancellationTokenSource _cts;
        private ApiRequestService _apiRequestService;
        private GeoCacheViewModel _geoCacheViewModel;
        private GeoLocationService _geoLocationService;

        public GeoCacheDetailPage()
        {
            this.InitializeComponent();
            this._apiRequestService = new ApiRequestService();
            this._geoCacheViewModel = new GeoCacheViewModel();
            this._geoLocationService = new GeoLocationService();
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if (e.Parameter != null)
            {
                ProgressBar.Visibility = Visibility.Visible;
                DetailPageParamModel paramModel = (DetailPageParamModel)e.Parameter;
                _geoCacheViewModel = await FindGeoCacheDetailsAsync(paramModel.Code, paramModel.Latitude, paramModel.Longitude);
                ProgressBar.Visibility = Visibility.Collapsed;
            }

            DetailView.DataContext = _geoCacheViewModel;
        }

        private async Task<GeoCacheViewModel> FindGeoCacheDetailsAsync(String code, String latitude, String longitude)
        {
            
            GeoCacheModel geoCacheModel = await this._apiRequestService.findCacheByCodeAsync(code, latitude, longitude);
            if (geoCacheModel != null)
            {
                return new GeoCacheViewModel(geoCacheModel);
            }
            else
            {
                return new GeoCacheViewModel();
            }
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
                Geoposition pos = await _geoLocationService.FindGeoLocation((uint)25, _cts);
                _cts = null;

                if (pos != null)
                {
                    String latitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Latitude);
                    String longitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Longitude);
                    _geoCacheViewModel = await FindGeoCacheDetailsAsync(_geoCacheViewModel.Code, latitude, longitude);
                    DetailView.DataContext = _geoCacheViewModel;
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
