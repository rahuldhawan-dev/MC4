using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Exceptions;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    [TestClass]
    public class WorkOrderDocumentsFormTest : EventFiringTestClass
    {
        #region Private Members
        
        private TestWorkOrderDocumentsForm _target;
        private IDocumentRepository _documentRepository;
        private IDocumentDataRepository _documentDataRepository;
        private IResponse _iResponse;
        private IServer _iServer;
        private IObjectDataSource _odsDocuments;
        private IViewState _iViewState;
        private IButton _btnToggleDetail;
        private IPage _iPage;
        private IMasterPage _iMasterPage;
        private ParameterCollection _odsDocumentsUsedParameters;
        private ISecurityService _securityService;
        private ISessionState _iSession;
        private IDetailControl _iDetailControl;
        private IPanel _pnlDetailsView;

        //GridView Setup
        private IGridView _gvDocuments;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;
        private IGridViewRow _iFooterRow, _iEditRow, _gridViewRow;
        private ILinkButton _editLink;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks.DynamicMock(out _documentRepository)
                .DynamicMock(out _documentDataRepository)
                .DynamicMock(out _iResponse)
                .DynamicMock(out _iServer)
                .DynamicMock(out _odsDocuments)
                .DynamicMock(out _gridViewRow)
                .DynamicMock(out _iViewState)
                .DynamicMock(out _btnToggleDetail)
                .DynamicMock(out _gvDocuments)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _editLink)
                .DynamicMock(out _iPage)
                .DynamicMock(out _iMasterPage)
                .DynamicMock(out _securityService)
                .DynamicMock(out _iSession)
                .DynamicMock(out _iDetailControl)
                .DynamicMock(out _pnlDetailsView);

            _odsDocumentsUsedParameters = new ParameterCollection();
            SetupResult.For(_odsDocuments.SelectParameters).Return(
                _odsDocumentsUsedParameters);
            SetupResult.For(_iPage.IMaster).Return(_iMasterPage);

            _target =
                new TestWorkOrderDocumentsFormBuilder()
                    .WithDocumentRepository(_documentRepository)
                    .WithDocumentDataRepository(_documentDataRepository)
                    .WithResponse(_iResponse)
                    .WithServer(_iServer)
                    .WithODSDocuments(_odsDocuments)
                    .WithViewState(_iViewState)
                    .WithToggleDetailButton(_btnToggleDetail)
                    .WithGvDocuments(_gvDocuments)
                    .WithIPage(_iPage)
                    .WithSecurityService(_securityService)
                    .WithISessionState(_iSession)
                    .WithIDetailControl(_iDetailControl)
                    .WithPnlDetailsView(_pnlDetailsView);

            SetupGridView();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;
            SetupResult.For(_gvDocuments.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_gvDocuments.IRows).Return(_rowCollection);
            SetupResult.For(_rowCollection.GetEnumerator()).Return(_rowCollectionEnum);

            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderDocumentsForm.ControlIDs.EDIT_LINK)).Return(_editLink);

            var moveNextCalled = false;
            SetupResult.For(_rowCollectionEnum.MoveNext()).Do((Func<bool>)delegate
            {
                if (!moveNextCalled)
                {
                    moveNextCalled = true;
                    return true;
                }
                return false;
            });
            SetupResult.For(_rowCollectionEnum.Current).Return(_iEditRow);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestDocumentRepositoryPropertyReturnsMockedRepositoryInstance()
        {
            Assert.AreSame(_documentRepository, _target.DocumentRepository);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSettingWorkOrderIDSetsSelectParameterForDataSource()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            _odsDocumentsUsedParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }

        [TestMethod]
        public void TestSettingDocumentDataIdSetsValueInSession()
        {
            var dataId = 1393;

            using(_mocks.Record())
            {
                _iSession[WorkOrderDocumentsForm.SessionParameters.DOCUMENT_DATA_ID] = dataId;
            }

            using(_mocks.Playback())
            {
                _target.DocumentDataId = dataId;
            }
        }

        [TestMethod]
        public void TestGettingDocumentDataIdGetsValueFromSession()
        {
            var dataId = 1;

            using (_mocks.Record())
            {
                SetupResult
                    .For(
                    _iSession[
                        WorkOrderDocumentsForm.SessionParameters.DOCUMENT_DATA_ID])
                    .Return(dataId);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(dataId, _target.DocumentDataId);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestGvDocumentsRegistersLinkButtonsForPostBack()
        {
            _mocks.ReplayAll();
            Assert.Inconclusive("This test needs to be rethought");
            //_mocks.ReplayAll();

            //var lbID = "lbView";
            //var smID = "smMain";
            //var lb = new MvpLinkButton {
            //    ID = lbID
            //};
            //var scriptManagerMock = new Mock<ScriptManager>();
            //var gridViewRowMock = new Mock<IGridViewRow>();
            //var pageMock = new Mock<IPage>();
            //var masterPageMock = new Mock<IMasterPage>();

            //_target = new TestWorkOrderDocumentsFormBuilder()
            //    .WithIPage(pageMock.Object);

            //var gvr = new MockGridViewRow();
            //gvr.AddMockedControl(lbID, lb);
            //var args = new GridViewRowEventArgs(gvr);

            //pageMock.SetupGet(o => o.IMaster).Returns(masterPageMock.Object);
            //scriptManagerMock.SetupGet(o => o.ID).Returns(smID);
            //gridViewRowMock
            //    .Setup(o => o.FindIControl<MvpLinkButton>(lbID))
            //    .Returns(lb);
            //masterPageMock.Setup(o => o.FindControl(smID))
            //    .Returns(scriptManagerMock.Object);
            //scriptManagerMock.Setup(o => o.RegisterAsyncPostBackControl(lb));

            //InvokeEventByName(_target, "gvDocuments_OnRowCreated",
            //    new object[] {
            //        null, args
            //    });

            //pageMock.VerifyAll();
            //scriptManagerMock.VerifyAll();
            //gridViewRowMock.VerifyAll();
            //masterPageMock.VerifyAll();
        }

        [TestMethod]
        public void TestODSDocumentsUpdatingThrowsExceptionWhenUserIsNotASupervisor()
        {
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(false);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<DomainLogicException>(()=> 
                    InvokeEventByName(_target, "odsDocuments_Updating",
                        new object[] {
                            null, args
                        })
                );
            }
        }

        [TestMethod]
        public void TestODSDocumentsUpdatingSetsInputParameters()
        {
            var employeeID = 1;
            var dateTime = DateTime.Now;
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using(_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
                SetupResult.For(_securityService.GetEmployeeID()).Return(
                    employeeID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsDocuments_Updating",
                    new object[] {
                        null, args
                    });
            }

            Assert.AreEqual(
                args.InputParameters[
                    WorkOrderDocumentsForm.DocumentParameterNames.
                        MODIFIED_BY_ID
                    ].ToString(), employeeID.ToString());
            MyAssert.AreClose(
                DateTime.Parse(
                    args.InputParameters[
                        WorkOrderDocumentsForm.DocumentParameterNames.
                            MODIFIED_ON].ToString()), dateTime);
        }

        [TestMethod]
        public void TestAfuDocumentUploadedCompleteSavesDocumentDataAndSetsDocumentDataId()
        {
            var afu = _mocks.DynamicMock<IAsyncFileUpload>();
            var fileBytes = new byte[0];
            var docData = new DocumentData() {
                Id = 93
            };
            
            var args = new AsyncFileUploadEventArgs(AsyncFileUploadState.Unknown, null, null, null);

            using(_mocks.Record())
            {
                SetupResult.For(
                    _iDetailControl.FindIControl<IAsyncFileUpload>(
                    WorkOrderDocumentsForm.ControlIDs.ASYNC_FILE_UPLOAD)).
                    Return(afu);
                SetupResult.For(afu.FileBytes).Return(fileBytes);
                SetupResult.For(
                    _documentDataRepository.SaveOrGetExisting(fileBytes))
                    .Return(docData);

                _iSession[WorkOrderDocumentsForm.SessionParameters.DOCUMENT_DATA_ID] =
                    docData.Id;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "afuDocument_UploadedComplete",
                    new object[] {
                        null, args
                    });
            }
        }

        [TestMethod]
        public void TestOdsInsertedSetsDocumentValuesAndInsertsDocument()
        {
            var doc = new Document();
            var args = new ObjectContainerDataSourceStatusEventArgs(doc, 0);
            int employeeID = 42;
            int workOrderID = 4;
            var docDataId = 1984;
            var docData = new DocumentData();

            using(_mocks.Record())
            {
                SetupResult.For(_securityService.GetEmployeeID()).Return(employeeID);
                SetupResult
                    .For(
                    _iSession[
                        WorkOrderDocumentsForm.SessionParameters.DOCUMENT_DATA_ID])
                    .Return(docDataId);
                SetupResult.For(
                    _iViewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult.For(_documentDataRepository.Get(docDataId))
                    .Return(docData);
                
                _documentRepository.InsertDocumentForWorkOrder(doc, workOrderID);
                _gvDocuments.DataBind();

                _iSession.Remove(WorkOrderDocumentsForm.SessionParameters.DOCUMENT_DATA_ID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ods_Inserted",
                    new object[] {
                        null, args
                    });

                Assert.AreSame(docData, doc.DocumentData);
            }


        }

        [TestMethod]
        public void TestBtnToggleDetailClick()
        {
            var e = new EventArgs();
            IStyle iStyle;
            _mocks.DynamicMock(out iStyle);

            using (_mocks.Record())
            {
                SetupResult.For(_pnlDetailsView.IStyle).Return(iStyle);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnToggleDetail_Click",
                    new object[] {
                        null, e
                    });
            }

        }

        #endregion
    }

    internal class TestWorkOrderDocumentsFormBuilder : TestDataBuilder<TestWorkOrderDocumentsForm>
    {
        #region Private Members

        private IDocumentRepository _documentRepository;
        private IDocumentDataRepository _documentDataRepository;
        private IResponse _iResponse;
        private IServer _iServer;
        private IViewState _iViewState;
        private IButton _btnToggleDetail;
        private IObjectDataSource _odsDocuments;
        private IGridView _gvDocuments;
        private IPage _iPage;
        private ISecurityService _securityService;
        private ISessionState _iSession;
        private IDetailControl _iDetailControl;
        private IPanel _pnlDetailsView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderDocumentsForm Build()
        {
            var obj = new TestWorkOrderDocumentsForm();
            if (_documentRepository != null)
                obj.SetDocumentRepository(_documentRepository);
            if (_documentDataRepository != null)
                obj.SetDocumentDataRepository(_documentDataRepository);
            if (_iResponse != null)
                obj.SetIResponse(_iResponse);
            if (_iServer != null)
                obj.SetIServer(_iServer);
            if (_odsDocuments != null)
                obj.SetODSDocuments(_odsDocuments);
            if (_iViewState != null)
                obj.SetIViewState(_iViewState);
            if (_btnToggleDetail != null)
                obj.SetToggleDetailButton(_btnToggleDetail);
            if (_gvDocuments != null)
                obj.SetGvDocuments(_gvDocuments);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_iSession != null)
                obj.SetISessionState(_iSession);
            if (_iDetailControl != null)
                obj.SetIDetailControl(_iDetailControl);
            if (_pnlDetailsView != null)
                obj.SetPnlDetailsView(_pnlDetailsView);

            return obj;
        }

        public TestWorkOrderDocumentsFormBuilder WithDocumentRepository(IDocumentRepository repository)
        {
            _documentRepository = repository;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithDocumentDataRepository(IDocumentDataRepository repository)
        {
            _documentDataRepository = repository;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithResponse(IResponse iResponse)
        {
            _iResponse = iResponse;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithGvDocuments(IGridView gridView)
        {
            _gvDocuments = gridView;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithServer(IServer iServer)
        {
            _iServer = iServer;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithToggleDetailButton(IButton btnToggleDetail)
        {
            _btnToggleDetail = btnToggleDetail;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithViewState(IViewState iViewState)
        {
            _iViewState = iViewState;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithODSDocuments(IObjectDataSource documents)
        {
            _odsDocuments = documents;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithIPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithISessionState(ISessionState session)
        {
            _iSession = session;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithIDetailControl(IDetailControl control)
        {
            _iDetailControl = control;
            return this;
        }

        public TestWorkOrderDocumentsFormBuilder WithPnlDetailsView(IPanel panel)
        {
            _pnlDetailsView = panel;
            return this;
        }
        #endregion

    }

    internal class TestWorkOrderDocumentsForm : WorkOrderDocumentsForm
    {
       #region Exposed Methods

        public void SetDocumentRepository(IDocumentRepository repository)
        {
            _documentRepository = repository;
        }

        public void SetDocumentDataRepository(IDocumentDataRepository repository)
        {
            _documentDataRepository = repository;
        }

        public void SetIResponse(IResponse iResponse)
        {
            _iResponse = iResponse;
        }

        public void SetIServer(IServer iServer)
        {
            _iServer = iServer;
        }

        public void SetToggleDetailButton(IButton button)
        {
            btnToggleDetail = button;
        }

        public void SetIViewState(IViewState iViewState)
        {
            _iViewState = iViewState;
        }

        public void SetODSDocuments(IObjectDataSource documents)
        {
            odsDocuments = documents;
        }

        public void SetGvDocuments(IGridView gridView)
        {
            gvDocuments = gridView;
        }

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        public void SetISessionState(ISessionState session)
        {
            _iSession = session;
        }

        public void SetIDetailControl(IDetailControl control)
        {
            dvDocument = control;
        }

        public void SetPnlDetailsView(IPanel panel)
        {
            pnlDetailsView = panel;
        }
    #endregion
    }

    internal class MockGridViewRow : GridViewRow, IGridViewRow
    {
        #region Private Members

        private readonly Dictionary<String, IControl> _controlDictionary;

        #endregion

        #region Constructors

        public MockGridViewRow() : this(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
        {
        }

        public MockGridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState) : base(rowIndex, dataItemIndex, rowType, rowState)
        {
            _controlDictionary = new Dictionary<string, IControl>();
        }

        #endregion

        #region Implementation of IControl
        
        public override Control FindControl(string id)
        {
            return (Control)_controlDictionary[id];
        }
        
        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return (TIControl)_controlDictionary[id];
        }

        #endregion

        #region Exposed Methods

        public void AddMockedControl(string id, IControl ctrl)
        {
            _controlDictionary.Add(id, ctrl);
        }

        #endregion
    }
}