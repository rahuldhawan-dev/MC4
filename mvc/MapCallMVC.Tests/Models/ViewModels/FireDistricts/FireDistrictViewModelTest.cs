using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels.FireDistricts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.FireDistricts
{
    [TestClass]
    public class FireDistrictViewModelTest
        : ViewModelTestBase<FireDistrict, FireDistrictViewModel>
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.Abbreviation)
               .CanMapBothWays(x => x.Address)
               .CanMapBothWays(x => x.AddressCity)
               .CanMapBothWays(x => x.AddressZip)
               .CanMapBothWays(x => x.Contact)
               .CanMapBothWays(x => x.DistrictName)
               .CanMapBothWays(x => x.Fax)
               .CanMapBothWays(x => x.Phone)
               .CanMapBothWays(x => x.PremiseNumber)
               .CanMapBothWays(x => x.State)
               .CanMapBothWays(x => x.UtilityDistrict)
               .CanMapBothWays(x => x.UtilityName);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.DistrictName)
               .PropertyIsRequired(x => x.State);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<State>(x => x.State);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.Abbreviation, FireDistrict.StringLengths.ABBREVIATION)
               .PropertyHasMaxStringLength(x => x.Address, FireDistrict.StringLengths.ADDRESS)
               .PropertyHasMaxStringLength(x => x.AddressCity, FireDistrict.StringLengths.ADDRESS_CITY)
               .PropertyHasMaxStringLength(x => x.AddressZip, FireDistrict.StringLengths.ADDRESS_ZIP)
               .PropertyHasMaxStringLength(x => x.Contact, FireDistrict.StringLengths.CONTACT)
               .PropertyHasMaxStringLength(x => x.DistrictName, FireDistrict.StringLengths.DISTRICT_NAME)
               .PropertyHasMaxStringLength(x => x.Fax, FireDistrict.StringLengths.FAX)
               .PropertyHasMaxStringLength(x => x.Phone, FireDistrict.StringLengths.PHONE)
               .PropertyHasMaxStringLength(x => x.PremiseNumber, FireDistrict.StringLengths.PREMISE_NUMBER)
               .PropertyHasMaxStringLength(x => x.UtilityName, FireDistrict.StringLengths.UTILITY_NAME);
        }
    }
}
