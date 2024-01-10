using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class EditWorkOrderComplianceDataTest : ViewModelTestBase<WorkOrder, EditWorkOrderComplianceData>
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
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.InitialServiceLineFlushTime);
            _vmTester.CanMapBothWays(x => x.HasPitcherFilterBeenProvidedToCustomer);
            _vmTester.CanMapBothWays(x => x.DatePitcherFilterDeliveredToCustomer);
            _vmTester.CanMapBothWays(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create());
            _vmTester.CanMapBothWays(x => x.PitcherFilterCustomerDeliveryOtherMethod);
            _vmTester.CanMapBothWays(x => x.DateCustomerProvidedAWStateLeadInformation);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(x => x.DatePitcherFilterDeliveredToCustomer, DateTime.Now, x => x.HasPitcherFilterBeenProvidedToCustomer, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PitcherFilterCustomerDeliveryMethod, GetEntityFactory<PitcherFilterCustomerDeliveryMethod>().Create().Id, x => x.HasPitcherFilterBeenProvidedToCustomer, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.PitcherFilterCustomerDeliveryOtherMethod, "Testing Other", x => x.PitcherFilterCustomerDeliveryMethod, PitcherFilterCustomerDeliveryMethod.Indices.OTHER);
            ValidationAssert.PropertyIsRequiredWhen(x => x.IsThisAMultiTenantFacility, true, x => x.HasPitcherFilterBeenProvidedToCustomer, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.NumberOfPitcherFiltersDelivered, 5, x => x.IsThisAMultiTenantFacility, true);
            ValidationAssert.PropertyIsRequiredWhen(x => x.DescribeWhichUnits, "Testing Units", x => x.IsThisAMultiTenantFacility, true);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntityAppendsNotes()
        {
            var today = DateTime.Now;
            var fullName = "Smith";

            _viewModel.InitialServiceLineFlushTime = 10;
            _entity.InitialFlushTimeEnteredBy = null;
            _entity.InitialFlushTimeEnteredAt = null;

            _user = GetFactory<AdminUserFactory>().Create(new { FullName = fullName });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);

            _vmTester.MapToEntity();

            Assert.AreEqual(fullName, _entity.InitialFlushTimeEnteredBy.FullName);
            Assert.AreEqual(today, _entity.InitialFlushTimeEnteredAt);
        }

        #endregion
    }
}
