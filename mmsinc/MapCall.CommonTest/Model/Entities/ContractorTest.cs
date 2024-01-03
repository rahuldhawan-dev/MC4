using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ContractorTest
    {
        [TestMethod]
        public void TestAddressLineOneReturnsLineOne()
        {
            var houseNum = "1428";
            var apartmentNum = "A";
            var street = new Street {FullStName = "Elm St."};
            var contractor = new Contractor {ApartmentNumber = apartmentNum, HouseNumber = houseNum, Street = street};

            Assert.AreEqual(string.Format(Contractor.FormatStrings.ADDRESS_LINE_ONE, houseNum, apartmentNum, street),
                contractor.AddressLineOne);

            contractor = new Contractor();
            Assert.AreEqual(string.Format(Contractor.FormatStrings.ADDRESS_LINE_ONE, "", "", ""),
                contractor.AddressLineOne);
        }

        [TestMethod]
        public void TestCityStateZipReturnsCityStateZip()
        {
            var town = new Town {ShortName = "Short Name"};
            var state = new State {Abbreviation = "NJ", Name = "New Jersey"};
            var zip = "12345";
            var contractor = new Contractor();

            Assert.AreEqual(String.Format(Contractor.FormatStrings.CITY_STATE_ZIP, "", "", ""),
                contractor.CityStateZip);

            contractor.Town = town;
            contractor.Zip = zip;
            contractor.State = state;

            Assert.AreEqual(String.Format(Contractor.FormatStrings.CITY_STATE_ZIP, town, state, zip),
                contractor.CityStateZip);
        }
    }
}
