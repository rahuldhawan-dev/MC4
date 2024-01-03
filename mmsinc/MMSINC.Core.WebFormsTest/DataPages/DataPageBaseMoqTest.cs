using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Auditing;
using MMSINC.Utilities.Permissions;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    // TODO: Move the rest of DataPageBaseTest here. 
    [TestClass]
    public class DataPageBaseMoqTest : EventFiringTestClass
    {
        #region fields

        private Mock<IPanel> _searchPanel, _detailPanel, _resultsPanel;
        private Mock<IControl> _homePanel;
        private Mock<IButton> _searchButton;
        private Mock<IUser> _user;
        private Mock<IGridView> _mockedResultsGridView;

        private Mock<IGridViewRowCollection> _rows;
        private Mock<IResponse> _mockedResponse;
        private Mock<IRequest> _mockedRequest;
        private List<IDataLink> _dataLinks;
        private List<Mock<IDataLink>> _dataLinkMocks;
        private Mock<IAuditor> _auditor;
        private Mock<IFilterBuilder> _filterBuilder;
        private Mock<IDataPageRouteHelper> _routeHelper;
        private Mock<ICache> _iCache;
        private Mock<IFilterCache> _mockediFilterCache;
        private SqlDataSource _mockedResultsDataSource;
        private DataControlFieldCollection _mockGridViewColumns;

        // This could probably all get moved to a MockPermissions class
        // cause I think it's gonna end up being used a lot.
        private Mock<IRoleBasedDataPagePermissions> _mockPermissions;
        private Mock<IPermission> _mockPageAccess;
        private Mock<IPermission> _mockCreateAccess;
        private Mock<IPermission> _mockEditAccess;
        private Mock<IPermission> _mockDeleteAccess;
        private Mock<IModulePermissions> _mockModPerms;

        #endregion

        #region Initialization

        private TestDataPageBaseBuilder InitializeBuilder()
        {
            _user = new Mock<IUser>();
            _searchPanel = new Mock<IPanel>();
            _detailPanel = new Mock<IPanel>();
            _resultsPanel = new Mock<IPanel>();
            _mockGridViewColumns = new DataControlFieldCollection();
            _mockedResultsGridView = new Mock<IGridView>();
            _searchButton = new Mock<IButton>();
            _mockedResponse = new Mock<IResponse>();
            _mockedRequest = new Mock<IRequest>();
            _auditor = new Mock<IAuditor>();
            _filterBuilder = new Mock<IFilterBuilder>();
            _iCache = new Mock<ICache>();
            _rows = new Mock<IGridViewRowCollection>();
            _mockedResultsDataSource = new SqlDataSource();
            _mockediFilterCache = new Mock<IFilterCache>();
            _homePanel = new Mock<IControl>();
            _routeHelper = new Mock<IDataPageRouteHelper>();
            _mockPermissions = new Mock<IRoleBasedDataPagePermissions>();
            _mockPageAccess = new Mock<IPermission>();
            _mockCreateAccess = new Mock<IPermission>();
            _mockDeleteAccess = new Mock<IPermission>();
            _mockEditAccess = new Mock<IPermission>();

            _mockPermissions.SetupGet(x => x.PageAccess)
                            .Returns(_mockPageAccess.Object);
            _mockPermissions.SetupGet(x => x.CreateAccess)
                            .Returns(_mockCreateAccess.Object);
            _mockPermissions.SetupGet(x => x.DeleteAccess)
                            .Returns(_mockDeleteAccess.Object);
            _mockPermissions.SetupGet(x => x.EditAccess)
                            .Returns(_mockEditAccess.Object);

            _mockedRequest.SetupGet(x => x.Url).Returns("/somepage.aspx");

            _mockModPerms = new Mock<IModulePermissions>();
            _dataLinkMocks = new List<Mock<IDataLink>>();

            var mockedDataLink = new Mock<IDataLink>();
            _dataLinkMocks.Add(mockedDataLink);

            _dataLinks = (from m in _dataLinkMocks select m.Object).ToList();

            return new TestDataPageBaseBuilder()
                  .WithIUser(_user.Object)
                  .WithPermissions(_mockPermissions.Object)
                  .WithRouteHelper(_routeHelper.Object)
                  .WithResultsGridView(_mockedResultsGridView.Object)
                  .WithDetailPanel(_detailPanel.Object)
                  .WithResultsPanel(_resultsPanel.Object)
                  .WithSearchPanel(_searchPanel.Object)
                  .WithHomePanel(_homePanel.Object)
                  .WithSearchButton(_searchButton.Object)
                  .WithDataLinkControls(_dataLinks)
                  .WithAuditor(_auditor.Object)
                  .WithIFilterCache(_mockediFilterCache.Object)
                  .WithMockedRequest(_mockedRequest.Object)
                  .WithMockedResponse(_mockedResponse.Object)
                  .WithModulePermissions(_mockModPerms.Object);
        }

        private TestDataPageBaseBuilder InitializeBuilderForParseQueryString()
        {
            return InitializeBuilder()
                  .WithPermissions(_mockPermissions.Object)
                  .WithMockedOnPageModeChanged(true);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestVerifyRenderingInServerFormDoesNotThrow()
        {
            var target = InitializeBuilder().Build();
            var testControl = new GridView();
            MyAssert.DoesNotThrow(() => target.VerifyRenderingInServerForm(testControl));
        }

        [TestMethod]
        public void TestPermissionsReturnsNotNullObject()
        {
            var target = InitializeBuilder().Build();
            var perm = target.Permissions;

            Assert.IsNotNull(perm);
        }

        [TestMethod]
        public void TestLoadDataRecordSetsCurrentRecordId()
        {
            const int expected = 43;

            var target = InitializeBuilder().Build();
            foreach (var dl in _dataLinkMocks)
            {
                dl.SetupGet(x => x.DataLinkID).Returns(expected);
            }

            target.LoadDataRecordTest(expected);
            Assert.AreEqual(target.CurrentDataRecordId, expected);
        }

        [TestMethod]
        public void TestResultCountIsEqualToResultsGridViewIRowCount()
        {
            var target = InitializeBuilder().Build();
            const int expectedRowCount = 4;
            _mockedResultsGridView.SetupGet(x => x.IRows).Returns(_rows.Object);
            _rows.SetupGet(x => x.Count).Returns(expectedRowCount);

            Assert.AreSame(_mockedResultsGridView.Object.IRows, target.ResultsGridViewTest.IRows);
            Assert.AreEqual(expectedRowCount, target.ResultCount);
        }

        [TestMethod]
        public void TestRenderResultsGridViewToExcelDoesNotThrowException()
        {
            var renderableGridView = new MvpGridView();
            renderableGridView.DataSource = new SqlDataSource();

            var target = InitializeBuilder()
                        .WithResultsGridView(renderableGridView)
                        .Build();

            MyAssert.DoesNotThrow(() => target.RenderResultsGridViewToExcelTest());
        }

        [TestMethod]
        public void TestOnInitComplete()
        {
            var target = InitializeBuilder()
               .Build();

            var mockedQueryString = new Mock<IQueryString>();
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);
            _mockedRequest.SetupGet(x => x.IQueryString).Returns(mockedQueryString.Object);

            InvokeEventByName(target, "OnInitComplete", EventArgs.Empty);

            // WTF was I testing here?
        }

        #region PageModes property

        [TestMethod]
        public void TestSettingPageModeToSearchShowsSearchPanelAndClearsCurrentDataRecordId()
        {
            var target = InitializeBuilder().Build();

            const PageModes expectedPageMode = PageModes.Search;
            const int expectedCurrentDataRecordId = 0;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId, "CurrentDataRecordId must be 0.");
            Assert.IsFalse(target.DocNotesVisibleTest, "DocNotesVisible must be false.");

            _detailPanel.VerifySet(x => x.Visible = false);
            _searchPanel.VerifySet(x => x.Visible = true);
            _resultsPanel.VerifySet(x => x.Visible = false);
            _homePanel.VerifySet(x => x.Visible = false);
        }

        [TestMethod]
        public void TestSettingPageModeToResultsShowsResultsPanelAndClearsCurrentDataRecordId()
        {
            var builder = InitializeBuilder()
               .WithCache(_iCache.Object);

            var cachedFilterKey = Guid.NewGuid();
            _mockediFilterCache.Setup(x => x.GetFilterBuilder(cachedFilterKey)).Returns(_filterBuilder.Object);

            var target = builder.Build();
            target.UseMockedApplyFilterBuilder = true;
            target.CachedFilterKey = cachedFilterKey;

            const PageModes expectedPageMode = PageModes.Results;
            const int expectedCurrentDataRecordId = 0;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId, "CurrentDataRecordId must be 0.");
            Assert.IsFalse(target.DocNotesVisibleTest, "DocNotesVisible must be false.");

            _detailPanel.VerifySet(x => x.Visible = false);
            _searchPanel.VerifySet(x => x.Visible = false);
            _resultsPanel.VerifySet(x => x.Visible = true);
            _homePanel.VerifySet(x => x.Visible = false);
        }

        [TestMethod]
        public void TestSettingPageModeToRecordReadOnlyShowsDetailPanelAndHasValidCurrentDataRecordId()
        {
            var target = InitializeBuilder().Build();

            const PageModes expectedPageMode = PageModes.RecordReadOnly;
            const int expectedCurrentDataRecordId = 350;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId,
                "CurrentDataRecordId must be a value greater than 0.");
            Assert.IsTrue(target.DocNotesVisibleTest, "DocNotesVisible must be true.");

            _detailPanel.VerifySet(x => x.Visible = true);
            _searchPanel.VerifySet(x => x.Visible = false);
            _resultsPanel.VerifySet(x => x.Visible = false);
            _homePanel.VerifySet(x => x.Visible = false);
        }

        [TestMethod]
        public void TestSettingPageModeToRecordUpdateShowsDetailPanelAndHasValidCurrentDataRecordId()
        {
            var target = InitializeBuilder().Build();
            const PageModes expectedPageMode = PageModes.RecordUpdate;
            const int expectedCurrentDataRecordId = 350;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId,
                "CurrentDataRecordId must be a value greater than 0.");
            Assert.IsFalse(target.DocNotesVisibleTest, "DocNotesVisible must be false.");

            _detailPanel.VerifySet(x => x.Visible = true);
            _searchPanel.VerifySet(x => x.Visible = false);
            _resultsPanel.VerifySet(x => x.Visible = false);
            _homePanel.VerifySet(x => x.Visible = false);
        }

        [TestMethod]
        public void TestSettingPageModeToRecordInsertShowsDetailPanelAndClearsCurrentDataRecordId()
        {
            var target = InitializeBuilder().Build();

            const PageModes expectedPageMode = PageModes.RecordInsert;
            const int expectedCurrentDataRecordId = 0;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId, "CurrentDataRecordId must be 0.");
            Assert.IsFalse(target.DocNotesVisibleTest, "DocNotesVisible must be false.");

            _detailPanel.VerifySet(x => x.Visible = true);
            _searchPanel.VerifySet(x => x.Visible = false);
            _resultsPanel.VerifySet(x => x.Visible = false);
            _homePanel.VerifySet(x => x.Visible = false);
        }

        [TestMethod]
        public void TestSettingPageModeToHomeShowsHomePanel()
        {
            var target = InitializeBuilder().Build();

            const PageModes expectedPageMode = PageModes.Home;
            const int expectedCurrentDataRecordId = 0;

            target.CurrentDataRecordId = expectedCurrentDataRecordId;
            target.PageModeTest = expectedPageMode;

            Assert.AreEqual(target.PageModeTest, expectedPageMode, "PageModes aren't equal.");
            Assert.AreEqual(target.CurrentDataRecordId, expectedCurrentDataRecordId, "CurrentDataRecordId must be 0.");
            Assert.IsFalse(target.DocNotesVisibleTest, "DocNotesVisible must be false.");

            _detailPanel.VerifySet(x => x.Visible = false);
            _searchPanel.VerifySet(x => x.Visible = false);
            _resultsPanel.VerifySet(x => x.Visible = false);
            _homePanel.VerifySet(x => x.Visible = true);
        }

        #endregion

        #region ParseQueryString tests

        [TestMethod]
        public void TestInitializeRoutesSetsPageModeToRecordInsertWhenRouteHelperIsCreateRouteIsTrue()
        {
            var target = InitializeBuilderForParseQueryString().Build();
            _routeHelper.SetupGet(x => x.IsCreateRoute).Returns(true);
            _routeHelper.SetupGet(x => x.HasKnownRoute).Returns(true);
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);
            _mockCreateAccess.SetupGet(x => x.IsAllowed).Returns(true);

            target.InitializeRouteTest(_routeHelper.Object);
            Assert.AreEqual(PageModes.RecordInsert, target.PageModeTest);
        }

        [TestMethod]
        public void TestInitializeRoutesSetsPageModeToRecordReadOnlyWhenRouteHelperIsViewRouteIsTrue()
        {
            var target = InitializeBuilderForParseQueryString().Build();
            _routeHelper.SetupGet(x => x.IsViewRoute).Returns(true);
            _routeHelper.SetupGet(x => x.HasKnownRoute).Returns(true);
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);

            target.InitializeRouteTest(_routeHelper.Object);
            Assert.AreEqual(PageModes.RecordReadOnly, target.PageModeTest);
        }

        [TestMethod]
        public void TestInitializeRoutesSetsPageModeToResultsWhenRouteHelperIsResultsRouteIsTrue()
        {
            var target = InitializeBuilderForParseQueryString().Build();

            _routeHelper.SetupGet(x => x.IsResultsRoute).Returns(true);
            _routeHelper.SetupGet(x => x.HasKnownRoute).Returns(true);
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);

            target.InitializeRouteTest(_routeHelper.Object);
            Assert.AreEqual(PageModes.Results, target.PageModeTest);
        }

        [TestMethod]
        public void TestInitializeRoutesSetsPageModeToResultsWhenRouteHelperIsExcelExportRouteIsTrue()
        {
            var target = InitializeBuilderForParseQueryString()
               .Build();

            _mockedResultsGridView.SetupGet(x => x.Columns)
                                  .Returns(new DataControlFieldCollection());

            _routeHelper.SetupGet(x => x.IsExcelExportRoute).Returns(true);
            _routeHelper.SetupGet(x => x.HasKnownRoute).Returns(true);
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);

            target.InitializeRouteTest(_routeHelper.Object);
            Assert.AreEqual(PageModes.Results, target.PageModeTest);
        }

        [TestMethod]
        public void TestParseQueryStringContainsExportWritesToResponse()
        {
            var builder = InitializeBuilderForParseQueryString().WithCache(_iCache.Object);

            var cachedFilterKey = Guid.NewGuid();

            _mockediFilterCache.Setup(x => x.GetFilterBuilder(cachedFilterKey)).Returns(_filterBuilder.Object);

            var target = builder.Build();
            target.UseMockedApplyFilterBuilder = true;
            target.CachedFilterKey = cachedFilterKey;

            target.UseMockRenderResultsGridViewToExcel = true;
            target.MockedExcelString = "some string";

            _routeHelper.SetupGet(x => x.IsExcelExportRoute).Returns(true);
            _routeHelper.SetupGet(x => x.HasKnownRoute).Returns(true);
            _mockPageAccess.SetupGet(x => x.IsAllowed).Returns(true);

            _mockedResponse.Setup(x => x.Clear());
            _mockedResponse.Setup(
                x =>
                    x.AddHeader("content-disposition",
                        "attachment;filename=Data.xls"));
            _mockedResponse.Setup(x => x.Write(target.MockedExcelString));
            _mockedResponse.Setup(x => x.End());

            target.InitializeRouteTest(_routeHelper.Object);

            _mockedResponse.VerifyAll();
        }

        #endregion

        #region Permissions Tests

        [TestMethod]
        public void TestCreatePermissionsAddsModulePermissionsToPermissionsCollection()
        {
            var target = InitializeBuilder().Build();

            var result = target.CreatePermissionsTest();

            // Not asserting because Single() will throw
            // if there's only one.
            var onlyPerm = result.Permissions.Single();
            Assert.IsNotNull(onlyPerm);
            var expectedPerm = (RoleBasedDataPagePermissions)onlyPerm;
            Assert.AreSame(_mockModPerms.Object, expectedPerm.ModulePermissions);
        }

        #endregion

        #region GetCachedFilterBuilder

        [TestMethod]
        public void
            TestGetCachedFilterBuilderReturnsFilterBuilderIfDefaultPageModeIsResultsAndCurrentPageModeIsResults()
        {
            var target = InitializeBuilder()
                        .WithDefaultPageMode(PageModes.Results)
                        .WithMockedOnPageModeChanged(true)
                        .Build();
            _mockedResultsGridView.Setup(x => x.DataSource).Returns(_mockedResultsDataSource);
            target.PageModeTest = PageModes.Results;
            var result = target.GetCachedFilterBuilder();
            Assert.IsNotNull(result,
                "Because the DefaultPageMode is set to Results, a search is never performed. A FilterBuilder needs to be returned or else an infinite redirect back to the search page will occur.");
        }

        #endregion

        #endregion
    }
}
