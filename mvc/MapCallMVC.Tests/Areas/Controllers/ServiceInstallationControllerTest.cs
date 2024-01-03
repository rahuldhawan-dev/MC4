using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class ServiceInstallationControllerTest : MapCallMvcControllerTestBase<ServiceInstallationController, ServiceInstallation>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Show", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Search", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Index", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/New", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Create", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Update", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceInstallation/Destroy", role, RoleActions.Delete);
            });
        }

        #region New/Create

        [TestMethod]
        public void TestNewRedirectsToSearchIfWorkOrderIsCompleted()
        {
            var wo = GetEntityFactory<WorkOrder>().Create(new { DateCompleted = DateTime.Now });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, wo.OperatingCenter, _currentUser, RoleActions.Read);

            var result = _target.New(wo.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Search");
            _target.AssertTempDataContainsMessage(ServiceInstallationController.ALREADY_FINALIZED,
                ServiceInstallationController.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestNewRedirectsToSearchIfWorkOrderIsRejected()
        {
            var wo = GetEntityFactory<WorkOrder>().Create(new { DateRejected = DateTime.Now });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, wo.OperatingCenter, _currentUser, RoleActions.Read);

            var result = _target.New(wo.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Search");
            _target.AssertTempDataContainsMessage(ServiceInstallationController.ALREADY_FINALIZED, ServiceInstallationController.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestCreateUpdatesWorkOrderMeterLocation()
        {
            // Inside and Outside MeterSupplementalLocation maps to Inside and Outside respectively in MeterLocation in WorkOrder
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var meterSppMeterLocation = GetFactory<InsideMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            var wo = GetEntityFactory<WorkOrder>().Create();
            var si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            var model = _viewModelFactory.BuildWithOverrides<CreateServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Create(model);

            Assert.AreEqual(si.MeterLocation.Id, Session.Get<WorkOrder>(wo.Id).MeterLocation.Id);

            // Other (SecureAccess, LS etc) MeterSupplementalLocation maps to Unknown in MeterLocation in WorkOrder
            GetFactory<MeterLocationFactory>().Create(new { SAPCode = "c1" });
            GetFactory<UnknownMeterLocationFactory>().Create(new { SAPCode = "c1" });
            meterSppMeterLocation = GetFactory<SecureAccessMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            wo = GetEntityFactory<WorkOrder>().Create();
            si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            model = _viewModelFactory.BuildWithOverrides<CreateServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Create(model);

            Assert.AreEqual(MeterLocation.Indices.UNKNOWN, Session.Get<WorkOrder>(wo.Id).MeterLocation.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditRedirectsToShowIfWorkOrderCompleted()
        {
            var wo = GetEntityFactory<WorkOrder>().Create(new { DateCompleted = DateTime.Now });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo });

            var result = _target.Edit(si.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Show", new { id = si.Id });
            _target.AssertTempDataContainsMessage(ServiceInstallationController.ALREADY_FINALIZED, ServiceInstallationController.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestEditRedirectsToShowIfWorkOrderRejected()
        {
            var wo = GetEntityFactory<WorkOrder>().Create(new { DateRejected = DateTime.Now });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo });

            var result = _target.Edit(si.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Show", new { id = si.Id });
            _target.AssertTempDataContainsMessage(ServiceInstallationController.ALREADY_FINALIZED, ServiceInstallationController.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            GetFactory<MeterLocationFactory>().Create(new { SAPCode = "c1" });
            var wo = GetEntityFactory<WorkOrder>().Create();
            var si = GetEntityFactory<ServiceInstallation>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });
            
            var result = _target.Update(model);

            Assert.AreEqual(wo.Id, Session.Get<ServiceInstallation>(si.Id).WorkOrder.Id);
        }

        [TestMethod]
        public void TestUpdateUpdatesWorkOrderMeterLocation()
        {
            // Inside and Outside MeterSupplementalLocation maps to Inside and Outside respectively in MeterLocation in WorkOrder
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var meterSppMeterLocation = GetFactory<InsideMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            var wo = GetEntityFactory<WorkOrder>().Create();
            var si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            var model = _viewModelFactory.BuildWithOverrides<EditServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Update(model);

            Assert.AreEqual(si.MeterLocation.Id, Session.Get<WorkOrder>(wo.Id).MeterLocation.Id);

            // Other (SecureAccess, LS etc) MeterSupplementalLocation maps to Unknown in MeterLocation in WorkOrder
            GetFactory<MeterLocationFactory>().Create(new { SAPCode = "c1" });
            GetFactory<UnknownMeterLocationFactory>().Create(new { SAPCode = "c1" });
            meterSppMeterLocation = GetFactory<SecureAccessMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            wo = GetEntityFactory<WorkOrder>().Create();
            si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            model = _viewModelFactory.BuildWithOverrides<EditServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Update(model);

            Assert.AreEqual(MeterLocation.Indices.UNKNOWN, Session.Get<WorkOrder>(wo.Id).MeterLocation.Id);
        }

        #endregion
    }
}