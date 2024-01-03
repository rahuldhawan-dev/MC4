using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Validation;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class DynamicModelBinderTest
    {
        #region Tests

        [TestMethod]
        public void TestOnModelUpdatedCallsModelsOnPreValidatingMethod()
        {
            // Mocks weren't allowing set for ModelMetadata or ModelMetadata.Model
            // Had to setup all this to get the method called.
            var target = new TestDynamicModelBinder();
            var controllerContext = new ControllerContext();
            var bindingContext = new ModelBindingContext();
            var provider = new Mock<ModelMetadataProvider>();
            var container = new TestModel();
            Func<String> func = () => container.Foo;
            var containerType = typeof(TestModel);
            var modelMetadata = new ModelMetadata(provider.Object, containerType, func, typeof(TestModel), "Foo");
            bindingContext.ModelMetadata = modelMetadata;
            modelMetadata.Model = container;

            target.TestOnModelUpdated(controllerContext, bindingContext);

            Assert.IsTrue(container.PreValidatingCalled);
            Assert.IsTrue(target.ModelUpdated);
        }

        #endregion
    }

    internal class TestDynamicModelBinder : DynamicModelBinder
    {
        public bool ModelUpdated { get; set; }

        internal void TestOnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
            ModelUpdated = true;
        }
    }

    internal class TestModel : IDynamicModel
    {
        public virtual bool PreValidatingCalled { get; set; }
        public virtual string Foo { get; set; }
        public string container;

        public void OnPreValidating()
        {
            PreValidatingCalled = true;
        }

        public IEnumerable<ModelValidator> GetValidators(DynamicValidationContext validationContext)
        {
            return Enumerable.Empty<ModelValidator>();
        }

        public override String ToString()
        {
            return string.Empty;
        }
    }
}
