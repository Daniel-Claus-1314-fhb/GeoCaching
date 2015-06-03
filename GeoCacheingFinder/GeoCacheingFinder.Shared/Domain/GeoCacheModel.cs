using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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
            this.Code = jsonObject.GetNamedString(CodeKey, "");
            this.GcCode = jsonObject.GetNamedString(GcCodeKey, "");
            this.Name = jsonObject.GetNamedString(NameKey, "");
            this.Location = jsonObject.GetNamedString(LocationKey, "");
            this.Type = jsonObject.GetNamedString(TypeKey, "");
            this.Status = jsonObject.GetNamedString(StatusKey, "");
            this.Distance = jsonObject.GetNamedNumber(DistanceKey, 0d).ToString();
            this.Bearing = jsonObject.GetNamedString(BearingKey, "");
            this.Size = jsonObject.GetNamedString(SizeKey, "");
            this.Difficulty = jsonObject.GetNamedNumber(DifficultyKey, 0d).ToString();
            this.Terrain = jsonObject.GetNamedNumber(TerrainKey, 0d).ToString();
            this.ShortDescription = jsonObject.GetNamedString(ShortDescriptionKey, "");
            this.Url = jsonObject.GetNamedString(UrlKey, "");

            string descriptionWithHtmlTags = jsonObject.GetNamedString(DescriptionKey, "");
            this.Description = removeHtmlTags(descriptionWithHtmlTags);
        }

        private String removeHtmlTags(String input)
        {
            string replacement = " ";
            Regex rgx = new Regex("(\\<[^\\>]*\\>)|(\\n)|(\\s+)");
            Regex rgx2 = new Regex("\\s+");
            input = rgx.Replace(input, replacement);
            return rgx2.Replace(input, replacement);
        }

        //Properties
        /// <summary>
        /// Name of the geocache set by the author.
        /// </summary>
        private string _name;
        public readonly string NameKey = "name";
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
        public readonly string CodeKey = "code";
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// Unique identifer of each geocache. 
        /// Consists of letters and numbers. 
        /// </summary>
        private string _gcCode;
        public readonly string GcCodeKey = "gc_code";
        public string GcCode
        {
            get { return _gcCode; }
            set { _gcCode = value; }
        }
        /// <summary>
        /// latitude|longitude
        /// 52.3871|13.0993 => Babelsberg, Potsdam
        /// </summary>
        private string _location;
        public readonly string LocationKey = "location";
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
        public readonly string DistanceKey = "distance";
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
        public readonly string TypeKey = "type";
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Size of the geo cache
        /// </summary>
        private string _size;
        public readonly string SizeKey = "size2";
        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// Does the geocache is available.
        /// </summary>
        private string _status;
        public readonly string StatusKey = "status";
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// Difficulty of finding the geo cache. 
        /// </summary>
        private string _difficulty;
        public readonly string DifficultyKey = "difficulty";
        public string Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }
        /// <summary>
        /// Difficulty of terrain.
        /// </summary>
        private string _terrain;
        public readonly string TerrainKey = "terrain";
        public string Terrain
        {
            get { return _terrain; }
            set { _terrain = value; }
        }
        /// <summary>
        /// Short description of the geo cache.
        /// </summary>
        private string _shortDescription;
        public readonly string ShortDescriptionKey = "short_description";
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { _shortDescription = value; }
        }
        /// <summary>
        /// Description of the geo cache.
        /// </summary>
        private string _description;
        public readonly string DescriptionKey = "description";
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// URL to the web page of the geo cache.
        /// </summary>
        private string _url;
        public readonly string UrlKey = "url";
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        /// <summary>
        /// Bearing from the current location to the location of the geo cache.
        /// </summary>
        private string _bearing;
        public readonly string BearingKey = "bearing2";
        public string Bearing
        {
            get { return _bearing; }
            set { _bearing = value; }
        }

        public override string ToString()
        {
            return "code: " + Code + ", name: " + Name + ", loaction: " + Location + 
                ", distance: " + Distance + ", type: " + Type + ", status: " + Status;
        }
    }
}
