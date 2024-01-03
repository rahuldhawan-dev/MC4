using System;
using System.Collections.Specialized;
using System.ComponentModel;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class QueryStringWrapper : IQueryString
    {
        #region Private Members

        private readonly NameValueCollection _innerQueryString;

        #endregion

        #region Properties

        #region Operators

        public string this[string key]
        {
            get { return GetValue(key); }
        }

        #endregion

        #endregion

        #region Constructors

        public QueryStringWrapper(NameValueCollection queryString)
        {
            _innerQueryString = queryString;
        }

        #endregion

        #region Exposed Methods

        public string GetValue(string key)
        {
            return _innerQueryString[key];
        }

        public TValue GetValue<TValue>(string key)
        {
            return GetValue(key).ChangeType<TValue>();
        }

        public TValue GetValue<TValue>(string key, Func<string, TValue> fn)
        {
            return fn(GetValue(key));
        }

        #endregion
    }

    internal static class StringExtensions
    {
        #region Extension Methods

        internal static TReturn ChangeType<TReturn>(this string value)
        {
            return (TReturn)value.ChangeType(typeof(TReturn));
        }

        private static object ChangeType(this string value, Type newType)
        {
            if (newType.IsGenericType &&
                newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (String.IsNullOrEmpty(value))
                    return null;

                var converter = new NullableConverter(newType);
                newType = converter.UnderlyingType;
            }

            return Convert.ChangeType(value, newType);
        }

        #endregion
    }
}
