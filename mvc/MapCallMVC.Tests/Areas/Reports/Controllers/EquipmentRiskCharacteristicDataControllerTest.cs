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
    public class EquipmentRiskCharacteristicDataControllerTest : MapCallMvcControllerTestBase<EquipmentRiskCharacteristicDataController, Facility>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchEquipmentRiskCharacteristicData.State)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchEquipmentRiskCharacteristicData.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
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
                const RoleModules role = RoleModules.ProductionEquipment;
                const string path = "~/Reports/EquipmentRiskCharacteristicData/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Equipment>().Create();
            entity0.RiskCharacteristicsLastUpdatedOn = _now;
            entity0.RiskCharacteristicsLastUpdatedBy = _currentUser;
            var entity1 = GetEntityFactory<Equipment>().Create();

            var search = new SearchEquipmentRiskCharacteristicData();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                Assert.AreEqual(2, search.Count);
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.RiskCharacteristicsLastUpdatedBy, "RiskCharacteristicsLastUpdatedBy");
                helper.AreEqual(entity0.RiskCharacteristicsLastUpdatedOn, "RiskCharacteristicsLastUpdatedOn");
            }
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
