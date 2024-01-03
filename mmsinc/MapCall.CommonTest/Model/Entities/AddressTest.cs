using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class AddressTest
    {
        #region Fields

        private Address _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new Address {
                Town = new Town {
                    ShortName = "Some Town",
                    County = new County {
                        State = new State {
                            Abbreviation = "NJ"
                        }
                    }
                }
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestStatePropertyReturnsTownState()
        {
            Assert.IsNotNull(_target.State);
            Assert.AreSame(_target.Town.County.State, _target.State);
            Assert.AreSame(_target.County.State, _target.State);
        }

        [TestMethod]
        public void TestStatePropertyReturnsNullIfTownIsNull()
        {
            _target.Town = null;
            Assert.IsNull(_target.State);
        }

        [TestMethod]
        public void TestCountyPropertyReturnsTownsCounty()
        {
            Assert.IsNotNull(_target.County);
            Assert.AreSame(_target.Town.County, _target.County);
        }

        [TestMethod]
        public void TestCountyPropertyReturnsNullIfTownIsNull()
        {
            _target.Town = null;
            Assert.IsNull(_target.County);
        }

        [TestMethod]
        public void TestToStringReturnsTypicalFormattedAddress()
        {
            var expected = "123 Fake St.\r\nSome Town, NJ 10001";
            _target.Address1 = "123 Fake St.";
            _target.ZipCode = "10001";
            Assert.AreEqual(expected, _target.ToString());

            expected = "123 Fake St.\r\nPO Box 4\r\nSome Town, NJ 10001";
            _target.Address2 = "PO Box 4";
            Assert.AreEqual(expected, _target.ToString());
        }

        [TestMethod]
        public void TestToStringUsesTownZipIfZipCodeIsNullOrEmpty()
        {
            var expected = "123 Fake St.\r\nSome Town, NJ 10002";
            _target.Address1 = "123 Fake St.";
            _target.ZipCode = null;
            _target.Town.Zip = "10002";
            Assert.AreEqual(expected, _target.ToString());

            _target.ZipCode = string.Empty;
            Assert.AreEqual(expected, _target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsStringEmptyIfEverythingIsNullorEmpty()
        {
            _target = new Address();
            Assert.AreEqual(string.Empty, _target.ToString());
        }

        #endregion
    }
}
