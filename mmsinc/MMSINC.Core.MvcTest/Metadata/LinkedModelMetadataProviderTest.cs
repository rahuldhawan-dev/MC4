using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class LinkedModelMetadataProviderTest
    {
        #region Fields

        private TestLinkedModelMetadataProvider _target;

        // For whatever reason, the Target property of a delegate is set to the test instance,
        // and not the container object. To mimic MVC's modelAccessor, we need a container
        // field to access. I don't claim to get it. -Ross 7/28/2012.
        public object container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestLinkedModelMetadataProvider();
        }

        #endregion

        [TestMethod]
        public void TestCreateMetadataReturnsLinkedModelMetadata()
        {
            var attr = new MockAttribute();
            var attributes = new[] {attr};

            var result = _target.TestCreateMetadata(attributes, typeof(MockViewModel), null,
                typeof(MockViewModel), "Property");

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(LinkedModelMetadata), result.GetType());
        }

        [TestMethod]
        public void TestContainerModelReturnsExpectedModel()
        {
            var model = new MockViewModel();
            var container = new MockViewModelContainer();
            //container = c;
            container.Property = model;
            model.container = container;
            //var testModel = new TestModel();
            //var model = new TestLinkedViewModel(testModel);
            Func<Object> func = () => container.Property;

            var result = (LinkedModelMetadata)_target.TestCreateMetadata(Enumerable.Empty<Attribute>(),
                typeof(MockViewModelContainer), func,
                typeof(MockViewModel), "Property");

            Assert.AreSame(container, result.ContainerModel);
        }

        private Func<object> NotSoAnonymousModelAccessor()
        {
            return () => container.ToString();
        }

        [TestMethod]
        public void TestContainerModelThrowsExceptionIfNotNullAndTypeIsNotContainerType()
        {
            //  var model = new MockViewModel();
            var c = new TestBrokenViewModel();
            container = c;

            // Working theory is that Visual Studio 15.5.2 update changed the way lambdas are compiled/how they're used.
            //
            // Something changed in how the Delegate.Target is specified for the below commented out line.
            // It seems like there was a change in how lambda functions created inside methods get their Target
            // property values. It used to be that the Target would be an instance of the class that created
            // the lambda(LinkedModelMetadataProviderTest in this case). This changed. Now it seems that whatever
            // class references and invokes it then becomes the Target(LinkedModelMetadata in this case). I
            // don't understand why the change was made and I can not find information about it.
            //Func<Object> modelAccessor = () => container.ToString();

            Func<Object> modelAccessor = NotSoAnonymousModelAccessor; // () => container.ToString();

            var result = (LinkedModelMetadata)_target.TestCreateMetadata(Enumerable.Empty<Attribute>(),
                typeof(MockViewModelContainer), modelAccessor,
                typeof(MockViewModelContainer), "Property");

            MyAssert.Throws<InvalidCastException>(() => {
                var x = result.ContainerModel;
            });
        }

        #region Test classes

        private class TestLinkedModelMetadataProvider : LinkedModelMetadataProvider
        {
            internal ModelMetadata TestCreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
                Func<object> modelAccessor, Type modelType, string propertyName)
            {
                return CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            }
        }

        private class MockViewModelContainer
        {
            [Mock]
            public MockViewModel Property { get; set; }
        }

        private class MockViewModel
        {
            public object container;
        }

        internal class TestBrokenViewModel
        {
            public TestBrokenViewModel()
            {
                container = this;
            }

            public TestBrokenViewModel container;
        }

        private class MockAttribute : Attribute { }

        #endregion
    }
}
