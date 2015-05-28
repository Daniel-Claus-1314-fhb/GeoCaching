using GeoCacheingFinder.Domain;
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
using System.Threading.Tasks;
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
        private ApiRequestService _apiRequestService;
        private SearchOptionViewModel _searchOptionViewModel;
        
        public GeoCacheListPage()
        {
            this.InitializeComponent();
            this._apiRequestService = new ApiRequestService();
            _searchOptionViewModel = new SearchOptionViewModel();
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
            List<GeoCacheModel> gcModels = await _apiRequestService.searchNearestCachesAsync(_searchOptionViewModel);
            GeoCacheList.DataContext = gcModels;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SearchOption.Visibility = Visibility.Visible;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SearchOption.Visibility = Visibility.Collapsed;
        }
    }
}
