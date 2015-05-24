using System;
using System.Collections.Generic;
using System.Text;

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
    }

    class GeoCacheCodes 
    {
        public GeoCacheCodes() { }
        public GeoCacheCodes(List<String> results)
        {
            this.results = results;
        }

        public List<String> results;
    }
}
