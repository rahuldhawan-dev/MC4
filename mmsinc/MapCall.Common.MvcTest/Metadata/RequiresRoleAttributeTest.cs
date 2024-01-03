using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.Common.MvcTest.Metadata
{
    [TestClass]
    public class RequiresRoleAttributeTest : InMemoryDatabaseTest<Module>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IModuleRepository>().Use<ModuleRepository>();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestSingleParameterConstructorDefaultsToReadAction()
        {
            Assert.AreEqual(RoleActions.Read, new RequiresRoleAttribute(RoleModules.FieldServicesReports).Action);
        }

        [TestMethod]
        public void TestConstructorSetsModuleProperty()
        {
            var target = new RequiresRoleAttribute(RoleModules.FieldServicesReports, RoleActions.Add);
            Assert.AreEqual(RoleModules.FieldServicesReports, target.Module);
        }

        [TestMethod]
        public void TestConstructorSetsActionProperty()
        {
            var target = new RequiresRoleAttribute(RoleModules.FieldServicesReports, RoleActions.Add);
            Assert.AreEqual(RoleActions.Add, target.Action);
        }

        #endregion

        #endregion
    }
}
