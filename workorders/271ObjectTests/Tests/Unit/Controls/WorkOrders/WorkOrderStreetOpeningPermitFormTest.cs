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
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderStreetOpeningPermitFormTest
    /// </summary>
    [TestClass]
    public class WorkOrderStreetOpeningPermitFormTest : EventFiringTestClass
    {
        #region Private Members
        
        private IPage _iPage;
        private IViewState _viewState;
        private IGridView _gvStreetOpeningPermits;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;
        private IGridViewRow _iFooterRow, _iEditRow;
        private IObjectDataSource _odsStreetOpeningPermits;
        private ILinkButton _editLink, _deleteLink, _permitLink;
        private ICheckBox _iChkIsPaidFor;
        private ILabel _lblPermitId;
        private IButton _btnCreatePermit;
        private IResponse _iResponse;

        private ITextBox _txtDateIssued,
                         _txtExpirationDate,
                         _txtNotes,
                         _txtDateRequested,
                         _txtStreetOpeningPermitNumber;
        
        private ParameterCollection _odsStreetOpeningPermitParameters;
        private TestWorkOrderStreetOpeningPermitForm _target;

        #endregion
        
        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _odsStreetOpeningPermits)
                .DynamicMock(out _viewState)
                .DynamicMock(out _gvStreetOpeningPermits)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _editLink)
                .DynamicMock(out _deleteLink)
                .DynamicMock(out _iPage)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _txtDateIssued)
                .DynamicMock(out _txtExpirationDate)
                .DynamicMock(out _txtNotes)
                .DynamicMock(out _txtDateRequested)
                .DynamicMock(out _txtStreetOpeningPermitNumber)
                .DynamicMock(out _btnCreatePermit)
                .DynamicMock(out _iResponse)
                .DynamicMock(out _iChkIsPaidFor)
                .DynamicMock(out _permitLink)
                .DynamicMock(out _lblPermitId);

            _odsStreetOpeningPermitParameters = new ParameterCollection();
            SetupResult.For(_odsStreetOpeningPermits.SelectParameters).Return(
                _odsStreetOpeningPermitParameters);

            _target = new TestWorkOrderStreetOpeningPermitFormBuilder()
                .WithODSStreetOpeningPermit(_odsStreetOpeningPermits)
                .WithGVStreetOpeningPermit(_gvStreetOpeningPermits)
                .WithViewState(_viewState)
                .WithIPage(_iPage)
                .WithBtnCreatePermitButton(_btnCreatePermit)
                .WithIResponse(_iResponse);

            SetupGridView();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;
            SetupResult.For(_gvStreetOpeningPermits.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_gvStreetOpeningPermits.IRows).Return(_rowCollection);
            SetupResult.For(_rowCollection.GetEnumerator()).Return(_rowCollectionEnum);

            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderStreetOpeningPermitForm.ControlIDs.DELETE_LINK)).Return(_deleteLink);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderStreetOpeningPermitForm.ControlIDs.EDIT_LINK)).Return(_editLink);
            SetupResult.For(_iEditRow.FindIControl<ILabel>(WorkOrderStreetOpeningPermitForm.ControlIDs.PERMIT_ID)).Return(_lblPermitId);
            SetupResult.For(_iEditRow.FindIControl<ICheckBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.IS_PAID_FOR)).Return(_iChkIsPaidFor);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderStreetOpeningPermitForm.ControlIDs.PERMIT_LINK)).Return(_permitLink);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.EXPIRATION_DATE)).Return(_txtExpirationDate);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.DATE_ISSUED)).Return(_txtDateIssued);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.NOTES)).Return(_txtNotes);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.DATE_REQUESTED)).Return(_txtDateRequested);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderStreetOpeningPermitForm.ControlIDs.STREETOPENINGPERMITNUMBER)).Return(_txtStreetOpeningPermitNumber);
            
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

        #region Property Tests

        [TestMethod]
        public void TestSettingWorkOrderIDSetsSelectParameterForDataSource()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            _odsStreetOpeningPermitParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }
        
        [TestMethod]
        public void TestDateIssuedPropertyReturnsStringValueOfDateIssuedField()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_txtDateIssued.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateIssued);
            }
        }

        [TestMethod]
        public void TestExpirationDatePropertyReturnsStringValueOfExpirationDateField()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_txtExpirationDate.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.ExpirationDate);
            }
        }

        [TestMethod]
        public void TestNotesPropertyReturnsStringValueOfNotesField()
        {
            var expected = "OMG SOME NOTES LOL!";

            using (_mocks.Record())
            {
                SetupResult.For(_txtNotes.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Notes);
            }
        }

        [TestMethod]
        public void TestDateRequestedPropertyReturnsStringValueOfDateRequestedField()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_txtDateRequested.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateRequested);
            }
        }

        [TestMethod]
        public void TestStreetOpeningPermitNumberPropertyReturnsStringValueOfStreetOpeningPermitNumberField()
        {
            var expected = "SOP NUMBAh";

            using (_mocks.Record())
            {
                SetupResult.For(_txtStreetOpeningPermitNumber.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetOpeningPermitNumber);
            }
        }

       #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowAndCreatePermitWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);
                
                _iFooterRow.Visible = false;
                _btnCreatePermit.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderHidesEditControlsWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);
                
                _editLink.Visible = false;
                _deleteLink.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderDoesNotHideInsertRowWhenCurrentMvpModeIsNotReadOnly()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.ReadOnly) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);
                    _btnCreatePermit.Visible = true;
                    _iFooterRow.Visible = true;
                }

                using (_mocks.Playback())
                {
                    InvokeEventByName(_target, "Page_Prerender");
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
                SetupGridView();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPagePrerenderDisplaysLinkWhenPermitIdAvailableAndHasNotBeenPaidFor()
        {
            var permitId = "1";
            var workOrderID = 808;
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.Edit);
                SetupResult.For(_lblPermitId.Text).Return(permitId);
                SetupResult.For(_iChkIsPaidFor.Checked).Return(false);
                _permitLink.Visible = true;
                _permitLink.PostBackUrl =
                    string.Format(
                        WorkOrderStreetOpeningPermitForm.CREATE_PERMIT_FORMAT_URL_WITH_ID,
                        workOrderID, 
                        permitId);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #endregion

        #region Control Events

        [TestMethod]
        public void TestODSStreetOpeningPermitInsertingSetsInputParameters()
        {
            int expectedWorkOrderID = 12;
            string expectedSOPNumber = "123",
                   expectedDateRequested = "5/18/1980",
                   expectedDateIssued = "5/19/1980",
                   expectedExpirationDate = "5/25/1980",
                   expectedNotes = "xpected nerts.";
                    
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expectedWorkOrderID);
                SetupResult.For(_txtStreetOpeningPermitNumber.Text).Return(
                    expectedSOPNumber);
                SetupResult.For(_txtDateRequested.Text).Return(
                    expectedDateRequested);
                SetupResult.For(_txtDateIssued.Text).Return(expectedDateIssued);
                SetupResult.For(_txtExpirationDate.Text).Return(expectedExpirationDate);
                SetupResult.For(_txtNotes.Text).Return(expectedNotes);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsStreetOpeningPermits_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedWorkOrderID,
                    args.InputParameters[
                        WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.
                            WORK_ORDER_ID]);
                Assert.AreEqual(expectedSOPNumber,
                    args.InputParameters[
                        WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.STREETOPENINGPERMIT_NUMBER]);
                Assert.AreEqual(expectedDateRequested,
                    args.InputParameters[
                        WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.DATE_REQUESTED]);
                Assert.AreEqual(expectedDateIssued,
                    args.InputParameters[WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.DATE_ISSUED]);
                Assert.AreEqual(expectedExpirationDate,
                    args.InputParameters[
                        WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.EXPIRATION_DATE]);
                Assert.AreEqual(expectedNotes,
                    args.InputParameters[
                        WorkOrderStreetOpeningPermitForm.StreetOpeningPermitsParameterNames.NOTES]);
            }
        }

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSourceWhenPageIsValid()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IsValid).Return(true);
                SetupResult.For(_odsStreetOpeningPermits.Insert()).Return(1);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbSOPInsert_Click");
            }
        }

        [TestMethod]
        public void TestCreatePermitClickRedirectsToCreatePermitUrl()
        {
            var expectedWorkOrderID = 108;
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expectedWorkOrderID);
                _iResponse.Redirect(String.Format(
                    WorkOrderStreetOpeningPermitForm.CREATE_PERMIT_FORMAT_URL,
                    expectedWorkOrderID));
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnCreatePermit_Click");
            }
        }

        #endregion
    }

    internal class TestWorkOrderStreetOpeningPermitFormBuilder : TestDataBuilder<TestWorkOrderStreetOpeningPermitForm>
    {
        #region Private Members

        private IPage _iPage;
        private IGridView _gvStreetOpeningPermit;
        private IViewState _viewState;
        private IObjectDataSource _odsStreetOpeningPermit;
        private IButton _btnCreatePermit;
        private IResponse _response;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStreetOpeningPermitForm Build()
        {
            var obj = new TestWorkOrderStreetOpeningPermitForm();
            if (_odsStreetOpeningPermit != null)
                obj.SetODSStreetOpeningPermit(_odsStreetOpeningPermit);
            if (_gvStreetOpeningPermit != null)
                obj.SetGVStreetOpeningPermit(_gvStreetOpeningPermit);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_btnCreatePermit != null)
                obj.SetBtnCreatePermit(_btnCreatePermit);
            if (_response != null)
                obj.SetResponse(_response);
            return obj;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithODSStreetOpeningPermit(IObjectDataSource ods)
        {
            _odsStreetOpeningPermit = ods;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithGVStreetOpeningPermit(IGridView gridView)
        {
            _gvStreetOpeningPermit = gridView;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithViewState(IViewState viewState)
        {
            _viewState = viewState;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithIPage(IPage iPage)
        {
            _iPage = iPage;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithBtnCreatePermitButton(IButton button)
        {
            _btnCreatePermit = button;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitFormBuilder WithIResponse(IResponse response)
        {
            _response = response;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStreetOpeningPermitForm : WorkOrderStreetOpeningPermitForm
    {
        #region Exposed Methods

        public void SetODSStreetOpeningPermit(IObjectDataSource ds)
        {
            odsStreetOpeningPermits = ds;
        }

        public void SetGVStreetOpeningPermit(IGridView gv)
        {
            gvStreetOpeningPermits = gv;
        }

        public void SetBtnCreatePermit(IButton button)
        {
            btnCreatePermit = button;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetIPage(IPage iPage)
        {
            _iPage = iPage;
        }
        
        public void SetResponse(IResponse response)
        {
            _iResponse = response;
        }

        #endregion
    }
}
