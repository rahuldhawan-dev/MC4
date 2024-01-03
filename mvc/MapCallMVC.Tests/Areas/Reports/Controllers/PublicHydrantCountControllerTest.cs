using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class PublicHydrantCountControllerTest : MapCallMvcControllerTestBase<PublicHydrantCountController, Hydrant, HydrantRepository>
    {
        #region Init/Cleanup

        protected override MapCall.Common.Model.Entities.Users.User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var role = RoleModules.FieldServicesAssets;

                a.RequiresRole("~/Reports/PublicHydrantCount/Index", role);
                a.RequiresRole("~/Reports/PublicHydrantCount/Search", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var hydrant = GetFactory<HydrantFactory>().Create(new {
                HydrantBilling = typeof(PublicHydrantBillingFactory)
            });
            var search = new SearchPublicHydrantCountReport { OperatingCenter = hydrant.OperatingCenter.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithNameAndModel(result, "Index", search);
            Assert.AreEqual(1, search.Count);
        }

        [TestMethod]
        public void TestIndexRespondsToExcel()
        {
            var hydrant = GetFactory<HydrantFactory>().Create(new
            {
                HydrantBilling = typeof(PublicHydrantBillingFactory),
                PremiseNumber = "123456"
            });
            var search = new SearchPublicHydrantCountReport { OperatingCenter = hydrant.OperatingCenter.Id };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                var viewModel = search.Results.Single();
                helper.AreEqual(viewModel.FireDistrict, "FireDistrict");
                helper.AreEqual(viewModel.OperatingCenter, "OperatingCenter");
                helper.AreEqual(viewModel.PremiseNumber, "PremiseNumber");
                helper.AreEqual(viewModel.Status, "Status");
                helper.AreEqual(viewModel.Total, "Total");
                helper.AreEqual(viewModel.Town, "Town");
            }
        }

        #endregion
    }
}
