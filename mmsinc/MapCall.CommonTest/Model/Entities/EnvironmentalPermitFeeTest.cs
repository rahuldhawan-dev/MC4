using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EnvironmentalPermitFeeTest
    {
        #region Tests

        [TestMethod]
        public void TestPaymentFrequencyReturnsExpectedFormat()
        {
            var target = new EnvironmentalPermitFee();
            target.PaymentDueInterval = 3;
            target.PaymentDueFrequencyUnit = new RecurringFrequencyUnit {Description = "Time Thing"};

            Assert.AreEqual("3 Time Thing", target.PaymentFrequency);
        }

        [TestMethod]
        public void TestPaymentFrequencyReturnsNullIfPaymentDueIntervalIsNull()
        {
            var target = new EnvironmentalPermitFee();
            target.PaymentDueInterval = null;
            target.PaymentDueFrequencyUnit = new RecurringFrequencyUnit {Description = "Time Thing"};

            Assert.IsNull(target.PaymentFrequency);
        }

        [TestMethod]
        public void TestPaymentFrequencyReturnsNullIfPaymentDueFrequencyUnitIsNull()
        {
            var target = new EnvironmentalPermitFee();
            target.PaymentDueInterval = 3;
            target.PaymentDueFrequencyUnit = null;

            Assert.IsNull(target.PaymentFrequency);
        }

        #endregion
    }
}
