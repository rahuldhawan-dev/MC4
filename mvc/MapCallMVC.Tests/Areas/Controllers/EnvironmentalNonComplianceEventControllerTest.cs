using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EnvironmentalNonComplianceEventControllerTest : MapCallMvcControllerTestBase<EnvironmentalNonComplianceEventController, EnvironmentalNonComplianceEvent>
    {
        #region Fields

        private Mock<INotificationService> _notifier;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
            _user = GetEntityFactory<User>().Create(new { FullName = "Full Name" });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateEnvironmentalNonComplianceEvent)vm;
                model.WaterType = GetEntityFactory<WaterType>().Create().Id;
                model.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditEnvironmentalNonComplianceEvent)vm;
                model.WaterType = GetEntityFactory<WaterType>().Create().Id;
                model.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Show/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Search/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Index/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Edit/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Update/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/New/", RoleModules.EnvironmentalGeneral, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Create/", RoleModules.EnvironmentalGeneral, RoleActions.Add);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/Destroy/", RoleModules.EnvironmentalGeneral, RoleActions.Delete);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEvent/AddEnvironmentalNonComplianceEventActionItem/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create(new {NameOfEntity = "description 0"});
            var entity1 = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create(new {NameOfEntity = "description 1"});
            var search = new SearchEnvironmentalNonComplianceEvent();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.NameOfEntity, "Name Of Entity");
                helper.AreEqual(entity1.NameOfEntity, "Name Of Entity", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsEnvironmentalNonComplianceEventCreatedNotification()
        {
            var entity = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create();

            var model = _viewModelFactory.Build<CreateEnvironmentalNonComplianceEvent, EnvironmentalNonComplianceEvent>(entity);
            model.Id = 0;
            model.WaterType = GetEntityFactory<WaterType>().Create().Id;
            model.WasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create().Id;
            model.RootCauses = new[] { GetEntityFactory<EnvironmentalNonComplianceEventRootCause>().Create().Id };

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            _target.Create(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.EnvironmentalGeneral, resultArgs.Module);
            Assert.AreEqual(EnvironmentalNonComplianceEventController.ENVIRONMENTAL_NON_COMPLIANCE_EVENT_CREATED_NOTIFICATION_PURPOSE, resultArgs.Purpose);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEnvironmentalNonComplianceEvent, EnvironmentalNonComplianceEvent>(eq, new {
                NameOfEntity = expected
            })) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<EnvironmentalNonComplianceEvent>(eq.Id).NameOfEntity);
        }

        #endregion

        #region Children

        #region Action Items

        [TestMethod]
        public void TestAddEnvironmentalNonComplianceEventActionItemDoesThatThing()
        {
            var notice = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create();
            var responsibleOwner = GetEntityFactory<EnvironmentalNonComplianceEvent>().Create();

            MyAssert.CausesIncrease(
                () => _target.AddEnvironmentalNonComplianceEventActionItem(_viewModelFactory.BuildWithOverrides<CreateEnvironmentalNonComplianceEventActionItem, EnvironmentalNonComplianceEvent>(notice, new {
                    Type = GetFactory<EntityLookupTestDataFactory<EnvironmentalNonComplianceEventActionItemType>>().Create().Id,
                    ActionItem = "foo",
                    ResponsibleOwner = _user.Id,
                    TargetedCompletionDate = DateTime.Now
                })),
                () => _container.GetInstance<RepositoryBase<EnvironmentalNonComplianceEventActionItem>>().GetAll().Count());
        }

        #endregion

        #endregion
    }
}