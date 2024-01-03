using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class OperatingCenterSpoilRemovalCostControllerTest : MapCallMvcControllerTestBase<OperatingCenterSpoilRemovalCostController, OperatingCenterSpoilRemovalCost>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            var state = GetEntityFactory<State>().Create();
            options.CreateValidEntity = () => {
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                    State = state
                });
                return GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create(new {
                    OperatingCenter = operatingCenter
                });
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateOperatingCenterSpoilRemovalCost)vm;
                model.State = state.Id;
            };
        }

        #endregion
        
        #region Roles
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Edit/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Update/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/New/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Create/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Index/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Show/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/OperatingCenterSpoilRemovalCost/Search/", RoleModules.FieldServicesWorkManagement, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var eq1 = GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create();
            var eq2 = GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create();
            var search = new SearchOperatingCenterSpoilRemovalCost();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchOperatingCenterSpoilRemovalCost)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create();
            var entity1 = GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create();
            var search = new SearchOperatingCenterSpoilRemovalCost();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Cost, "Cost");
                helper.AreEqual(entity1.Cost, "Cost", 1);
            }
        }

        #endregion

        #region Edit/Upadate

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });

            var eq = GetEntityFactory<OperatingCenterSpoilRemovalCost>().Create(new {
                OperatingCenter = operatingCenter
            });

            const int costValue = 1500;
            _target.Update(_viewModelFactory.BuildWithOverrides<EditOperatingCenterSpoilRemovalCost, OperatingCenterSpoilRemovalCost>(eq, new {
                Cost = costValue
            }));

            Assert.AreEqual(costValue, eq.Cost);
        }

        #endregion
    }
}
