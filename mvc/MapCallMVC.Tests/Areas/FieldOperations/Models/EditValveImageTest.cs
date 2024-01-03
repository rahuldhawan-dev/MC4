using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditValveImageTest : ViewModelTestBase<ValveImage, EditValveImage>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IValveImageRepository> _mockImageRepo;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IValveNormalPositionRepository>().Use<ValveNormalPositionRepository>();
            e.For<IValveOpenDirectionRepository>().Use<ValveOpenDirectionRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            _mockImageRepo = e.For<IValveImageRepository>().Mock();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ApartmentNumber);
            _vmTester.CanMapBothWays(x => x.CrossStreet);
            _vmTester.CanMapBothWays(x => x.CrossStreetPrefix);
            _vmTester.CanMapBothWays(x => x.CrossStreetSuffix);
            _vmTester.CanMapBothWays(x => x.DateCompleted);
            _vmTester.CanMapBothWays(x => x.IsDefaultImageForValve);
            _vmTester.CanMapBothWays(x => x.Location);
            _vmTester.CanMapBothWays(x => x.ValveSize);
            _vmTester.CanMapBothWays(x => x.NumberOfTurns);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.StreetPrefix);
            _vmTester.CanMapBothWays(x => x.StreetSuffix);
            _vmTester.CanMapBothWays(x => x.TownSection);
            _vmTester.CanMapBothWays(x => x.ValveNumber);
            _vmTester.CanMapBothWays(x => x.OfficeReviewRequired);
        }

        [TestMethod]
        public void TestOpenDirectionCanMapBothWays()
        {
            var opc = GetFactory<ValveOpenDirectionFactory>().Create();
            _entity.OpenDirection = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OpenDirection);

            _entity.OpenDirection = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OpenDirection);
        }

        [TestMethod]
        public void TestNormalPositionCanMapBothWays()
        {
            var opc = GetFactory<ValveNormalPositionFactory>().Create();
            _entity.NormalPosition = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.NormalPosition);

            _entity.NormalPosition = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.NormalPosition);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var t = GetFactory<TownFactory>().Create();
            _entity.Town = t;
            _vmTester.MapToViewModel();
            Assert.AreEqual(t.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();
            Assert.AreSame(t, _entity.Town);
        }

        [TestMethod]
        public void TestValveCanMapBothWays()
        {
            var v = GetFactory<ValveFactory>().Create();
            _entity.Valve = v;
            _vmTester.MapToViewModel();
            Assert.AreEqual(v.Id, _viewModel.Valve);

            _entity.Valve = null;
            _vmTester.MapToEntity();
            Assert.AreSame(v, _entity.Valve);
        }

        [TestMethod]
        public void TestMapSetStreetIdentifyingIntegerFromServiceIfServiceAndServiceStreetAreNotNull()
        {
            var street = GetFactory<StreetFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create();

            _entity.Valve = null;
            _viewModel.StreetIdentifyingInteger = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.StreetIdentifyingInteger);

            valve.Street = null;
            _entity.Valve = valve;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.StreetIdentifyingInteger);

            valve.Street = street;
            _vmTester.MapToViewModel();
            Assert.AreEqual(street.Id, _viewModel.StreetIdentifyingInteger);
        }

        [TestMethod]
        public void TestMapToEntitySetsStreetInformationIfStreetIdentifyingIntegerIsSet()
        {
            _viewModel.StreetIdentifyingInteger = null;
            _viewModel.Street = "NoStreetId";
            _viewModel.StreetPrefix = "Q";
            _viewModel.StreetSuffix = "Ave";

            _vmTester.MapToEntity();

            Assert.AreEqual("NoStreetId", _entity.Street);
            Assert.AreEqual("Q", _entity.StreetPrefix);
            Assert.AreEqual("Ave", _entity.StreetSuffix);

            var s = GetFactory<StreetFactory>().Create(new
            {
                Name = "SomeStreet",
                Prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "Z" }),
                Suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" })
            });

            _viewModel.StreetIdentifyingInteger = s.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("SomeStreet", _entity.Street);
            Assert.AreEqual("Z", _entity.StreetPrefix);
            Assert.AreEqual("St", _entity.StreetSuffix);
        }
        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.CrossStreet)
                            .PropertyIsRequired(x => x.IsDefaultImageForValve, BaseValveImageViewModel.ErrorMessages.IS_DEFAULT_IMAGE)
                            .PropertyIsRequired(x => x.ValveSize)
                            .PropertyIsRequired(x => x.OperatingCenter)
                            .PropertyIsRequired(x => x.Town);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create());
            ValidationAssert.EntityMustExist(x => x.Town, GetFactory<TownFactory>().Create());
            ValidationAssert.EntityMustExist(x => x.Valve, GetFactory<ValveFactory>().Create());
            ValidationAssert.EntityMustExist(x => x.OpenDirection, GetFactory<ValveOpenDirectionFactory>().Create());
            ValidationAssert.EntityMustExist(x => x.NormalPosition, GetFactory<ValveNormalPositionFactory>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.TownSection, ValveImage.StringLengths.TOWNSECTION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetNumber, ValveImage.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ApartmentNumber, ValveImage.StringLengths.APARTMENT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetPrefix, ValveImage.StringLengths.STREET_PREFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Street, ValveImage.StringLengths.STREET);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetSuffix, ValveImage.StringLengths.STREET_SUFFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ValveNumber, ValveImage.StringLengths.VALVE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreetPrefix, ValveImage.StringLengths.CROSS_STREET_PREFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreet, ValveImage.StringLengths.CROSS_STREET);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreetSuffix, ValveImage.StringLengths.CROSS_STREET_SUFFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Location, ValveImage.StringLengths.LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.NumberOfTurns, ValveImage.StringLengths.NUMBER_OF_TURNS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ValveSize, ValveImage.StringLengths.VALVE_SIZE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.DateCompleted, ValveImage.StringLengths.DATE_COMPLETED);
        }

        #endregion

        #endregion
    }
}
