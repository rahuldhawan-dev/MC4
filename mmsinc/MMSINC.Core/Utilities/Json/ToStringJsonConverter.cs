using System;
using Newtonsoft.Json;

namespace MMSINC.Utilities.Json
{
    public class ToStringJsonConverter : JsonConverter
    {
        #region Properties

        public override bool CanRead => false;

        #endregion

        #region Exposed Methods

        public override bool CanConvert(Type objectType) => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
