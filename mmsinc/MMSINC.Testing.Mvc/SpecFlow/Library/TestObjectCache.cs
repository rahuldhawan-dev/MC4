using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.StepDefinitions;

namespace MMSINC.Testing.SpecFlow.Library
{
    [Serializable]
    public class TestObjectCache : SerializableDictionary<string, SerializableDictionary<string, object>>
    {
        #region Private Members

        private static TestObjectCache _instance;

        #endregion

        #region Properties

        public static TestObjectCache Instance
        {
            get { return _instance ?? (_instance = new TestObjectCache()); }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Why is this being nulled?");
                }

                _instance = value;
            }
        }

        #endregion

        #region Constructors

        public TestObjectCache() { }

        public TestObjectCache(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Exposed Methods

        public Dictionary<string, object> EnsureDictionary(string type)
        {
            if (ContainsKey(type))
            {
                return this[type];
            }

            var ret = new SerializableDictionary<string, object>();
            Add(type, ret);
            return ret;
        }

        public object Lookup(string type, string name)
        {
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (type.EndsWith(":"))
            {
                throw new Exception(string.Format(
                    "The object cache does not contain any instance of type '{0}', probably because you left a : after it on a step that doesn't require it.",
                    type));
            }

            if (!ContainsKey(type))
            {
                throw new Exception(
                    String.Format(
                        "The object cache dictionary does not contain any instances of type '{0}'.",
                        type));
            }

            if (!this[type].ContainsKey(name))
            {
                throw new Exception(
                    String.Format(
                        "The object cache dictionary does not contain a named instance '{0}' of type '{1}'.",
                        name,
                        type));
            }

            return this[type][name];
        }

        /// <summary>
        /// Returns the property value for a named object.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public string LookupValue(string type, string name, string property)
        {
            var namedItem = Lookup(type, name);
            return (property == "ToString"
                ? namedItem.ToString()
                : namedItem.GetPropertyValueByName(property).ToString());
        }

        public object GetOrNull(string key, NameValueCollection nvc)
        {
            return GetOrNull(key, key, nvc);
        }

        public object GetOrNull(string type, string key, NameValueCollection nvc)
        {
            return String.IsNullOrEmpty(nvc[key]) ? null : Lookup(type, nvc[key]);
        }

        public object GetValueOrDefault<TEntityFactory>(string key, NameValueCollection nvc)
            where TEntityFactory : TestDataFactory
        {
            return GetValueOrDefault<TEntityFactory>(key, key, nvc);
        }

        public object GetValueOrDefault<TEntityFactory>(string type, string key, NameValueCollection nvc)
            where TEntityFactory : TestDataFactory
        {
            return String.IsNullOrEmpty(nvc[key]) ? typeof(TEntityFactory) : Lookup(type, nvc[key]);
        }

        public TObject Lookup<TObject>(string type, string name)
        {
            return (TObject)Lookup(type, name);
        }

        public static void Reset()
        {
            _instance = new TestObjectCache();
        }

        #endregion
    }
}
