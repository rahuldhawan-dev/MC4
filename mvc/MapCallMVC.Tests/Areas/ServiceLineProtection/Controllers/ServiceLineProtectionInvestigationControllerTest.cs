using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ServiceLineProtection.Controllers;
using MapCallMVC.Areas.ServiceLineProtection.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.ServiceLineProtection.Controllers
{
    [TestClass]
    public class ServiceLineProtectionInvestigationControllerTest : MapCallMvcControllerTestBase<ServiceLineProtectionInvestigationController, ServiceLineProtectionInvestigation, ServiceLineProtectionInvestigationRepository>
    {
        #region Fields

        private User _user;
        private Mock<INotificationService> _noteServ;

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
            _container.Inject(Repository);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateServiceLineProtectionInvestigation)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.CustomerServiceSize = GetEntityFactory<ServiceSize>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditServiceLineProtectionInvestigation)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.CustomerServiceSize = GetEntityFactory<ServiceSize>().Create().Id;
                model.DateInstalled = DateTime.Now;
                model.CompanyServiceMaterial = GetEntityFactory<ServiceMaterial>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ServiceLineProtectionInvestigationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Search/", role);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Show/", role);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Index/", role);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/New/", role,
                    RoleActions.Add);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Create/", role,
                    RoleActions.Add);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Edit/", role,
                    RoleActions.Edit);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Update/", role,
                    RoleActions.Edit);
                a.RequiresRole("~/ServiceLineProtection/ServiceLineProtectionInvestigation/Destroy/", role,
                    RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ServiceLineProtectionInvestigation>().Create(new {CustomerName = "description 0"});
            var entity1 = GetEntityFactory<ServiceLineProtectionInvestigation>().Create(new {CustomerName = "description 1"});
            var search = new SearchServiceLineProtectionInvestigation();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.CustomerName, "CustomerName");
                helper.AreEqual(entity1.CustomerName, "CustomerName", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var entity = _viewModelFactory.Build<CreateServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation>(GetEntityFactory<ServiceLineProtectionInvestigation>().BuildWithConcreteDependencies(new { OperatingCenter = opc }));
            NotifierArgs resultArgs = new NotifierArgs { OperatingCenterId = opc.Id, Module = ServiceLineProtectionInvestigationController.ROLE, Data = entity, Purpose = ServiceLineProtectionInvestigationController.CREATE_NOTIFICATION };
            _noteServ.Setup(x => x.Notify(resultArgs)).Callback<NotifierArgs>(x => resultArgs = x);

            _target.Create(entity);

            Assert.AreEqual(ServiceLineProtectionInvestigationController.CREATE_NOTIFICATION, resultArgs.Purpose);
            Assert.AreEqual(ServiceLineProtectionInvestigationController.ROLE, resultArgs.Module);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ServiceLineProtectionInvestigation>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditServiceLineProtectionInvestigation, ServiceLineProtectionInvestigation>(eq, new {
                CustomerName = expected
            }));

            Assert.AreEqual(expected, Session.Get<ServiceLineProtectionInvestigation>(eq.Id).CustomerName);
        }

        #endregion
    }
}
