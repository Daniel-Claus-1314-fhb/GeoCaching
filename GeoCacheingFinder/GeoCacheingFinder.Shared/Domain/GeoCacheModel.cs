using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace GeoCacheingFinder.Domain
{
    public class GeoCacheModel
    {
        public GeoCacheModel() { }
        public GeoCacheModel(String Name, String Code, String Location, String Type, String Status)
        {
            this.Name = Name;
            this.Code = Code;
            this.Location = Location;
            this.Type = Type;
            this.Status = Status;
        }

        public GeoCacheModel(String jsonString)
            : this(JsonObject.Parse(jsonString))
        { }

        public GeoCacheModel(IJsonValue jsonValue)
            : this(jsonValue.GetObject())
        { }

        public GeoCacheModel(JsonObject jsonObject)
        {
            this.Code = jsonObject.GetNamedString("code", "");
            this.Name = jsonObject.GetNamedString("name", "");
            this.Location = jsonObject.GetNamedString("location", "");
            this.Type = jsonObject.GetNamedString("type", "");
            this.Status = jsonObject.GetNamedString("status", "");

            // extract latitude and longitude double values from location string 
            String[] splitValue = jsonObject.GetNamedString("location", "").Split(new Char[] { '|' });
            String lat = splitValue.GetValue(new int[] { 0 }).ToString();
            String lon = splitValue.GetValue(new int[] { 1 }).ToString();
            double doubleResult = 0d;
            if (Double.TryParse(lat, out doubleResult))
            {
                this.Latitude = doubleResult;
            }
            doubleResult = 0d;
            if (Double.TryParse(lon, out doubleResult))
            {
                this.Longitude = doubleResult;
            }
        }

        /// <summary>
        /// Name of the geocache set by the author.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifer of each geocache. 
        /// Consists of letters and numbers. 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// latitude|longitude
        /// 52.3871|13.0993 => Babelsberg, Potsdam
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Latitude of the given geolocation.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude of the given geolocation.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// distance from the current location to the cache.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Kind of the geocache.
        /// e.g. "traditional"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Does the geocache is available.
        /// </summary>
        public string Status { get; set; }

        public override string ToString()
        {
            return "code: " + Code + ", name: " + Name + ", loaction: " + Location + ", distance: " + Distance + ", type: " + Type + ", status: " + Status;
        }
    }
}
