using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SewerOverflowControllerTest : MapCallMvcControllerTestBase<SewerOverflowController, SewerOverflow, SewerOverflowRepository>
    {
        #region Fields

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSewerOverflow)vm;
                model.State = GetEntityFactory<State>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditSewerOverflow)vm;
                model.State = GetEntityFactory<State>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SewerOverflow/Search/", SewerOverflowController.ROLE);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Show/", SewerOverflowController.ROLE);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Index/", SewerOverflowController.ROLE);
                a.RequiresRole("~/FieldOperations/SewerOverflow/New/", SewerOverflowController.ROLE, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Create/", SewerOverflowController.ROLE, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Edit/", SewerOverflowController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Update/", SewerOverflowController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOverflow/Destroy/", SewerOverflowController.ROLE, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOverflow/ByStreetId/");
            });
        }				

        #endregion

        #region Index

        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SewerOverflow>().Create(new {EnforcingAgencyCaseNumber = "EnforcingAgencyCaseNumber 0"});
            var entity1 = GetEntityFactory<SewerOverflow>().Create(new {EnforcingAgencyCaseNumber = "EnforcingAgencyCaseNumber 1"});

            var search = new SearchSewerOverflow();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.EnforcingAgencyCaseNumber, "EnforcingAgencyCaseNumber");
                helper.AreEqual(entity1.EnforcingAgencyCaseNumber, "EnforcingAgencyCaseNumber", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewSetsPropertiesFromWorkOrderIdIfSent()
        {
            var wasterWaterSystem = GetEntityFactory<WasteWaterSystem>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.WasteWaterSystems.Add(wasterWaterSystem);
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { Town = town });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.Add);
            
            var result = (CreateSewerOverflow)((ViewResult)_target.New(workOrder.Id)).Model;

            Assert.AreEqual(workOrder.Id, result.WorkOrder);
            Assert.AreEqual(workOrder.State.Id, result.State);
            Assert.AreEqual(workOrder.OperatingCenter.Id, result.OperatingCenter);
            Assert.AreEqual(workOrder.Town.Id, result.Town);
            Assert.AreEqual(workOrder.Street.Id, result.Street);
            Assert.AreEqual(workOrder.StreetNumber, result.StreetNumber);
            Assert.AreEqual(workOrder.NearestCrossStreet.Id, result.CrossStreet);
            MyAssert.AreClose(DateTime.Now, result.IncidentDate.Value);
            Assert.AreEqual(wasterWaterSystem.Id, result.WasteWaterSystem);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var sewerOverflow = GetEntityFactory<SewerOverflow>().Create();
            var enforcingAgencyCaseNumber = "EnforcingAgencyCaseNumber field";

            _target.Update(_viewModelFactory.BuildWithOverrides<EditSewerOverflow, SewerOverflow>(sewerOverflow, new {
                EnforcingAgencyCaseNumber = enforcingAgencyCaseNumber
            }));

            Assert.AreEqual(enforcingAgencyCaseNumber, Session.Get<SewerOverflow>(sewerOverflow.Id).EnforcingAgencyCaseNumber);
        }

        #endregion

        #region Cascades

        [TestMethod]
        public void TestByStreetIdReturnsSewerOverflowsOnStreet()
        {
            var street = GetEntityFactory<Street>().Create();
            var otherStreet = GetEntityFactory<Street>().Create();
            var overflow = GetEntityFactory<SewerOverflow>().Create(new {Street = street});
            GetEntityFactory<SewerOverflow>().Create(new {Street = otherStreet});

            var results = (CascadingActionResult)_target.ByStreetId(street.Id);
            var actual = results.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(overflow.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(overflow.Id.ToString(), actual.Last().Text);
        }

        #endregion

        #region Notification

        [TestMethod]
        public void TestNotificationIsNotSentOutWhenCreatedAndEnforcingAgencyCaseNumberIsNotPopulated()
        {
            var sewerOverflow = GetEntityFactory<SewerOverflow>().Build();
            var viewModel = _viewModelFactory.Build<CreateSewerOverflow, SewerOverflow>(sewerOverflow);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>()))
                     .Callback<NotifierArgs>(x => resultArgs = x);

            _target.Create(viewModel);
            var entity = Repository.Find(viewModel.Id);

            Assert.IsNull(resultArgs);
            Assert.IsNull(entity.RecordUrl);
        }

        [TestMethod]
        public void TestNotificationIsSentOutWhenCreatedAndEnforcingAgencyCaseNumberIsPopulated()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var sewerOverflow = GetEntityFactory<SewerOverflow>().Build(new {
                EnforcingAgencyCaseNumber = "a real value", 
                OperatingCenter = operatingCenter
            });

            var viewModel = _viewModelFactory.Build<CreateSewerOverflow, SewerOverflow>(sewerOverflow);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>()))
                     .Callback<NotifierArgs>(x => resultArgs = x);

            _target.Create(viewModel);
            var entity = Repository.Find(viewModel.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual("http://localhost/FieldOperations/SewerOverflow/Show/" + entity.Id, entity.RecordUrl);
        }

        #endregion
    }
}
