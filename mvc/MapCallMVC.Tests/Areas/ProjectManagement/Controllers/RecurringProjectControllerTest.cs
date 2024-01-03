using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class RecurringProjectControllerTest : MapCallMvcControllerTestBase<RecurringProjectController, RecurringProject>
    {
        #region Fields

        private Mock<INotificationService> _noteServ;
        private Mock<IRoleService> _roleServ;
        private User _user;

        private RecurringProjectStatus projectStatusComplete,
            projectStatusProposed,
            projectStatusSubmitted,
            projectStatusCanceled,
            projectStatusManagerEndorsed,
            projectStatusReviewed,
            projectStatusAPEndorsed,
            projectStatusMunicipalRelocationApproved,
            projectStatusAPApproved;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
            _roleServ = new Mock<IRoleService>();
            _container.Inject(_roleServ.Object);
            SetupProjectStatuses();
        }

        private void SetupProjectStatuses()
        {
            projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            projectStatusProposed = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            projectStatusSubmitted = GetFactory<SubmittedRecurringProjectStatusFactory>().Create();
            projectStatusCanceled = GetFactory<CanceledRecurringProjectStatusFactory>().Create();
            projectStatusManagerEndorsed = GetFactory<ManagerEndorsedRecurringProjectStatusFactory>().Create();
            projectStatusReviewed = GetFactory<ReviewedRecurringProjectStatusFactory>().Create();
            projectStatusAPEndorsed = GetFactory<APEndorsedRecurringProjectStatusFactory>().Create();
            projectStatusMunicipalRelocationApproved = GetFactory<MunicipalRelocationApprovedRecurringProjectStatusFactory>().Create();
            projectStatusAPApproved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateRecurringProject)vm;
                model.RecurringProjectMains.Add(new CreateRecurringProjectMain(_container) {
                    Guid = "asdf",
                    Layer = "A",
                    Length = 14m,
                    TotalInfoMasterScore = 2m
                });
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RecurringProjectController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/RecurringProject/Search/", role);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Show/", role);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Index/", role);
                a.RequiresRole("~/ProjectManagement/RecurringProject/New/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Create/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RecurringProject/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/ProjectManagement/RecurringProject/AddRecurringProjectEndorsement", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RecurringProject/RemoveRecurringProjectEndorsement", role, RoleActions.Edit);
            });
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<RecurringProject>().Create(new {ProjectTitle = "description 0"});
            var entity1 = GetEntityFactory<RecurringProject>().Create(new {ProjectTitle = "description 1"});
            var search = new SearchRecurringProject();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ProjectTitle, "ProjectTitle");
                helper.AreEqual(entity1.ProjectTitle, "ProjectTitle", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            _currentUser.IsAdmin = true;
            var model = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies());
            _target.Create(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.AtMostOnce);
        }

        [TestMethod]
        public void TestCreateSendGISNotificationOnNormalSave()
        {
            var reasons = GetEntityFactory<OverrideInfoMasterReason>().CreateList(3);
            _currentUser.IsAdmin = true;
            var model = _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies(), new {
                OverrideInfoMasterReason = OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT
            });
            model.SendGISDataIncorrectOnSave = true;
            model.RecurringProjectMains.Add(new CreateRecurringProjectMain(_container) {
                Guid = "asdf",
                Layer = "A",
                Length = 14m,
                TotalInfoMasterScore = 2m
            });
            model.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] { 1, 2, 3 }
            };
            _target.Create(model);

            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(z => z.Attachments.Count == 1)), Times.Once);
            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(z => z.Attachments.Count == 0)), Times.Once);
        }

        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #endregion

        #region Edit/Update
       
        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<RecurringProject>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(eq, new {
                ProjectTitle = expected
            }));

            Assert.AreEqual(expected, Session.Get<RecurringProject>(eq.Id).ProjectTitle);
        }

        [TestMethod]
        public void TestUpdateSendsNotification()
        {
            _currentUser.IsAdmin = true;
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var model = _viewModelFactory.Build<EditRecurringProject, RecurringProject>( GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusPending }));
            model.WBSNumber = "wbs!";
            model.Status = projectStatusComplete.Id;

            ValidationAssert.ModelStateIsValid(model); // Sanity check

            _target.Update(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForAdmin()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create();

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.AreEqual(9, ddl.Count());
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForFieldServicesProjectsRole()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.FieldServicesProjects,RoleActions.Read,null)).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusProposed});

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusProposed.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusCanceled.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusSubmitted.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusManagerEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusReviewed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPApproved.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusMunicipalRelocationApproved.Id.ToString()));
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForLocalApprovalRole()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.FieldServicesLocalApproval, RoleActions.Read, null)).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusProposed });

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusProposed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusCanceled.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusSubmitted.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusManagerEndorsed.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusReviewed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPApproved.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusMunicipalRelocationApproved.Id.ToString()));
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForAssetPlanningEndorsementRole()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssetPlanningEndorsement, RoleActions.Read, null)).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusProposed });

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusProposed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusCanceled.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusSubmitted.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusManagerEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusReviewed.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusAPEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPApproved.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusMunicipalRelocationApproved.Id.ToString()));
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForAssetPlanningApprovalRole()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.FieldServicesAssetPlanningApproval, RoleActions.Read, null)).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusCanceled });

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusProposed.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusCanceled.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusSubmitted.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusManagerEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusReviewed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPEndorsed.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusAPApproved.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusMunicipalRelocationApproved.Id.ToString()));
        }

        [TestMethod]
        public void TestEditAddsCorrectRecurringProjectStatusesForCapitalPlanningRole()
        {
            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.FieldServicesCapitalPlanning, RoleActions.Read, null)).Returns(true);
            var eq = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusProposed });

            var result = _target.Edit(eq.Id) as ViewResult;
            var ddl = (IEnumerable<SelectListItem>)_target.ViewData["Status"];

            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusProposed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusCanceled.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusSubmitted.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusManagerEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusReviewed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPEndorsed.Id.ToString()));
            Assert.IsFalse(ddl.Any(x => x.Value == projectStatusAPApproved.Id.ToString()));
            Assert.IsTrue(ddl.Any(x => x.Value == projectStatusMunicipalRelocationApproved.Id.ToString()));
        }
        
        #endregion

        #region Add/Remove Endorsements

        [TestMethod]
        public void TestAddRecurringProjectEndorsementCreatesAndAddsEndorsement()
        {
            var proj = GetEntityFactory<RecurringProject>().Create();
            var status = GetEntityFactory<EndorsementStatus>().Create();

            MyAssert.CausesIncrease(() => _target.AddRecurringProjectEndorsement(new AddRecurringProjectEndorsement(_container) {
                Id = proj.Id,
                EndorsementStatus = status.Id,
                Comment="this is a comment"
            }), _container.GetInstance<RepositoryBase<RecurringProjectEndorsement>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveRecurringProjectEndorsementRemovesEndorsement()
        {
            var recurringProject = GetEntityFactory<RecurringProject>().Create();
            var recurringProjectEndorsement = GetEntityFactory<RecurringProjectEndorsement>().Create(new { RecurringProject = recurringProject});
            Session.Evict(recurringProject);
            recurringProject = Session.Load<RecurringProject>(recurringProject.Id);

            MyAssert.CausesDecrease(
                () => _target.RemoveRecurringProjectEndorsement(
                    _viewModelFactory.BuildWithOverrides<RemoveRecurringProjectEndorsement, RecurringProject>(
                        recurringProject, new {RecurringProjectEndorsementId = recurringProjectEndorsement.Id})),
                _container.GetInstance<RepositoryBase<RecurringProjectEndorsement>>().GetAll().Count);

            Session.Evict(recurringProject);
            recurringProject = Session.Load<RecurringProject>(recurringProject.Id);
            Assert.AreEqual(0, recurringProject.ProjectEndorsements.Count);
        }

        #endregion
    }
}
