using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using NHibernate;
using StructureMap;

namespace MMSINC.Testing.SpecFlow.Library
{
    [Serializable]
    public class TestTypeDictionary : Dictionary<string, TestTypeRegistration>
    {
        #region Constructors

        public TestTypeDictionary() { }

        // Need the serializer constructor for the Serializable attribute to work on this.
        public TestTypeDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Private Members

        private TestTypeRegistration EnsureType(string type)
        {
            if (!ContainsKey(type))
            {
                throw new KeyNotFoundException(
                    String.Format(
                        "Type '{0}' has not been registered in RegressionTests.Steps.Data.TYPE_DICTIONARY.",
                        type));
            }

            return this[type];
        }

        #endregion

        #region Exposed Methods

        public TestTypeRegistration GetTypeRegistration(string type)
        {
            return EnsureType(type);
        }

        public void Add(string typeName, Type type,
            Func<NameValueCollection, TestObjectCache, IContainer, object> retrievalFn)
        {
            Add(typeName, new TestTypeRegistration(type, retrievalFn));
        }

        #endregion
    }

    [Serializable]
    public class TestTypeRegistration
    {
        #region Properties

        public Type Type { get; private set; }
        public Func<NameValueCollection, TestObjectCache, IContainer, object> RetrievalFn { get; private set; }
        public bool SaveToDatabase { get; set; }

        #endregion

        #region Constructors

        public TestTypeRegistration(Type type,
            Func<NameValueCollection, TestObjectCache, IContainer, object> retrievalFn)
        {
            Type = type;
            RetrievalFn = retrievalFn;
        }

        #endregion
    }
}
