using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Validation;
using Moq;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class DynamicModelValidatorProviderTest
    {
        #region Private Members

        private DynamicModelValidatorProvider _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new DynamicModelValidatorProvider();
        }

        #endregion

        [TestMethod]
        public void TestGetValidatorsReturnsEmptyEnumerableIfMetadataIsNotLinkedModelMetadata()
        {
            var controllerContext = new ControllerContext();
            var provider = new Mock<ModelMetadataProvider>();
            var container = new Metadata.TestModel();
            Func<String> func = () => container.Foo;
            var containerType = typeof(Metadata.TestModel);
            var metadata = new ModelMetadata(provider.Object, containerType, func, typeof(Metadata.TestModel),
                "ToString");

            var result = _target.GetValidators(metadata, controllerContext);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TestGetValidatorsReturnsEmptyEnumerableIfContainerIsNull()
        {
            var controllerContext = new ControllerContext();
            var provider = new Mock<ModelMetadataProvider>();
            var container = new TestModelWithoutContainer();
            Func<String> func = () => container.Foo;
            var containerType = typeof(TestModelWithoutContainer);
            var metadata = new LinkedModelMetadata(provider.Object, containerType, func,
                typeof(TestModelWithoutContainer), "ToString", null);

            var result = _target.GetValidators(metadata, controllerContext);

            Assert.IsNull(metadata.ContainerModel as IDynamicModel);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TestGetValidatorsReturnsValidators()
        {
            var controllerContext = new ControllerContext();
            var provider = new Mock<ModelMetadataProvider>();
            var container = new TestDynamicModel();
            Func<String> func = () => container.Foo;
            Func<TestDynamicModel> containerAccessor = () => container;
            var containerType = typeof(TestDynamicModel);
            var metadata = new LinkedModelMetadata(provider.Object, containerType, func, typeof(TestDynamicModel),
                "ToString", containerAccessor);

            var result = _target.GetValidators(metadata, controllerContext).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(typeof(RequiredAttributeAdapter), result.First().GetType());
        }
    }

    internal class TestModelWithoutContainer
    {
        public virtual string Foo { get; set; }

        public void OnPreValidating()
        {
            //noop
        }

        public IEnumerable<ModelValidator> GetValidators(DynamicValidationContext validationContext)
        {
            yield return new RequiredAttributeAdapter(
                validationContext.ModelMetadata,
                validationContext.ControllerContext,
                new RequiredAttribute {ErrorMessage = "ERROR'D"});
        }

        public override String ToString()
        {
            return string.Empty;
        }
    }

    internal class TestDynamicModel : IDynamicModel
    {
        public virtual string Foo { get; set; }
        public string container;

        public void OnPreValidating()
        {
            //noop
        }

        public IEnumerable<ModelValidator> GetValidators(DynamicValidationContext validationContext)
        {
            yield return new RequiredAttributeAdapter(
                validationContext.ModelMetadata,
                validationContext.ControllerContext,
                new RequiredAttribute {ErrorMessage = "ERROR'D"});
        }

        public override String ToString()
        {
            return string.Empty;
        }
    }
}
