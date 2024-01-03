using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
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
    /// Summary description for DetailsViewDataPageBaseTest
    /// </summary>
    [TestClass]
    public class DetailsViewDataPageBaseTest : EventFiringTestClass
    {
        #region Private Fields

        private TestDetailsPage _target;
        private IRoleBasedDataPagePermissions _permissions;
        private IPanel _searchPanel, _detailPanel, _resultsPanel;
        private IControl _homePanel;

        private IButton _addNewRecordButton,
                        _backToSearchButton,
                        _backToResultsButton,
                        _exportButton,
                        _searchButton,
                        _resetSearchButton;

        private IUser _user;
        private IGridView _mockedResultsGridView;
        private IResponse _mockedResponse;
        private IRequest _mockedRequest;
        private List<IDataLink> _dataLinks = new List<IDataLink>();
        private IAuditor _auditor;
        private IDetailsView _detailsView;
        private DataControlFieldCollection _fields;
        private string _primaryFieldName = "DataElementPrimaryFieldName";
        private SqlDataSource _mockedResultsDataSource;
        private SqlDataSource _mockedDetailsDataSource;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void DataPageBaseTestInitialize()
        {
            _mocks = new MockRepository();
            _target = InitializeBuilder();
        }

        [TestCleanup]
        public void DataPageBaseTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Private Methods

        private TestDetailsPageBuilder InitializeBuilder()
        {
            _mocks
               .DynamicMock(out _user)
               .DynamicMock(out _permissions)
               .DynamicMock(out _searchPanel)
               .DynamicMock(out _detailPanel)
               .DynamicMock(out _resultsPanel)
               .DynamicMock(out _mockedResultsGridView)
               .DynamicMock(out _addNewRecordButton)
               .DynamicMock(out _backToResultsButton)
               .DynamicMock(out _backToSearchButton)
               .DynamicMock(out _exportButton)
               .DynamicMock(out _searchButton)
               .DynamicMock(out _resetSearchButton)
               .DynamicMock(out _mockedResponse)
               .DynamicMock(out _mockedRequest)
               .DynamicMock(out _auditor)
               .DynamicMock(out _detailsView)
               .DynamicMock(out _mockedResultsDataSource)
               .DynamicMock(out _mockedDetailsDataSource)
               .DynamicMock(out _homePanel);

            IDataLink mockedDataLink;
            _mocks.DynamicMock(out mockedDataLink);
            _dataLinks.Add(mockedDataLink);
            _fields = new DataControlFieldCollection();

            return new TestDetailsPageBuilder()
                  .WithIUser(_user)
                  .WithPermissions(_permissions)
                  .WithResultsGridView(_mockedResultsGridView)
                  .WithHomePanel(_homePanel)
                  .WithDetailPanel(_detailPanel)
                  .WithResultsPanel(_resultsPanel)
                  .WithSearchPanel(_searchPanel)
                  .WithAddNewRecordButton(_addNewRecordButton)
                  .WithBackToResultsButton(_backToResultsButton)
                  .WithBackToSearchButton(_backToSearchButton)
                  .WithExportButton(_exportButton)
                  .WithSearchButton(_searchButton)
                  .WithResetSearchButton(_resetSearchButton)
                  .WithDataLinkControls(_dataLinks)
                  .WithAuditor(_auditor)
                  .WithDetailsView(_detailsView)
                  .WithMockedRequest(_mockedRequest)
                  .WithMockedResponse(_mockedResponse)
                  .WithDataElementPrimaryFieldName(_primaryFieldName);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestLoadDataRecordThrowsExceptionIfDetailsViewDataSourceIsMissingPrimaryKeyParameter()
        {
            _target = InitializeBuilder()
                     .WithMockedOnPageModeChanged(true)
                     .Build();

            var ds = new SqlDataSource();
            var expectedRecordId = 43;

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws(() => _target.LoadDataRecordTest(expectedRecordId));
            }
        }

        [TestMethod]
        public void TestLoadDataRecordSetsSelectParameterDefaultValue()
        {
            _target = InitializeBuilder()
                     .WithMockedOnPageModeChanged(true)
                     .Build();

            var ds = new SqlDataSource();
            ds.SelectParameters.Add(_primaryFieldName, TypeCode.Int32, "");
            var expectedRecordId = 43;

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
            }

            using (_mocks.Playback())
            {
                _target.LoadDataRecordTest(expectedRecordId);

                var targetVal = ds.SelectParameters[_primaryFieldName].DefaultValue;
                Assert.AreEqual(targetVal, expectedRecordId.ToString());
            }
        }

        #region Events

        #region OnInit Event Listener Registration

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewModeChangedEvent()
        {
            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                _detailsView.ModeChanged += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewItemInsertingEvent()
        {
            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                _detailsView.ItemInserting += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewItemUpdatingEvent()
        {
            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                _detailsView.ItemUpdating += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewItemCommandEvent()
        {
            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                _detailsView.ItemCommand += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewDataSourceInsertedEvent()
        {
            _target = InitializeBuilder()
                     .WithMockedRequest(_mockedRequest)
                     .WithMockedResponse(_mockedResponse);

            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult
                   .For(_mockedRequest.Url)
                   .Return("http://www.google.com");
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                ds.Inserted += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestOnInitSubscribesToDetailsViewDataSourceUpdatedEvent()
        {
            var detailsDataSource = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(detailsDataSource);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                detailsDataSource.Updated += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);
            }
        }

        #endregion

        [TestMethod]
        public void TestOnInitAddsDetailsViewCrudFieldToDetailsViewFieldsProperty()
        {
            var ds = new SqlDataSource();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedResultsGridView.DataSourceObject).Return(_mockedResultsDataSource);
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(_detailsView.Fields).Return(_fields);
                _detailsView.ModeChanged += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnInit", EventArgs.Empty);

                Assert.AreEqual(1, _fields.Count);

                var f = _fields[0] as DetailsViewCrudField;
                Assert.IsNotNull(f);
            }
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceInsertingCallsSetupTransaction()
        {
            _target = InitializeBuilder()
                     .WithMockedSetupTransactionCall(true)
                     .Build();

            var args = new SqlDataSourceCommandEventArgs(null);
            InvokeEventByName(_target, "OnDetailsViewDataSourceInserting",
                _detailsView, args);

            Assert.IsTrue(_target.MockedSetupTransactionMethodCalled);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceUpdatingCallsSetupTransaction()
        {
            _target = InitializeBuilder()
                     .WithMockedSetupTransactionCall(true)
                     .Build();

            var args = new SqlDataSourceCommandEventArgs(null);
            InvokeEventByName(_target, "OnDetailsViewDataSourceUpdating",
                _detailsView, args);

            Assert.IsTrue(_target.MockedSetupTransactionMethodCalled);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceDeletingCallsSetupTransaction()
        {
            _target = InitializeBuilder()
                     .WithMockedSetupTransactionCall(true)
                     .Build();

            var args = new SqlDataSourceCommandEventArgs(null);
            InvokeEventByName(_target, "OnDetailsViewDataSourceDeleting",
                _detailsView, args);

            Assert.IsTrue(_target.MockedSetupTransactionMethodCalled);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewItemDeletingIsCallable()
        {
            // This method is only there for consistancy(since ItemInserting and ItemUpdating are both used.
            // This is so all of the available Insert/Update/Delete events are available as overrides.
            // Might be feature creep, but I'm not worried about it in this instance.

            _target = InitializeBuilder()
               .Build();

            InvokeEventByName(_target, "OnDetailsViewItemDeleting", null, null);

            Assert.IsTrue(_target.OnDetailsViewItemDeletingCalled);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceInsertedThrowsExceptionIfEventArgsHasException()
        {
            var mockException = new Exception("Mock exception");
            var args = new SqlDataSourceStatusEventArgs(null, 0, mockException);
            MyAssert.Throws(() => InvokeEventByName(_target, "OnDetailsViewDataSourceInserted", _detailsView, args));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTransactionSupport()
        {
            // Let's mock up an SqlConnection and Command.

            DbCommand com;
            DbConnection conn;
            DbTransaction tran;
            DbParameterCollection paramCol;

            _mocks.DynamicMock(out com)
                  .DynamicMock(out conn)
                  .DynamicMock(out tran)
                  .DynamicMock(out paramCol);

            _target = InitializeBuilder()
                     .WithTransaction(tran)
                     .WithMockedResponse(_mockedResponse)
                     .WithMockedRequest(_mockedRequest)
                     .Build();

            // Things required for the two events being raised.
            var ds = new SqlDataSource();
            var updatingArgs = new SqlDataSourceCommandEventArgs(com);
            var updatedArgs = new SqlDataSourceStatusEventArgs(com, 0, null);

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(com.Connection).Return(conn);
                SetupResult.For(com.Parameters).Return(paramCol);
                SetupResult.For(conn.BeginTransaction()).Return(tran);
                SetupResult.For(conn.State).Return(ConnectionState.Open);
                SetupResult.For(tran.Connection).Return(conn);
                SetupResult.For(_mockedResponse.IsClientConnected).Return(true);
                tran.Commit();
                conn.Close();
                com.Transaction = tran;
            }

            using (_mocks.Playback())
            {
                _target.PreviousDataRecordSavingArgs =
                    new DataRecordSavingEventArgs(DataRecordSaveTypes.Update, 0, null);
                // This event calls the SetupTransaction private method.
                InvokeEventByName(_target, "OnDetailsViewDataSourceUpdating", ds, updatingArgs);
                Assert.IsNotNull(_target.TransactionTest);
                Assert.AreSame(_target.TransactionTest, tran);

                // This event calls the CompleteTransaction private method.
                InvokeEventByName(_target, "OnDetailsViewDataSourceUpdated", ds, updatedArgs);
                Assert.IsNull(_target.TransactionTest);
            }
        }

        [TestMethod]
        public void TestSetupTransactionOpensConnection()
        {
            DbCommand com;
            DbConnection conn;
            DbTransaction tran;

            _mocks.DynamicMock(out com)
                  .DynamicMock(out conn)
                  .DynamicMock(out tran);

            _target = InitializeBuilder()
                     .WithTransaction(tran)
                     .Build();

            // Things required for the two events being raised.
            var ds = new SqlDataSource();
            var args = new SqlDataSourceCommandEventArgs(com);

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(com.Connection).Return(conn);
                SetupResult.For(conn.BeginTransaction()).Return(tran);
                SetupResult.For(conn.State).Return(ConnectionState.Closed);
                SetupResult.For(tran.Connection).Return(conn);
                conn.Open();
            }

            using (_mocks.Playback())
            {
                _target.SetupTransactionTest(args);
            }
        }

        [TestMethod]
        public void TestCompleteTransactionCallsRollbackOnException()
        {
            // Let's mock up an SqlConnection and Command.

            DbCommand com;
            DbConnection conn;
            DbTransaction tran;
            DbParameterCollection paramCol;

            _mocks.DynamicMock(out com)
                  .DynamicMock(out conn)
                  .DynamicMock(out tran)
                  .DynamicMock(out paramCol);

            _target = InitializeBuilder()
                     .WithTransaction(tran)
                     .WithMockedResponse(_mockedResponse)
                     .WithMockedRequest(_mockedRequest)
                     .Build();

            _target.ThrowExceptionDuringPerformExtendedDataSaving = true;

            // Things required for the two events being raised.
            var ds = new SqlDataSource();
            var updatingArgs = new SqlDataSourceCommandEventArgs(com);
            var updatedArgs = new SqlDataSourceStatusEventArgs(com, 0, null);

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
                SetupResult.For(com.Connection).Return(conn);
                SetupResult.For(com.Parameters).Return(paramCol);
                SetupResult.For(conn.BeginTransaction()).Return(tran);
                SetupResult.For(conn.State).Return(ConnectionState.Open);
                SetupResult.For(tran.Connection).Return(conn);
                SetupResult.For(_mockedResponse.IsClientConnected).Return(true);
                tran.Rollback();
                conn.Close();
                com.Transaction = tran;
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws(() => {
                    // This event calls the SetupTransaction private method.
                    InvokeEventByName(_target, "OnDetailsViewDataSourceUpdating", ds, updatingArgs);

                    // This event calls the CompleteTransaction private method.
                    InvokeEventByName(_target, "OnDetailsViewDataSourceUpdated", ds, updatedArgs);
                });
            }
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceInsertedSetsCurrentDataRecordId()
        {
            DbCommand com;
            DbConnection conn;
            DbTransaction tran;
            DbParameterCollection paramCol;

            _mocks.DynamicMock(out com)
                  .DynamicMock(out conn)
                  .DynamicMock(out tran)
                  .DynamicMock(out paramCol);

            _target = InitializeBuilder()
                     .WithMockedResponse(_mockedResponse)
                     .WithMockedRequest(_mockedRequest)
                     .WithTransaction(tran)
                     .Build();

            var expectedRecordId = 533;
            var mockCommie = new SqlCommand();
            mockCommie.Parameters.AddWithValue("@" + _primaryFieldName, expectedRecordId);

            using (_mocks.Record())
            {
                SetupResult
                   .For(_mockedRequest.Url)
                   .Return("http://www.google.com/");
                SetupResult.For(tran.Connection).Return(conn);
                _target.CurrentDataRecordId = expectedRecordId;

                foreach (var dataLink in _dataLinks)
                {
                    SetupResult.For(dataLink.DataLinkID).Return(expectedRecordId);
                }
            }

            using (_mocks.Playback())
            {
                _target.PreviousDataRecordSavingArgs =
                    new DataRecordSavingEventArgs(DataRecordSaveTypes.Insert, 0, null);
                var args = new SqlDataSourceStatusEventArgs(mockCommie, 0, null);
                InvokeEventByName(_target, "OnDetailsViewDataSourceInserted", null, args);
            }
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceUpdatedThrowsExceptionIfEventArgsHasException()
        {
            var mockException = new Exception("Mock exception");
            var args = new SqlDataSourceStatusEventArgs(null, 0, mockException);
            MyAssert.Throws(() => InvokeEventByName(_target, "OnDetailsViewDataSourceUpdated", _detailsView, args));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceDeletedThrowsExceptionIfEventArgsHasException()
        {
            var mockException = new Exception("Mock exception");
            var args = new SqlDataSourceStatusEventArgs(null, 0, mockException);
            MyAssert.Throws(() => InvokeEventByName(_target, "OnDetailsViewDataSourceDeleted", _detailsView, args));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnDetailsViewDataSourceUpdatedCallsOnRecordSaved()
        {
            DbCommand com;
            DbConnection conn;
            DbTransaction tran;
            DbParameterCollection paramCol;

            _mocks.DynamicMock(out com)
                  .DynamicMock(out conn)
                  .DynamicMock(out tran)
                  .DynamicMock(out paramCol);

            _target = InitializeBuilder()
                     .WithMockedResponse(_mockedResponse)
                     .WithMockedRequest(_mockedRequest)
                     .WithTransaction(tran)
                     .Build();

            using (_mocks.Record())
            {
                SetupResult.For(tran.Connection).Return(conn);
                SetupResult.For(com.Connection).Return(conn);
                SetupResult.For(com.Parameters).Return(paramCol);
                SetupResult.For(_mockedRequest.Url).Return(
                    "http://www.google.com/");
            }

            using (_mocks.Playback())
            {
                _target.PreviousDataRecordSavingArgs =
                    new DataRecordSavingEventArgs(DataRecordSaveTypes.Update, 253, null);
                var args = new SqlDataSourceStatusEventArgs(com, 253, null);
                InvokeEventByName(_target, "OnDetailsViewDataSourceUpdated",
                    _detailsView, args);
                Assert.IsTrue(_target.OnRecordSavedCalled);
            }
        }

        [TestMethod]
        public void TestOnDetailsViewItemCommandCancelSetsPageModeToSearchWhenPageModeRecordInsert()
        {
            _target = InitializeBuilder()
                     .WithMockedOnPageModeChanged(true)
                     .Build();

            // Setting this to a different value because PageMode
            // is set to Search by default. 
            _target.PageModeTest = PageModes.RecordInsert;

            using (_mocks.Record())
            {
                SetupResult.For(_mockedRequest.Url).Return("stuff");
                _target.PageModeTest = PageModes.Search;
            }

            using (_mocks.Playback())
            {
                var args = new DetailsViewCommandEventArgs(_detailsView,
                    new CommandEventArgs("Cancel", null));
                InvokeEventByName(_target, "OnDetailsViewItemCommand", _detailsView, args);
            }

            _mocks.VerifyAll();
        }

        //[TestMethod]
        //public void TestOnDetailsViewItemCommandCancelSetsPageModeToRecordReadOnlyWhenPageModeNotRecordInsert()
        //{
        //    Assert.Inconclusive("Fix me! I'm an invalid mock!");

        //    //var target = InitializeBuilder()
        //    //    .WithMockedOnPageModeChanged(true)
        //    //    .Build();

        //    //foreach (PageModes item in Enum.GetValues(typeof(PageModes)))
        //    //{
        //    //  //  if (item == PageModes.RecordInsert) return;
        //    //    target.PageModeTest = item;

        //    //    using (_mocks.Record())
        //    //    {
        //    //        SetupResult.For(_mockedRequest.Url).Return("stuff");
        //    //        target.PageModeTest = PageModes.RecordReadOnly;
        //    //    }

        //    //    using (_mocks.Playback())
        //    //    {
        //    //        var args = new DetailsViewCommandEventArgs(_detailsView,
        //    //            new CommandEventArgs("Cancel", null));
        //    //        InvokeEventByName(target, "OnDetailsViewItemCommand",
        //    //            _detailsView, args);
        //    //    }

        //    //    _mocks.VerifyAll();
        //    //}
        //}

        [TestMethod]
        public void TestOnDetailsViewItemInsertingCallsCleanValues()
        {
            var args = new DetailsViewInsertEventArgs("Some command");

            const string key1 = "somekey";
            const string initValue1 = "193:::BRICK TWP:::";
            const string expectedValue1 = "193";

            const string key2 = "i have too many colons";
            const string initValue2 = ":::";

            const string key3 = "non crappy ajax toolkit value";
            const string expectedValue3 = "Hoorah!";

            args.Values[key1] = initValue1;
            args.Values[key2] = initValue2;
            args.Values[key3] = expectedValue3;

            var ds = new SqlDataSource();
            ds.InsertParameters.Add("CreatedBy", null);

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.DataSourceObject).Return(ds);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDetailsViewItemInserting", _detailsView, args);
                Assert.AreEqual(expectedValue1, args.Values[key1]);
                Assert.AreEqual(null, args.Values[key2]);
                Assert.AreEqual(expectedValue3, args.Values[key3]);
            }
        }

        [TestMethod]
        public void TestOnDetailsViewItemUpdatingCallsCleanValues()
        {
            var args = new DetailsViewUpdateEventArgs("Some command");

            const string key1 = "somekey";
            const string initValue1 = "193:::BRICK TWP:::";
            const string expectedValue1 = "193";

            const string key2 = "i have too many colons";
            const string initValue2 = ":::";

            const string key3 = "non crappy ajax toolkit value";
            const string expectedValue3 = "Hoorah!";

            args.NewValues[key1] = initValue1;
            args.NewValues[key2] = initValue2;
            args.NewValues[key3] = expectedValue3;

            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDetailsViewItemUpdating", _detailsView, args);
                Assert.AreEqual(expectedValue1, args.NewValues[key1]);
                Assert.AreEqual(null, args.NewValues[key2]);
                Assert.AreEqual(expectedValue3, args.NewValues[key3]);
            }
        }

        [TestMethod]
        public void TestOnDetailsViewModeChangedEditCommandChangesPageModeToRecordUpdate()
        {
            _target = InitializeBuilder()
                     .WithMockedOnPageModeChanged(true)
                     .Build();

            using (_mocks.Record())
            {
                SetupResult.For(_detailsView.CurrentMode).Return(DetailsViewMode.Edit);
                _target.PageModeTest = PageModes.RecordUpdate;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDetailsViewModeChanged");
            }
        }

        #endregion

        #endregion
    }

    public class TestDetailsPage : DetailsViewDataPageBase
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
            get { return DataElementPrimaryFieldNameTest ?? "Default"; }
        }

        protected override IDetailsView DetailsView
        {
            get { return DetailsViewTest; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Test properties

        public IAuditor AuditorTest { get; set; }

        internal PageModes PageModeTest
        {
            get { return PageMode; }
            set { PageMode = value; }
        }

        public IModulePermissions ModulePermissionsTest
        {
            get { return ModulePermissions; }
        }

        public bool DocNotesVisibleTest
        {
            get { return DocNotesVisible; }
            set { DocNotesVisible = value; }
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

        public IDetailsView DetailsViewTest { get; set; }
        public string DataElementPrimaryFieldNameTest { get; set; }

        public DbTransaction TransactionTest
        {
            get { return Transaction; }
            set { Transaction = value; }
        }

        public bool ThrowExceptionDuringPerformExtendedDataSaving { get; set; }
        public bool UseMockedSetupTransactionMethod { get; set; }
        public bool UseMockedCompleteTransactionMethod { get; set; }

        public bool MockedSetupTransactionMethodCalled { get; set; }
        public bool MockedCompleteTransactinoMethodCalled { get; set; }
        public bool OnDetailsViewItemDeletingCalled { get; set; }

        #endregion

        #region Test methods

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
            return base.SaveControlState();
        }

        public void OnRecordSavedTest(DataRecordSavedEventArgs e)
        {
            base.OnRecordSaved(e);
        }

        public string BuildFilterExpressionTest()
        {
            return "Some filter";
        }

        public void ParseQueryStringTest()
        {
            Assert.Fail("Fix");
            //base.InitializeRoute();
        }

        private bool _detailsViewModeChangedCalled;

        protected override void OnDetailsViewModeChanged(object sender, EventArgs e)
        {
            _detailsViewModeChangedCalled = true;
            base.OnDetailsViewModeChanged(sender, e);
        }

        public bool OnDetailsViewModeChangeCalled
        {
            get { return _detailsViewModeChangedCalled; }
        }

        protected override void OnRecordSaved(DataRecordSavedEventArgs e)
        {
            _onRecordSavedCalled = true;
        }

        private bool _onRecordSavedCalled;

        public bool OnRecordSavedCalled
        {
            get { return _onRecordSavedCalled; }
        }

        public void LoadDataRecordTest(int recordId)
        {
            LoadDataRecord(recordId);
        }

        public void SetupTransactionTest(SqlDataSourceCommandEventArgs e)
        {
            base.SetupTransaction(e);
        }

        #endregion

        #region Override Methods

        protected override void OnDetailsViewItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            OnDetailsViewItemDeletingCalled = true;
            base.OnDetailsViewItemDeleting(sender, e);
        }

        protected override void SetupTransaction(SqlDataSourceCommandEventArgs e)
        {
            if (!UseMockedSetupTransactionMethod)
            {
                base.SetupTransaction(e);
            }
            else
            {
                MockedSetupTransactionMethodCalled = true;
            }
        }

        protected override void CompleteTransaction(SqlDataSourceStatusEventArgs e, DataRecordSaveTypes saveType)
        {
            if (!UseMockedCompleteTransactionMethod)
            {
                base.CompleteTransaction(e, saveType);
            }
            else
            {
                MockedCompleteTransactinoMethodCalled = true;
            }
        }

        protected override void PerformExtendedDataSaving(SqlDataSourceStatusEventArgs e,
            DataRecordSavingEventArgs savingArgs)
        {
            if (ThrowExceptionDuringPerformExtendedDataSaving)
            {
                throw new Exception("Expected test exception.");
            }

            base.PerformExtendedDataSaving(e, savingArgs);
        }

        protected override IAuditor CreateAuditor()
        {
            return this.AuditorTest;
        }

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            // Do nothing 
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
    }

    public class TestDetailsPageBuilder : TestDataBuilder<TestDetailsPage>
    {
        #region Private Members

        private IUser _user;
        private IGridView _resultsGridView;
        private IPanel _detailPanel;
        private IPanel _searchPanel;
        private IPanel _resultPanel;
        private IControl _homePanel;

        private IButton _addNewRecordButton,
                        _backToSearchButton,
                        _backToResultsButton,
                        _exportButton,
                        _searchButton,
                        _resetSearchButton;

        private bool _useMockedOnPageModeChanged;
        private IResponse _response;
        private IRequest _request;
        private IEnumerable<IDataLink> _dataLinks;
        private IRoleBasedDataPagePermissions _permissions;
        private IAuditor _auditor;
        private IDetailsView _detailsView;
        private string _primaryFieldName;
        private DbTransaction _transaction;
        private bool _withMockedSetupTransaction;
        private bool _withMockedCompleteTransaction;

        #endregion

        #region Exposed Methods

        public TestDetailsPageBuilder WithHomePanel(IControl homePanel)
        {
            _homePanel = homePanel;
            return this;
        }

        public TestDetailsPageBuilder WithMockedSetupTransactionCall(bool val)
        {
            _withMockedSetupTransaction = val;
            return this;
        }

        public TestDetailsPageBuilder WithMockedCompleteTransactionCall(bool val)
        {
            _withMockedCompleteTransaction = val;
            return this;
        }

        public TestDetailsPageBuilder WithDataElementPrimaryFieldName(string name)
        {
            _primaryFieldName = name;
            return this;
        }

        public TestDetailsPageBuilder WithDetailsView(IDetailsView details)
        {
            _detailsView = details;
            return this;
        }

        public TestDetailsPageBuilder WithAuditor(IAuditor auditor)
        {
            _auditor = auditor;
            return this;
        }

        public TestDetailsPageBuilder WithPermissions(IRoleBasedDataPagePermissions perms)
        {
            _permissions = perms;
            return this;
        }

        public TestDetailsPageBuilder WithIUser(IUser user)
        {
            _user = user;
            return this;
        }

        public TestDetailsPageBuilder WithAddNewRecordButton(IButton addBtn)
        {
            _addNewRecordButton = addBtn;
            return this;
        }

        public TestDetailsPageBuilder WithBackToSearchButton(IButton sb)
        {
            _backToSearchButton = sb;
            return this;
        }

        public TestDetailsPageBuilder WithBackToResultsButton(IButton btrb)
        {
            _backToResultsButton = btrb;
            return this;
        }

        public TestDetailsPageBuilder WithExportButton(IButton expBtn)
        {
            _exportButton = expBtn;
            return this;
        }

        public TestDetailsPageBuilder WithSearchButton(IButton search)
        {
            _searchButton = search;
            return this;
        }

        public TestDetailsPageBuilder WithResetSearchButton(IButton resetbtn)
        {
            _resetSearchButton = resetbtn;
            return this;
        }

        public TestDetailsPageBuilder WithResultsGridView(IGridView igv)
        {
            _resultsGridView = igv;
            return this;
        }

        public TestDetailsPageBuilder WithDetailPanel(IPanel dp)
        {
            _detailPanel = dp;
            return this;
        }

        public TestDetailsPageBuilder WithResultsPanel(IPanel rp)
        {
            _resultPanel = rp;
            return this;
        }

        public TestDetailsPageBuilder WithSearchPanel(IPanel sp)
        {
            _searchPanel = sp;
            return this;
        }

        public TestDetailsPageBuilder WithMockedOnPageModeChanged(bool mock)
        {
            _useMockedOnPageModeChanged = mock;
            return this;
        }

        public TestDetailsPageBuilder WithMockedResponse(IResponse resp)
        {
            _response = resp;
            return this;
        }

        public TestDetailsPageBuilder WithMockedRequest(IRequest req)
        {
            _request = req;
            return this;
        }

        public TestDetailsPageBuilder WithDataLinkControls(IEnumerable<IDataLink> controls)
        {
            _dataLinks = controls;
            return this;
        }

        public TestDetailsPageBuilder WithTransaction(DbTransaction transaction)
        {
            _transaction = transaction;
            return this;
        }

        public override TestDetailsPage Build()
        {
            var woot = new TestDetailsPage();

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

            woot.ResultsGridViewTest = _resultsGridView;
            woot.HomePanelTest = _homePanel;
            woot.DetailPanelTest = _detailPanel;
            woot.ResultsPanelTest = _resultPanel;
            woot.SearchPanelTest = _searchPanel;
            woot.AddNewRecordButtonTest = _addNewRecordButton;
            woot.ResetSearchButtonTest = _resetSearchButton;
            woot.SearchButtonTest = _searchButton;
            woot.ExportButtonTest = _exportButton;
            woot.BackToResultsButtonTest = _backToResultsButton;
            woot.BackToSearchButtonTest = _backToSearchButton;
            woot.UseMockedOnPageModeChanged = _useMockedOnPageModeChanged;
            woot.DataLinkControlsTest = _dataLinks;
            woot.PermissionsTest = _permissions;
            woot.AuditorTest = _auditor;
            woot.DetailsViewTest = _detailsView;
            woot.DataElementPrimaryFieldNameTest = _primaryFieldName;
            woot.TransactionTest = _transaction;
            woot.UseMockedCompleteTransactionMethod = _withMockedCompleteTransaction;
            woot.UseMockedSetupTransactionMethod = _withMockedSetupTransaction;
            return woot;
        }

        #endregion
    }
}
