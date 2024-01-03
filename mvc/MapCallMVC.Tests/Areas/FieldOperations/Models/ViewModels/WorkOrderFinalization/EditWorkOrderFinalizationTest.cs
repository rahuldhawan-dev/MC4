using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization
{
    [TestClass]
    public class EditWorkOrderFinalizationTest : ViewModelTestBase<WorkOrder, EditWorkOrderFinalization>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.FlushingNoticeType,
                GetEntityFactory<WorkOrderFlushingNoticeType>().Create());
            ValidationAssert.EntityMustExist(x => x.PreviousServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.PreviousServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.CompanyServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.CompanyServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.CustomerServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.CustomerServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateCompleted);
            _vmTester.CanMapBothWays(x => x.FlushingNoticeType);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltCompleted);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.CompletedDate);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PreviousServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PreviousServiceLineSize, GetEntityFactory<ServiceSize>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.CustomerServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.CustomerServiceLineSize, GetEntityFactory<ServiceSize>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.CompanyServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.CompanyServiceLineSize, GetEntityFactory<ServiceSize>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.DoorNoticeLeftDate, DateTime.Now, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.InitialServiceLineFlushTime, 23, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.HasPitcherFilterBeenProvidedToCustomer, true, x => x.FinalWorkDescription, (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
            ValidationAssert.PropertyIsRequiredWhen(x => x.DatePitcherFilterDeliveredToCustomer, DateTime.Now, x => x.HasPitcherFilterBeenProvidedToCustomer, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create().Id, x => x.HasPitcherFilterBeenProvidedToCustomer, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PitcherFilterCustomerDeliveryOtherMethod, "Testing Other", x => x.PitcherFilterCustomerDeliveryMethod, PitcherFilterCustomerDeliveryMethod.Indices.OTHER);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate string length
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsForMainBreakRepairWorkOrderWithNoMainBreaks()
        {
            _viewModel.FinalWorkDescription = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR;
            _viewModel.WorkOrder.MainBreaks.Clear();

            ValidationAssert.ModelStateHasNonPropertySpecificError(EditWorkOrderFinalization.MAIN_BREAK_INFO_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestValidationFailsIfWorkOrderHasOpenCrewAssignments()
        {
            var ca = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = _viewModel.WorkOrder,
                DateStarted = DateTime.Now
            });
            ca.DateEnded = null;
            _viewModel.WorkOrder.CrewAssignments.Add(ca);

            ValidationAssert.ModelStateHasNonPropertySpecificError(EditWorkOrderFinalization.OPEN_CREW_ASSIGNMENTS_ERROR_MESSAGE);
        }

        [TestMethod]
        public void TestValidationFailsIfWorkOrderHasNoScheduleOfValues()
        {
            _viewModel.WorkOrder.OperatingCenter.HasWorkOrderInvoicing = true;
            _viewModel.WorkOrder.WorkOrdersScheduleOfValues.Clear();

            ValidationAssert.ModelStateHasNonPropertySpecificError(EditWorkOrderFinalization.SCHEDULE_OF_VALUES_ERROR_MESSAGE);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsCompletedBy()
        {
            var fullName = "Smith";

            _entity.CompletedBy = null;

            _user = GetFactory<AdminUserFactory>().Create(new { FullName = fullName });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _vmTester.MapToEntity();

            Assert.IsNotNull(_entity.CompletedBy);
            Assert.AreEqual(fullName, _entity.CompletedBy.FullName);
        }

        [TestMethod]
        public void TestMapToEntityTransfersServiceLineInfoToLinkedService()
        {
            var service = GetEntityFactory<Service>().Create();

            service.PreviousServiceCustomerMaterial = null;
            service.PreviousServiceSize = null;
            service.CustomerSideMaterial = null;
            service.CustomerSideSize = null;
            service.ServiceMaterial = null;
            service.ServiceSize = null;

            service.WorkOrders = new List<WorkOrder>{ _entity };
            _entity.Service = service;

            var prevServiceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var prevServiceSize = GetEntityFactory<ServiceSize>().Create();
            var customerSideMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var customerSideSize = GetEntityFactory<ServiceSize>().Create();
            var companySideMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var companySideSize = GetEntityFactory<ServiceSize>().Create();

            _viewModel.PreviousServiceLineMaterial = prevServiceMaterial.Id;
            _viewModel.PreviousServiceLineSize = prevServiceSize.Id;
            _viewModel.CustomerServiceLineMaterial = customerSideMaterial.Id;
            _viewModel.CustomerServiceLineSize = customerSideSize.Id;
            _viewModel.CompanyServiceLineMaterial = companySideMaterial.Id;
            _viewModel.CompanyServiceLineSize = companySideSize.Id;
            
            _user = GetFactory<AdminUserFactory>().Create(new { FullName = "Smith" });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _vmTester.MapToEntity();

            Assert.AreEqual(prevServiceMaterial, service.PreviousServiceMaterial);
            Assert.AreEqual(prevServiceSize, service.PreviousServiceSize);
            Assert.AreEqual(customerSideMaterial, service.CustomerSideMaterial);
            Assert.AreEqual(customerSideSize, service.CustomerSideSize);
            Assert.AreEqual(companySideMaterial, service.ServiceMaterial);
            Assert.AreEqual(companySideSize, service.ServiceSize);
        }

        [TestMethod]
        [DataRow(null, null, null, null, false)]
        [DataRow(1, 1, 2, 2, false)]
        [DataRow(1, 2, 1, 2, true)]
        [DataRow(1, 1, 1, 2, true)]
        [DataRow(1, 2, 2, 2, true)]
        public void TestMapToEntitySetsServiceNeedsToSync(
            int? oldCompanyMaterial, int? newCompanyMaterial, int? oldCustomerMaterial, int? newCustomerMaterial, bool flag)
        {
            var serviceUtilityType = GetEntityFactory<ServiceUtilityType>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create(new { ServiceUtilityType = serviceUtilityType });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = "123456789",
                PremiseNumber = "9100327803",
                ServiceUtilityType = serviceUtilityType
            });
            
            var service = GetEntityFactory<Service>().Create(new { Premise = premise, NeedsToSync = false });
            
            service.ServiceMaterial = (oldCompanyMaterial.HasValue)
                ? new ServiceMaterial { Id = oldCompanyMaterial.Value }
                : null;

            service.CustomerSideMaterial = (oldCustomerMaterial.HasValue)
                ? new ServiceMaterial { Id = oldCustomerMaterial.Value }
                : null;

            _viewModel.CustomerServiceLineMaterial = newCustomerMaterial;
            _viewModel.CompanyServiceLineMaterial = newCompanyMaterial;
            _entity.Service = service;
            
            _user = GetFactory<AdminUserFactory>().Create(new { FullName = "Smith" });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _vmTester.MapToEntity();
            
            Assert.AreEqual(flag, service.NeedsToSync);
        }
        
        [TestMethod]
        [DataRow(1, 2, 1, 2, false)]
        [DataRow(1, 1, 1, 2, false)]
        [DataRow(1, 2, 2, 2, false)]
        public void TestMapToEntityDoesNotSetServiceNeedsToSyncIfServiceNotLinkedToAPremise(int? oldCompanyMaterial, int? newCompanyMaterial, int? oldCustomerMaterial, int? newCustomerMaterial, bool flag)
        {
            var service = GetEntityFactory<Service>().Create(new { NeedsToSync = false });
            
            service.ServiceMaterial = (oldCompanyMaterial.HasValue)
                ? new ServiceMaterial { Id = oldCompanyMaterial.Value }
                : null;

            service.CustomerSideMaterial = (oldCustomerMaterial.HasValue)
                ? new ServiceMaterial { Id = oldCustomerMaterial.Value }
                : null;

            _viewModel.CustomerServiceLineMaterial = newCustomerMaterial;
            _viewModel.CompanyServiceLineMaterial = newCompanyMaterial;
            _entity.Service = service;
            
            _user = GetFactory<AdminUserFactory>().Create(new { FullName = "Smith" });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _vmTester.MapToEntity();
            
            Assert.AreEqual(flag, service.NeedsToSync);
        }

        #endregion
    }
}
