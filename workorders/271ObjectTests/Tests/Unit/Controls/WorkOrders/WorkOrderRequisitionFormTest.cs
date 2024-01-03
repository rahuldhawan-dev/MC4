using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    [TestClass]
    public class WorkOrderRequisitionFormTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl; // this is needed?
        private IViewState _viewState;
        private IUpdatePanel _updatePanel;
        private IGridView _gvRequisitions;
        private IObjectDataSource _odsRequisitions;
        private ILinkButton _editLink, _deleteLink;
        private ILabel _lblRequisitionError;

        private ITextBox _txtSAPRequisitionNumber;
        private IDropDownList _ddlRequisitionType;
        private ISecurityService _securityService;

        private IGridViewRow _iFooterRow, _iEditRow;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;

        private TestWorkOrderRequisitionForm _target;
        private ParameterCollection _odsRequisitionParameters;

        #endregion
        
        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            MockUsing(_mocks.DynamicMock);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void MockUsing(Func<Type, object[], object> mock)
        {
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _lblRequisitionError)
                .DynamicMock(out _updatePanel)
                .DynamicMock(out _txtSAPRequisitionNumber)
                .DynamicMock(out _ddlRequisitionType)
                .DynamicMock(out _gvRequisitions)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _viewState)
                .DynamicMock(out _odsRequisitions)
                .DynamicMock(out _editLink)
                .DynamicMock(out _deleteLink)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _securityService);

            _odsRequisitionParameters = new ParameterCollection();
            SetupResult.For(_odsRequisitions.SelectParameters)
                .Return(_odsRequisitionParameters);

            _target = new TestWorkOrderRequisitionFormBuilder()
                .WithObjectDataSource(_odsRequisitions)
                .WithUpdatePanel(_updatePanel)
                .WithViewState(_viewState)
                .WithGridView(_gvRequisitions)
                .WithSecurityService(_securityService);

            SetupResult.For(_detailControl.FindIControl<IUpdatePanel>(WorkOrderRequisitionForm.ControlIDs.UPDATE_PANEL)).Return(_updatePanel);
            SetupResult.For(_updatePanel.FindIControl<ILabel>(WorkOrderRequisitionForm.ControlIDs.REQUISITION_ERROR_LABEL)).Return(_lblRequisitionError);

            SetupGridView();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;
            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_gvRequisitions.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_gvRequisitions.IRows).Return(_rowCollection);
            SetupResult.For(_rowCollection.GetEnumerator()).Return(_rowCollectionEnum);

            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderRequisitionForm.ControlIDs.ROW_DELETE_LINKBUTTON)).Return(_deleteLink);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderRequisitionForm.ControlIDs.ROW_EDIT_LINKBUTTON)).Return(_editLink);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderRequisitionForm.ControlIDs.SAP_REQUISITION_NUMBER_TEXTBOX)).Return(_txtSAPRequisitionNumber);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderRequisitionForm.ControlIDs.REQUISITION_TYPE_DROPDOWNLIST)).Return(_ddlRequisitionType);

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

        #endregion

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowWhenCurrentMvpModeIsReadOnly()
        {
            using(_mocks.Record())
            {
                SetupResult.For(
                                _viewState.GetValue(
                                                    WorkOrderDetailControlBase.
                                                        ViewStateKeys.
                                                        CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);
                _iFooterRow.Visible = false;
            }
            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderHidesEditControlsWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_viewState.GetValue(
                    WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                .Return(DetailViewMode.ReadOnly);
                _editLink.Visible = false;
                _deleteLink.Visible = false;
            }
            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #region Control Events

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSource()
        {
            using(_mocks.Record())
            {
                SetupResult.For(_odsRequisitions.Insert()).Return(1);
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }
        
        #endregion
    }

    internal class TestWorkOrderRequisitionFormBuilder : TestDataBuilder<TestWorkOrderRequisitionForm>
    {
        #region Private Members

        private IViewState _viewState;
        private IUpdatePanel _updatePanel;
        private IGridView _gvRequisitions;
        private IObjectDataSource _odsRequisitions;

        private ITextBox _txtSAPRequisitionNumber;
        private IDropDownList _ddlRequisitionType;
        private ISecurityService _securityService;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderRequisitionForm Build()
        {
            var obj = new TestWorkOrderRequisitionForm();
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_updatePanel != null)
                obj.SetUpdatePanel(_updatePanel);
            if (_gvRequisitions != null)
                obj.SetGridView(_gvRequisitions);
            if (_odsRequisitions != null)
                obj.SetODSRequisitions(_odsRequisitions);
            if (_txtSAPRequisitionNumber != null)
                obj.SetTxtRequisitionNumber(_txtSAPRequisitionNumber);
            if (_ddlRequisitionType != null)
                obj.SetDdlRequisitioNType(_ddlRequisitionType);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrderRequisitionFormBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithUpdatePanel(IUpdatePanel up)
        {
            _updatePanel = up;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithGridView(IGridView gv)
        {
            _gvRequisitions = gv;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithObjectDataSource(IObjectDataSource ds)
        {
            _odsRequisitions = ds;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithTxtRequisitionNumber(ITextBox tb)
        {
            _txtSAPRequisitionNumber = tb;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithddlRequisitionType(IDropDownList ddl)
        {
            _ddlRequisitionType = ddl;
            return this;
        }

        public TestWorkOrderRequisitionFormBuilder WithSecurityService(ISecurityService ss)
        {
            _securityService = ss;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderRequisitionForm : WorkOrderRequisitionForm
    {
        public void SetODSRequisitions(IObjectDataSource requisitions)
        {
            odsRequisitions = requisitions;
        }

        public void SetUpdatePanel(IUpdatePanel up)
        {
            upRequisitions = up;
        }

        public void SetGridView(IGridView gv)
        {
            gvRequisitions = gv;
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public void SetViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetTxtRequisitionNumber(ITextBox tb)
        {
            _txtSAPRequisitionNumber = tb;
        }

        public void SetDdlRequisitioNType(IDropDownList ddl)
        {
            _ddlRequisitionType = ddl;
        }

    }
}