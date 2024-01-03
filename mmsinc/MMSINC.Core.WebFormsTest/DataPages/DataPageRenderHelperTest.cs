using System;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DataPageRenderHelperTest
    /// </summary>
    [TestClass]
    public class DataPageRenderHelperTest
    {
        private Mock<IDataPageBase> _mockDataPage;
        private Mock<IDataPagePath> _mockPath;
        private Mock<IRoleBasedDataPagePermissions> _mockPermissions;
        private Mock<IPermission> _mockCreatePermission;
        private DataPageRenderHelper _target;

        [TestInitialize]
        public void DataPageRenderHelperTestInitialize()
        {
            _mockDataPage = new Mock<IDataPageBase>();
            _mockPath = new Mock<IDataPagePath>();
            _mockPermissions = new Mock<IRoleBasedDataPagePermissions>();
            _mockCreatePermission = new Mock<IPermission>();

            _mockPermissions.SetupGet(x => x.CreateAccess).Returns(
                _mockCreatePermission.Object);

            _mockDataPage
               .Setup(x => x.Permissions)
               .Returns(_mockPermissions.Object);

            _mockDataPage
               .Setup(x => x.PathHelper)
               .Returns(_mockPath.Object);

            _target = new DataPageRenderHelper(_mockDataPage.Object);
        }

        #region Test Methods

        [TestMethod]
        public void TestConstructorThrowsForNullOwnerParameter()
        {
            MyAssert.Throws<ArgumentNullException>(
                () => new DataPageRenderHelper(null));
        }

        [TestMethod]
        public void TestOwnerPropertyReturnsOwnerPassedInConstructor()
        {
            Assert.AreSame(_target.Owner, _mockDataPage.Object);
        }

        [TestMethod]
        public void TestRenderLinkButtonReturnsFormattedString()
        {
            const string format = DataPageRenderHelper.LINKBUTTON_FORMAT;
            const string expectedUrl = "http://www.thegoogle.com/";
            const string expectedText = "I am a banana!";

            var expectedResult = String.Format(format, expectedUrl, expectedText);

            var actualResult = _target.RenderLinkButton(expectedUrl,
                expectedText);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestRenderBackToResultsButtonReturnsEmptyStringWhenOwnerDoesNotHaveValidCachedFilterKey()
        {
            Assert.AreEqual(string.Empty, _target.RenderBackToResultsButton());
        }

        [TestMethod]
        public void TestRenderBackToResultsButtonUsesPathHelperGetSearchResultsUrl()
        {
            _mockDataPage
               .SetupGet(x => x.CachedFilterKey)
               .Returns(Guid.NewGuid());

            _mockPath.Setup(x => x.GetSearchResultsUrl());

            var result = _target.RenderBackToResultsButton();

            _mockDataPage.Verify();
            _mockPath.Verify();
        }

        [TestMethod]
        public void TestRenderBackToSearchButtonUsesOwnerGetBaseUrlForUrl()
        {
            _mockPath.Setup(x => x.GetBaseUrl());
            var result = _target.RenderBackToSearchButton();
            _mockPath.Verify();
        }

        [TestMethod]
        public void TestRenderCreateNewRecordButtonReturnsEmptyStringIfPermissionsCanAddRecordsIsFalse()
        {
            Assert.AreEqual(string.Empty, _target.RenderCreateNewRecordButton("some text"));
        }

        [TestMethod]
        public void TestRenderCreateNewRecordButtonUsesPathHelperGetCreateNewRecordUrlForUrl()
        {
            _mockCreatePermission.Setup(x => x.IsAllowed).Returns(true);
            _mockPath.Setup(x => x.GetCreateNewRecordUrl());

            var result = _target.RenderCreateNewRecordButton("text");

            _mockPermissions.Verify();
            _mockCreatePermission.Verify();
            _mockPath.Verify();
        }

        [TestMethod]
        public void TestRenderExportToExcelButtonUsesPathHelperGetExportToExcelUrlForUrl()
        {
            _mockPath.Setup(x => x.GetExportToExcelUrl());
            var result = _target.RenderExportToExcelButton();
            _mockPath.Verify();
        }

        #endregion
    }
}
