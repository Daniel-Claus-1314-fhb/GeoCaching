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
            this.Radius = 10;
            this.Latitude = "52.3871";
            this.Longitude = "13.0993";
        }

        public SearchOptionViewModel(int radius, string longitude, string latitude, bool usesGPS) 
        {
            this.Radius = radius;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.UsesGPS = usesGPS;
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
        /// <summary>
        /// 
        /// </summary>
        private bool _usesGPS;
        public bool UsesGPS
        {
            get { return _usesGPS; }
            set { this.SetProperty(ref this._usesGPS, value); }
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
