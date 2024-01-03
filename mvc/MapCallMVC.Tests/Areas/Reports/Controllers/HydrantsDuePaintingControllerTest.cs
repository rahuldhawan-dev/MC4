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
    public class HydrantsDuePaintingControllerTest : MapCallMvcControllerTestBase<HydrantsDuePaintingController, Hydrant, HydrantRepository>
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
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/HydrantsDuePainting/Index");
                a.RequiresLoggedInUserOnly("~/Reports/HydrantsDuePainting/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var hydrantOPC1 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1 });
            var hydrantOPC2 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc2 });

            var search = new SearchHydrantsDuePaintingReport { OperatingCenter = new[] { opc1.Id }};
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithNameAndModel(result, "Index", search);

            Assert.AreEqual(1, search.Count);
        }

        [TestMethod]
        public void TestIndexRespondsToExcel()
        {
            var hydrant = GetFactory<HydrantFactory>().Create();
            var search = new SearchHydrantsDuePaintingReport();
            search.EnablePaging = true;
            search.OperatingCenter = new[] { hydrant.OperatingCenter.Id };

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;
            Assert.IsFalse(search.EnablePaging, "EnablePaging should be disabled always.");

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                var viewModel = search.Results.Single();
                helper.AreEqual(viewModel.OperatingCenter, "OperatingCenter");
                helper.AreEqual(viewModel.Town, "Town");
                helper.AreEqual(viewModel.Count, "Count");
            }
        }

        #endregion
    }
}
