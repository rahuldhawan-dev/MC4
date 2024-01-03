using System;
using System.Web.Mvc;

namespace MMSINC.Core.MvcTest.Metadata
{
    /// <summary>
    ///  Mock needed for testing metadata attributes
    /// </summary>
    public class MockModelMetadata : ModelMetadata
    {
        #region Constructors

        public MockModelMetadata(ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor,
            Type modelType, string propertyName)
            : base(provider, containerType, modelAccessor, modelType, propertyName) { }

        #endregion
    }
}
