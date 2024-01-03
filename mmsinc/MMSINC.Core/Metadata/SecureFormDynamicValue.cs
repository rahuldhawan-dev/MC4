using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MMSINC.Data;

namespace MMSINC.Metadata
{
    public abstract class SecureFormDynamicValueBase<TThis, TToken> : ISecureFormDynamicValue<TThis, TToken>
        where TThis : ISecureFormDynamicValue<TThis, TToken>
        where TToken : ISecureFormToken<TToken, TThis>
    {
        #region Private Members

        private Lazy<object> _deserializedValue;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Key { get; set; }
        public virtual string XmlValue { get; protected set; }
        public virtual string Type { get; set; }
        public virtual TToken SecureFormToken { get; set; }

        public virtual object DeserializedValue => _deserializedValue.Value;

        public virtual object Value
        {
            set
            {
                var valueType =
                    value != null ? value.GetType() : typeof(object);
                XmlValue = Serialize(value, valueType);
                Type = valueType.FullName;
            }
        }

        #endregion

        protected SecureFormDynamicValueBase()
        {
            _deserializedValue =
                new Lazy<object>(() => Deserialize(XmlValue, Type));
        }

        #region Private Methods

        private static object Deserialize(string value, string typeName)
        {
            var type = System.Type.GetType(typeName);
            using (var stringReader = new StringReader(value))
            using (var xmlReader = new XmlTextReader(stringReader))
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(xmlReader);
            }
        }

        private static string Serialize(object value, Type type = null)
        {
            type = type ?? value.GetType();
            var serializer = new XmlSerializer(type);
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return $"\"{Key}\" - \"{XmlValue}\"";
        }

        #endregion
    }

    public class SecureFormDynamicValue : SecureFormDynamicValueBase<SecureFormDynamicValue, SecureFormToken> { }

    public interface ISecureFormDynamicValue<TThis, TToken> : IEntity
        where TThis : ISecureFormDynamicValue<TThis, TToken>
        where TToken : ISecureFormToken<TToken, TThis>
    {
        #region Abstract Properties

        int Id { get; }
        string Key { get; set; }
        string XmlValue { get; }
        string Type { get; set; }
        object DeserializedValue { get; }
        TToken SecureFormToken { get; set; }
        object Value { set; }

        #endregion
    }
}
