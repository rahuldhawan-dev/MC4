using System.IO;
using System.Web.UI;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrdersReportTestTest
    /// </summary>
    [TestClass]
    public class WorkOrdersReportTest : EventFiringTestClass
    {
        #region Private Members

        private IPanel _pnlSearch, _pnlResults;
        private IGridView _gvSearchResults;
        private TestWorkOrdersReport _target;
        private ISecurityService _securityService;
        private IUser _iUser;
        private IResponse _iResponse;
        private IRequest _iRequest;
        private IViewState _viewState;
        private IRepository<ReportViewing> _reportViewingRepository;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _pnlSearch)
                .DynamicMock(out _pnlResults)
                .DynamicMock(out _gvSearchResults)
                .DynamicMock(out _securityService)
                .DynamicMock(out _iUser)
                .DynamicMock(out _iResponse)
                .DynamicMock(out _viewState)
                .DynamicMock(out _reportViewingRepository)
                .DynamicMock(out _iRequest);

            _target = new TestWorkOrdersReportBuilder()
                .WithPNLSearch(_pnlSearch)
                .WithPNLResults(_pnlResults)
                .WithGVSearchResults(_gvSearchResults)
                .WithSecurityService(_securityService)
                .WithUser(_iUser)
                .WithResponse(_iResponse)
                .WithViewState(_viewState)
                .WithReportViewingRepository(_reportViewingRepository)
                .WithRequest(_iRequest);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSecurityServiceCanBeInjected()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_securityService, _target.SecurityService);
        }

        [TestMethod]
        public void TestSecurityServiceGetsSetToSecurityServiceInstanceIfNotInjected()
        {
            _mocks.ReplayAll();

            _target = new TestWorkOrdersReportBuilder()
                .WithSecurityService(null);

            Assert.AreSame(SecurityService.Instance, _target.SecurityService);
        }

        [TestMethod]
        public void TestIUserCanBeInjected()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_iUser, _target.IUser);
        }
        
        [TestMethod]
        public void TestSettingSortDirectionSetsValueInViewState()
        {
            var expected = "foo";

            using (_mocks.Record())
            {
                _viewState.SetValue(
                    WorkOrdersReport.ViewStateKeys.SORT_DIRECTION, expected);
            }
            using (_mocks.Playback())
            {
                _target.SortDirection = expected;  
            }
        }

        [TestMethod]
        public void TestGettingSortDirectionRetrievesValueFromViewState()
        {
            var expected = "foo";

            using (_mocks.Record())
            {
                SetupResult.For(_viewState.GetValue(
                    WorkOrdersReport.ViewStateKeys.SORT_DIRECTION))
                    .Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.SortDirection);
            }
        }

        [TestMethod]
        public void TestSettingSortExpressionSetsValueInViewState()
        {
            var expected = "foo";

            using (_mocks.Record())
            {
                _viewState.SetValue(
                    WorkOrdersReport.ViewStateKeys.SORT_EXPRESSION, expected);
            }
            using (_mocks.Playback())
            {
                _target.SortExpression = expected;
            }
        }

        [TestMethod]
        public void TestGettingSortExpressionRetrievesValueFromViewState()
        {
            var expected = "foo";

            using (_mocks.Record())
            {
                SetupResult.For(_viewState.GetValue(
                    WorkOrdersReport.ViewStateKeys.SORT_EXPRESSION))
                    .Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.SortExpression);
            }
        }
        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageInitInitializesTheSecurityService()
        {
            using (_mocks.Record())
            {
                _securityService.Init(_iUser);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Init");
            }
        }

        [TestMethod]
        public void TestSearchButtonClickHidesSearchShowsResultsAndBindsGridViewAndRecordsReportViewing()
        {
            var expectedEmployee = new Employee {
                EmployeeID = 123
            };

            using (_mocks.Record())
            {
                SetupResult
                    .For(_securityService.Employee)
                    .Return(expectedEmployee);
                SetupResult
                    .For(_iRequest.Url)
                    .Return("/This is some url.asdf");
                _reportViewingRepository.InsertNewEntity(null);
                LastCall.IgnoreArguments();
                _pnlSearch.Visible = false;
                _pnlResults.Visible = true;
                _gvSearchResults.DataBind();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSearch_Click");
            }
        }

        [TestMethod]
        public void TestReturnToSearchButtonClickHidesResultsAndShowsSearch()
        {
            using (_mocks.Record())
            {
                _pnlResults.Visible = false;
                _pnlSearch.Visible = true;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnReturnToSearch_Click");
            }
        }

        [TestMethod]
        public void TestExportButtonExportsExcelFile()
        {
            using (_mocks.Record())
            {
                _target.CallExportToExcel();
            }

            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "btnExport_Click");
            }
        }

        [TestMethod]
        public void TestExportToExcelRendersGridView()
        {
            var stringWriter = new StringWriter();
            var htmlTextWriter = new HtmlTextWriter(stringWriter);
            _target.SetHiddenFieldValueByName("_reportStringWriter",stringWriter);
            _target.SetHiddenFieldValueByName("_reportHtmlTextWriter",htmlTextWriter);

            using (_mocks.Record())
            {
                _gvSearchResults.AllowSorting = false;
                _gvSearchResults.DataBind();
                _iResponse.Clear();
                _iResponse.AddHeader(WorkOrdersReport.RESPONSE_HEADER_NAME,WorkOrdersReport.RESPONSE_HEADER_VALUE);
                _gvSearchResults.RenderControl(htmlTextWriter);
                _iResponse.Write(stringWriter.ToString());
                _iResponse.End();
            }
            using (_mocks.Playback())
            {
                _target.CallExportToExcel();
            }
        }

        #endregion
    }

    internal class TestWorkOrdersReportBuilder : TestDataBuilder<TestWorkOrdersReport>
    {
        #region Private Members

        private IPanel _pnlSearch,
                       _pnlResults;
        private IGridView _gvSearchResults;
        private ISecurityService _securityService;
        private IUser _iUser;
        private IResponse _iResponse;
        private IRequest _iRequest;
        private IViewState _viewState;
        private IRepository<ReportViewing> _reportViewingRepository;

        #endregion

        #region Exposed Methods

        public override TestWorkOrdersReport Build()
        {
            var obj = new TestWorkOrdersReport();
            if (_pnlSearch != null)
                obj.SetPNLSearch(_pnlSearch);
            if (_pnlResults != null)
                obj.SetPNLResults(_pnlResults);
            if (_gvSearchResults != null)
                obj.SetGVSearchResults(_gvSearchResults);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_iUser != null)
                obj.SetUser(_iUser);
            if (_iResponse != null)
                obj.SetResponse(_iResponse);
            if (_iRequest != null)
                obj.SetRequest(_iRequest);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_reportViewingRepository != null)
                obj.SetReportViewingRepository(_reportViewingRepository);
            return obj;
        }

        public TestWorkOrdersReportBuilder WithPNLSearch(IPanel search)
        {
            _pnlSearch = search;
            return this;
        }

        public TestWorkOrdersReportBuilder WithPNLResults(IPanel results)
        {
            _pnlResults = results;
            return this;
        }

        public TestWorkOrdersReportBuilder WithGVSearchResults(IGridView results)
        {
            _gvSearchResults = results;
            return this;
        }

        public TestWorkOrdersReportBuilder WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }

        public TestWorkOrdersReportBuilder WithUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        public TestWorkOrdersReportBuilder WithResponse(IResponse response)
        {
            _iResponse = response;
            return this;
        }

        public TestWorkOrdersReportBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestWorkOrdersReportBuilder WithReportViewingRepository(IRepository<ReportViewing> reportViewingRepository)
        {
            _reportViewingRepository = reportViewingRepository;
            return this;
        }

        public TestWorkOrdersReportBuilder WithRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrdersReport : WorkOrdersReport
    {
        #region Exposed Methods

        public void SetPNLSearch(IPanel search)
        {
            pnlSearch = search;
        }

        public void SetPNLResults(IPanel results)
        {
            pnlResults = results;
        }

        public void SetGVSearchResults(IGridView results)
        {
            gvSearchResults = results;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        public void SetUser(IUser user)
        {
            _iUser = user;
        }

        public void SetResponse(IResponse response)
        {
            _iResponse = response;
        }

        public void SetRequest(IRequest request)
        {
            _iRequest = request;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetReportViewingRepository(IRepository<ReportViewing> reportViewingRepository)
        {
            _reportViewingRepository = reportViewingRepository;
        }

        public void CallExportToExcel()
        {
            ExportToExcel();
        }

        #endregion
    }
}
