using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace GeoCacheingFinder.Domain
{
    class GeoCacheDetailModel
    {
        public GeoCacheDetailModel() { }
        
        public GeoCacheDetailModel(String jsonString)
            : this(JsonObject.Parse(jsonString))
        { }

        public GeoCacheDetailModel(IJsonValue jsonValue)
            : this(jsonValue.GetObject())
        { }

        public GeoCacheDetailModel(JsonObject jsonObject)
        {
            this.Code = jsonObject.GetNamedString("code", "");
            this.Name = jsonObject.GetNamedString("name", "");
            this.Location = jsonObject.GetNamedString("location", "");
            this.Type = jsonObject.GetNamedString("type", "");
            this.Status = jsonObject.GetNamedString("status", "");
            this.Distance  = jsonObject.GetNamedNumber("distance", 0d).ToString();
        
        }
        //Properties
        /// <summary>
        /// Name of the geocache set by the author.
        /// </summary>
        private string _name;
        public string Name
        { 
            get { return _name; }
            set { _name = value; } 
        }
        /// <summary>
        /// Unique identifer of each geocache. 
        /// Consists of letters and numbers. 
        /// </summary>
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// latitude|longitude
        /// 52.3871|13.0993 => Babelsberg, Potsdam
        /// </summary>
        private string _location;
        public string Location
        {
            get { return _location; }
            set 
            { 
                _location = value;

                // extract latitude and longitude double values from location string 
                String[] splitValue = value.Split(new Char[] { '|' });
                this.Latitude = splitValue.GetValue(new int[] { 0 }).ToString();
                this.Longitude = splitValue.GetValue(new int[] { 1 }).ToString();
            }
        }
        /// <summary>
        /// Latitude of the given geolocation.
        /// Not given by API
        /// </summary>
        private string _latitude;
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// Longitude of the given geolocation.
        /// Not given by API
        /// </summary>
        private string _longitude;
        public string  Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        /// <summary>
        /// distance from the current location to the cache.
        /// </summary>
        private string _distance;
        public string Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        /// <summary>
        /// Kind of the geocache.
        /// e.g. "traditional"
        /// </summary>
        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Does the geocache is available.
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        public override string ToString()
        {
            return "code: " + Code + ", name: " + Name + ", loaction: " + Location + 
                ", distance: " + Distance + ", type: " + Type + ", status: " + Status;
        }
    }
}
