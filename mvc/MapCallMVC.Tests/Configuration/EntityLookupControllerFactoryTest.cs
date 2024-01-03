using MapCallMVC.Configuration;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class EntityLookupControllerFactoryTest
    {
        #region Fields

        private EntityLookupControllerFactory _target;
        private MapCallMvcApplicationTester _app;
        private FakeMvcHttpHandler _handler;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = MapCallMvcApplicationTester.InitializeDummyObjectFactory();
            _container.Inject<IViewModelFactory>(_container.GetInstance<ViewModelFactory>());
            _app = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _handler = _app.CreateRequestHandler();
            _target = _container.GetInstance<EntityLookupControllerFactory>();
            _target.Assemblies.Add(GetType().Assembly);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCreateControllerReturnsExpectedControllerType()
        {
            var result = _target.CreateController(_handler.RequestContext, "TestEntityLookup");
            Assert.IsInstanceOfType(result,
                typeof(EntityLookupController<MMSINC.Data.NHibernate.IRepository<TestEntityLookup>, TestEntityLookup>));
        }

        [TestMethod]
        public void TestCanHandleControllerReturnsFalseWhenControllerNameDoesNotMatchKnownEntityLookupType()
        {
            Assert.IsFalse(_target.CanHandleController(_handler.RequestContext, "ThisIsANoNo"));
        }

        [TestMethod]
        public void TestCanHandleControllerReturnsTrueWhenControllerNameDoesMatchKnownEntityLookupType()
        {
            Assert.IsTrue(_target.CanHandleController(_handler.RequestContext, "TestEntityLookup"));
        }

        [TestMethod]
        public void TestCanHandleControllerIsNotCaseSensitive()
        {
            Assert.IsTrue(_target.CanHandleController(_handler.RequestContext, "testentitylookup"));
        }
        

        #endregion

        #region Test Class
        
        private class TestEntityLookup : EntityLookup
        {
            
        }

        #endregion
    }
}
