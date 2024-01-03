using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Contractors.Models
{
    public abstract class ContractorViewModelTest<TViewModel> : ViewModelTestBase<Contractor, TViewModel>
        where TViewModel : ContractorViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.HouseNumber);
            _vmTester.CanMapBothWays(x => x.ApartmentNumber);
            _vmTester.CanMapBothWays(x => x.Zip);
            _vmTester.CanMapBothWays(x => x.Phone);
            _vmTester.CanMapBothWays(x => x.IsUnionShop);
            _vmTester.CanMapBothWays(x => x.IsBcpPartner);
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.ContractorsAccess);
            _vmTester.CanMapBothWays(x => x.AWR);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.Town);
            _vmTester.CanMapBothWays(x => x.Street);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Name);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(x => x.Street, GetEntityFactory<Street>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.ApartmentNumber, Contractor.StringLengths.APARTMENT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.HouseNumber, Contractor.StringLengths.HOUSE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Name, Contractor.StringLengths.NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Phone, Contractor.StringLengths.PHONE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Zip, Contractor.StringLengths.ZIP);
            ValidationAssert.PropertyHasMaxStringLength(x => x.VendorId, Contractor.StringLengths.VENDOR_ID);
        }

        #endregion
    }

    [TestClass]
    public class CreateContractorTest : ContractorViewModelTest<CreateContractor> { }
}
