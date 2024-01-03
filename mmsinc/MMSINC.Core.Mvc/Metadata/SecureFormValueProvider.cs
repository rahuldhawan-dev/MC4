using System;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using System.Linq;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Responsible for reading values from the SecureFormToken during model binding.
    /// </summary>
    public class SecureFormValueProvider<TToken, TValues> : IValueProvider
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>
    {
        #region Fields

        private readonly TToken _token;

        #endregion

        #region Properties

        /// <summary>
        /// This property exists for testing purposes only.
        /// </summary>
        internal TToken Token => _token;

        #endregion

        #region Constructor

        public SecureFormValueProvider(TToken token)
        {
            token.ThrowIfNull("token");
            _token = token;
        }

        #endregion

        public bool ContainsPrefix(string prefix)
        {
            return _token.DynamicValues.Any(v => v.Key.ToLowerInvariant() == prefix.ToLowerInvariant());
        }

        public ValueProviderResult GetValue(string key)
        {
            if (!ContainsPrefix(key))
            {
                // The way ValueProviderCollection.ContainsPrefix/GetValue works is dumb.
                // If any of the registered providers return true for ContainsPrefix, then
                // GetValue is called on all of the providers until one actually returns a result.
                // VPC then checks to see if the result is null before moving on to the next provider.
                return null;
            }

            var item = _token.DynamicValues.Single(v => v.Key.ToLowerInvariant() == key.ToLowerInvariant());
            return new ValueProviderResult(item.DeserializedValue,
                item.DeserializedValue != null ? item.DeserializedValue.ToString() : String.Empty,
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
