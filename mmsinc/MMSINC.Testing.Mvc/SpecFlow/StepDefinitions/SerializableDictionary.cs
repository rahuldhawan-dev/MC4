using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        #region Constructors

        public SerializableDictionary() { }

        protected SerializableDictionary(SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
