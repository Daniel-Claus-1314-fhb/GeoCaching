using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace GeoCacheingFinder.Domain
{
    class GeoCacheCodesModel
    {
        private const string results = "results";
        private const string seperator = "|";

        public GeoCacheCodesModel() { }

        public GeoCacheCodesModel(List<String> geoCacheCodes)
        {
            this.Codes = geoCacheCodes;
        }

        public GeoCacheCodesModel(String jsonString)
            : this(JsonObject.Parse(jsonString))
        { }

        public GeoCacheCodesModel(JsonObject jsonObject)
        {
            Codes = new List<String>();
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(results, new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.String)
                {
                    Codes.Add(jsonValue.GetString());
                }
            }
        }

        /// <summary>
        /// List of geo cache codes
        /// </summary>
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
                strValue = strValue.Substring(0, strValue.Length - 1);
            }
            return strValue;
        }
    }
}
