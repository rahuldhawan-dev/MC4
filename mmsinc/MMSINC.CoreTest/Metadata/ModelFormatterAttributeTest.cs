using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.CoreTest.Metadata
{
    [TestClass]
    public class ModelFormatterAttributeTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(new Container()));
        }

        #region Tests

        [TestMethod]
        public void TestTryGetAttributeFromModelMetadataReturnsInstanceAddedToMetadataByProcessMethod()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var target = new TestFormatAttribute();
            target.Process(metadata);
            var result = ModelFormatterAttribute.TryGetAttributeFromModelMetadata(metadata);
            Assert.AreSame(target, result);
        }

        [TestMethod]
        public void TestTryGetAttributeFromModelMetadataReturnsNullIfNotFoundInMetadata()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            Assert.IsNull(ModelFormatterAttribute.TryGetAttributeFromModelMetadata(metadata));
        }

        #endregion

        #region Test classes

        private class TestFormatAttribute : ModelFormatterAttribute
        {
            public override string FormatValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
