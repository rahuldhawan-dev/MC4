using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditOperatingCenterTest : ViewModelTestBase<OperatingCenter, EditOperatingCenter>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMappingId()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 12);
            _vmTester.DoesNotMapToEntity(x => x.Id, 43);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.ArcMobileMapId)
               .CanMapBothWays(x => x.CompanyInfo, "blah", "blah again")
               .CanMapBothWays(x => x.DataCollectionMapUrl)
               .CanMapBothWays(x => x.FaxNumber, "blah", "blah again")
               .CanMapBothWays(x => x.HydrantInspectionFrequency, 2, 3)
               .CanMapBothWays(x => x.HydrantPaintingFrequency, 2, 3)
               .CanMapBothWays(x => x.HydrantPaintingFrequencyUnit)
               .CanMapBothWays(x => x.InfoMasterMapId)
               .CanMapBothWays(x => x.LargeValveInspectionFrequency, 2, 3)
               .CanMapBothWays(x => x.MailingAddressCityStateZip, "blah", "blah again")
               .CanMapBothWays(x => x.MailingAddressName, "blah", "blah again")
               .CanMapBothWays(x => x.MailingAddressStreet, "blah", "blah again")
               .CanMapBothWays(x => x.MapId)
               .CanMapBothWays(x => x.OperatingCenterName, "blah", "blah again")
               .CanMapBothWays(x => x.PermitsCapitalUserName, "blah", "blah again")
               .CanMapBothWays(x => x.PermitsOMUserName, "blfuaee", "fapejiaepjge")
               .CanMapBothWays(x => x.PhoneNumber, "phone", "other phone")
               .CanMapBothWays(x => x.ServiceContactPhoneNumber, "phone", "other phone")
               .CanMapBothWays(x => x.SewerOpeningInspectionFrequency, 2, 3)
               .CanMapBothWays(x => x.SmallValveInspectionFrequency, 2, 3)
               .CanMapBothWays(x => x.WorkOrdersEnabled, false, true)
               .CanMapBothWays(x => x.ZoneStartYear)
               .CanMapBothWays(x => x.PaintingZoneStartYear);
        }

        #region HydrantInspectionFrequencyUnit

        [TestMethod]
        public void TestMapToModelSetsHydrantInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.HydrantInspectionFrequencyUnit = freq;
            _viewModel.HydrantInspectionFrequencyUnit = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(freq.Id, _viewModel.HydrantInspectionFrequencyUnit);
        }

        [TestMethod]
        public void TestMapToEntitySetsHydrantInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _viewModel.HydrantInspectionFrequencyUnit = freq.Id;
            _entity.HydrantInspectionFrequencyUnit = null;

            _vmTester.MapToEntity();

            Assert.AreSame(freq, _entity.HydrantInspectionFrequencyUnit);
        }

        #endregion

        #region LargeValveInspectionFrequencyUnit

        [TestMethod]
        public void TestMapToModelSetsLargeValveInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.LargeValveInspectionFrequencyUnit = freq;
            _viewModel.LargeValveInspectionFrequencyUnit = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(freq.Id, _viewModel.LargeValveInspectionFrequencyUnit);
        }

        [TestMethod]
        public void TestMapToEntitySetsLargeValveInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _viewModel.LargeValveInspectionFrequencyUnit = freq.Id;
            _entity.LargeValveInspectionFrequencyUnit = null;

            _vmTester.MapToEntity();

            Assert.AreSame(freq, _entity.LargeValveInspectionFrequencyUnit);
        }

        #endregion

        [TestMethod]
        public void TestMappingOperatingCenterCode()
        {
            _vmTester.CanMapToViewModel(x => x.OperatingCenterCode, "QQ1");
            _entity.OperatingCenterCode = "NJ4";
            _vmTester.DoesNotMapToEntity(x => x.OperatingCenterCode, "ZZTOP");
        }

        [TestMethod]
        public void TestSewerOpeningInspectionFrequencyUnitMapsBothWays()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.SewerOpeningInspectionFrequencyUnit = freq;
            _viewModel.SewerOpeningInspectionFrequencyUnit = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(freq.Id, _viewModel.SewerOpeningInspectionFrequencyUnit);

            _entity.SewerOpeningInspectionFrequencyUnit = null;

            _vmTester.MapToEntity();

            Assert.AreSame(freq, _entity.HydrantInspectionFrequencyUnit);
        }

        #region SmallValveInspectionFrequencyUnit

        [TestMethod]
        public void TestMapToModelSetsSmallValveInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _entity.SmallValveInspectionFrequencyUnit = freq;
            _viewModel.SmallValveInspectionFrequencyUnit = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(freq.Id, _viewModel.SmallValveInspectionFrequencyUnit);
        }

        [TestMethod]
        public void TestMapToEntitySetsSmallValveInspectionFrequencyUnit()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            _viewModel.SmallValveInspectionFrequencyUnit = freq.Id;
            _entity.SmallValveInspectionFrequencyUnit = null;

            _vmTester.MapToEntity();

            Assert.AreSame(freq, _entity.SmallValveInspectionFrequencyUnit);
        }

        #endregion

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.ArcMobileMapId,
                    OperatingCenter.MaxLengths.ARC_MOBILE_MAP_ID)
               .PropertyHasMaxStringLength(x => x.CompanyInfo, OperatingCenter.MaxLengths.COMPANY_INFO)
               .PropertyHasMaxStringLength(x => x.DataCollectionMapUrl,
                    OperatingCenter.MaxLengths.DATA_COLLECTION_MAP_URL)
               .PropertyHasMaxStringLength(x => x.FaxNumber, OperatingCenter.MaxLengths.FAX_NUMBER)
               .PropertyHasMaxStringLength(x => x.MailingAddressCityStateZip,
                    OperatingCenter.MaxLengths.MAILING_ADDRESS_CITY_STATE_ZIP)
               .PropertyHasMaxStringLength(x => x.MailingAddressName,
                    OperatingCenter.MaxLengths.MAILING_ADDRESS_NAME)
               .PropertyHasMaxStringLength(x => x.MailingAddressStreet,
                    OperatingCenter.MaxLengths.MAILING_ADDRESS_STREET)
               .PropertyHasMaxStringLength(x => x.MapId, OperatingCenter.MaxLengths.MAP_ID)
               .PropertyHasMaxStringLength(x => x.OperatingCenterName,
                    OperatingCenter.MaxLengths.OPERATING_CENTER_NAME)
               .PropertyHasMaxStringLength(x => x.PermitsCapitalUserName,
                    OperatingCenter.MaxLengths.PERMITS_CAPITAL_USER_NAME)
               .PropertyHasMaxStringLength(x => x.PermitsOMUserName,
                    OperatingCenter.MaxLengths.PERMITS_OM_USER_NAME)
               .PropertyHasMaxStringLength(x => x.PhoneNumber, OperatingCenter.MaxLengths.PHONE_NUMBER)
               .PropertyHasRequiredRange(x => x.RSADivisionNumber, 0, 9999,
                    "The field RSA/Division # must be between 0 and 9999.")
               .PropertyHasMaxStringLength(x => x.ServiceContactPhoneNumber,
                    OperatingCenter.MaxLengths.SERVICE_CONTACT_PHONE_NUMBER);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.HydrantInspectionFrequency);
            ValidationAssert.PropertyIsRequired(x => x.HydrantInspectionFrequencyUnit);
            ValidationAssert.PropertyIsRequired(x => x.IsContractedOperations);
            ValidationAssert.PropertyIsRequired(x => x.LargeValveInspectionFrequency);
            ValidationAssert.PropertyIsRequired(x => x.LargeValveInspectionFrequencyUnit);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenterName);
            ValidationAssert.PropertyIsRequired(x => x.SewerOpeningInspectionFrequency);
            ValidationAssert.PropertyIsRequired(x => x.SewerOpeningInspectionFrequencyUnit);
            ValidationAssert.PropertyIsRequired(x => x.SmallValveInspectionFrequency);
            ValidationAssert.PropertyIsRequired(x => x.SmallValveInspectionFrequencyUnit);
            ValidationAssert.PropertyIsRequired(x => x.SAPEnabled);
            ValidationAssert.PropertyIsRequired(x => x.SAPWorkOrdersEnabled);
            ValidationAssert.PropertyIsRequired(x => x.State);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<OperatingCenter>(x => x.OperatedByOperatingCenter)
               .EntityMustExist<RecurringFrequencyUnit>(x => x.HydrantInspectionFrequencyUnit)
               .EntityMustExist<RecurringFrequencyUnit>(x => x.HydrantPaintingFrequencyUnit)
               .EntityMustExist<RecurringFrequencyUnit>(x => x.SewerOpeningInspectionFrequencyUnit)
               .EntityMustExist<RecurringFrequencyUnit>(x => x.LargeValveInspectionFrequencyUnit)
               .EntityMustExist<RecurringFrequencyUnit>(x => x.SmallValveInspectionFrequencyUnit)
               .EntityMustExist<State>(x => x.State)
               .EntityMustExist<StateRegion>(x => x.StateRegion)
               .EntityMustExist<TimeZone>(x => x.TimeZone);
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsOperatingCenterCode()
        {
            _viewModel.OperatingCenterCode = "OPC413";
            Assert.AreEqual("OPC413", _viewModel.ToString());
        }
        
        #endregion
    }
}
