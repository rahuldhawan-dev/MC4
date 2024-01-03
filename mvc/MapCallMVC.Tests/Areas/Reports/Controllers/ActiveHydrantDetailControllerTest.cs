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
    public class ActiveHydrantDetailControllerTest : MapCallMvcControllerTestBase<ActiveHydrantDetailController, Hydrant, HydrantRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            // This needs to exist.
            GetFactory<ActiveAssetStatusFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/ActiveHydrantDetail/Index", role);
                a.RequiresRole("~/Reports/ActiveHydrantDetail/Search", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var billing1 = GetFactory<PublicHydrantBillingFactory>().Create();
            var billing2 = GetFactory<PrivateHydrantBillingFactory>().Create();

            var hydrantOPC1Billing1 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1, HydrantBilling = billing1 });
            var hydrantOPC1Billing2 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1, HydrantBilling = billing2 });

            var search = new SearchActiveHydrantDetailReport { OperatingCenter = opc1.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");

            Assert.AreEqual(2, search.Count);
        }


        #endregion
    }
}
