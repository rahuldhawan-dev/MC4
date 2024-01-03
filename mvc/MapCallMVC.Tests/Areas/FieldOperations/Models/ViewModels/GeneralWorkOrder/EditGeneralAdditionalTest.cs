using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class EditGeneralAdditionalTest : ViewModelTestBase<WorkOrder, EditGeneralAdditional>
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
            ValidationAssert.EntityMustExist(x => x.PreviousServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.PreviousServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.CompanyServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.CompanyServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.CustomerServiceLineMaterial, GetEntityFactory<ServiceMaterial>().Create());
            ValidationAssert.EntityMustExist(x => x.CustomerServiceLineSize, GetEntityFactory<ServiceSize>().Create());
            ValidationAssert.EntityMustExist(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create());
            ValidationAssert.EntityMustExist(x => x.FinalWorkDescription, GetEntityFactory<WorkDescription>().Create());
            ValidationAssert.EntityMustExist(x => x.CustomerImpact, GetEntityFactory<CustomerImpactRange>().Create());
            ValidationAssert.EntityMustExist(x => x.RepairTime, GetEntityFactory<RepairTimeRange>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PreviousServiceLineMaterial);
            _vmTester.CanMapBothWays(x => x.PreviousServiceLineSize);
            _vmTester.CanMapBothWays(x => x.CompanyServiceLineMaterial);
            _vmTester.CanMapBothWays(x => x.CompanyServiceLineSize);
            _vmTester.CanMapBothWays(x => x.CustomerServiceLineMaterial);
            _vmTester.CanMapBothWays(x => x.CustomerServiceLineSize);
            _vmTester.CanMapBothWays(x => x.PitcherFilterCustomerDeliveryOtherMethod);
            _vmTester.CanMapBothWays(x => x.WorkDescription);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
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
            ValidationAssert.PropertyIsRequiredWhen(x => x.LostWater, 12, x => x.FinalWorkDescription, (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PitcherFilterCustomerDeliveryOtherMethod, "Testing Other", x => x.PitcherFilterCustomerDeliveryMethod, PitcherFilterCustomerDeliveryMethod.Indices.OTHER);
            ValidationAssert.PropertyIsRequiredWhen(x => x.CustomerImpact, GetEntityFactory<CustomerImpactRange>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
            ValidationAssert.PropertyIsRequiredWhen(x => x.RepairTime, GetEntityFactory<RepairTimeRange>().Create().Id, x => x.FinalWorkDescription, (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
            ValidationAssert.PropertyIsRequiredWhen(x => x.TrafficImpact, true, x => x.FinalWorkDescription, (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate string length
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntityAppendsNotes()
        {
            var today = DateTime.Now;
            var fullName = "Smith";
            var notes1 = "Testing Notes";
            var notes2 = "Additional Notes";

            _entity.Notes = null;
            _viewModel.AppendNotes = notes1;

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);
            _user = GetFactory<AdminUserFactory>().Create(new { FullName = fullName });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _vmTester.MapToEntity();

            var expectedOutput1 = string.Format("{0} {1} {2}", fullName,
                today.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS), notes1);
            Assert.AreEqual(expectedOutput1, _entity.Notes);

            var expectedOutput2 = string.Format("{0} {1} {2}", fullName,
                today.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS), notes2);
            _viewModel.AppendNotes = notes2;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedOutput1 + Environment.NewLine + expectedOutput2, _entity.Notes);
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

            service.WorkOrders = new List<WorkOrder> { _entity };
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

        #endregion
    }
}
