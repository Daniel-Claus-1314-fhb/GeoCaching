using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeoCacheingFinder.Domain.ViewModel
{
    public class GeoCacheViewModel : INotifyPropertyChanged
    {
        public GeoCacheViewModel() 
        {
            this.Name = "Geo Cache konnte nicht gefunden werden.";
        }

        public GeoCacheViewModel(GeoCacheModel geoCacheModel) 
        {
            this.Name = geoCacheModel.Name;
            this.Code = geoCacheModel.Code;
            this.GcCode = geoCacheModel.GcCode;
            this.Latitude = geoCacheModel.Latitude;
            this.Longitude = geoCacheModel.Longitude;
            this.Distance = geoCacheModel.Distance;
            this.Type = geoCacheModel.Type;
            this.Status = geoCacheModel.Status;
            this.Difficulty = geoCacheModel.Difficulty;
            this.Terrain = geoCacheModel.Terrain;
            this.Url = geoCacheModel.Url;
            this.Size = geoCacheModel.Size;
            this.ShortDescription = geoCacheModel.ShortDescription;
            this.Description = geoCacheModel.Description;
            this.Bearing = geoCacheModel.Bearing;
        }
        
        //Properties
        /// <summary>
        /// Name of the geocache set by the author.
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref this._name, value); }
        }
        /// <summary>
        /// Unique identifer of each geocache. 
        /// Consists of letters and numbers. 
        /// </summary>
        private string _code;
        public string Code
        {
            get { return _code; }
            set { this.SetProperty(ref this._code, value); }
        }
        /// <summary>
        /// Unique identifer of each geocache. 
        /// Consists of letters and numbers. 
        /// </summary>
        private string _gcCode;
        public string GcCode
        {
            get { return _gcCode; }
            set { this.SetProperty(ref this._gcCode, value); }
        }
        /// <summary>
        /// Latitude of the given geolocation.
        /// Not given by API
        /// </summary>
        private string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set { this.SetProperty(ref this._latitude, value); }
        }
        /// <summary>
        /// Longitude of the given geolocation.
        /// Not given by API
        /// </summary>
        private string _longitude;
        public string Longitude
        {
            get { return _longitude; }
            set { this.SetProperty(ref this._longitude, value); }
        }
        /// <summary>
        /// distance from the current location to the cache.
        /// </summary>
        private string _distance;
        public string Distance
        {
            get { return _distance; }
            set { this.SetProperty(ref this._distance, value); }
        }
        /// <summary>
        /// Kind of the geocache.
        /// e.g. "traditional"
        /// </summary>
        private string _type;
        public string Type
        {
            get { return _type; }
            set { this.SetProperty(ref this._type, value); }
        }
        /// <summary>
        /// Size of the geo cache
        /// </summary>
        private string _size;
        public string Size
        {
            get { return _size; }
            set { this.SetProperty(ref this._size, value); }
        }
        /// <summary>
        /// Does the geocache is available.
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set { this.SetProperty(ref this._status, value); }
        }
        /// <summary>
        /// Difficulty of finding the geo cache. 
        /// </summary>
        private string _difficulty;
        public string Difficulty
        {
            get { return _difficulty; }
            set { this.SetProperty(ref this._difficulty, value); }
        }
        /// <summary>
        /// Difficulty of terrain.
        /// </summary>
        private string _terrain;
        public string Terrain
        {
            get { return _terrain; }
            set { this.SetProperty(ref this._terrain, value); }
        }
        /// <summary>
        /// Short description of the geo cache.
        /// </summary>
        private string _shortDescription;
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { this.SetProperty(ref this._shortDescription, value); }
        }
        /// <summary>
        /// Description of the geo cache.
        /// </summary>
        private string _description;
        public string Description
        {
            get { return _description; }
            set { this.SetProperty(ref this._description, value); }
        }
        /// <summary>
        /// URL to the web page of the geo cache.
        /// </summary>
        private string _url;
        public string Url
        {
            get { return _url; }
            set { this.SetProperty(ref this._url, value); }
        }
        /// <summary>
        /// Bearing from the current location to the location of the geo cache.
        /// </summary>
        private string _bearing;
        public string Bearing
        {
            get { return _bearing; }
            set { this.SetProperty(ref this._bearing, value); }
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
