using System.Text;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class ContactViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Contact>
    {
        private ViewModelTester<ContactViewModel, Contact> _vmTester;
        private ContactViewModel _viewModel;
        private Contact _entity;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new ContactViewModel(_container);
            _entity = GetFactory<ContactFactory>().Create();
            _vmTester = new ViewModelTester<ContactViewModel, Contact>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestCanMapAddressToViewModel()
        {
            _entity.Address.Address1 = "123 Neato St";
            _viewModel.Map(_entity);
            Assert.AreEqual("123 Neato St", _viewModel.Address.Address1);

            _entity.Address = null;
            _viewModel.Map(_entity);
            Assert.IsNull(_viewModel.Address, "No Address on Contact should then null the Address object on ContactViewModel.");
        }

        [TestMethod]
        public void TestMapToEntityRemovesAddressReferenceIfAddressIsNotValid()
        {
            Assert.IsNotNull(_entity.Address, "Test isn't setup correctly");
            _viewModel.Address = new AddressViewModel(_container);
            _viewModel.MapToEntity(_entity);
            Assert.IsNull(_entity.Address);
        }

        [TestMethod]
        public void TestMapToEntityAddsNewAddressObjectToEntityIfItIsMissingOne()
        {
            _entity.Address = null;
            _viewModel.Address = new AddressViewModel(_container) { 
                Address1 = "blah",
                ZipCode = "00000",
                Town = 1,
            };
            Assert.IsTrue(_viewModel.Address.HasRequiredValues(), "Test isn't setup correctly");

            _viewModel.MapToEntity(_entity);
            Assert.IsNotNull(_entity.Address);
            Assert.AreEqual(0, _entity.Address.Id);
        }

        [TestMethod]
        public void TestMapToEntityMapsAddressObject()
        {
            _viewModel.Address = new AddressViewModel(_container) {
                Address1 = "blah",
                ZipCode = "00000",
                Town = 1,
            };
            Assert.IsTrue(_viewModel.Address.HasRequiredValues(), "Test isn't setup correctly");

            _viewModel.MapToEntity(_entity);
            Assert.IsNotNull(_entity.Address);
            Assert.AreEqual("blah", _entity.Address.Address1);
        }

        [TestMethod]
        public void TestCanMapToFirstNameBothWays()
        {
            _vmTester.CanMapBothWays(x => x.FirstName);
        }

        [TestMethod]
        public void TestCanMapToLastNameBothWays()
        {
            _vmTester.CanMapBothWays(x => x.LastName);
        }

        [TestMethod]
        public void TestCanMapEmailBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Email);
        }

        [TestMethod]
        public void TestCanMapBusinessPhoneNumberBothWays()
        {
            _vmTester.CanMapBothWays(x => x.BusinessPhoneNumber);
        }

        [TestMethod]
        public void TestCanMapHomePhoneNumberBothWays()
        {
            _vmTester.CanMapBothWays(x => x.HomePhoneNumber);
        }

        [TestMethod]
        public void TestCanMapMobilePhoneNumberBothWays()
        {
            _vmTester.CanMapBothWays(x => x.MobilePhoneNumber);
        }

        [TestMethod]
        public void TestCanMapFaxNumberBothWays()
        {
            _vmTester.CanMapBothWays(x => x.FaxNumber);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestBusinessPhoneNumberHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.BusinessPhoneNumber,
                Contact.StringLengths.BUSINESS_PHONE);
        }

        [TestMethod]
        public void TestEmailMustBeValidEmailAddress()
        {
            _viewModel.Email = "not an email";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Email, "Invalid email address.");

            _viewModel.Email = "email@address.com";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Email);
        }

        [TestMethod]
        public void TestEmailHasMaxLength()
        {
            const string @dotcom = "@dot.com";
            var sb = new StringBuilder();
            for (var i = 0; i < Contact.StringLengths.EMAIL - @dotcom.Length; i++)
            {
                sb.Append("a");
            }
            sb.Append(@dotcom);
            _viewModel.Email = sb.ToString();
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Email, Contact.StringLengths.EMAIL, useCurrentPropertyValue: true);
        }

        [TestMethod]
        public void TestFaxNumberHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FaxNumber,
                Contact.StringLengths.FAX);
        }

        [TestMethod]
        public void TestFirstNameIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirstName);
        }

        [TestMethod]
        public void TestFirstNameHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FirstName, Contact.StringLengths.FIRST_NAME);
        }
        
        [TestMethod]
        public void TestHomePhoneNumberHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.HomePhoneNumber, Contact.StringLengths.HOME_PHONE);
        }

        [TestMethod]
        public void TestLastNameIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LastName);
        }

        [TestMethod]
        public void TestLastNameHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LastName, Contact.StringLengths.LAST_NAME);
        }

        [TestMethod]
        public void TestMobilePhoneNumberHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MobilePhoneNumber, Contact.StringLengths.MOBILE);
        }

        [TestMethod]
        public void TestMiddleInitialHasMaxLength()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MiddleInitial, Contact.StringLengths.MIDDLE_INITIAL);
        }

        [TestMethod]
        public void TestAddressValidationOnlyHappensIfTheAddressObjectItselfIsNotNull()
        {
            _viewModel.Address = null;
            ValidationAssert.ModelStateIsValid(_viewModel, "Address.Address1");

            _viewModel.Address = new AddressViewModel(_container);
            ValidationAssert.ModelStateIsValid(_viewModel, "Address.Address1");

            Assert.Inconclusive("TODO: The Address property isn't having its validators called even though in practice it does.");
        }

        #endregion

        #endregion
    }
}
