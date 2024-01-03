using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class AdvancedModelMetadataProviderTest
    {
        #region Private Members

        private TestAdvancedModelMetadataProvider _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new TestAdvancedModelMetadataProvider();
        }

        #endregion

        [TestMethod]
        public void TestGetProviderReturnsProvider()
        {
            var type = typeof(String);
            var expected = new LinkedModelMetadataProvider();
            _target.Add(type, expected);

            Assert.AreEqual(expected, _target.GetProvider(type));
        }

        [TestMethod]
        public void TestAddAddsProviderToRegisteredProviders()
        {
            Assert.AreEqual(0, _target.TestRegisteredProviders.Count());

            _target.Add(typeof(String), new LinkedModelMetadataProvider());

            Assert.AreEqual(1, _target.TestRegisteredProviders.Count());
        }

        [TestMethod]
        public void TestAddThrowsExceptionIfTypeIsNull()
        {
            MyAssert.Throws(() => _target.Add(null, new LinkedModelMetadataProvider()));
        }

        [TestMethod]
        public void TestAddThrowsExceptionIfProviderIsNull()
        {
            MyAssert.Throws(() => _target.Add(typeof(String), null));
        }

        [TestMethod]
        public void TestAddThrowsInvalidOperationExceptionIfProviderEqualsThis()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.Add(typeof(String), _target));
        }

        [TestMethod]
        public void TestRemoveRemovesProviderForType()
        {
            var type = typeof(String);
            var expected = new LinkedModelMetadataProvider();
            _target.Add(type, expected);

            _target.Remove(type);
            Assert.AreEqual(0, _target.TestRegisteredProviders.Count);
        }

        [TestMethod]
        public void TestGetMetadataForPropertiesWithContainerAndTypeReturnsProviderMetadataForProperties()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var modelMetadata = new Mock<IEnumerable<ModelMetadata>>();
            var container = new object();
            var containerType = typeof(String);

            _target.Add(containerType, provider.Object);

            provider.Setup(x => x.GetMetadataForProperties(container, containerType)).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForProperties(container, containerType);

            Assert.AreSame(modelMetadata.Object, result);
        }

        [TestMethod]
        public void TestGetMetadataForPropertiesWithContainerAndTypeReturnsDefault()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var modelMetadata = new Mock<IEnumerable<ModelMetadata>>();
            var container = new object();
            var containerType = typeof(String);

            _target.Default = provider.Object;

            provider.Setup(x => x.GetMetadataForProperties(container, containerType)).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForProperties(container, containerType);

            Assert.AreSame(modelMetadata.Object, result);
        }

        [TestMethod]
        public void TestGetMetadataForPropertyWithModelAccessorTypeAndPropertyReturnsProviderMetadataForProperties()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var container = new {Foo = "bar"};
            Func<object> func = () => container.Foo;
            var containerType = typeof(String);
            var modelMetadata = new Mock<ModelMetadata>(provider.Object, containerType, func, containerType, "Foo");

            _target.Add(containerType, provider.Object);

            provider.Setup(x => x.GetMetadataForProperty(func, containerType, "Foo")).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForProperty(func, containerType, "Foo");

            Assert.AreSame(modelMetadata.Object, result);
        }

        [TestMethod]
        public void TestGetMetadataForPropertyWithModelAccessorTypeAndPropertyReturnsDefault()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var container = new {Foo = "bar"};
            Func<object> func = () => container.Foo;
            var containerType = typeof(String);
            var modelMetadata = new Mock<ModelMetadata>(provider.Object, containerType, func, containerType, "Foo");

            _target.Default = provider.Object;

            provider.Setup(x => x.GetMetadataForProperty(func, containerType, "Foo")).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForProperty(func, containerType, "Foo");

            Assert.AreSame(modelMetadata.Object, result);
        }

        [TestMethod]
        public void TestGetMetadataForTypeWithModelAccessorAndTypeReturnsProviderMetadataForProperties()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var container = new {Foo = "bar"};
            Func<object> func = () => container.Foo;
            var containerType = typeof(String);
            var modelMetadata = new Mock<ModelMetadata>(provider.Object, containerType, func, containerType, "Foo");

            _target.Add(containerType, provider.Object);

            provider.Setup(x => x.GetMetadataForType(func, containerType)).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForType(func, containerType);

            Assert.AreSame(modelMetadata.Object, result);
        }

        [TestMethod]
        public void TestGetMetadataForTypeWithModelAccessorAndTypeReturnsDefault()
        {
            var provider = new Mock<ModelMetadataProvider>();
            var container = new {Foo = "bar"};
            Func<object> func = () => container.Foo;
            var containerType = typeof(String);
            var modelMetadata = new Mock<ModelMetadata>(provider.Object, containerType, func, containerType, "Foo");

            _target.Default = provider.Object;

            provider.Setup(x => x.GetMetadataForType(func, containerType)).Returns(modelMetadata.Object);

            var result = _target.GetMetadataForType(func, containerType);

            Assert.AreSame(modelMetadata.Object, result);
        }
    }

    internal class TestAdvancedModelMetadataProvider : AdvancedModelMetadataProvider
    {
        public IDictionary<Type, ModelMetadataProvider> TestRegisteredProviders
        {
            get { return base.RegisteredProviders; }
        }
    }
}
