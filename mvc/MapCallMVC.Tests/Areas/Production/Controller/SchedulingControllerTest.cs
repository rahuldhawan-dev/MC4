using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class SchedulingControllerTest : MapCallMvcControllerTestBase<SchedulingController, ProductionWorkOrder, ProductionWorkOrderRepository>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Production/Scheduling/Search", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/Production/Scheduling/Index", role, RoleActions.UserAdministrator);
            });
        }

        #endregion
    }
}