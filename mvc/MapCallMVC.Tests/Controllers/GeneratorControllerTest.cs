using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class GeneratorControllerTest : MapCallMvcControllerTestBase<GeneratorController, Generator>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentModelRepository>().Use<EquipmentModelRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "Equipment", area = "", id = vm.Equipment };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "Equipment", area = "", id = vm.Equipment };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionFacilities;
                a.RequiresRole("~/Generator/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Generator/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Generator/New", module, RoleActions.Add);
                a.RequiresRole("~/Generator/Create", module, RoleActions.Add);
            });
        }
      
    }
}
