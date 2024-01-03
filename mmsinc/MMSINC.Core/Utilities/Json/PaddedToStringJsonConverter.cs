using System;
using System.Linq;
using Newtonsoft.Json;

namespace MMSINC.Utilities.Json
{
    public class PaddedToStringJsonConverter : JsonConverter
    {
        public override bool CanRead => false;
        public int _length;

        public override bool CanConvert(Type objectType)
        {
            return new[] {typeof(int), typeof(long), typeof(int?), typeof(long?)}.Contains(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().PadLeft(_length, '0'));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public PaddedToStringJsonConverter(int length)
        {
            _length = length;
        }
    }
}
