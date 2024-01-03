using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MaterialUsedControllerTest : MapCallMvcControllerTestBase<MaterialUsedController, MaterialUsed>
    {
        #region Setup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create();
                GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.UserAdministrator);
                return GetEntityFactory<MaterialUsed>().Build(new { WorkOrder = workOrder, NonStockDescription = "Some non-stock description" });
            };

            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.NewReturnsPartialView = true;
            options.ExpectedNewViewName = "_New";
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
            options.ExpectedShowViewName = "_Show";
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MaterialUsed/New");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Create");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Edit");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Update");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Show");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreateCreatesAMaterialUsedWithStockMaterials()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var material = GetFactory<MaterialFactory>().Create();
            var stockLocation = GetFactory<StockLocationFactory>().Create();

            var model = new CreateMaterialUsed(_container) {
                Material = material.Id,
                WorkOrder = workOrder.Id,
                Quantity = 2,
                StockLocation = stockLocation.Id
            };

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (MaterialUsed)result.Model;

            Assert.AreEqual(2, resultModel.Quantity);
            Assert.AreEqual(material.Id, resultModel.Material?.Id);
            Assert.AreEqual(stockLocation.Id, resultModel.StockLocation?.Id);
        }

        [TestMethod]
        public void TestCreateCreatesAMaterialUsedWithNonStockMaterials()
        {
            var expected = "Testing Non-Stock";
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.Add);

            var model = new CreateMaterialUsed(_container) {
                WorkOrder = workOrder.Id,
                Quantity = 2,
                NonStockDescription = expected
            };

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (MaterialUsed)result.Model;

            Assert.AreEqual(2, resultModel.Quantity);
            Assert.IsNull(resultModel.Material);
            Assert.IsNull(resultModel.StockLocation);
            Assert.AreEqual(expected, resultModel.NonStockDescription);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.Add);

            var result = (PartialViewResult)_target.New(workOrder.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CreateMaterialUsed));
            Assert.AreEqual(((CreateMaterialUsed)result.Model).WorkOrder, workOrder.Id);
            Assert.AreEqual(((CreateMaterialUsed)result.Model).OperatingCenter, workOrder.OperatingCenter.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = "Testing Non-Stock";
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var materialUsed = GetEntityFactory<MaterialUsed>().Create(new {
                WorkOrder = workOrder
            });

            var model = _viewModelFactory.BuildWithOverrides<EditMaterialUsed, MaterialUsed>(materialUsed, new {
                NonStockDescription = expected
            });

            var result = (PartialViewResult)_target.Update(model);
            var resultMaterialUsed = (MaterialUsed)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(expected, resultMaterialUsed.NonStockDescription);
        }

        #endregion
    }
}
