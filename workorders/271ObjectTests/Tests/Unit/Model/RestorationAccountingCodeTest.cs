using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationAccountingCodeTest.
    /// </summary>
    [TestClass]
    public class RestorationAccountingCodeTest
    {
        #region Private Members

        private TestRestorationAccountingCode _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationAccountingCodeTestInitialize()
        {
            _target = new TestRestorationAccountingCodeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsCodeValue()
        {
            string code = "123456", subCode = "78";
            _target.Code = code;
            _target.SubCode = subCode;

            Assert.AreEqual(String.Format("{0}.{1}", code, subCode),
                _target.ToString());

            _target.SubCode = null;

            Assert.AreEqual(code, _target.ToString());
        }

        [TestMethod]
        public void TestCanDeleteReturnsFalseWhenPrimaryWorkDescriptionsExist()
        {
            var rpc = new RestorationAccountingCode();
            rpc.PrimaryWorkDescriptions.Add(new WorkDescription());

            Assert.IsFalse(rpc.CanDelete);
        }

        [TestMethod]
        public void TestCanDeleteReturnsFalseWhenSecondaryWorkDescriptionsExist()
        {
            var rpc = new RestorationAccountingCode();
            rpc.SecondaryWorkDescriptions.Add(new WorkDescription());

            Assert.IsFalse(rpc.CanDelete);
        }

        [TestMethod]
        public void TestCanDeleteReturnsFalseWhenPrimaryAndSecondaryWorkDescriptionsExist()
        {
            var rpc = new RestorationAccountingCode();
            rpc.PrimaryWorkDescriptions.Add(new WorkDescription());
            rpc.SecondaryWorkDescriptions.Add(new WorkDescription());

            Assert.IsFalse(rpc.CanDelete);
        }

        [TestMethod]
        public void TestCanDeleteReturnsTrueWhenNoWorkDescriptionsExist()
        {
            var rpc = new RestorationAccountingCode();

            Assert.IsTrue(rpc.CanDelete);
        }

        [TestMethod]
        public void TestDeletingErrorMessageIsEmptyWhenNoWorkDescriptionsExist()
        {
            var rpc = new RestorationAccountingCode();

            Assert.AreEqual(string.Empty, rpc.DeletingErrorMessage);
        }

        [TestMethod]
        public void TestDeletingErrorMessageReturnsTheCorrectErrorMessage()
        {
            var rpc = new RestorationAccountingCode();
            var wdprimary = new WorkDescription() { WorkDescriptionID = 1};
            var wdsecondary = new WorkDescription() {WorkDescriptionID = 2};
            rpc.PrimaryWorkDescriptions.Add(wdprimary);
            rpc.SecondaryWorkDescriptions.Add(wdsecondary);
            var expected = String.Format("{0}{1} {2} ",
                RestorationAccountingCode.DELETING_ERROR_MESSAGE, wdprimary.WorkDescriptionID,
                wdsecondary.WorkDescriptionID);

            Assert.AreEqual(expected, rpc.DeletingErrorMessage);
        }
    }

    internal class TestRestorationAccountingCodeBuilder : TestDataBuilder<TestRestorationAccountingCode>
    {
        #region Exposed Methods

        public override TestRestorationAccountingCode Build()
        {
            var obj = new TestRestorationAccountingCode();
            return obj;
        }

        #endregion
    }

    internal class TestRestorationAccountingCode : RestorationAccountingCode
    {
    }
}
