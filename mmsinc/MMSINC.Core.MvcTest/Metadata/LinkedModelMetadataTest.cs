using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using StructureMap;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class LinkedModelMetadataTest
    {
        #region Private Members

        private LinkedModelMetadata _target;
        private LinkedModelMetadataProvider _provider;
        private Type _containerType;
        private TestModel _container;
        private Func<object> _modelAccessor;
        private Func<object> _containerAccessor;

        #endregion

        #region Properties

        public string container
        {
            get { return "Bar"; }
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new LinkedModelMetadataProvider();
            _containerType = typeof(TestModel);
            _container = new TestModel();
            _modelAccessor = () => _container.Foo;
            _containerAccessor = () => _container;
            _target = new LinkedModelMetadata(
                _provider,
                _containerType,
                _modelAccessor,
                _containerType,
                "Foo",
                _containerAccessor
            );
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesReturnsEnumerableOfLinkedModelMetadata()
        {
            // This is the by-design way that ModelMetadata works. If that changes,
            // I want a test that can catch that. Just call _target.Properties and
            // ensure all the items are typed as LinkModelMetadaTest
            var result = _target.Properties;

            Assert.AreNotEqual(0, result.Count());
            foreach (var x in result)
                Assert.AreEqual(typeof(LinkedModelMetadata), x.GetType());
        }

        #region ContainerModel

        [TestMethod]
        public void TestContainerModelReturnsNullIfNullContainerAccessorIsPassedToConstructor()
        {
            _target = new LinkedModelMetadata(_provider, _containerType, _modelAccessor, _containerType, "Foo", null);
            Assert.IsNull(_target.ContainerModel);
        }

        // TODO: Move to LinkedModelMetadataProviderTest
        //[TestMethod]
        //public void TestContainerModelThrowsExceptionIfNotNullAndTypeIsNotContainerType()
        //{
        //    var testModel = new TestModel();
        //    var model = new TestBrokenViewModel(testModel);
        //    Func<String> func = model.ToString;
        //    _target = new LinkedModelMetadata(
        //        _provider,
        //        _containerType,
        //        func,
        //        _containerType,
        //        "Foo",
        //        _containerAccessor);
        //    MyAssert.Throws<InvalidCastException>(() => { var x = _target.ContainerModel; });
        //}

        #endregion

        #endregion
    }

    internal class TestClassWithContainerField
    {
        public string container;

        public string SomeMethod()
        {
            return string.Empty;
        }
    }

    internal class TestLinkedViewModel : MMSINC.Data.ViewModel<TestModel>
    {
        public TestLinkedViewModel(IContainer container, TestModel entity) : base(container)
        {
            if (entity != null)
            {
                Map(entity);
            }

            entityContainer = entity;
        }

        public TestModel entityContainer;
    }

    internal class TestBrokenViewModel : MMSINC.Data.ViewModel<TestModel>
    {
        public TestBrokenViewModel(IContainer container, TestModel entity) : base(container)
        {
            if (entity != null)
            {
                Map(entity);
            }

            entityContainer = this;
        }

        public TestBrokenViewModel entityContainer;
    }
}
