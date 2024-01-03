using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Library;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Library
{
    /// <summary>
    /// Summary description for AddressVerificationTest
    /// </summary>
    [TestClass]
    public class AddressVerificationTest
    {
        #region Constants

        private const string VALID_STREET_NUMBER = "123";
        private const string VALID_STREET_NAME = "Maple";
        private const string VALID_TOWN_NAME = "AnyTown";
        private const int VALID_TOWN_ID = 15;
        private const int INVALID_TOWN_ID = 0;
        private const string INVALID_TOWN_NAME = "OtherTown";
        private const string VALID_TOWN_SECTION_NAME = "RightSideOfTheTracks";

        #endregion

        #region Private Members

        private WithAddress _validAddress;

        #endregion

        #region Private Static Members

        private static Street _validStreet, _invalidStreet;
        private static Town _validTown;
        private static TownSection _validTownSection, _invalidTownSection;

        #endregion

        #region Properties

        internal WithAddress ValidAddress
        {
            get
            {
                if (_validAddress == null)
                    _validAddress = new WithAddress {
                        StreetNumber = VALID_STREET_NUMBER,
                        Street = ValidStreet,
                        NearestCrossStreet = ValidStreet,
                        Town = ValidTown,
                        TownSection = ValidTownSection
                    };
                return _validAddress;
            }
        }

        #endregion

        #region Static Properties

        public static Street ValidStreet
        {
            get
            {
                if (_validStreet == null)
                    _validStreet = new Street {
                        StreetName = VALID_STREET_NAME,
                        TownID = ValidTown.TownID
                    };
                return _validStreet;
            }
        }

        public static Street InvalidStreet
        {
            get
            {
                if (_invalidStreet == null)
                {
                    _invalidStreet = ValidStreet;
                    _invalidStreet.TownID = INVALID_TOWN_ID;
                }
                return _invalidStreet;
            }
        }

        public static Town ValidTown
        {
            get
            {
                if (_validTown == null)
                    _validTown = new Town {
                        Name = VALID_TOWN_NAME, 
                        TownID = VALID_TOWN_ID
                    };
                return _validTown;
            }
        }

        public static TownSection ValidTownSection
        {
            get
            {
                if (_validTownSection == null)
                    _validTownSection = new TownSection {
                        TownID = VALID_TOWN_ID,
                        Name = VALID_TOWN_SECTION_NAME
                    };
                return _validTownSection;
            }
        }

        public static TownSection InvalidTownSection
        {
            get
            {
                if (_invalidTownSection == null)
                    _invalidTownSection = new TownSection {
                        TownID = INVALID_TOWN_ID,
                        Name = VALID_TOWN_SECTION_NAME
                    };
                return _invalidTownSection;
            }
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AddressVerificationTestInitialize()
        {
            _validTownSection = null;
            _validTown = null;
            _validStreet = null;
            _validAddress = null;
        }

        #endregion

        [TestMethod]
        public void TestValidAddress()
        {
            Assert.IsTrue(AddressVerifier.Test(ValidAddress),
                "False condition encountered testing address which should have been valid.");
            MyAssert.DoesNotThrow(() => AddressVerifier.Verify(ValidAddress),
                "Error testing address which should have been valid.");
        }

        [TestMethod]
        public void TestWithNullTownConsideredValid()
        {
            var target = ValidAddress;
            target.Town = null;
            target.TownSection = InvalidTownSection;
            target.Street = InvalidStreet;
            target.NearestCrossStreet = InvalidStreet;

            Assert.IsTrue(AddressVerifier.Test(target),
                "An address with no Town set cannot be considered invalid.");
            MyAssert.DoesNotThrow(() => AddressVerifier.Verify(target),
                "Error testing address which should have been valid.");
        }

        [TestMethod]
        public void TestWithBothStreetsAndTownSectionNullConsideredValid()
        {
            var target = ValidAddress;
            target.Street = null;
            target.NearestCrossStreet = null;
            target.TownSection = null;
            Assert.IsTrue(AddressVerifier.Test(target),
                "An address with no Streets set cannot be considered invalid.");
            MyAssert.DoesNotThrow(() => AddressVerifier.Verify(target),
                "Error testing address which should have been valid.");
        }

        [TestMethod]
        public void TestWithInvalidStreet()
        {
            var target = ValidAddress;
            target.Street = InvalidStreet;
            target.NearestCrossStreet = null;

            Assert.IsFalse(AddressVerifier.Test(target),
                "An address where the Street listed does not exist in the town listed should be considered invalid.");
            MyAssert.Throws(() => AddressVerifier.Verify(target),
                typeof(DomainLogicException),
                "Verifying an address where the Street listed does not exist in the town listed should throw an exception.");
        }

        [TestMethod]
        public void TestWithInvalidNearestCrossStreet()
        {
            var target = ValidAddress;
            target.NearestCrossStreet = InvalidStreet;
            target.Street = null;

            Assert.IsFalse(AddressVerifier.Test(target),
                "An address where the Street listed does not exist in the town listed should be considered invalid.");
            MyAssert.Throws(() => AddressVerifier.Verify(target),
                typeof(DomainLogicException),
                "Verifying an address where the Street listed does not exist in the town listed should throw an exception.");
        }

        [TestMethod]
        public void TestWithInvalidTownSection()
        {
            var target = ValidAddress;
            target.TownSection = InvalidTownSection;

            Assert.IsFalse(AddressVerifier.Test(target),
                "An address where the Town Section listed is not part of the Town listed should be considered invalid.");

            MyAssert.Throws(() => AddressVerifier.Verify(target),
                typeof(DomainLogicException),
                "Verifying an address where the Town Section listed is not part of the Town listed should throw an exception.");
        }
    }

    internal class WithAddress : IVerifiesAddress
    {
        #region IVerifiesAddress Members

        public string StreetNumber { get; set; }

        public Street Street { get; set; }

        public Street NearestCrossStreet { get; set; }

        public Town Town { get; set; }

        public TownSection TownSection { get; set; }

        #endregion
    }
}
