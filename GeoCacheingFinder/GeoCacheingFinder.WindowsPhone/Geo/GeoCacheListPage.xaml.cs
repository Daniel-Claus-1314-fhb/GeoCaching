using GeoCacheingFinder.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public GeoCacheListPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            SetTestData();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            base.OnNavigatedFrom(e);
        }

        private void SetTestData()
        {
            List<GeoCacheModel> gcModels = new List<GeoCacheModel>();
            GeoCacheModel gcModel1 = new GeoCacheModel("Erster Cache", "RE33GH", "53.26|13.1154", "Traditional", "Available");
            GeoCacheModel gcModel2 = new GeoCacheModel("Zweiter Cache", "GHASER", "51.566|12.1564", "Traditional", "NotAvailable");
            GeoCacheModel gcModel3 = new GeoCacheModel("Dritter Cache", "UOP99F", "51.566|12.1564", "Quest", "Available");
            gcModels.Add(gcModel1);
            gcModels.Add(gcModel2);
            gcModels.Add(gcModel3);

            GeoCacheList.DataContext = gcModels;
        }

        private async void TestRequest()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://www.opencaching.de/okapi/services/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            HttpResponseMessage response = await client.GetAsync("caches/search/nearest?consumer_key=LyLvkDtKsucTA4cUzhFv&center=52.3871%7C13.0993&radius=10");
            if (response.IsSuccessStatusCode)
            {
                String gcCodes = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(gcCodes);
            }
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

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            TestRequest();
        }
    }
}
