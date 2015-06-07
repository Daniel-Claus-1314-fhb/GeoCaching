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
    public sealed partial class SearchListPage : Page
    {
        private ApiRequestService _apiRequestService;
        private SearchOptionViewModel _searchOptionViewModel;
        private OptionStorageService _optionStorageService;

        public SearchListPage()
        {
            this.InitializeComponent();
            this._apiRequestService = new ApiRequestService();
            this._searchOptionViewModel = new SearchOptionViewModel();
            this._optionStorageService = new OptionStorageService();
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            _searchOptionViewModel = _optionStorageService.loadSearchOptionFromSetting();
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

        private void GeoCacheList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBox listbox = (ListBox)sender;
            GeoCacheModel selectedGeoCacheModel = (GeoCacheModel)listbox.SelectedItem;
            if (selectedGeoCacheModel != null)
            {
                DetailPageParamModel paramModel = new DetailPageParamModel(selectedGeoCacheModel.Code, selectedGeoCacheModel.IsFavorite, 
                    _searchOptionViewModel.Latitude, _searchOptionViewModel.Longitude);
                Frame.Navigate(typeof(Geo.GeoCacheDetailPage), paramModel);
            }
        }

        private void FavoritenAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Geo.FavoriteListPage));
        }

        private async void SearchAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            SearchAppBarButton.IsEnabled = false;

            List<GeoCacheModel> gcModels = await _apiRequestService.searchNearestCachesAsync(_searchOptionViewModel);
            GeoCacheList.DataContext = gcModels;

            ProgressBar.Visibility = Visibility.Collapsed;
            SearchAppBarButton.IsEnabled = true;
        }

        private void OptionAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Geo.OptionPage));
        }
    }
}
