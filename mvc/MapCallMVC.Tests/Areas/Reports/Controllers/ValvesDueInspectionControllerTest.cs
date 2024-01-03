using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ValvesDueInspectionControllerTest : MapCallMvcControllerTestBase<ValvesDueInspectionController, Valve, ValveRepository>
    {
        #region Init/Cleanup

        protected override MapCall.Common.Model.Entities.Users.User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        #endregion
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/ValvesDueInspection/Index");
                a.RequiresLoggedInUserOnly("~/Reports/ValvesDueInspection/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            var valveControls = GetFactory<SomeValveControlFactory>().Create();
            var valveBilling = GetFactory<PublicValveBillingFactory>().Create();
            var activeValveStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var val1 = GetFactory<ValveFactory>().Create(new { OperatingCenter = opc1, Status = activeValveStatus, ValveZone = valveZones[6], ValveControls = valveControls, ValveBilling = valveBilling});
            var val2 = GetFactory<ValveFactory>().Create(new { OperatingCenter = opc2, Status = activeValveStatus, ValveZone = valveZones[6], ValveControls = valveControls, ValveBilling = valveBilling });
            
            var search = new SearchValvesDueInspectionReport { OperatingCenter = new[] { opc1.Id }};
            var result = _target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithNameAndModel(result, "Index", search);
            Assert.AreEqual(1, search.Count);
        }
    }
}