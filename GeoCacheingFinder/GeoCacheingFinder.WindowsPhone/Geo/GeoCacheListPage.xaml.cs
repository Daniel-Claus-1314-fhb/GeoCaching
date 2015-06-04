using GeoCacheingFinder.Domain;
using GeoCacheingFinder.Domain.NavigationModel;
using GeoCacheingFinder.Domain.ViewModel;
using GeoCacheingFinder.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public sealed partial class GeoCacheListPage : Page
    {
        private CancellationTokenSource _cts = null;

        private ApiRequestService _apiRequestService;
        private SearchOptionViewModel _searchOptionViewModel;
        private GeoLocationService _geoLocationService;
        
        public GeoCacheListPage()
        {
            this.InitializeComponent();
            this._apiRequestService = new ApiRequestService();
            this._searchOptionViewModel = new SearchOptionViewModel();
            this._geoLocationService = new GeoLocationService();
        }
        
        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            SearchOption.DataContext = _searchOptionViewModel;
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

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchButton.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;

            List<GeoCacheModel> gcModels = await _apiRequestService.searchNearestCachesAsync(_searchOptionViewModel);
            GeoCacheList.DataContext = gcModels;

            SearchButton.IsEnabled = true;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SearchOption.Visibility = Visibility.Visible;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SearchOption.Visibility = Visibility.Collapsed;
        }
        
        private async void GPSButton_Checked(object sender, RoutedEventArgs e)
        {
            GPSButton.Content = "Cancel";
            ProgressBar.Visibility = Visibility.Visible;            

            // Get cancellation token
            _cts = new CancellationTokenSource();
            // Carry out the operation
            Geoposition pos = await _geoLocationService.FindGeoLocation((uint) this._searchOptionViewModel.GPSAccuracy, _cts);
            _cts = null;

            if (pos != null)
            {
                this._searchOptionViewModel.Latitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Latitude);
                this._searchOptionViewModel.Longitude = String.Format("{0:0.####}", pos.Coordinate.Point.Position.Longitude);
            }
            
            GPSButton.Content = "GPS";
            GPSButton.IsChecked = false;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void GPSButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        private void GeoCacheList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBox listbox = (ListBox) sender;
            GeoCacheModel selectedGeoCacheModel = (GeoCacheModel) listbox.SelectedItem;
            if (selectedGeoCacheModel != null)
            {
                DetailPageParamModel paramModel = new DetailPageParamModel(selectedGeoCacheModel.Code, _searchOptionViewModel.Latitude, _searchOptionViewModel.Longitude);
                Frame.Navigate(typeof(Geo.GeoCacheDetailPage), paramModel);
            }
        }

        private void FavoritenAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Geo.FavoritenPage));
        }
    }
}
