using System;
using System.Linq;
using System.Web.Mvc;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class HydrantInspectionControllerTest : MapCallMvcControllerTestBase<HydrantInspectionController, HydrantInspection, HydrantInspectionRepository>
    {
        #region Fields

        private User _user;
        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ILog>().Use(new Mock<ILog>().Object);
        }

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                // These props have special logic that needs to be tested seperately.
                tester.IgnoredPropertyNames.Add("WorkOrderRequired");
                tester.IgnoredPropertyNames.Add("SAPEquipmentOnly");
            };
            // NOTE: These view model tests are incredibly fragile because HydrantInspectionStatus
            // is not a proper readonly entity/factory. 
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateHydrantInspection)vm;
                model.HydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create().Id;
                model.HydrantInspectionType = GetEntityFactory<HydrantInspectionType>().Create().Id;
                model.FreeNoReadReason = GetEntityFactory<NoReadReason>().Create().Id;
                model.TotalNoReadReason = GetEntityFactory<NoReadReason>().Create().Id;
                model.GPM = 2m;
                model.MinutesFlowed = 3m;
                model.StaticPressure = 3m;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditHydrantInspection)vm;
                model.HydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create().Id;
                model.HydrantInspectionType = GetEntityFactory<HydrantInspectionType>().Create().Id;
                model.FreeNoReadReason = GetEntityFactory<NoReadReason>().Create().Id;
                model.TotalNoReadReason = GetEntityFactory<NoReadReason>().Create().Id;
                model.GPM = 2m;
                model.MinutesFlowed = 3m;
                model.StaticPressure = 3m;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesAssets;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/HydrantInspection/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/HydrantInspection/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/HydrantInspection/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/HydrantInspection/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/HydrantInspection/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/HydrantInspection/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/HydrantInspection/Update/", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/HydrantInspection/Destroy/");
            });
        }

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action.
            var hydrant = GetFactory<HydrantFactory>().Create();
            var result = (ViewResult)_target.New(hydrant.Id);
            MvcAssert.IsViewNamed(result, "New");

            var model = (CreateHydrantInspection)result.Model;
            Assert.AreEqual(hydrant.Id, model.Hydrant);
        }

        [TestMethod]
        public void TestNewReturns404IfHydrantDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(0));
        }

        [TestMethod]
        public void TestCreateRedirectsToEditPageWhenIsMapPopupIsTrue()
        {
            var inspection = GetFactory<HydrantInspectionFactory>().BuildWithConcreteDependencies();
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(inspection);
            model.IsMapPopup = true;
            var result = _target.Create(model);
            MvcAssert.RedirectsToRoute(result, "HydrantInspection", "Edit", new { id = model.Id });
        }

        [TestMethod]
        public void TestCreateUpdatesHydrantTagStatus()
        {
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });
            var inspection = GetFactory<HydrantInspectionFactory>().BuildWithConcreteDependencies(new { Hydrant = hydrant, HydrantTagStatus = hydrantTagStatusInstalled });
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(inspection);

            var result = _target.Create(model);
            
            Session.Evict(hydrant);
            hydrant = Session.Load<Hydrant>(hydrant.Id);
            Assert.AreEqual(hydrantTagStatusInstalled.Id, hydrant.HydrantTagStatus.Id);
        }

        [TestMethod]
        public void TestCreateDoesntSendNotificationWhenBothOk()
        {
            //Assemble
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(acceptableValue);
            model.TotalChlorine = Convert.ToDecimal(acceptableValue);
            _target.Create(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestCreateSendsNotificationHighValues()
        {
            //Assemble
            var highOutOfLimitValue = 3.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(highOutOfLimitValue);
            model.TotalChlorine = Convert.ToDecimal(highOutOfLimitValue);
            _target.Create(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }
        
        [TestMethod]
        public void TestCreateSendsNotificationLowValues()
        {
            //Assemble
            var lowOutOfLimitValue = 0.0;
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(lowOutOfLimitValue);
            model.TotalChlorine = Convert.ToDecimal(lowOutOfLimitValue);
            _target.Create(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }
        
        [TestMethod]
        public void TestCreateDoesntSendNotificationWhenBothNull()
        {
            //Assemble
            decimal? nullvalue = null;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = nullvalue;
            model.TotalChlorine = nullvalue;
            _target.Create(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }
        
        #region SAP

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var hydrant = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<HydrantInspectionFactory>().BuildWithConcreteDependencies(new { Hydrant = hydrant });
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(inspection);

            _target.Create(model);

            var hydrantInspection = Repository.Find(model.Id);
            Assert.IsTrue(hydrantInspection.SAPErrorCode.StartsWith(HydrantInspectionController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<HydrantInspectionFactory>().BuildWithConcreteDependencies(new { Hydrant = hydrant });
            var model = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection {SAPErrorCode = string.Empty, SAPNotificationNumber = "2345"};
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Create(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateUpdatesHydrantInspectionAndUpdatesHydrantTagStatusIfItIsTheLatestHydrantInspection()
        {
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });
            var olderInspection = GetEntityFactory<HydrantInspection>().Create(new {
                HydrantTagStatus = hydrantTagStatusInstalled, 
                Hydrant = hydrant,
                DateInspected = DateTime.Now.Subtract(TimeSpan.FromDays(1))
            });
            var latestInspection = GetEntityFactory<HydrantInspection>().Create(new {
                HydrantTagStatus = hydrantTagStatusPresent, 
                Hydrant = hydrant, 
                DateInspected = DateTime.Now
            });
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(latestInspection);
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;

            var result = _target.Update(model);

            Session.Evict(hydrant);
            hydrant = Session.Load<Hydrant>(hydrant.Id);
            Assert.AreEqual(hydrantTagStatusInstalled.Id, hydrant.HydrantTagStatus.Id);
        }

        [TestMethod]
        public void TestUpdateUpdatesHydrantInspectionAndDoesNotUpdatesHydrantTagStatusIfItIsNotTheLatestHydrantInspection()
        {
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });
            var olderInspection = GetEntityFactory<HydrantInspection>().Create(new
            {
                HydrantTagStatus = hydrantTagStatusInstalled,
                Hydrant = hydrant,
                DateInspected = DateTime.Now.Subtract(TimeSpan.FromDays(1))
            });
            var latestInspection = GetEntityFactory<HydrantInspection>().Create(new
            {
                HydrantTagStatus = hydrantTagStatusPresent,
                Hydrant = hydrant,
                DateInspected = DateTime.Now
            });
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(olderInspection);
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            Session.Evict(hydrant);

            var result = _target.Update(model);

            Session.Evict(hydrant);
            hydrant = Session.Load<Hydrant>(hydrant.Id);
            Assert.AreEqual(hydrantTagStatusPresent.Id, hydrant.HydrantTagStatus.Id);
        }

        [TestMethod]
        public void TestUpdateRedirectsToEditPageWhenIsMapPopupIsTrue()
        {
            var eq = GetEntityFactory<HydrantInspection>().Create();
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditHydrantInspection, HydrantInspection>(eq, new { IsMapPopup = true }));
            MvcAssert.RedirectsToRoute(result, new { action = "Edit", controller = "HydrantInspection", id = eq.Id });
        }

        [TestMethod]
        public void TestUpdateSendsNotificationHighTot()
        {
            //Assemble
            var highOutOfLimitValue = 3.3;
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(acceptableValue);
            model.TotalChlorine = Convert.ToDecimal(highOutOfLimitValue);
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestUpdateSendsNotificationHighRes()
        {
            //Assemble
            var highOutOfLimitValue = 3.3;
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create(new{ ResidualChlorine = Convert.ToDecimal(highOutOfLimitValue)
            }));
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(highOutOfLimitValue);
            model.TotalChlorine = Convert.ToDecimal(acceptableValue);
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestUpdateDoesntSendNotificationWhenBothOk()
        {
            //Assemble
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(acceptableValue);
            model.TotalChlorine = Convert.ToDecimal(acceptableValue);
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateDoesntSendNotificationWhenBothNull()
        {
            //Assemble
            decimal? nullvalue = null;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = nullvalue;
            model.TotalChlorine = nullvalue;
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationLowRes()
        {
            //Assemble
            var lowOutOfLimitValue = 0.0;
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(lowOutOfLimitValue);
            model.TotalChlorine = Convert.ToDecimal(acceptableValue);
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestUpdateSendsNotificationLowTot()
        {
            //Assemble
            var lowOutOfLimitValue = 0.0;
            var acceptableValue = 2.3;
            _user.IsAdmin = true;
            var hydrantTagStatusInstalled = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrantTagStatusPresent = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { HydrantTagStatus = hydrantTagStatusPresent });

            //Act
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(GetEntityFactory<HydrantInspection>().Create());
            model.HydrantTagStatus = hydrantTagStatusInstalled.Id;
            model.Hydrant = hydrant.Id;
            model.ResidualChlorine = Convert.ToDecimal(acceptableValue);
            model.TotalChlorine = Convert.ToDecimal(lowOutOfLimitValue);
            _target.Update(model);

            //Assert
            NotifierArgs resultArgs = null;
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #region SAP

        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPNotificationNumberIsNullish()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant });
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection { SAPErrorCode = string.Empty, SAPNotificationNumber = "2345", CostCenter = "987654"};
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Update(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
            Assert.AreEqual(sapInspection.CostCenter, actual.BusinessUnit);
        }

        [TestMethod]
        public void TestUpdateDoesNotAttemptToCreatesInSAPWhenSAPNotificationNumberIsNotNull()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant, SAPNotificationNumber = "321" });
            var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            _container.Inject(repository.Object);

            _target.Update(model);

            repository.Verify(x => x.Save(It.IsAny<SAPInspection>()), Times.Never);
        }

        #endregion

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchWithWorkOrderRequiredTrueReturnsOnlyInspectionsWhereWorkOrderRequestOneIsNotNull()
        {
            var hydrant = GetFactory<HydrantFactory>().Create();
            var woReq = GetFactory<WorkOrderRequestFactory>().Create();
            var inspectionWithRequest = GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant, WorkOrderRequestOne = woReq });
            var inspectionNoRequest = GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant });

            var model = new SearchHydrantInspection();
            model.WorkOrderRequired = true;
            model.OperatingCenter = new[] { hydrant.OperatingCenter.Id };
            model.Town = hydrant.Town.Id;

            // Actually testing that Index calls Search properly.
            var result = (ViewResult)_target.Index(model);
            Assert.IsTrue(model.Results.Any(x => x.Id == inspectionWithRequest.Id));
            Assert.IsFalse(model.Results.Any(x => x.Id == inspectionNoRequest.Id));
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/HydrantInspection/Index.map");
            var search = new SearchHydrantInspection();
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestIndexForcesAFewSearchParametersWhenRespondingToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/HydrantInspection/Index.map");
            var search = new SearchHydrantInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;
            search.SortBy = null; // This will be changed by the model's ModifyValues method.
            search.SortAscending = false;

            var hydrant1 = GetFactory<HydrantFactory>().Create();
            var hydrant2 = GetFactory<HydrantFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant1 });
            GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant2 });

            Session.Refresh(hydrant1);
            Session.Refresh(hydrant2);

            _target.Index(search);
            Assert.IsFalse(search.EnablePaging, "EnablePaging needs to be disabled for maps");
            Assert.AreEqual("DateInspected", search.SortBy);
        }

        [TestMethod]
        public void TestIndexCreatesLineLayerForMaps()
        {
            InitializeControllerAndRequest("~/FieldOperations/HydrantInspection/Index.map");
            var search = new SearchHydrantInspection();
            search.DateInspected = new DateRange
            {
                Start = new DateTime(2015, 5, 15),
                End = new DateTime(2015, 5, 25)
            };

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;

            var hydrant1 = GetFactory<HydrantFactory>().Create();
            var hydrant2 = GetFactory<HydrantFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant1, DateInspected = search.DateInspected.End });
            GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant2, DateInspected = search.DateInspected.Start, WorkOrderRequestOne = typeof(WorkOrderRequestFactory) });

            Session.Refresh(hydrant1);
            Session.Refresh(hydrant2);

            var result = (MapResult)_target.Index(search);
            var lineLayer = result.CoordinateSets.First();
            Assert.AreEqual("lineLayer", lineLayer.LayerId, "The first layer MUST be the line layer.");
            var lineCoords = lineLayer.Coordinates.ToArray();
            Assert.AreEqual(2, lineCoords.Count(), "The line layer MUST include all the coordinates!");
            Assert.AreEqual(hydrant2.Id, lineCoords[0].Id, "The earliest inspection must have its coordinate first.");
            Assert.AreEqual(hydrant1.Id, lineCoords[1].Id, "The latest inspection must have its coordinate last.");
        }

        [TestMethod]
        public void TestIndexSetsCoordinateRouteValuesToGoToHydrantShowPageWhenRespondingToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/HydrantInspection/Index.map");
            var search = new SearchHydrantInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;

            var hydrant1 = GetFactory<HydrantFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<HydrantInspectionFactory>().Create(new { Hydrant = hydrant1 });

            var result = (MapResult)_target.Index(search);
            var iconLayer = result.CoordinateSets[1];
            var iconRVD = iconLayer.CoordinateRouteValues;

            Assert.AreEqual("FieldOperations", iconRVD["area"]);
            Assert.AreEqual("Hydrant", iconRVD["controller"]);
            Assert.AreEqual("Show", iconRVD["action"]);
            Assert.AreEqual("frag", iconRVD["ext"]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var inspection = GetFactory<HydrantInspectionFactory>().Create(new
            {
                Hydrant = GetFactory<HydrantFactory>().Create(new
                {
                    Coordinate = typeof(CoordinateFactory)
                }),
                HydrantInspectionType = GetFactory<HydrantInspectionTypeFactory>().Create(),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3),
                GPM = 2.0m,
                MinutesFlowed = 3.0m,
                GallonsFlowed = Convert.ToInt32(2.0m * 3.0m),
                PreResidualChlorine = 4.0m,
                ResidualChlorine = 4.0m,
                FreeNoReadReason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create(),
                PreTotalChlorine = 5.0m,
                TotalChlorine = 5.0m,
                TotalNoReadReason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create(),
                StaticPressure = 6.0m,
                Remarks = "Hhheheh", // http://knowyourmeme.com/memes/laughing-lizard-hhhehehe
                WorkOrderRequestOne = typeof(WorkOrderRequestFactory)
            });

            var inspectionWithNulls = GetFactory<HydrantInspectionFactory>().Create(new
            {
                Hydrant = GetFactory<HydrantFactory>().Create(),
            });
            inspectionWithNulls.Hydrant.Coordinate = null;
            Session.Flush();
            Assert.IsNull(inspectionWithNulls.Hydrant.Coordinate, "Coordinate is nullable and needs to be tested.");
            Assert.IsNull(inspectionWithNulls.HydrantInspectionType, "HydrantInspectionType is nullable and needs to be tested.");

            var search = new SearchHydrantInspection();
            search.SortBy = "Id";
            search.SortAscending = true;
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(inspection.Id, "Id");
                helper.AreEqual(inspection.Hydrant.Id, "HydrantId");
                helper.AreEqual(inspection.Hydrant.HydrantNumber, "HydrantNumber");
                helper.AreEqual(inspection.Hydrant.OperatingCenter.OperatingCenterCode, "OperatingCenter");
                helper.AreEqual(inspection.Hydrant.Town.ShortName, "Town");
                helper.AreEqual(inspection.Hydrant.Coordinate.Latitude, "Latitude");
                helper.AreEqual(inspection.Hydrant.Coordinate.Longitude, "Longitude");
                helper.AreEqual(inspection.DateInspected, "DateInspected");
                helper.AreEqual(inspection.HydrantInspectionType.Description, "Inspection Type");
                helper.AreEqual(inspection.GallonsFlowed, "GallonsFlowed");
                helper.AreEqual(inspection.GPM, "GPM");
                helper.AreEqual(inspection.MinutesFlowed, "MinutesFlowed");
                helper.AreEqual(inspection.PreResidualChlorine, "Pre Residual/Free Chlorine");
                helper.AreEqual(inspection.ResidualChlorine, "Post Residual/Free Chlorine");
                helper.AreEqual(inspection.FreeNoReadReason, "FreeNoReadReason");
                helper.AreEqual(inspection.PreTotalChlorine, "Pre Total Chlorine");
                helper.AreEqual(inspection.TotalChlorine, "Post Total Chlorine");
                helper.AreEqual(inspection.TotalNoReadReason, "TotalNoReadReason");
                helper.AreEqual(inspection.StaticPressure, "StaticPressure");
                helper.AreEqual(inspection.WorkOrderRequestOne, "WorkOrderRequestOne");
                helper.AreEqual(inspection.Remarks, "Remarks");
                helper.AreEqual(inspection.InspectedBy.UserName, "InspectedBy");
                helper.AreEqual(inspection.CreatedAt, "DateAdded");

                helper.IsNull("Latitude", 1);
                helper.IsNull("Longitude", 1);
                helper.IsNull("Inspection Type", 1);
            }
        }
        #endregion

        #endregion
    }
}
