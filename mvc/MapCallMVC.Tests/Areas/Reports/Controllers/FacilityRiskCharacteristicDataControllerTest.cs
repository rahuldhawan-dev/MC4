using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class FacilityRiskCharacteristicDataControllerTest : MapCallMvcControllerTestBase<FacilityRiskCharacteristicDataController, Facility>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchFacilityRiskCharacteristicData.TownState)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchFacilityRiskCharacteristicData.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        protected override User CreateUser()
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
                const RoleModules role = RoleModules.ProductionFacilities;
                const string path = "~/Reports/FacilityRiskCharacteristicData/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Facility>().Create();
            var entity1 = GetEntityFactory<Facility>().Create();
            var search = new SearchFacilityRiskCharacteristicData();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.FacilityName, "FacilityName");
                helper.AreEqual(entity1.FacilityName, "FacilityName", 1);
            }
        }

        #endregion
    }
}
