using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Auditing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    [TestClass]
    public class DetailsViewDataPageBaeMoqTest
    {
        #region fields

        private string _primaryFieldName = "DataElementPrimaryFieldName";

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
        private DataControlFieldCollection _fields;
        private Mock<IDetailsView> _detailsView;

        // This could probably all get moved to a MockPermissions class
        // cause I think it's gonna end up being used a lot.
        private Mock<IRoleBasedDataPagePermissions> _mockPermissions;
        private Mock<IPermission> _mockPageAccess;
        private Mock<IPermission> _mockCreateAccess;
        private Mock<IPermission> _mockEditAccess;
        private Mock<IPermission> _mockDeleteAccess;

        #endregion

        #region Private Methods

        private TestDetailsPageBuilder InitializeBuilder()
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
            _detailsView = new Mock<IDetailsView>();

            _mockPermissions.SetupGet(x => x.PageAccess)
                            .Returns(_mockPageAccess.Object);
            _mockPermissions.SetupGet(x => x.CreateAccess)
                            .Returns(_mockCreateAccess.Object);
            _mockPermissions.SetupGet(x => x.DeleteAccess)
                            .Returns(_mockDeleteAccess.Object);
            _mockPermissions.SetupGet(x => x.EditAccess)
                            .Returns(_mockEditAccess.Object);

            _dataLinkMocks = new List<Mock<IDataLink>>();

            var mockedDataLink = new Mock<IDataLink>();
            _dataLinkMocks.Add(mockedDataLink);

            _dataLinks = (from m in _dataLinkMocks select m.Object).ToList();
            _fields = new DataControlFieldCollection();

            return new TestDetailsPageBuilder()
                  .WithIUser(_user.Object)
                  .WithPermissions(_mockPermissions.Object)
                   // .WithRouteHelper(_routeHelper.Object)
                  .WithResultsGridView(_mockedResultsGridView.Object)
                  .WithDetailPanel(_detailPanel.Object)
                  .WithResultsPanel(_resultsPanel.Object)
                  .WithSearchPanel(_searchPanel.Object)
                  .WithHomePanel(_homePanel.Object)
                  .WithSearchButton(_searchButton.Object)
                  .WithDataLinkControls(_dataLinks)
                  .WithAuditor(_auditor.Object)
                   //  .WithIFilterCache(_mockediFilterCache.Object)
                  .WithMockedRequest(_mockedRequest.Object)
                  .WithMockedResponse(_mockedResponse.Object)
                  .WithDetailsView(_detailsView.Object)
                  .WithDataElementPrimaryFieldName(_primaryFieldName);
        }

        #endregion

        #region Test methods

        #region OnPageModeChanged tests

        private void TestOnPageModeChangedAndDetailsView(PageModes pageMode, DetailsViewMode expectedDvm)
        {
            var target = InitializeBuilder().Build();
            target.PageModeTest = pageMode;
            _detailsView.Verify(x => x.ChangeMode(expectedDvm));
        }

        [TestMethod]
        public void TestOnPageModeChangedToRecordReadOnlySetsDetailsViewCurrentModeToReadOnly()
        {
            TestOnPageModeChangedAndDetailsView(PageModes.RecordReadOnly, DetailsViewMode.ReadOnly);
        }

        [TestMethod]
        public void TestOnPageModeChangedToRecordInsertSetsDetailsViewCurrentModeToInsert()
        {
            TestOnPageModeChangedAndDetailsView(PageModes.RecordInsert, DetailsViewMode.Insert);
        }

        [TestMethod]
        public void TestOnPageModeChangedToRecordUpdateSetsDetailsViewCurrentModeToEdit()
        {
            TestOnPageModeChangedAndDetailsView(PageModes.RecordUpdate, DetailsViewMode.Edit);
        }

        [TestMethod]
        public void TestOnPageModeChangedToRecordInsertThrowsIfAutoGenerateRowsTrue()
        {
            var target = InitializeBuilder().Build();
            _detailsView.SetupGet(x => x.AutoGenerateRows).Returns(true);
            MyAssert.Throws(() => target.PageModeTest = PageModes.RecordInsert);
        }

        #endregion

        #endregion
    }
}
