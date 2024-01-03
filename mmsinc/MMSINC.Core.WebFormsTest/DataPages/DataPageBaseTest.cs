using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Auditing;
using MMSINC.Utilities.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DataPageBaseTest
    /// </summary>
    [TestClass]
    public class DataPageBaseTest : EventFiringTestClass
    {
        #region Private Members

        private IPanel _searchPanel, _detailPanel, _resultsPanel;
        private IControl _homePanel;
        private IButton _searchButton;
        private IUser _user;
        private IGridView _mockedResultsGridView;

        private IResponse _mockedResponse;
        private IRequest _mockedRequest;
        private List<IDataLink> _dataLinks = new List<IDataLink>();
        private IAuditor _auditor;
        private IFilterBuilder _filterBuilder;
        private IDataPageRouteHelper _routeHelper;
        private ICache _iCache;
        private IFilterCache _mockediFilterCache;
        private SqlDataSource _mockedResultsDataSource;
        private IRoleBasedDataPagePermissions _mockPermissions;
        private DataControlFieldCollection _mockGridViewColumns;

        #endregion

        #region Test Initialization/Cleanup

        [TestInitialize]
        public void DataPageBaseTestInitialize() { }

        private TestDataPageBaseBuilder InitializeBuilder()
        {
            _mockGridViewColumns = new DataControlFieldCollection();

            _mocks = new MockRepository();
            _mocks
               .DynamicMock(out _user)
               .DynamicMock(out _searchPanel)
               .DynamicMock(out _detailPanel)
               .DynamicMock(out _resultsPanel)
               .DynamicMock(out _mockedResultsGridView)
               .DynamicMock(out _searchButton)
               .DynamicMock(out _mockedResponse)
               .DynamicMock(out _mockedRequest)
               .DynamicMock(out _auditor)
               .DynamicMock(out _filterBuilder)
               .DynamicMock(out _iCache)
               .DynamicMock(out _mockedResultsDataSource)
               .DynamicMock(out _mockPermissions)
               .DynamicMock(out _mockediFilterCache)
               .DynamicMock(out _homePanel)
               .DynamicMock(out _routeHelper);

            IDataLink mockedDataLink;
            _mocks.DynamicMock(out mockedDataLink);
            _dataLinks.Add(mockedDataLink);

            return new TestDataPageBaseBuilder()
                  .WithIUser(_user)
                  .WithPermissions(_mockPermissions)
                  .WithRouteHelper(_routeHelper)
                  .WithResultsGridView(_mockedResultsGridView)
                  .WithDetailPanel(_detailPanel)
                  .WithResultsPanel(_resultsPanel)
                  .WithSearchPanel(_searchPanel)
                  .WithHomePanel(_homePanel)
                  .WithSearchButton(_searchButton)
                  .WithDataLinkControls(_dataLinks)
                  .WithAuditor(_auditor)
                  .WithIFilterCache(_mockediFilterCache)
                  .WithMockedRequest(_mockedRequest)
                  .WithMockedResponse(_mockedResponse);
        }

        [TestCleanup]
        public void DataPageBaseTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Test Methods

        #region CurrentDataRecordId property

        [TestMethod]
        public void TestSettingCurrentDataRecordIdToNegativeNumberThrowsArgumentOutOfRangeException()
        {
            var target = InitializeBuilder().Build();
            MyAssert.Throws(() => target.CurrentDataRecordId = -153);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSettingCurrentDataRecordIdToZeroOrGreaterDoesNotThrowArgumentOutOfRangeException()
        {
            var target = InitializeBuilder().Build();
            const int expected = 134;

            using (_mocks.Record())
            {
                foreach (var dataLink in _dataLinks)
                {
                    SetupResult.For(dataLink.DataLinkID).Return(expected);
                }
            }

            using (_mocks.Playback())
            {
                MyAssert.DoesNotThrow(() => target.CurrentDataRecordId = 135);
                MyAssert.DoesNotThrow(() => target.CurrentDataRecordId = 0);
            }
        }

        #endregion

        #region Auditing

        [TestMethod]
        public void TestAuditMethodThrowsExceptionForNotSupportedAuditCategories()
        {
            var target = InitializeBuilder().Build();
            var categories =
                Enum.GetValues(typeof(AuditCategory)).Cast<AuditCategory>().ToList();

            // Remove the supported ones.
            categories.Remove(AuditCategory.DataInsert);
            categories.Remove(AuditCategory.DataView);
            categories.Remove(AuditCategory.DataUpdate);
            categories.Remove(AuditCategory.DataDelete);

            foreach (var cat in categories)
            {
                var curCat = cat;
                MyAssert.Throws(
                    () =>
                        target.AuditWithAuditCategoryAndRecordId(curCat, 3523));
            }

            _mocks.ReplayAll();
        }

        #endregion

        [TestMethod]
        public void TestSearchThrowsNullReferenceExceptionIfPassedNullFilterBuilder()
        {
            var target = InitializeBuilder().Build();
            MyAssert.Throws<NullReferenceException>(() => target.Search(null));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestApplyFilterSetsSelectCommandAndSelectParameters()
        {
            var target = InitializeBuilder()
                        .WithFilterBuilder(_filterBuilder)
                        .WithCache(_iCache)
                        .Build();

            var cacheGuid = Guid.NewGuid();

            target.CachedFilterKey = cacheGuid;

            var expectedParams = new List<Parameter>();
            expectedParams.Add(new Parameter("Something"));

            const string fakeSelectCommand = "SELECT * SOMETHING";
            var ds = new SqlDataSource();
            ds.SelectCommand = fakeSelectCommand;

            using (_mocks.Record())
            {
                SetupResult.For(_filterBuilder.BuildCompleteCommand()).Return(
                    fakeSelectCommand);
                SetupResult.For(_filterBuilder.BuildParameters()).Return(expectedParams);
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(ds);
                SetupResult.For(_mockediFilterCache.GetFilterBuilder(cacheGuid))
                           .Return(_filterBuilder);
            }

            using (_mocks.Playback())
            {
                target.ApplyFilter();
                Assert.AreEqual(ds.SelectCommand, fakeSelectCommand);

                foreach (var p in expectedParams)
                {
                    Assert.IsTrue(ds.SelectParameters.Contains(p), "All parameters must be added to SelectParameters.");
                }
            }
        }

        [TestMethod]
        public void TestSearchSetsCachedFilterKeyProperty()
        {
            var target = InitializeBuilder()
                        .WithResultsGridView(_mockedResultsGridView)
                        .WithFilterBuilder(_filterBuilder)
                        .WithCache(_iCache)
                        .Build();

            var expectedKey = Guid.NewGuid();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSource).Return(
                    new SqlDataSource());
                SetupResult.For(
                                _mockediFilterCache.AddFilterBuilderToCache(_filterBuilder))
                           .Return(expectedKey);
                SetupResult.For(_mockedRequest.Url).Return("http://www.gigglebytes.com/");
            }

            _mocks.ReplayAll();
            target.Search();

            Assert.IsFalse(target.CachedFilterKey == Guid.Empty, "CachedFilterKey property must be set.");

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestAddPrimaryKeyToDataKeyNamesAddsNameIfItIsntAlreadyInArray()
        {
            var target = InitializeBuilder().Build();

            var testArray = new[] {"yeah"};
            var expectedPrimaryKey = target.DataElementPrimaryFieldNameTest;

            var result = target.AddPrimaryKeyToDataKeyNames(testArray);

            Assert.IsTrue(result.Contains(expectedPrimaryKey));

            _mocks.ReplayAll();
        }

        #region Event handlers

        [TestMethod]
        public void TestCreateFilterBuilderDoesNotReturnNull()
        {
            var target = InitializeBuilder()
                        .WithResultsGridView(_mockedResultsGridView)
                        .WithFilterBuilder(_filterBuilder)
                        .WithCache(_iCache)
                        .Build();

            var expectedKey = Guid.NewGuid();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSource).Return(
                    new SqlDataSource());
                SetupResult.For(
                                _mockediFilterCache.AddFilterBuilderToCache(_filterBuilder))
                           .Return(expectedKey);
                SetupResult.For(_mockedRequest.Url).Return("http://www.gigglebytes.com/");
            }

            Assert.IsNotNull(target.CreateFilterBuilderTest());
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSearchButtonClickedCallsCreateFilter()
        {
            var target = InitializeBuilder()
                        .WithCache(_iCache)
                        .WithMockedOnPageModeChanged(true)
                        .WithFilterBuilder(_filterBuilder)
                        .Build();

            target.UseMockedCreateFilter = true;

            var expectedCacheKey = Guid.NewGuid();

            var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?search=" + expectedCacheKey.ToString());

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());
                // target.CreateFilter();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(target, "OnSearchButtonClicked");
            }
        }

        [TestMethod]
        public void TestSearchButtonClickedRedirectsToSearchResultsPage()
        {
            var target = InitializeBuilder()
                        .WithCache(_iCache)
                        .WithResultsGridView(_mockedResultsGridView)
                        .WithMockedOnPageModeChanged(true)
                        .WithFilterBuilder(_filterBuilder).Build();

            target.UseMockedCreateFilter = true;

            var expectedCacheKey = Guid.NewGuid();
            target.CachedFilterKey = expectedCacheKey;

            var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?search=" + expectedCacheKey.ToString());

            using (_mocks.Record())
            {
                SetupResult.For(
                                _mockediFilterCache.AddFilterBuilderToCache(_filterBuilder))
                           .Return(expectedCacheKey);
                SetupResult.For(_mockedResultsGridView.DataSource).Return(
                    new SqlDataSource());
                SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());
                _mockedResponse.Redirect(testUrl.OriginalString, true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(target, "OnSearchButtonClicked");
            }
        }

        [TestMethod]
        public void TestRedirectPageSetsEndResponseParameterToTrue()
        {
            var target = InitializeBuilder()
               .Build();

            var expectedUrl = "someurl";

            using (_mocks.Record())
            {
                _mockedResponse.Redirect(expectedUrl, true);
            }

            using (_mocks.Playback())
            {
                target.TestRedirectPage(expectedUrl);
            }
        }

        #region OnRecordSaved

        [TestMethod]
        public void TestOnRecordSavedRedirectsPageToRecordIfSaveTypeIsInsert()
        {
            var target = InitializeBuilder().Build();

            const int expectedRecordId = 43;
            const string expectedRootUrl = "http://www.donuts.com/Glazed.aspx";
            var expectedRedirectUrl = String.Format("{0}?{1}={2}", expectedRootUrl, "view", expectedRecordId);

            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Insert, expectedRecordId, null);

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(expectedRootUrl);
                _mockedResponse.Redirect(expectedRedirectUrl, true);
            }

            using (_mocks.Playback())
            {
                target.OnRecordSavedTest(args);
            }
        }

        [TestMethod]
        public void TestOnRecordSavedRedirectsPageToRecordIfSaveTypeIsUpdate()
        {
            var target = InitializeBuilder().Build();

            const int expectedRecordId = 43;
            const string expectedRootUrl = "http://www.donuts.com/Glazed.aspx";
            var expectedRedirectUrl = String.Format("{0}?{1}={2}", expectedRootUrl, "view", expectedRecordId);

            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, expectedRecordId, null);

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(expectedRootUrl);
                _mockedResponse.Redirect(expectedRedirectUrl, true);
            }

            using (_mocks.Playback())
            {
                target.OnRecordSavedTest(args);
            }
        }

        [TestMethod]
        public void TestOnRecordSavedRedirectsPageToDefaultIfSaveTypeIsDelete()
        {
            var target = InitializeBuilder().Build();

            const int expectedRecordId = 43;
            const string expectedRootUrl = "http://www.donuts.com/Glazed.aspx";

            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Delete, expectedRecordId, null);

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(expectedRootUrl);
                _mockedResponse.Redirect(expectedRootUrl, true);
            }

            using (_mocks.Playback())
            {
                target.OnRecordSavedTest(args);
            }
        }

        [TestMethod]
        public void TestOnRecordSavedAuditsDataInserts()
        {
            var expected = AuditCategory.DataInsert;
            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Insert, 43, null);

            var target = InitializeBuilder()
                        .WithMockedAuditMethod(true)
                        .Build();

            var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx");

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());
            }

            using (_mocks.Playback())
            {
                target.OnRecordSavedTest(args);
                Assert.AreEqual(target.LastAuditCategory, expected);
            }
        }

        [TestMethod]
        public void TestOnRecordSavedAuditsDataUpdates()
        {
            var expected = AuditCategory.DataUpdate;
            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, 3453, null);

            var target = InitializeBuilder()
                        .WithMockedAuditMethod(true)
                        .Build();

            var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx");

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());
            }

            using (_mocks.Playback())
            {
                target.OnRecordSavedTest(args);
                Assert.AreEqual(target.LastAuditCategory, expected);
            }
        }

        [TestMethod]
        public void TestOnRecordSavedAuditsDataDeletes()
        {
            var expected = AuditCategory.DataDelete;
            var args = new DataRecordSavedEventArgs(DataRecordSaveTypes.Delete, 3453, null);

            var target = InitializeBuilder()
                        .WithMockedAuditMethod(true)
                        .Build();

            _mocks.ReplayAll();
            target.OnRecordSavedTest(args);
            Assert.AreEqual(target.LastAuditCategory, expected);
        }

        #endregion

        #region OnInit

        [TestMethod]
        public void TestOnInitRegistersRequiresControlState()
        {
            var target = InitializeBuilder()
                        .WithResultsGridView(_mockedResultsGridView)
                        .Build();

            using (_mocks.Record())
            {
                SetupResult
                   .For(_mockedResultsGridView.Columns)
                   .Return(_mockGridViewColumns);
                SetupResult
                   .For(_mockedResultsGridView.DataSourceObject)
                   .Return(_mockedResultsDataSource);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(target, "OnInit", EventArgs.Empty);

                Assert.IsTrue(target.RequiresControlState(target),
                    "DataPageBase requires ControlState registration.");
            }
        }

        [TestMethod]
        public void TestOnInitDisablesResultsGridViewViewState()
        {
            var target = InitializeBuilder()
                        .WithResultsGridView(_mockedResultsGridView)
                        .Build();

            using (_mocks.Record())
            {
                SetupResult
                   .For(_mockedResultsGridView.Columns)
                   .Return(_mockGridViewColumns);
                SetupResult
                   .For(_mockedResultsGridView.DataSourceObject)
                   .Return(
                        _mockedResultsDataSource);
                _mockedResultsGridView.EnableViewState = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitDisablesResultsDataSourceViewState()
        {
            var target = InitializeBuilder()
                        .WithResultsGridView(_mockedResultsGridView)
                        .Build();

            using (_mocks.Record())
            {
                SetupResult
                   .For(_mockedResultsGridView.Columns)
                   .Return(_mockGridViewColumns);
                SetupResult
                   .For(_mockedResultsGridView.DataSourceObject)
                   .Return(_mockedResultsDataSource);
                _mockedResultsDataSource.EnableViewState = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(target, "OnInit", EventArgs.Empty);
                Assert.IsFalse(_mockedResultsDataSource.EnableViewState,
                    "ViewState is required to be disabled on ResultsDataSource.");
            }
        }

        #endregion

        [TestMethod]
        public void TestOnPreRenderSetsDataLinkControlVisibility()
        {
            var target = InitializeBuilder().Build();
            var expected = true;

            using (_mocks.Record())
            {
                foreach (var dataLink in target.DataLinkControlsTest)
                {
                    SetupResult.For(dataLink.Visible).Return(expected);
                }
            }

            using (_mocks.Playback())
            {
                target.DocNotesVisibleTest = expected;
                InvokeEventByName(target, "OnPreRender", EventArgs.Empty);

                foreach (var dataLink in target.DataLinkControlsTest)
                {
                    Assert.AreEqual(dataLink.Visible, expected);
                }
            }
        }

        [TestMethod]
        public void TestSaveControlStateAddsExpectedPropertiesToControlState()
        {
            var expectedGuid = Guid.NewGuid();
            var expectedPageMode = PageModes.RecordReadOnly;
            var expectedRecordId = 25;

            var target = InitializeBuilder()
                        .WithDataLinkControls(null)
                        .WithMockedOnPageModeChanged(true)
                        .Build();
            target.CurrentDataRecordId = expectedRecordId;
            target.CachedFilterKey = expectedGuid;
            target.PageModeTest = expectedPageMode;

            var state = target.SaveControlStateTest();
            Assert.IsNotNull(state, "Control state must be returned from page when values are non-default.");

            // Takes the state returning by SaveControlState and loads it into
            // a new ControlState instance.
            var cs = new ControlState(state);

            Assert.AreEqual(cs.Get<Guid>("CachedFilterKey"), expectedGuid);
            Assert.AreEqual(cs.Get<PageModes>("PageMode"), expectedPageMode);
            Assert.AreEqual(cs.Get<int>("CurrentDataRecordId"), expectedRecordId);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestLoadControlStateSetsDeserializedValues()
        {
            var target = InitializeBuilder()
                        .WithMockedOnPageModeChanged(true)
                        .WithDataLinkControls(null)
                        .Build();

            var testControlState = new ControlState();
            var expectedGuid = Guid.NewGuid();
            var expectedPageMode = PageModes.RecordReadOnly;
            var expectedRecordId = 35;

            testControlState.Add("CachedFilterKey", expectedGuid, Guid.Empty);
            testControlState.Add("PageMode", expectedPageMode, PageModes.Search);
            testControlState.Add("CurrentDataRecordId", expectedRecordId, 0);

            target.LoadControlStateTest(testControlState.GetControlStateObject());

            Assert.AreEqual(target.CachedFilterKey, expectedGuid);
            Assert.AreEqual(target.PageModeTest, expectedPageMode);
            Assert.AreEqual(target.CurrentDataRecordId, expectedRecordId);

            _mocks.ReplayAll();
        }

        #endregion

        #endregion
    }

    public class TestDataPageBase : DataPageBase
    {
        #region Implemented properties

        protected override IControl HomePanel
        {
            get { return HomePanelTest; }
        }

        protected override IButton SearchButton
        {
            get { return SearchButtonTest; }
        }

        protected override IGridView ResultsGridView
        {
            get { return ResultsGridViewTest; }
        }

        protected override IControl DetailPanel
        {
            get { return DetailPanelTest; }
        }

        protected override IControl ResultsPanel
        {
            get { return ResultsPanelTest; }
        }

        protected override IControl SearchPanel
        {
            get { return SearchPanelTest; }
        }

        protected override string DataElementTableName
        {
            get { return "DataElementTableName"; }
        }

        protected override string DataElementPrimaryFieldName
        {
            get { return "DataElementPrimaryFieldName"; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return ModulePermissionsTest; }
        }

        #endregion

        #region Test properties

        public IDataPageRouteHelper RouteHelperTest
        {
            get { return this.RouteHelper; }
            set { this.RouteHelper = value; }
        }

        public string DataElementPrimaryFieldNameTest
        {
            get { return DataElementPrimaryFieldName; }
        }

        public IAuditor AuditorTest { get; set; }

        internal PageModes PageModeTest
        {
            get { return this.PageMode; }
            set { this.PageMode = value; }
        }

        public IModulePermissions ModulePermissionsTest { get; set; }

        public bool DocNotesVisibleTest
        {
            get { return this.DocNotesVisible; }
            set { this.DocNotesVisible = value; }
        }

        public IControl SearchPanelTest { get; set; }
        public IControl ResultsPanelTest { get; set; }
        public IControl DetailPanelTest { get; set; }
        public IControl HomePanelTest { get; set; }
        public IGridView ResultsGridViewTest { get; set; }
        public IButton AddNewRecordButtonTest { get; set; }
        public IButton SearchButtonTest { get; set; }
        public IButton BackToSearchButtonTest { get; set; }
        public IButton ExportButtonTest { get; set; }
        public IButton ResetSearchButtonTest { get; set; }
        public IButton BackToResultsButtonTest { get; set; }
        public bool UseMockedOnPageModeChanged { get; set; }
        public IResponse IResponseTest { get; set; }
        public IRequest IRequestTest { get; set; }
        public bool UseMockedAuditMethod { get; set; }
        public IFilterBuilder MockedFilterBuilder { get; set; }
        public ICache MockedCache { get; set; }
        public IFilterCache MockedFilterCache { get; set; }
        public bool UseMockedApplyFilterBuilder { get; set; }
        public PageModes TestDefaultPageMode { get; set; }

        protected override PageModes DefaultPageMode
        {
            get { return TestDefaultPageMode; }
        }

        public override IResponse IResponse
        {
            get
            {
                if (IResponseTest != null)
                {
                    return IResponseTest;
                }

                return base.IResponse;
            }
        }

        public override IRequest IRequest
        {
            get
            {
                if (IRequestTest != null)
                {
                    return IRequestTest;
                }

                return base.IRequest;
            }
        }

        private IEnumerable<IDataLink> _dataLinks;

        public IEnumerable<IDataLink> DataLinkControlsTest
        {
            get { return _dataLinks; }
            set { _dataLinks = value; }
        }

        public IRoleBasedDataPagePermissions PermissionsTest { get; set; }

        public override IDataPagePermissions Permissions
        {
            get { return PermissionsTest; }
        }

        public override ICache ICache
        {
            get { return MockedCache ?? base.ICache; }
        }

        public bool UseMockedApplyFilter { get; set; }
        public bool UseMockedCreateFilter { get; set; }
        public bool UseMockRenderResultsGridViewToExcel { get; set; }

        public string MockedExcelString { get; set; }

        #endregion

        #region Test methods

        public void TestRedirectPage(string url)
        {
            RedirectPage(url);
        }

        protected override string RenderResultsGridViewToExcel()
        {
            if (UseMockRenderResultsGridViewToExcel)
            {
                return MockedExcelString;
            }

            return base.RenderResultsGridViewToExcel();
        }

        public void AuditWithAuditCategoryAndRecordId(AuditCategory category, int recordId)
        {
            base.Audit(category, recordId);
        }

        public string RenderResultsGridViewToExcelTest()
        {
            return base.RenderResultsGridViewToExcel();
        }

        public void SetIUser(IUser value)
        {
            this._iUser = value;
        }

        public void LoadControlStateTest(object state)
        {
            base.LoadControlState(state);
        }

        public object SaveControlStateTest()
        {
            return this.SaveControlState();
        }

        public void OnRecordSavedTest(DataRecordSavedEventArgs e)
        {
            base.OnRecordSaved(e);
        }

        public string BuildFilterExpressionTest()
        {
            return "Some filter";
        }

        public void InitializeRouteTest(IDataPageRouteHelper routHelper)
        {
            this.InitializeRoute(routHelper);
        }

        public void LoadDataRecordTest(int recordId)
        {
            LoadDataRecord(recordId);
        }

        public IFilterBuilder CreateFilterBuilderTest()
        {
            return CreateFilterBuilder();
        }

        protected override IFilterBuilder CreateFilterBuilder()
        {
            if (MockedFilterBuilder != null)
            {
                return MockedFilterBuilder;
            }
            else
            {
                return base.CreateFilterBuilder();
            }
        }

        internal override void ApplyFilter()
        {
            if (!UseMockedApplyFilter)
            {
                base.ApplyFilter();
            }
        }

        internal protected override IFilterBuilder CreateFilter()
        {
            if (!UseMockedCreateFilter)
            {
                return base.CreateFilter();
            }

            return this.MockedFilterBuilder;
        }

        #endregion

        #region Override Methods

        internal override IDataPageRouteHelper CreateRouteHelper()
        {
            return RouteHelper;
        }

        internal override IFilterCache GetFilterCache()
        {
            return MockedFilterCache;
        }

        public AuditCategory LastAuditCategory { get; set; }

        protected internal override void Audit(AuditCategory category, int recordId)
        {
            if (!UseMockedAuditMethod)
            {
                base.Audit(category, recordId);
            }

            LastAuditCategory = category;
        }

        protected override IAuditor CreateAuditor()
        {
            return this.AuditorTest;
        }

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            // Do nothing
        }

        protected internal override void ApplyFilterBuilder(IFilterBuilder fb)
        {
            if (!UseMockedApplyFilterBuilder)
            {
                base.ApplyFilterBuilder(fb);
            }
        }

        protected override IEnumerable<IDataLink> GetIDataLinkControls()
        {
            return _dataLinks;
        }

        protected override void OnPageModeChanged(PageModes newMode)
        {
            if (!UseMockedOnPageModeChanged)
            {
                base.OnPageModeChanged(newMode);
            }
        }

        #endregion

        #region Test Method Helpers

        public AggregatedDataPagePermissions CreatePermissionsTest()
        {
            return CreatePermissions();
        }

        #endregion
    }

    public class TestDataPageBaseBuilder : TestDataBuilder<TestDataPageBase>
    {
        #region Private Members

        private IUser _user;
        private IGridView _resultsGridView;
        private IControl _detailPanel;
        private IControl _searchPanel;
        private IControl _resultPanel;
        private IControl _homePanel;
        private IButton _searchButton;

        private bool _useMockedOnPageModeChanged,
                     _useMockedAuditMethod;

        private IResponse _response;
        private IRequest _request;
        private IEnumerable<IDataLink> _dataLinks;
        private IRoleBasedDataPagePermissions _permissions;
        private IAuditor _auditor;
        private IFilterBuilder _filterBuilder;
        private ICache _iCache;
        private IFilterCache _iFilterCache;
        private IDataPageRouteHelper _routeHelper;
        private IModulePermissions _modPerms;
        private PageModes _defaultPageMode;

        #endregion

        #region Exposed Methods

        public TestDataPageBaseBuilder WithAuditor(IAuditor auditor)
        {
            _auditor = auditor;
            return this;
        }

        public TestDataPageBaseBuilder WithCache(ICache cache)
        {
            _iCache = cache;
            return this;
        }

        public TestDataPageBaseBuilder WithDataLinkControls(IEnumerable<IDataLink> controls)
        {
            _dataLinks = controls;
            return this;
        }

        public TestDataPageBaseBuilder WithDefaultPageMode(PageModes defaultPageMode)
        {
            _defaultPageMode = defaultPageMode;
            return this;
        }

        public TestDataPageBaseBuilder WithDetailPanel(IControl dp)
        {
            _detailPanel = dp;
            return this;
        }

        public TestDataPageBaseBuilder WithHomePanel(IControl homePanel)
        {
            _homePanel = homePanel;
            return this;
        }

        public TestDataPageBaseBuilder WithFilterBuilder(IFilterBuilder builder)
        {
            _filterBuilder = builder;
            return this;
        }

        public TestDataPageBaseBuilder WithIFilterCache(IFilterCache cache)
        {
            _iFilterCache = cache;
            return this;
        }

        public TestDataPageBaseBuilder WithModulePermissions(IModulePermissions mp)
        {
            _modPerms = mp;
            return this;
        }

        public TestDataPageBaseBuilder WithPermissions(IRoleBasedDataPagePermissions perms)
        {
            _permissions = perms;
            return this;
        }

        public TestDataPageBaseBuilder WithIUser(IUser user)
        {
            _user = user;
            return this;
        }

        public TestDataPageBaseBuilder WithResultsGridView(IGridView igv)
        {
            _resultsGridView = igv;
            return this;
        }

        public TestDataPageBaseBuilder WithResultsPanel(IControl rp)
        {
            _resultPanel = rp;
            return this;
        }

        public TestDataPageBaseBuilder WithRouteHelper(IDataPageRouteHelper rh)
        {
            _routeHelper = rh;
            return this;
        }

        public TestDataPageBaseBuilder WithSearchButton(IButton search)
        {
            _searchButton = search;
            return this;
        }

        public TestDataPageBaseBuilder WithSearchPanel(IControl sp)
        {
            _searchPanel = sp;
            return this;
        }

        public TestDataPageBaseBuilder WithMockedOnPageModeChanged(bool mock)
        {
            _useMockedOnPageModeChanged = mock;
            return this;
        }

        public TestDataPageBaseBuilder WithMockedAuditMethod(bool doItRockapella)
        {
            _useMockedAuditMethod = doItRockapella;
            return this;
        }

        public TestDataPageBaseBuilder WithMockedResponse(IResponse resp)
        {
            _response = resp;
            return this;
        }

        public TestDataPageBaseBuilder WithMockedRequest(IRequest req)
        {
            _request = req;
            return this;
        }

        public override TestDataPageBase Build()
        {
            var woot = new TestDataPageBase();

            if (_user != null)
            {
                woot.SetIUser(_user);
            }

            if (_response != null)
            {
                woot.IResponseTest = _response;
            }

            if (_request != null)
            {
                woot.IRequestTest = _request;
            }

            woot.RouteHelperTest = _routeHelper;
            woot.ResultsGridViewTest = _resultsGridView;
            woot.DetailPanelTest = _detailPanel;
            woot.HomePanelTest = _homePanel;
            woot.ResultsPanelTest = _resultPanel;
            woot.SearchPanelTest = _searchPanel;
            woot.SearchButtonTest = _searchButton;
            woot.UseMockedOnPageModeChanged = _useMockedOnPageModeChanged;
            woot.DataLinkControlsTest = _dataLinks;
            woot.PermissionsTest = _permissions;
            woot.AuditorTest = _auditor;
            woot.UseMockedAuditMethod = _useMockedAuditMethod;
            woot.MockedFilterBuilder = _filterBuilder;
            woot.MockedCache = _iCache;
            woot.MockedFilterCache = _iFilterCache;
            woot.ModulePermissionsTest = _modPerms;
            woot.TestDefaultPageMode = _defaultPageMode;
            return woot;
        }

        #endregion
    }
}
