using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace GeoCacheingFinder.Domain
{
    class GeoCacheModel
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

    class GeoCacheCodes 
    {
        private const string results = "results";
        private const string seperator = "|";

        public GeoCacheCodes() { }

        public GeoCacheCodes(List<String> geoCacheCodes)
        {
            this.Codes = geoCacheCodes;
        }

        public GeoCacheCodes(String jsonString) : this()
        {
            Codes = new List<String>();
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(results, new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.String)
                {
                    Codes.Add(jsonValue.GetString());
                }
            }
        }

        public List<String> Codes;

        public override string ToString()
        {
            String strValue = "";

            foreach (String Code in this.Codes)
            {
                strValue = strValue + Code + seperator;
            }

            if (strValue.Length > 0)
            {
                int strLength = strValue.Length;
                strValue = strValue.Substring(0, strLength - 1);
            }

            return strValue;
        }
    }
}
