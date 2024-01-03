using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Configuration;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MMSINC.Core.MvcTest.Configuration
{
    [TestClass]
    public class DependencyRegistrarTest
    {
        #region Private Members

        private TestDependencyRegistrar _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new TestDependencyRegistrar();
            // ensure that we use the sqlite configuration and such
            MvcApplication.IsInTestMode = true;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            MvcApplication.IsInTestMode = false;
        }

        #endregion

        [TestMethod]
        public void TestCreateSessionFactoryCreatesTheNHibernateConfigurationAndBuildsASessionFactoryFromIt()
        {
            // TODO: test this much more thoroughly
            // it's not designed with testing in mind, so this test is
            // clunky and inaccurate.  it will probably need to be refactored,
            // especially so that inheriting classes can provide extra
            // configuration
            Assert.Inconclusive("Make this testable, and test it.");
        }

        [TestMethod]
        public void TestRegisterNHibernateRegistersVariousNHibernateTypes()
        {
            Assert.Inconclusive("Make this testable, and test it.");
        }
    }

    public class TestDependencyRegistrar : DependencyRegistrar<TestUser, TestUser>
    {
        protected override void RegisterModels(ConfigurationExpression i)
        {
            throw new System.NotImplementedException();
        }

        protected override void RegisterUtilities(ConfigurationExpression i)
        {
            throw new System.NotImplementedException();
        }
    }
}
