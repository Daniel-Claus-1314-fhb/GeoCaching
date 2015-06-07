using System;
using System.Collections.Generic;
using System.Text;

namespace GeoCacheingFinder.Domain.NavigationModel
{
    public class DetailPageParamModel
    {

        public DetailPageParamModel() { }

        public DetailPageParamModel(String code, bool isFavorite, String currentLatitude, String CurrentLongitude) 

        {
            this.Code = code;
            this.IsFavorite = isFavorite;
            this.Latitude = currentLatitude;
            this.Longitude = CurrentLongitude;
        }

        private String _code;
        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private String _latitude;
        public String Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private String _longitude;
        public String Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { _isFavorite = value; }
        }
    }
}
