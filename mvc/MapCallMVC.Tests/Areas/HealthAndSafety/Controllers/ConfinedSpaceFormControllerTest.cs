using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class ConfinedSpaceFormControllerTest : MapCallMvcControllerTestBase<ConfinedSpaceFormController, ConfinedSpaceForm>
    {
        #region Fields

        private DataType _csfDataType;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {Employee = GetEntityFactory<Employee>().Create()});
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            // The Hazards property needs to be initialized for some of the automatic tests because
            // the property itself is set via SetDefaults. In practice, the Hazards property will never
            // be null, and if it is null then something has gone horribly wrong.
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateConfinedSpaceForm)vm;
                model.Hazards = new List<ConfinedSpaceFormHazardViewModel>();
                model.GasMonitor = GetEntityFactory<GasMonitor>().Create().Id;
                model.ProductionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create().Id;
            };  
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditConfinedSpaceForm)vm;
                model.Hazards = new List<ConfinedSpaceFormHazardViewModel>();
                model.GasMonitor = GetEntityFactory<GasMonitor>().Create().Id;
                model.ProductionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create().Id;
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (model) => {
                var viewModel = (CreateConfinedSpaceForm)model;
                Assert.AreNotEqual(0, viewModel.Id);
                return new {action = "Edit", controller = "ConfinedSpaceForm", id = viewModel.Id};
            };
            options.UpdateRedirectsToRouteOnSuccessArgs = (model) => {
                var viewModel = (EditConfinedSpaceForm)model;
                Assert.AreNotEqual(0, viewModel.Id);
                return new {action = "Edit", controller = "ConfinedSpaceForm", id = viewModel.Id};
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _csfDataType = GetEntityFactory<DataType>().Create(new {TableName = nameof(ConfinedSpaceForm) + "s"});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>()
             .Add(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.OperationsLockoutForms;

            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Search/", role);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Show/", role);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Index/", role);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/New/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/PostCompletionEdit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/PostCompletionUpdate/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/ConfinedSpaceForm/Destroy/", role, RoleActions.Delete);
			});
		}

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // noop, other tests needed to cover this.
        }

        [TestMethod]
        public void TestNewReturnsNewViewWithNewViewModelAndProductionWorkOrderInfoWhenProductionWorkOrderParamIsUsed()
        {
            var expected = 12345;
            var result = (ViewResult)_target.New(productionWorkOrderId: expected);
            var resultModel = (CreateConfinedSpaceForm)result.Model;

            Assert.AreEqual(expected, resultModel.ProductionWorkOrder);
            Assert.IsNull(resultModel.ShortCycleWorkOrderNumber, "Only the PWO value should have been set");
            Assert.IsNull(resultModel.WorkOrder, "Only the PWO value should have been set");
            MvcAssert.IsViewNamed(result, "New");
        }
        
        [TestMethod]
        public void TestNewReturnsNewViewWithNewViewModelAndShortCycleWorkOrderInfoWhenShortCycleWorkOrderParamIsUsed()
        {
            var expected = 12345;
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var pp = GetEntityFactory<PlanningPlant>().Create(new{ OperatingCenter = opc });
            var result = (ViewResult)_target.New(shortCycleWorkOrderNumber: expected);
            var resultModel = (CreateConfinedSpaceForm)result.Model;

            Assert.AreEqual(expected, resultModel.ShortCycleWorkOrderNumber);
            Assert.IsNull(resultModel.ProductionWorkOrder, "Only the SCWO value should have been set");
            Assert.IsNull(resultModel.WorkOrder, "Only the SCWO value should have been set");
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestNewReturnsNewViewWithNewViewModelAndWorkOrderInfoWhenWorkOrderParamIsUsed()
        {
            var expected = 12345;
            var result = (ViewResult)_target.New(workOrderId: expected);
            var resultModel = (CreateConfinedSpaceForm)result.Model;

            Assert.AreEqual(expected, resultModel.WorkOrder);
            Assert.IsNull(resultModel.ProductionWorkOrder, "Only the WO value should have been set");
            Assert.IsNull(resultModel.ShortCycleWorkOrderNumber, "Only the WO value should have been set");
            MvcAssert.IsViewNamed(result, "New");
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSavesANoteIfPermitCancelled()
        {
            var noteRepo = _container.GetInstance<IRepository<Note>>();
            var csf = GetEntityFactory<ConfinedSpaceForm>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateConfinedSpaceForm, ConfinedSpaceForm>(csf, new {
                IsPermitCancelledSectionSigned = true,
                PermitCancellationNote = "This is a test note and junk",
                Hazards = new System.Collections.Generic.List<ConfinedSpaceFormHazardViewModel>(),
                ProductionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create().Id
            });

            var result = (ActionResult)_target.Create(model);

            var note = noteRepo.GetAll().ToList().Last();

            Assert.AreEqual(model.PermitCancellationNote, note.Text);
            Assert.AreEqual(_currentUser.UserName, note.CreatedBy);
            Assert.AreEqual(model.Id, note.LinkedId);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var entity = GetEntityFactory<ConfinedSpaceForm>().Create();
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            var viewModel = _viewModelFactory.BuildWithOverrides<EditConfinedSpaceForm, ConfinedSpaceForm>(entity, new {
                ProductionWorkOrder = pwo.Id
            });
            viewModel.Hazards = new System.Collections.Generic.List<ConfinedSpaceFormHazardViewModel>();
            
            var result = _target.Update(viewModel);

            Assert.AreEqual(pwo.Id, Session.Get<ConfinedSpaceForm>(entity.Id).ProductionWorkOrder.Id);
        }

        [TestMethod]
        public void TestEditRedirectsToShowPageIfThePermitHasBeenCancelled()
        {
            //Assemble
            var employee = GetEntityFactory<Employee>().Create();
            var entity = GetEntityFactory<ConfinedSpaceForm>().Create(new { PermitCancelledBy = employee, IsPermitCancelledSectionSigned = true});
            var result = _target.Edit(entity.Id);
    
            //Assert
            MvcAssert.RedirectsToRoute(result, "ConfinedSpaceForm", "Show", new { id = entity.Id });
        }

        [TestMethod]
        public void TestEditRedirectsToPostCompletionEditPageIfTheFormIsComplete()
        {
            //Assemble
            var entity = GetFactory<CompletedConfinedSpaceFormFactory>().Create();
            entity.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());
            entity.CanBeControlledByVentilationAlone = true;

            //Act
            var result = _target.Edit(entity.Id);
    
            //Assert
            MvcAssert.RedirectsToRoute(result, "ConfinedSpaceForm", "PostCompletionEdit", new { id = entity.Id });
        }

        [TestMethod]
        public void TestPostCompletionEditPageHasCorrectReadingCaptureTimes()
        {
            //Assemble
            var captureTimes = GetFactory<ConfinedSpaceFormReadingCaptureTimeFactory>().CreateList(3);
            var entity = GetFactory<CompletedConfinedSpaceFormFactory>().Create();
            entity.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());
            entity.CanBeControlledByVentilationAlone = true;

            //Act
            var result = _target.PostCompletionEdit(entity.Id);

            //Assert
            var ddlData = (IEnumerable<SelectListItem>)_target.ViewData["NewAtmosphericTests.ConfinedSpaceFormReadingCaptureTime"];
            Assert.AreEqual(4, ddlData.Count());
        }
        
        [TestMethod]
        public void TestEditPageHasCorrectReadingCaptureTimes()
        {
            //Assemble
            var captureTimes = GetFactory<ConfinedSpaceFormReadingCaptureTimeFactory>().CreateList(3);
            var entity = GetFactory<CompletedConfinedSpaceFormFactory>().Create();

            //Act
            var result = _target.PostCompletionEdit(entity.Id);

            //Assert
            var ddlData = (IEnumerable<SelectListItem>)_target.ViewData["NewAtmosphericTests.ConfinedSpaceFormReadingCaptureTime"];
            Assert.AreEqual(1, ddlData.Count());
        }

        [TestMethod]
        public void TestUpdateSavesANoteIfPermitIsCancelled()
        {
            var noteRepo = _container.GetInstance<IRepository<Note>>();
            var csf = GetEntityFactory<ConfinedSpaceForm>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditConfinedSpaceForm, ConfinedSpaceForm>(csf, new {
                IsPermitCancelledSectionSigned = true,
                PermitCancellationNote = "This is a test note and junk",
                Hazards = new System.Collections.Generic.List<ConfinedSpaceFormHazardViewModel>(),
                ProductionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create().Id
            });

            var result = (ActionResult)_target.Update(model);

            var note = noteRepo.GetAll().ToList().Last();

            Assert.AreEqual(model.PermitCancellationNote, note.Text);
            Assert.AreEqual(_currentUser.UserName, note.CreatedBy);
            Assert.AreEqual(model.Id, note.LinkedId);
        }

        [TestMethod]
        public void TestPostCompletionUpdateSavesANoteIfPermitIsCancelled()
        {
            var noteRepo = _container.GetInstance<IRepository<Note>>();
            var csf = GetEntityFactory<ConfinedSpaceForm>().Create();
            var model = _viewModelFactory.BuildWithOverrides<PostCompletionConfinedSpaceForm, ConfinedSpaceForm>(csf, new {
                IsPermitCancelledSectionSigned = true,
                PermitCancellationNote = "This is a test note and junk",
            });

            var result = (ActionResult)_target.PostCompletionUpdate(model);

            var note = noteRepo.GetAll().ToList().Last();

            Assert.AreEqual(model.PermitCancellationNote, note.Text);
            Assert.AreEqual(_currentUser.UserName, note.CreatedBy);
            Assert.AreEqual(model.Id, note.LinkedId);
        }

        #endregion

        #region Notifications

        [TestMethod]
        public void TestShowDisplaysEntryMayCommenceWithoutPermitAndLockedForm()
        {
            var entity = GetFactory<CompletedConfinedSpaceFormFactory>().Create();
            entity.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ConfinedSpaceFormController.FORM_LOCKED_COMPLETION, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
            _target.AssertTempDataContainsMessage(ConfinedSpaceFormController.ENTRY_MAY_COMMENCE_WITHOUT_PERMIT, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysEntryMayCommenceWithPermitLockedForm()
        {
            var entity = GetFactory<CompletedConfinedSpaceFormFactory>().Create();
            entity.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());
            // Section 5 for CSF
            entity.PermitBeginsAt = DateTime.Now;
            entity.PermitEndsAt = DateTime.Now;
            entity.HasRetrievalSystem = true;
            entity.HasContractRescueService = true;
            entity.EmergencyResponseAgency = "Emergency Response Agency";
            entity.EmergencyResponseContact = "Jeff Winger";

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ConfinedSpaceFormController.FORM_LOCKED_COMPLETION, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
            _target.AssertTempDataContainsMessage(ConfinedSpaceFormController.ENTRY_MAY_COMMENCE_WITH_PERMIT, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
        }

        #endregion
    }
}
