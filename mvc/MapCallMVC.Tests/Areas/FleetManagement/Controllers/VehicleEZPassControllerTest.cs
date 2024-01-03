using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FleetManagement.Controllers;
using MapCallMVC.Areas.FleetManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FleetManagement.Controllers
{
    [TestClass]
    public class VehicleEZPassControllerTest : MapCallMvcControllerTestBase<VehicleEZPassController, VehicleEZPass>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IVehicleRepository>().Use<VehicleRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules module = RoleModules.FleetManagementGeneral;
            Authorization.Assert(a => {
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Index/", module);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Show/", module);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/New/", module, RoleActions.Add);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FleetManagement/VehicleEZPass/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<VehicleEZPassFactory>().Create();
            var expected = "BAH-1";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<VehicleEZPassViewModel, VehicleEZPass>(eq,
                new {
                    BillingInfo = expected
                }));

            Assert.AreEqual(expected, Session.Get<VehicleEZPass>(eq.Id).BillingInfo);
        }

        #endregion

        #endregion
    }
}
