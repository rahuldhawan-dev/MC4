using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateEnvironmentalPermitFeeTest : MapCallMvcInMemoryDatabaseTestBase<EnvironmentalPermitFee>
    {
        #region Fields

        private ViewModelTester<CreateEnvironmentalPermitFee, EnvironmentalPermitFee> _vmTester;
        private CreateEnvironmentalPermitFee _viewModel;
        private EnvironmentalPermitFee _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<EnvironmentalPermitFee>().Create();
            _viewModel = _viewModelFactory.Build<CreateEnvironmentalPermitFee>();
            _vmTester = new ViewModelTester<CreateEnvironmentalPermitFee, EnvironmentalPermitFee>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestSetDefaultsSetsOperatingCenterFromEnvironmentalPermit()
        {
            var permit = GetEntityFactory<EnvironmentalPermit>().Create();
            var opc = GetEntityFactory<OperatingCenter>().Create();
            permit.OperatingCenters.Add(opc);
            var expected = permit.OperatingCenters.Select(x => x.Id).ToArray();

            _viewModel.OperatingCenter = null;
            _viewModel.EnvironmentalPermit = permit.Id;

            _viewModel.SetDefaults();

            CollectionAssert.AreEqual(expected, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EnvironmentalPermit, GetEntityFactory<EnvironmentalPermit>().Create());
            _vmTester.CanMapBothWays(x => x.EnvironmentalPermitFeeType, GetEntityFactory<EnvironmentalPermitFeeType>().Create());
            _vmTester.CanMapBothWays(x => x.Fee);
            _vmTester.CanMapBothWays(x => x.PaymentOwner, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.PaymentDueInterval);
            _vmTester.CanMapBothWays(x => x.PaymentEffectiveDate);
            _vmTester.CanMapBothWays(x => x.PaymentDueFrequencyUnit, GetEntityFactory<RecurringFrequencyUnit>().Create());
            _vmTester.CanMapBothWays(x => x.PaymentOrganizationName);
            _vmTester.CanMapBothWays(x => x.PaymentOrganizationContactInfo);
            _vmTester.CanMapBothWays(x => x.PaymentMethod, GetEntityFactory<EnvironmentalPermitFeePaymentMethod>().Create());
        }

        [TestMethod]
        public void TestPropertiesThatOnlyMapToViewModel()
        {
            _vmTester.CanMapToViewModel(x => x.PaymentMethodMailAddress, "Sure");
            _vmTester.CanMapToViewModel(x => x.PaymentMethodPhone, "Sure");
            _vmTester.CanMapToViewModel(x => x.PaymentMethodUrl, "Sure");
        }

        [TestMethod]
        public void TestPaymentMethodMailAddressOnlyMapsToEntityWhenPaymentMethodIsMail()
        {
            var method = GetFactory<MailEnvironmentalPermitFeePaymentMethodFactory>().Create();
            _entity.PaymentMethodMailAddress = null;
            _viewModel.PaymentMethodMailAddress = "viewmodel";
            _viewModel.PaymentMethod = 0;

            _vmTester.MapToEntity();
            Assert.IsNull(_entity.PaymentMethodMailAddress);

            _viewModel.PaymentMethod = method.Id;
            _vmTester.MapToEntity();
            Assert.AreEqual("viewmodel", _entity.PaymentMethodMailAddress);
        }

        [TestMethod]
        public void TestPaymentMethodPhoneOnlyMapsToEntityWhenPaymentMethodIsPhone()
        {
            var method = GetFactory<PhoneEnvironmentalPermitFeePaymentMethodFactory>().Create();
            _entity.PaymentMethodPhone = null;
            _viewModel.PaymentMethodPhone = "viewmodel";
            _viewModel.PaymentMethod = 0;

            _vmTester.MapToEntity();
            Assert.IsNull(_entity.PaymentMethodPhone);

            _viewModel.PaymentMethod = method.Id;
            _vmTester.MapToEntity();
            Assert.AreEqual("viewmodel", _entity.PaymentMethodPhone);
        }

        [TestMethod]
        public void TestPaymentMethodUrlOnlyMapsToEntityWhenPaymentMethodIsUrl()
        {
            var method = GetFactory<UrlEnvironmentalPermitFeePaymentMethodFactory>().Create();
            _entity.PaymentMethodUrl = null;
            _viewModel.PaymentMethodUrl = "viewmodel";
            _viewModel.PaymentMethod = 0;

            _vmTester.MapToEntity();
            Assert.IsNull(_entity.PaymentMethodUrl);

            _viewModel.PaymentMethod = method.Id;
            _vmTester.MapToEntity();
            Assert.AreEqual("viewmodel", _entity.PaymentMethodUrl);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentalPermit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Fee);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PaymentMethod);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PaymentMethodMailAddress, "address", x => x.PaymentMethod, GetFactory<MailEnvironmentalPermitFeePaymentMethodFactory>().Create().Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PaymentMethodPhone, "123-123-1233", x => x.PaymentMethod, GetFactory<PhoneEnvironmentalPermitFeePaymentMethodFactory>().Create().Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PaymentMethodUrl, "https://ww.google.com", x => x.PaymentMethod, GetFactory<UrlEnvironmentalPermitFeePaymentMethodFactory>().Create().Id);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.EnvironmentalPermit, GetEntityFactory<EnvironmentalPermit>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PaymentOwner, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PaymentDueFrequencyUnit, GetEntityFactory<RecurringFrequencyUnit>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PaymentMethod, GetEntityFactory<EnvironmentalPermitFeePaymentMethod>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PaymentMethod, GetEntityFactory<EnvironmentalPermitFeePaymentMethod>().Create());
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PaymentMethodPhone, EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PaymentOrganizationName, EnvironmentalPermitFee.StringLengths.PAYMENT_ORGANIZATION_NAME);

            // Value needs to be a url, so we need to generate one for this test.
            var url = "http://www.google.com/";
            url = url + url.PadRight(EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_URL - url.Length, 'o');
            _viewModel.PaymentMethodUrl = url;
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PaymentMethodUrl, EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_URL, true);
        }

        [TestMethod]
        public void TestPaymentMethodUrlMustBeAUrl()
        {
            ValidationAssert.PropertyMustBeUrl(_viewModel, x => x.PaymentMethodUrl);
        }

        [TestMethod]
        public void TestValidationFailsIfEnvironmentalPermitDoesNotRequireFees()
        {
            _viewModel.EnvironmentalPermit = _entity.EnvironmentalPermit.Id;
            _entity.EnvironmentalPermit.RequiresFees = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.EnvironmentalPermit, $"Permit#{_viewModel.EnvironmentalPermit} must require fees in order to add or edit a fee.");

            _entity.EnvironmentalPermit.RequiresFees = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.EnvironmentalPermit);
        }
        #endregion

        #endregion
    }
}
