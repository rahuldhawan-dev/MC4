using System;
using Newtonsoft.Json;

namespace MMSINC.Utilities.Json
{
    public class YesNoBooleanConverter : JsonConverter
    {
        #region Exposed Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string toWrite;

            if (value is bool b)
            {
                toWrite = b ? "Yes" : "No";
            }
            else
            {
                toWrite = null;
            }

            writer.WriteValue(toWrite);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool) || objectType == typeof(bool?);
        }

        #endregion
    }
}
