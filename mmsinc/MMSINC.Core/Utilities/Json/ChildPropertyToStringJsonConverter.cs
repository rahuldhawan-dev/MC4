using System;
using System.Collections.Concurrent;
using System.Reflection;
using MMSINC.ClassExtensions.ObjectExtensions;
using Newtonsoft.Json;

namespace MMSINC.Utilities.Json
{
    /// <summary>
    /// Use this to have your json use a property from your entity displayed instead of the default ToString
    /// E.g. [JsonConverter(typeof(MMSINC.Utilities.Json.ChildPropertyToStringJsonConverter), "APropertyName")]
    /// </summary>
    public class ChildPropertyToStringJsonConverter : JsonConverter
    {
        #region Private Members

        private readonly string _childProperty;

        private static readonly ConcurrentDictionary<(Guid ObjectGuid, string PropertyName), PropertyInfo>
            _propertyDictionary = new ConcurrentDictionary<(Guid ObjectGuid, string PropertyName), PropertyInfo>();

        #endregion

        #region Constructors

        public ChildPropertyToStringJsonConverter(string childProperty)
        {
            _childProperty = childProperty;
        }

        #endregion

        #region Exposed Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueGuid = value.GetType().GUID;

            if (!_propertyDictionary.ContainsKey((valueGuid, _childProperty)))
            {
                if (!value.HasPublicProperty(_childProperty, out var prop))
                {
                    throw new ArgumentOutOfRangeException("propertyName",
                        String.Format(
                            "Exposed instance property {0} was not found for object type {1}.",
                            _childProperty, value.GetType()));
                }

                _propertyDictionary.TryAdd((valueGuid, _childProperty), prop);
            }

            writer.WriteValue(_propertyDictionary[(valueGuid, _childProperty)].GetValue(value, null));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) => true;

        #endregion
    }
}
