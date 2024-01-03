using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class JobSiteCheckListControllerTest : MapCallMvcControllerTestBase<JobSiteCheckListController, JobSiteCheckList, JobSiteCheckListRepository>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Fields

        private OperatingCenter _opCenter;
        private Mock<INotificationService> _notifier;
        private WorkOrder _workOrder;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _opCenter = GetEntityFactory<OperatingCenter>().Create();
            var user = GetEntityFactory<User>().Create(new
            {
                DefaultOperatingCenter = _opCenter
            });

            GetFactory<RoleFactory>().Create(new
            {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = _opCenter,
                User = user
            });

            Session.Save(user);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);

            _target = Request.CreateAndInitializeController<JobSiteCheckListController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<JobSiteCheckList>().Create(new {
                // SupervisorSignOffEmployee needs to be null or else the Edit tests will fail
                // by returing a redirect instead of the expected view.
                SupervisorSignOffEmployee = (Employee)null
            });
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateJobSiteCheckList)vm;
                model.AllEmployeesWearingAppropriatePersonalProtectionEquipment = true;
                model.AllStructuresSupportedOrProtected = true;
                model.CrewMembers = "Some crew";
                model.HasExcavation = false;
                model.IsMarkoutValidForSite = true;
                model.RestraintMethod = GetEntityFactory<JobSiteCheckListRestraintMethodType>().Create().Id;
                model.SupervisorSignOffEmployee = GetEntityFactory<Employee>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditJobSiteCheckList)vm;
                model.AllEmployeesWearingAppropriatePersonalProtectionEquipment = true;
                model.AllStructuresSupportedOrProtected = true;
                model.CrewMembers = "Some crew";
                model.HasExcavation = false;
                model.IsMarkoutValidForSite = true;
                model.RestraintMethod = GetEntityFactory<JobSiteCheckListRestraintMethodType>().Create().Id;
                model.SupervisorSignOffEmployee = GetEntityFactory<Employee>().Create().Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Index/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Search/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Show/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/JobSiteCheckList/Destroy/", module, RoleActions.Delete);
            });
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/HealthAndSafety/JobSiteCheckList/Index.map");
            var good = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = _opCenter });
            var bad = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchJobSiteCheckList
            {
                OperatingCenter = good.OperatingCenter.Id
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        [TestMethod]
        public void TestSearchReturnsOnlySignedOffCheckListsIfIsSignedOffIsTrue()
        {
            var signedOff = GetFactory<JobSiteCheckListFactory>().Create(new {
                OperatingCenter = _opCenter,
            });

            var anotherSignedOff = GetFactory<JobSiteCheckListFactory>().Create(new {
                OperatingCenter = _opCenter
            });

            var notSignedOff = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new {
                OperatingCenter = _opCenter
            });
            var anotherNotSignedOff = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new {
                OperatingCenter = _opCenter
            });
            var search = new SearchJobSiteCheckList {
                IsSignedOffBySupervisor = true
            };

            _target.Index(search);

            Assert.IsNotNull(search.Results, "This should like not be null and stuff.");

            Assert.IsTrue(search.Results.Contains(signedOff));
            Assert.IsTrue(search.Results.Contains(anotherSignedOff));
            Assert.IsFalse(search.Results.Contains(notSignedOff));
            Assert.IsFalse(search.Results.Contains(anotherNotSignedOff));
        }

        [TestMethod]
        public void TestSearchIndexReturnsOnlyNotSignedOffCheckListsIfIsSignedOffIsFalse()
        {
            var signedOff = GetFactory<JobSiteCheckListFactory>().Create(new {
                OperatingCenter = _opCenter
            });

            var anotherSignedOff = GetFactory<JobSiteCheckListFactory>().Create(new {
                OperatingCenter = _opCenter
            });

            var notSignedOff = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new {
                OperatingCenter = _opCenter,
                HasExcavation = false
            });
            var anotherNotSignedOff = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new {
                OperatingCenter = _opCenter,
                HasExcavation = false
            });

            var search = new SearchJobSiteCheckList {
                IsSignedOffBySupervisor = false
            };

            _target.Index(search);

            Assert.IsFalse(search.Results.Contains(signedOff));
            Assert.IsFalse(search.Results.Contains(anotherSignedOff));
            Assert.IsTrue(search.Results.Contains(notSignedOff));
            Assert.IsTrue(search.Results.Contains(anotherNotSignedOff));
        }

        [TestMethod]
        public void TestNewSetsMapCallWorkOrderIdToPassedInParameterIfParameter()
        {
            var result = (CreateJobSiteCheckList)((ViewResult)_target.New(12345)).Model;
            Assert.AreEqual(12345, result.MapCallWorkOrder);

            result = (CreateJobSiteCheckList)((ViewResult)_target.New()).Model;
            Assert.IsNull(result.MapCallWorkOrder);
        }

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            var no = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var noRestraintReason = GetFactory<JobSiteCheckListNoRestraintReasonTypeFactory>().Create();
            var wo = GetEntityFactory<WorkOrder>().Create();
            var ent = GetFactory<JobSiteCheckListFactory>().Build(new { MapCallWorkOrder = wo, PressurizedRiskRestrainedType = no, NoRestraintReason = noRestraintReason });
            var oc = GetEntityFactory<OperatingCenter>().Create();
            var emp = GetEntityFactory<Employee>().Create();
            var coord = GetEntityFactory<Coordinate>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateJobSiteCheckList, JobSiteCheckList>(ent, new {
                Comments = "Argh",
                CrewMembers = "None",
                CompetentEmployee = emp.Id,
                Coordinate = coord.Id,
                OperatingCenter = oc.Id,
                SupervisorSignOffEmployee = emp.Id

            });

            var result = (RedirectToRouteResult)_target.Create(model);
            var entity = Repository.Find(model.Id);

            MvcAssert.RedirectsToRoute(result, "WorkOrderFinalization", "Edit", new { area = "FieldOperations", id = entity.MapCallWorkOrder.Id });
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.OperatingCenter.Id, model.OperatingCenter.Value);
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            var existingRecordCount = Repository.GetAll().Count();
            var no = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var noRestraintReason = GetFactory<JobSiteCheckListNoRestraintReasonTypeFactory>().Create();
            var wo = GetEntityFactory<WorkOrder>().Create();
            var ent = GetFactory<JobSiteCheckListFactory>().Build(new { MapCallWorkOrder = wo, PressurizedRiskRestrainedType = no, NoRestraintReason = noRestraintReason });
            var oc = GetEntityFactory<OperatingCenter>().Create();
            var emp = GetEntityFactory<Employee>().Create();
            var coord = GetEntityFactory<Coordinate>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateJobSiteCheckList, JobSiteCheckList>(ent, new
            {
                Comments = "Argh",
                CrewMembers = "None",
                CompetentEmployee = emp.Id,
                Coordinate = coord.Id,
                OperatingCenter = oc.Id,
                SupervisorSignOffEmployee = emp.Id

            });
            var result = _target.Create(model);
            var newRecordCount = Repository.GetAll().Count();
            Assert.IsTrue(_target.ModelState.IsValid);
            Assert.AreEqual(existingRecordCount + 1, newRecordCount);
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/JobSiteCheckList/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = _opCenter });
            InitializeControllerAndRequest("~/HealthAndSafety/JobSiteCheckList/Show/" + good.Id + ".frag");

            var result = (PartialViewResult)_target.Show(good.Id);
            MvcAssert.IsViewNamed(result, "_ShowPopup");
            Assert.AreSame(good, result.Model);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = _opCenter });
            var bad = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = _opCenter });
            InitializeControllerAndRequest("~/HealthAndSafety/JobSiteCheckList/Show/" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditReturnsEditViewIfCheckListHasNotBeenApprovedByASupervisor()
        {
            var entity = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new { OperatingCenter = _opCenter });
            var result = _target.Edit(entity.Id);
            MvcAssert.IsViewNamed(result, "Edit");
        }

        [TestMethod]
        public void TestEditReturnsEditViewIfCheckListIsApprovedByASupervisorByTheUserIsAlsoAnAdminAndThatMeansTheyCanDoAllTheFunThings()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            var entity = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new { OperatingCenter = _opCenter });
            var result = _target.Edit(entity.Id);
            MvcAssert.IsViewNamed(result, "Edit");
        }

        [TestMethod]
        public void TestEditRedirectsToShowPageIfRecordIsApprovedBySupervisorAndTheUserIsNotAnAdmin()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            var entity = GetFactory<JobSiteCheckListFactory>().Create(new { OperatingCenter = _opCenter });
            var result = _target.Edit(entity.Id);
            MvcAssert.RedirectsToRoute(result, "Show", new { id = entity.Id });
        }

        #endregion

        #region Update

        [TestMethod]
        public void TestUpdateSendsNotificationWhenHasExcavationChanges()
        {
            //Arrange
            var yes = GetFactory<YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var no = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var entity = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new { MapCallWorkOrder = typeof(WorkOrderFactory) });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, entity.MapCallWorkOrder.OperatingCenter, _currentUser, RoleActions.Read);
            var excavation = GetFactory<JobSiteExcavationFactory>().Create(new { JobSiteCheckList = entity });
            var model = _viewModelFactory.BuildWithOverrides<EditJobSiteCheckList, JobSiteCheckList>(entity,
                new {
                    HasExcavation = true,
                    AllEmployeesWearingAppropriatePersonalProtectionEquipment = true,
                    AllMaterialsSetBackFromEdgeOfTrenches = true,
                    AllStructuresSupportedOrProtected = true,
                    AreExposedUtilitiesProtected = true,
                    CrewMembers = "yes",
                    IsMarkoutValidForSite = false,
                    WaterControlSystemsInUse = true,
                    SpotterAssigned = true,
                    IsTheExcavationGuardedFromAccidentalEntry = true,
                    IsTheExcavationSubjectToVibration = false,
                    AreThereAnyVisualSignsOfPotentialSoilCollapse = false,
                    IsManufacturerDataOnSiteForShoringOrShieldingEquipment = false
                });
            //santity check
            // we can't get past here if the model is not valid
            ValidationAssert.ModelStateIsValid(model);

            //Act
            var result = _target.Update(model);

            //Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateDoesNotSendsNotificationWhenHasExcavationHasNotChanged()
        {
            //Arrange
            var yes = GetFactory<YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var no = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            var entity = GetFactory<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>().Create(new { HasExcavation = true, MapCallWorkOrder = typeof(WorkOrderFactory) });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, entity.MapCallWorkOrder.OperatingCenter, _currentUser, RoleActions.Read);
            var excavation = GetFactory<JobSiteExcavationFactory>().Create(new { JobSiteCheckList = entity });
            var model = _viewModelFactory.BuildWithOverrides<EditJobSiteCheckList, JobSiteCheckList>(entity,
                new {
                    AllEmployeesWearingAppropriatePersonalProtectionEquipment = true,
                    AllMaterialsSetBackFromEdgeOfTrenches = true,
                    AllStructuresSupportedOrProtected = true,
                    AreExposedUtilitiesProtected = true,
                    CrewMembers = "yes",
                    IsMarkoutValidForSite = false,
                    WaterControlSystemsInUse = true,
                    SpotterAssigned = true,
                    IsTheExcavationGuardedFromAccidentalEntry = true,
                    IsTheExcavationSubjectToVibration = false,
                    AreThereAnyVisualSignsOfPotentialSoilCollapse = false,
                    IsManufacturerDataOnSiteForShoringOrShieldingEquipment = false
                });
            //santity check
            // we can't get past here if the model is not valid
            ValidationAssert.ModelStateIsValid(model);

            //Act
            var result = _target.Update(model);

            //Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion

        #endregion
    }
}