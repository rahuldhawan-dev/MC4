using System;
using MMSINC.DataPages;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DataPagePathTest
    /// </summary>
    [TestClass]
    public class DataPagePathTest
    {
        #region Constants

        private const string REQUEST_BASE_URL =
            "http://www.website.com/page.aspx";

        #endregion

        #region Fields

        private Mock<IDataPageBase> _mockDataPage;
        private Mock<IRequest> _mockRequest;
        private DataPagePath _target;

        #endregion

        [TestInitialize]
        public void DataPagePathTestInitialize()
        {
            _mockDataPage = new Mock<IDataPageBase>();
            _mockRequest = new Mock<IRequest>();

            _mockDataPage
               .SetupGet(x => x.IRequest)
               .Returns(_mockRequest.Object);

            _mockRequest
               .SetupGet(x => x.Url).Returns(REQUEST_BASE_URL);

            _target = new DataPagePath(_mockDataPage.Object);
        }

        #region Test methods

        [TestMethod]
        public void TestConstructorThrowsForNullOwnerParameter()
        {
            MyAssert.Throws<ArgumentNullException>(
                () => new DataPagePath(null));
        }

        [TestMethod]
        public void TestOwnerPropertyReturnsOwnerPassedInConstructor()
        {
            Assert.AreSame(_target.Owner, _mockDataPage.Object);
        }

        [TestMethod]
        public void TestGetCreateNewRecordUrlAddsCreateEqualsToBaseUrl()
        {
            var expectedUrl = REQUEST_BASE_URL + "?create=";

            Assert.AreEqual(expectedUrl, _target.GetCreateNewRecordUrl());
        }

        [TestMethod]
        public void TestGetExportToExcelUrlReturnsProperUrl()
        {
            var expectedGuid = Guid.NewGuid();
            var expectedUrl = REQUEST_BASE_URL + "?search=" + expectedGuid.ToString() + "&export=";

            _mockDataPage
               .SetupGet(x => x.CachedFilterKey)
               .Returns(expectedGuid);

            Assert.AreEqual(expectedUrl, _target.GetExportToExcelUrl());

            _mockDataPage.Verify();
        }

        [TestMethod]
        public void TestGetSearchResultsUrlReturnsProperUrl()
        {
            var expectedGuid = Guid.NewGuid();
            var expectedUrl = REQUEST_BASE_URL + "?search=" + expectedGuid.ToString();

            _mockDataPage
               .SetupGet(x => x.CachedFilterKey)
               .Returns(expectedGuid);

            Assert.AreEqual(expectedUrl, _target.GetSearchResultsUrl());
        }

        #endregion
    }
}
