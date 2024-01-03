using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Testing.MSTest.TestExtensions;

namespace SAP.DataTest.Model.Entities
{
    [TestClass]
    public class SAPNotificationCollectionTest
    {
        #region Fields

        private SAPNotificationCollection _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SAPNotificationCollection();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSetsItemsToNewEmptyListInstance()
        {
            var target1 = new SAPNotificationCollection();
            var target2 = new SAPNotificationCollection();

            Assert.IsNotNull(target1.Items);
            Assert.IsNotNull(target2.Items);
            Assert.AreEqual(0, target1.Items.Count);
            Assert.AreEqual(0, target2.Items.Count);
            Assert.AreNotSame(target1.Items, target2.Items);
        }

        [TestMethod]
        public void TestSAPErrorCodeReturnsTheSAPErrorCodeOFTheFirstNotificationInTheItemsList()
        {
            var expectedMessage = "This is an error code";
            var item1 = new SAPNotification {SAPErrorCode = expectedMessage};
            var item2 = new SAPNotification {SAPErrorCode = "Something else"};
            _target.Items.Add(item1);
            _target.Items.Add(item2);

            Assert.AreEqual(expectedMessage, _target.SAPErrorCode);
        }

        [TestMethod]
        public void TestResultReturnsSuccessWhenFirstSAPNotificationRecordSAPErrorCodeStartsWithTheWordSuccess()
        {
            var expectedMessage = "Success!";
            var item1 = new SAPNotification {SAPErrorCode = expectedMessage};
            var item2 = new SAPNotification
                {SAPErrorCode = "This item should not have been included when checking for SAPErrorCode"};
            _target.Items.Add(item1);
            _target.Items.Add(item2);

            Assert.AreEqual(SAPNotificationCollectionResult.Success, _target.Result);
        }

        [TestMethod]
        public void TestResultReturnsNoResultsWhenFirstSAPNotificationRecordSAPErrorCodeStatesNoResults()
        {
            var expectedMessage = "No Records found in SAP for given selection";
            var item1 = new SAPNotification {SAPErrorCode = expectedMessage};
            var item2 = new SAPNotification
                {SAPErrorCode = "This item should not have been included when checking for SAPErrorCode"};
            _target.Items.Add(item1);
            _target.Items.Add(item2);

            Assert.AreEqual(SAPNotificationCollectionResult.NoResults, _target.Result);
        }

        [TestMethod]
        public void TestResultReturnsErrorWhenFirstSAPNotificationRecordSAPErrorCodeIsUnknown()
        {
            var expectedMessage = "Some messsage we don't know how to parse";
            var item1 = new SAPNotification {SAPErrorCode = expectedMessage};
            var item2 = new SAPNotification
                {SAPErrorCode = "This item should not have been included when checking for SAPErrorCode"};
            _target.Items.Add(item1);
            _target.Items.Add(item2);

            Assert.AreEqual(SAPNotificationCollectionResult.Error, _target.Result);
        }

        [TestMethod]
        public void TestSAPErrorCodeThrowsIfItemsIsEmpty()
        {
            // This was coded under the assumption that Items would never be empty.
            _target.Items.Clear();
            MyAssert.Throws<InvalidOperationException>(() => _target.SAPErrorCode);
        }

        [TestMethod]
        public void TestResultThrowsExceptionIfSAPErrorCodeIsNullSomehow()
        {
            // This was coded under the assumption that Items would never be empty.
            _target.Items.Clear();
            MyAssert.Throws<InvalidOperationException>(() => _target.Result);
        }

        #endregion
    }
}
