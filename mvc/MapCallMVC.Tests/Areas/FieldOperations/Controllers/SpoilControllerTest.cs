using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SpoilControllerTest : MapCallMvcControllerTestBase<SpoilController, Spoil>
    {
        #region Setup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

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
                a.RequiresLoggedInUserOnly("~/Spoil/New");
                a.RequiresLoggedInUserOnly("~/Spoil/Create");
                a.RequiresLoggedInUserOnly("~/Spoil/Edit");
                a.RequiresLoggedInUserOnly("~/Spoil/Update");
                a.RequiresLoggedInUserOnly("~/Spoil/Show");
                a.RequiresLoggedInUserOnly("~/Spoil/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreateCreatesSpoil()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var spoilStorageLocation = GetFactory<SpoilStorageLocationFactory>().Create();

            var model = new CreateSpoil(_container) {
                WorkOrder = workOrder.Id,
                Quantity = 12.0M,
                SpoilStorageLocation = spoilStorageLocation.Id
            };

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (Spoil)result.Model;

            Assert.AreEqual(12.0M, resultModel.Quantity);
            Assert.AreEqual(spoilStorageLocation.Id, resultModel.SpoilStorageLocation?.Id);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.Add);

            var result = (PartialViewResult)_target.New(workOrder.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CreateSpoil));
            Assert.AreEqual(((CreateSpoil)result.Model).WorkOrder, workOrder.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = 17.0M;
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var spoilStorageLocation = GetFactory<SpoilStorageLocationFactory>().Create();
            var spoil = GetEntityFactory<Spoil>().Create(new {
                WorkOrder = workOrder,
                SpoilStorageLocation = spoilStorageLocation,
                Quantity = 22M
            });

            var model = _viewModelFactory.BuildWithOverrides<EditSpoil, Spoil>(spoil, new {
                Quantity = expected
            });
            
            var result = (PartialViewResult)_target.Update(model);
            var resultMaterialUsed = (Spoil)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(expected, resultMaterialUsed.Quantity);
        }

        #endregion
    }
}
