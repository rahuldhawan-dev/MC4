using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MapCall.Common.Model.Entities;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class OperatorLicenseTest
    {
        #region Fields

        private OperatorLicense _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new OperatorLicense();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestIsValidAndNotExpiredForDateReturnsTrueWhenDateFallsIntoDateRangeOfValidationDateAndExpirationDate()
        {
            _target.ValidationDate = new DateTime(2023, 1, 1);
            _target.ExpirationDate = new DateTime(2023, 2, 28);
            
            Assert.IsTrue(_target.IsValidAndNotExpiredForDate(new DateTime(2023, 1, 12)), "Must be valid in between both dates.");
            Assert.IsTrue(_target.IsValidAndNotExpiredForDate(new DateTime(2023, 1, 1)), "Must be valid at the beginning of validation date.");
            Assert.IsTrue(_target.IsValidAndNotExpiredForDate(new DateTime(2023, 3, 1).AddSeconds(-1)), "Must be valid through to the end of the expiration date.");
            
            Assert.IsFalse(_target.IsValidAndNotExpiredForDate(new DateTime(2023, 1, 1).AddSeconds(-1)), "Must not be valid prior to the beginning of validation date.");
            Assert.IsFalse(_target.IsValidAndNotExpiredForDate(new DateTime(2023, 3, 1)), "Must not be valid after the end of the expiration date.");
        }

        #endregion
    }
}
