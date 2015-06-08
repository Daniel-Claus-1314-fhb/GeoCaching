using System;
using System.Collections.Generic;
using System.Text;

namespace GeoCacheingFinder.Domain.NavigationModel
{
    public class DetailPageParamModel
    {

        public DetailPageParamModel() { }
        
        public DetailPageParamModel(String code, bool isFavorite)
        {
            this.Code = code;
            this.IsFavorite = isFavorite;
        }

        private String _code;
        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { _isFavorite = value; }
        }
    }
}
