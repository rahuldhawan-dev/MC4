using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class ServiceInstallationControllerTest : ContractorControllerTestBase<ServiceInstallationController, ServiceInstallation, ServiceInstallationRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IDateTimeProvider>(new Mock<IDateTimeProvider>().Object);
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
                return GetFactory<ServiceInstallationFactory>().Create(new { WorkOrder = workOrder, MiuInstallReason = GetEntityFactory<MiuInstallReasonCode>().Create() });
            };
            options.InitializeSearchTester = (tester) => {
                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(SearchServiceInstallation.OperatingCenter)] = GetFactory<UniqueOperatingCenterFactory>().Create().Id;
            };
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action
            var workorder = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });
            var result = (ViewResult)_target.New(workorder.Id);
            MyAssert.IsInstanceOfType<CreateServiceInstallation>(result.Model);
        }

        [TestMethod]
        public void TestNewReturns404IfWorkOrderDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(43134));
        }

        [TestMethod]
        public void TestCreateUpdatesWorkOrderMeterLocation()
        {
            // Inside and Outside MeterSupplementalLocation maps to Inside and Outside respectively in MeterLocation in WorkOrder
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var meterSppMeterLocation = GetFactory<InsideMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            var wo = GetFactory<FinalizationWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            var model = _viewModelFactory.BuildWithOverrides<CreateServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Create(model);

            Assert.AreEqual(si.MeterLocation.Id, Session.Get<MapCall.Common.Model.Entities.WorkOrder>(wo.Id).MeterLocation.Id);

            // Other (SecureAccess, LS etc) MeterSupplementalLocation maps to Unknown in MeterLocation in WorkOrder
            GetFactory<MeterLocationFactory>().Create(new { SAPCode = "c1" });
            GetFactory<UnknownMeterLocationFactory>().Create(new { SAPCode = "c1" });
            meterSppMeterLocation = GetFactory<SecureAccessMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            wo = GetFactory<FinalizationWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            si = GetEntityFactory<ServiceInstallation>().Create(new { MeterLocation = meterSppMeterLocation });
            model = _viewModelFactory.BuildWithOverrides<CreateServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Create(model);

            Assert.AreEqual(MeterLocation.Indices.UNKNOWN, Session.Get<MapCall.Common.Model.Entities.WorkOrder>(wo.Id).MeterLocation.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditRedirectsToShowIfWorkOrderCompleted()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new { DateCompleted = DateTime.Now, AssignedContractor = _currentUser.Contractor });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo });

            var result = _target.Edit(si.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Show", new { id = si.Id });
            Assert.AreEqual(ServiceInstallationController.ALREADY_FINALIZED, ((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestEditRedirectsToShowIfWorkOrderRejected()
        {
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var meterSppMeterLocation = GetFactory<InsideMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });

            var wo = GetFactory<WorkOrderFactory>().Create(new { DateRejected = DateTime.Now, AssignedContractor = _currentUser.Contractor });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo, MeterLocation = meterSppMeterLocation });

            var result = _target.Edit(si.Id) as RedirectToRouteResult;
           
            Assert.IsNotNull(result);
            MvcAssert.RedirectsToRoute(result, "ServiceInstallation", "Show", new { id = si.Id });
            Assert.AreEqual(ServiceInstallationController.ALREADY_FINALIZED, ((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestUpdateUpdatesWorkOrderMeterLocation()
        {
            // Inside and Outside MeterSupplementalLocation maps to Inside and Outside respectively in MeterLocation in WorkOrder
            GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" });
            var meterSppMeterLocation = GetFactory<InsideMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            var wo = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo, MeterLocation = meterSppMeterLocation });
            var model = _viewModelFactory.BuildWithOverrides<EditServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });
            
            _target.Update(model);

            Assert.AreEqual(si.MeterLocation.Id, Session.Get<MapCall.Common.Model.Entities.WorkOrder>(wo.Id).MeterLocation.Id);

            // Other (SecureAccess, LS etc) MeterSupplementalLocation maps to Unknown in MeterLocation in WorkOrder
            GetFactory<MeterLocationFactory>().Create(new { SAPCode = "c1" });
            GetFactory<UnknownMeterLocationFactory>().Create(new { SAPCode = "c1" });
            meterSppMeterLocation = GetFactory<SecureAccessMeterSupplementalLocationFactory>().Create(new { SAPCode = "c1" });
            wo = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            si = GetEntityFactory<ServiceInstallation>().Create(new { WorkOrder = wo, MeterLocation = meterSppMeterLocation });
            model = _viewModelFactory.BuildWithOverrides<EditServiceInstallation, ServiceInstallation>(si, new {
                WorkOrder = wo.Id
            });

            _target.Update(model);

            Assert.AreEqual(MeterLocation.Indices.UNKNOWN, Session.Get<MapCall.Common.Model.Entities.WorkOrder>(wo.Id).MeterLocation.Id);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Create");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/New");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Edit");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Update");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Show");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Index");
                a.RequiresLoggedInUserOnly("~/ServiceInstallation/Search");
            });
        }
    }
}