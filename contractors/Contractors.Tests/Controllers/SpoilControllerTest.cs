using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MMSINC.Data;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class SpoilControllerTest : ContractorControllerTestBase<SpoilController, Spoil, SpoilRepository>
    {
        #region Private Members

        private MapCall.Common.Model.Entities.WorkOrder _workOrder;
        private Spoil _spoil;
        private SpoilStorageLocation _storageLocation;

        #endregion

        #region Setup

        protected override ContractorUser CreateUser()
        {
            var opCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            _automatedTestOperatingCenter = opCenter;
            var contractor = GetFactory<ContractorFactory>().Create(new { OperatingCenters = new[] { opCenter } });
            var user = GetFactory<ContractorUserFactory>().Create(new { Contractor = contractor });
            _workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = user.Contractor, OperatingCenter = opCenter });
            _workOrder.OperatingCenter = opCenter;
            _storageLocation =
                GetFactory<SpoilStorageLocationFactory>().Create(new {
                    OperatingCenter = opCenter
                });
            _spoil = GetFactory<SpoilFactory>().Create(new { WorkOrder = _workOrder, SpoilStorageLocation = _storageLocation });
            return user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var opCenter = _currentUser.Contractor.OperatingCenters.Single();
                _workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor, OperatingCenter = opCenter });
                _workOrder.OperatingCenter = opCenter;
                _storageLocation =
                    GetFactory<SpoilStorageLocationFactory>().Create(new
                    {
                        OperatingCenter = opCenter
                    });
                return GetFactory<SpoilFactory>().Create(new { WorkOrder = _workOrder, SpoilStorageLocation = _storageLocation });
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (SpoilNew)vm;
                model.WorkOrder = _workOrder.Id;
            };
            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ExpectedNewViewName = "_New";
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        #endregion

        #region Tests

        #region Authorize

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Spoil/New");
                a.RequiresLoggedInUserOnly("~/Spoil/Create");
                a.RequiresLoggedInUserOnly("~/Spoil/Edit");
                a.RequiresLoggedInUserOnly("~/Spoil/Update");
                a.RequiresLoggedInUserOnly("~/Spoil/Index");
                a.RequiresLoggedInUserOnly("~/Spoil/Destroy");
            });
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateThrowsDomainLogicExceptionIfWorkOrderDoesNotExist()
        {
            var model = _viewModelFactory.Build<SpoilNew>();
            model.WorkOrder = 0;
            MyAssert.Throws<DomainLogicException>(() => _target.Create(model));
        }

        [TestMethod]
        public void TestCreateReturnsNewPartialViewWithModelIfModelStateIsInvalid()
        {
            var model = _viewModelFactory.Build<SpoilNew>();
            model.WorkOrder = _workOrder.Id;
            _target.ModelState.AddModelError("No", "No");

            var result = (PartialViewResult)_target.Create(model);
            MvcAssert.IsPartialView(result);
            Assert.AreEqual("_New", result.ViewName);
            Assert.AreSame(model, result.Model);
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: Index does not act like typical Index action.
        }

        [TestMethod]
        public void TestIndexReturnsPartialViewWithWorkOrderModelForWorkOrderID()
        {
            var result = (PartialViewResult)_target.Index(_workOrder.Id);
            MvcAssert.IsPartialView(result);
            Assert.AreEqual("_Index", result.ViewName);
            Assert.AreEqual(_workOrder.Id, ((MapCall.Common.Model.Entities.WorkOrder)result.Model).Id);
        }

        [TestMethod]
        public void TestIndexReturns404NotFoundIfWorkOrderDoesNotExist()
        {
            var result = (HttpNotFoundResult)_target.Index(int.MaxValue);
            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action.
            var result = (PartialViewResult)_target.New(_workOrder.Id);
            MvcAssert.IsPartialView(result);
            var model = (SpoilNew)result.Model;
            Assert.AreEqual(_workOrder.Id, model.WorkOrder);
        }

        [TestMethod]
        public void TestNewReturns404NotFoundIfWorkOrderDoesNotExist()
        {
            MvcAssert.IsStatusCode(404, SpoilController.NO_SUCH_WORK_ORDER, _target.New(int.MaxValue));
        }

        [TestMethod]
        public void TestNewSetsStorageLocationsInViewData()
        {
            _target.New(_workOrder.Id);
            var result = (IEnumerable<SelectListItem>)_target.ViewData[SpoilController.VIEWDATA_SPOIL_STORAGE_LOCATION];

            Assert.IsNotNull(result);
            Assert.IsTrue(
                result.Any(i => i.Value == _storageLocation.Id.ToString()));
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var model = _container.GetInstance<EditSpoil>();
            model.Map(_spoil);
            model.Id = _spoil.Id;
            model.Quantity = (decimal)22.2;
            model.SpoilStorageLocation = _storageLocation.Id;

            var result = (PartialViewResult)_target.Update(model);
            var entity = (Spoil)result.Model;

            MvcAssert.IsPartialView(result);
            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Quantity, entity.Quantity);
            Assert.AreEqual(model.SpoilStorageLocation, entity.SpoilStorageLocation.Id);
        }

        [TestMethod]
        public void TestUpdateReturnsEditViewIfModelStateIsInvalid()
        {
            var model = _viewModelFactory.Build<EditSpoil, Spoil>(_spoil);
            _target.ModelState.AddModelError("Error", "ERROR!");
            var result = (PartialViewResult)_target.Update(model);
            Assert.AreEqual("_Edit", result.ViewName);
        }

        #endregion

        #endregion
    }
}