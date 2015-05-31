using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeoCacheingFinder.Domain.ViewModel
{
    public class SearchOptionViewModel : INotifyPropertyChanged
    {
        // Constructor
        public SearchOptionViewModel() 
        {
            this.Radius = 25;
            this.GPSAccuracy = 50;
            this.Latitude = "52.3871";
            this.Longitude = "13.0993";
        }

        public SearchOptionViewModel(int radius, string longitude, string latitude, int gpsAccuracy) 
        {
            this.Radius = radius;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.GPSAccuracy = gpsAccuracy;
        }

        // Properties
        /// <summary>
        /// Search radius around the given geolocation.
        /// </summary>
        private int _radius;
        public int Radius 
        {
            get { return _radius; }
            set { this.SetProperty(ref this._radius, value); }
        }
        /// <summary>
        /// Accuray of the detected geo location.
        /// </summary>
        private int _gpsAccuracy;
        public int GPSAccuracy 
        {
            get { return _gpsAccuracy; }
            set { this.SetProperty(ref this._gpsAccuracy, value); }
        }
        /// <summary>
        /// Latitude of the given geolocation.
        /// </summary>
        private string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set { this.SetProperty(ref this._latitude, value); }
        }
        /// <summary>
        /// Longitude of the given geolocation.
        /// </summary>
        private string _longitude;
        public string Longitude
        {
            get { return _longitude; }
            set { this.SetProperty(ref this._longitude, value); }
        }

        // property changed logic by jump start
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
