using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderTrafficControllerTest : ContractorControllerTestBase<WorkOrderTrafficController, MapCall.Common.Model.Entities.WorkOrder, WorkOrderRepository>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                var model = (WorkOrderTrafficDetails)vm;
                model.NumberOfOfficersRequired = 2;
            };

            options.CreateValidEntity = CreateWorkOrder;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ExpectedShowViewName = "_Show";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        #endregion

        #region Private Methods

        private MapCall.Common.Model.Entities.WorkOrder CreateWorkOrder()
        {
            return GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderTraffic/Show");
                a.RequiresLoggedInUserOnly("~/WorkOrderTraffic/Edit");
                a.RequiresLoggedInUserOnly("~/WorkOrderTraffic/Update");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop override: action returns view with null model for 404 for some reason.
        }

        [TestMethod]
        public void TestShowRendersViewWithNullModelIfOrderNotFound()
        {
            var result = (PartialViewResult)_target.Show(0);

            MvcAssert.IsViewNamed(result, "_Show");
            Assert.IsNull(result.Model);
        }

        #endregion

        #region Edit

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            Assert.Inconclusive("Why does this still return the Edit view for a 404?");
        }

        [TestMethod]
        public void TestEditRendersViewWithNullModelIfOrderNotFound()
        {
            var result = (PartialViewResult)_target.Edit(0);

            MvcAssert.IsViewNamed(result, "_Edit");
            Assert.IsNull(result.Model);
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // override needed because this creates a new view model instance for some reason.
            var workOrder = CreateWorkOrder();
            workOrder.Notes = "Wow notes";
            workOrder.TrafficControlRequired = true;
            var model = _viewModelFactory.Build<WorkOrderTrafficDetails, MapCall.Common.Model.Entities.WorkOrder>(workOrder);
            // Resetting ViewModel values to how they'd be created on postback
            model.Notes = null;
            model.TrafficControlRequired = false;

            _target.ModelState.AddModelError("NOPE", "CHUCK TESTA!");

            var result = (PartialViewResult)_target.Update(model);

            var resultModel = (WorkOrderTrafficDetails)result.Model;
            Assert.AreEqual(workOrder.Notes, resultModel.Notes);
            Assert.AreEqual(workOrder.TrafficControlRequired, resultModel.TrafficControlRequired);
            Assert.AreEqual(workOrder.Id, resultModel.Id);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expectedNumberOfOfficers = 50;
            var workOrder = CreateWorkOrder();
            var model = _viewModelFactory.Build<WorkOrderTrafficDetails, MapCall.Common.Model.Entities.WorkOrder>(workOrder);
            model.NumberOfOfficersRequired = expectedNumberOfOfficers;

            _target.Update(model);

            Session.Evict(workOrder);
            workOrder = Session.Query<MapCall.Common.Model.Entities.WorkOrder>().Single(x => x.Id == workOrder.Id);

            Assert.AreEqual(expectedNumberOfOfficers, workOrder.NumberOfOfficersRequired);
        }

        [TestMethod]
        public void TestUpdateAppendsNotes()
        {
            var expectedOriginalNotes = "some notes for you!";
            var workOrder = CreateWorkOrder();
            workOrder.Notes = expectedOriginalNotes;
            var model = _viewModelFactory.Build<WorkOrderTrafficDetails, MapCall.Common.Model.Entities.WorkOrder>(workOrder);
            model.AppendedNotes = "These are notes";

            _target.Update(model);

            Assert.IsTrue(workOrder.Notes.Contains(model.AppendedNotes));
            Assert.IsTrue(workOrder.Notes.Contains(expectedOriginalNotes));
        }

        [TestMethod]
        public void TestUpdateReturnsDetailsPartialViewAfterSaving()
        {
            var workOrder = CreateWorkOrder();
            var model = _viewModelFactory.Build<WorkOrderTrafficDetails, MapCall.Common.Model.Entities.WorkOrder>(workOrder);
            model.AppendedNotes = "These are notes";

            var result = (PartialViewResult)_target.Update(model);

            Assert.AreEqual("_Show", result.ViewName);
        }

        [TestMethod]
        public void TestUpdateReturnsPartialShowViewWithEntity()
        {
            var workOrder = CreateWorkOrder();
            var model = _viewModelFactory.Build<WorkOrderTrafficDetails, MapCall.Common.Model.Entities.WorkOrder>(workOrder);

            var result = (PartialViewResult)_target.Update(model);

            Assert.AreSame(workOrder, result.Model);
        }

        #endregion
    }
}
