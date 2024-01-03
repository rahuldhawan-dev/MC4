using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationProductCodeTest.
    /// </summary>
    [TestClass]
    public class RestorationProductCodeTest
    {
        #region Private Members

        private TestRestorationProductCode _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationProductCodeTestInitialize()
        {
            _target = new TestRestorationProductCodeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsCode()
        {
            _target = new TestRestorationProductCode {
                Code = "ASDF"
            };

            Assert.AreEqual(_target.Code, _target.ToString());
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
            var wdprimary = new WorkDescription() { WorkDescriptionID = 1 };
            var wdsecondary = new WorkDescription() { WorkDescriptionID = 2 };
            rpc.PrimaryWorkDescriptions.Add(wdprimary);
            rpc.SecondaryWorkDescriptions.Add(wdsecondary);
            var expected = String.Format("{0}{1} {2} ",
                RestorationAccountingCode.DELETING_ERROR_MESSAGE, wdprimary.WorkDescriptionID,
                wdsecondary.WorkDescriptionID);

            Assert.AreEqual(expected, rpc.DeletingErrorMessage);
        }

    }

    internal class TestRestorationProductCodeBuilder : TestDataBuilder<TestRestorationProductCode>
    {
        #region Exposed Methods

        public override TestRestorationProductCode Build()
        {
            var obj = new TestRestorationProductCode();
            return obj;
        }

        #endregion
    }

    internal class TestRestorationProductCode : RestorationProductCode
    {
    }
}
