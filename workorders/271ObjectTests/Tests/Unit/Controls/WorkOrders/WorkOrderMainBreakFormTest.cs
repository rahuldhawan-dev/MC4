using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;
using _271ObjectTests.Tests.Unit.Views.WorkOrders.General;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderMainBreakFormTest
    /// </summary>
    [TestClass]
    public class WorkOrderMainBreakFormTest : EventFiringTestClass
    {
        #region Private Members

        private IPage _iPage;
        private IViewState _viewState;
        private IGridView _gvMainBreak;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;
        private IGridViewRow _iFooterRow, _iEditRow;
        private IObjectDataSource _odsMainBreak;
        private ILinkButton _editLink, _deleteLink;

        private IDropDownList _ddlMaterial,
                              _ddlMainCondition,
                              _ddlFailureType,
                              _ddlSoilCondition,
                              _ddlDisinfectionMethod,
                              _ddlFlushMethod,
                              _ddlServiceSize,
                              _ddlReplacedWith;

        private ITextBox _txtDepth,
                         _txtCustomersAffected,
                         _txtShutdownTime,
                         _txtChlorineResidual, 
                         _txtFootageReplaced;

        private ICheckBox _cbBoilAlertIssued;

        private ILabel _lblWorkDescriptionID;

        private IRequiredFieldValidator _rfvFootageReplaced;

        private ParameterCollection _odsMainBreakParameters;
        private TestWorkOrderMainBreakForm _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _odsMainBreak)
                .DynamicMock(out _viewState)
                .DynamicMock(out _gvMainBreak)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _editLink)
                .DynamicMock(out _deleteLink)
                .DynamicMock(out _iPage)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _ddlMaterial)
                .DynamicMock(out _ddlMainCondition)
                .DynamicMock(out _ddlFailureType)
                .DynamicMock(out _ddlSoilCondition)
                .DynamicMock(out _ddlDisinfectionMethod)
                .DynamicMock(out _ddlFlushMethod)
                .DynamicMock(out _txtDepth)
                .DynamicMock(out _txtCustomersAffected)
                .DynamicMock(out _txtShutdownTime)
                .DynamicMock(out _txtChlorineResidual)
                .DynamicMock(out _cbBoilAlertIssued)
                .DynamicMock(out _ddlServiceSize)
                .DynamicMock(out _txtFootageReplaced)
                .DynamicMock(out _lblWorkDescriptionID)
                .DynamicMock(out _rfvFootageReplaced)
                .DynamicMock(out _ddlReplacedWith);


            _odsMainBreakParameters = new ParameterCollection();
            SetupResult.For(_odsMainBreak.SelectParameters).Return(
                _odsMainBreakParameters);

            _target = new TestWorkOrderMainBreakFormBuilder()
                .WithODSMainBreak(_odsMainBreak)
                .WithGVMainBreak(_gvMainBreak)
                .WithLBLWorkDescriptionID(_lblWorkDescriptionID)
                .WithViewState(_viewState)
                .WithIPage(_iPage);

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
            SetupResult.For(_gvMainBreak.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_gvMainBreak.IRows).Return(_rowCollection);
            SetupResult.For(_rowCollection.GetEnumerator()).Return(_rowCollectionEnum);

            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderMainBreakForm.ControlIDs.DELETE_LINK)).Return(_deleteLink);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderMainBreakForm.ControlIDs.EDIT_LINK)).Return(_editLink);

            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.MATERIAL)).Return(_ddlMaterial);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.MAIN_CONDITION)).Return(_ddlMainCondition);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.FAILURE_TYPE)).Return(_ddlFailureType);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMainBreakForm.ControlIDs.DEPTH)).Return(_txtDepth);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.SOIL_CONDITION)).Return(_ddlSoilCondition);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMainBreakForm.ControlIDs.CUSTOMERS_AFFECTED)).Return(_txtCustomersAffected);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMainBreakForm.ControlIDs.SHUTDOWN_TIME)).Return(_txtShutdownTime);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.DISINFECTION_METHOD)).Return(_ddlDisinfectionMethod);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.FLUSH_METHOD)).Return(_ddlFlushMethod);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMainBreakForm.ControlIDs.CHLORINE_RESIDUAL)).Return(_txtChlorineResidual);
            SetupResult.For(_iFooterRow.FindIControl<ICheckBox>(WorkOrderMainBreakForm.ControlIDs.BOIL_ALERT_ISSUED)).Return(_cbBoilAlertIssued);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.SERVICE_SIZE)).Return(_ddlServiceSize);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMainBreakForm.ControlIDs.FOOTAGE_REPLACED)).Return(_txtFootageReplaced);
            SetupResult.For(_iFooterRow.FindIControl<IRequiredFieldValidator>(WorkOrderMainBreakForm.ControlIDs.FOOTAGE_REPLACED_REQUIRED_VALIDATOR)).Return(_rfvFootageReplaced);
            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMainBreakForm.ControlIDs.REPLACED_WITH)).Return(_ddlReplacedWith);

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
            _odsMainBreakParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);

                _iFooterRow.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePreRenderSetsFooterRequiredFieldValidatorWhenCurrentMvpModeIsNotReadOnlyAndWorkDescriptionIsRight()
        {
            var requiredFieldValidator = new RequiredFieldValidator() { Enabled = false };
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.ReadOnly) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);
                    
                    SetupResult.For(
                        _iFooterRow.FindControl<RequiredFieldValidator>(
                            WorkOrderMainBreakForm.ControlIDs
                                .FOOTAGE_REPLACED_REQUIRED_VALIDATOR))
                        .Return(requiredFieldValidator);

                    SetupResult.For(_lblWorkDescriptionID.Text)
                        .Return(
                            WorkDescription.WATER_MAIN_BREAK_REPLACE_ID.ToString());

                    
                }

                using (_mocks.Playback())
                {
                    InvokeEventByName(_target, "Page_Prerender");
                    Assert.IsTrue(requiredFieldValidator.Enabled);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
                SetupGridView();
            }

            _mocks.ReplayAll();
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
        public void TestPreRenderSetsLblWorkDescriptionIDTextToWorkDescriptionID()
        {
            var expected = 79;
            _target.WorkDescriptionID = expected;
            using (_mocks.Record())
            {
                SetupResult.For(_lblWorkDescriptionID.Text).Return(String.Empty);
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);

                _lblWorkDescriptionID.Text = expected.ToString();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPreRenderDoesNotSetLblWorkDescriptionIDTextToWorkDescriptionIDIfAlreadySet()
        {
            var expected = 0;
            _target.WorkDescriptionID = expected;
            using (_mocks.Record())
            {
                SetupResult.For(_lblWorkDescriptionID.Text).Return("80");
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys
                            .CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
                Assert.AreEqual("80", _lblWorkDescriptionID.Text);
            }
        }

        #endregion

        #region Control Events

        [TestMethod]
        public void TestODSMainBreakInsertingSetsInputParameters()
        {
            const int expectedWorkOrderID = 12,
                expectedMaterialID = 1,
                expectedMainConditionID = 1,
                expectedFailureID = 1,
                expectedSoilConditionID = 1,
                expectedDisinfectionMethod = 1,
                expectedFlushMethod = 1,
                expectedCustomersAffected = 43,
                expectedServiceSizeID = 1,
                expectedFootageReplaced = 4,
                expectedReplacedWithId = 1;
            const decimal expectedDepth = 1.1m,
                    expectedShutdownTime = 2.2m,
                    expectedChlorineResidual = 1.1m;

            bool expectedBoilAlertIssued = false;

            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expectedWorkOrderID);
                SetupResult.For(_ddlMaterial.SelectedValue).Return(expectedMaterialID.ToString());
                SetupResult.For(_ddlMainCondition.SelectedValue).Return(expectedMainConditionID.ToString());
                SetupResult.For(_ddlFailureType.SelectedValue).Return(expectedFailureID.ToString());
                SetupResult.For(_txtDepth.Text).Return(expectedDepth.ToString());
                SetupResult.For(_ddlSoilCondition.SelectedValue).Return(expectedSoilConditionID.ToString());
                SetupResult.For(_txtCustomersAffected.Text).Return(expectedCustomersAffected.ToString());
                SetupResult.For(_txtShutdownTime.Text).Return(
                    expectedShutdownTime.ToString());
                SetupResult.For(_ddlDisinfectionMethod.SelectedValue).Return(
                    expectedDisinfectionMethod.ToString());
                SetupResult.For(_ddlFlushMethod.SelectedValue).Return(
                    expectedFlushMethod.ToString());
                SetupResult.For(_txtChlorineResidual.Text).Return(
                    expectedChlorineResidual.ToString());
                SetupResult.For(_cbBoilAlertIssued.Checked).Return(expectedBoilAlertIssued);
                SetupResult.For(_ddlServiceSize.SelectedValue).Return(expectedServiceSizeID.ToString());
                SetupResult.For(_txtFootageReplaced.Text).Return(expectedFootageReplaced.ToString());
                SetupResult.For(_ddlReplacedWith.SelectedValue).Return(expectedReplacedWithId.ToString());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsMainBreak_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedWorkOrderID,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.WORK_ORDER_ID]);
                Assert.AreEqual(expectedBoilAlertIssued,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.BOIL_ALERT_ISSUED]);
                Assert.AreEqual(expectedChlorineResidual,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.CHLORINE_RESIDUAL]);
                Assert.AreEqual(expectedCustomersAffected,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.CUSTOMERS_AFFECTED]);
                Assert.AreEqual(expectedDepth,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.DEPTH]);
                Assert.AreEqual(expectedDisinfectionMethod,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_BREAK_DISINFECTION_METHOD_ID]);
                Assert.AreEqual(expectedFailureID,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_FAILURE_TYPE_ID]);
                Assert.AreEqual(expectedFlushMethod,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_BREAK_FLUSH_METHOD_ID]);
                Assert.AreEqual(expectedMainConditionID,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_CONDITION_ID]);
                Assert.AreEqual(expectedMaterialID,args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_BREAK_MATERIAL_ID]);
                Assert.AreEqual(expectedShutdownTime, args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.SHUTDOWN_TIME]);
                Assert.AreEqual(expectedSoilConditionID, args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.MAIN_BREAK_SOIL_CONDITION_ID]);
                Assert.AreEqual(expectedServiceSizeID, args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.SERVICE_SIZE_ID]);
                Assert.AreEqual(expectedFootageReplaced, args.InputParameters[WorkOrderMainBreakForm.MainBreakParameterNames.FOOTAGE_REPLACED]);
            }
        }

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSourceWhenPageIsValid()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IsValid).Return(true);
                SetupResult.For(_odsMainBreak.Insert()).Return(1);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbMainBreakInsert_Click");
            }
        }

        [TestMethod]
        public void TestRowCreatedEnablesRequiredFieldValidator()
        {
            WorkOrder order = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Today.GetPreviousDay());
            var row = _mocks.StrictMock<TestGridViewRow>(order); //new TestGridViewRow(order));
            var args = new GridViewRowEventArgs(row);
            var rfv1 = new RequiredFieldValidator() { Enabled = false };
            var rfv2 = new RequiredFieldValidator() { Enabled = false };

            using (_mocks.Record())
            {
                SetupResult.For(_lblWorkDescriptionID.Text).Return("80");

                SetupResult.For(
                    row.FindControl(
                        WorkOrderMainBreakForm.ControlIDs
                            .FOOTAGE_REPLACED_REQUIRED_VALIDATOR))
                    .Return(rfv1);

                SetupResult.For(
                    row.FindControl(
                        WorkOrderMainBreakForm.ControlIDs
                            .REPLACED_WITH_VALIDATOR))
                    .Return(rfv2);

                rfv1.Enabled = true;
                rfv2.Enabled = true;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "gvMainBreak_OnRowCreated", new object[] { null, args });
            }
        }

        #endregion
    }

    internal class TestWorkOrderMainBreakFormBuilder : TestDataBuilder<TestWorkOrderMainBreakForm>
    {
        #region Private Members

        private IPage _iPage;
        private IGridView _gvMainBreak;
        private IViewState _viewState;
        private IObjectDataSource _odsMainBreak;
        private ILabel _lbWorkDescriptionID;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderMainBreakForm Build()
        {
            var obj = new TestWorkOrderMainBreakForm();
            if (_odsMainBreak != null)
                obj.SetODSMainBreak(_odsMainBreak);
            if (_gvMainBreak != null)
                obj.SetGVMainBreak(_gvMainBreak);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_lbWorkDescriptionID != null)
                obj.SetLBLWorkDescriptionID(_lbWorkDescriptionID);
            return obj;
        }

        public TestWorkOrderMainBreakFormBuilder WithODSMainBreak(IObjectDataSource ods)
        {
            _odsMainBreak = ods;
            return this;
        }

        public TestWorkOrderMainBreakFormBuilder WithGVMainBreak(IGridView gridView)
        {
            _gvMainBreak = gridView;
            return this;
        }

        public TestWorkOrderMainBreakFormBuilder WithLBLWorkDescriptionID(ILabel label)
        {
            _lbWorkDescriptionID = label;
            return this;
        }

        public TestWorkOrderMainBreakFormBuilder WithViewState(IViewState viewState)
        {
            _viewState = viewState;
            return this;
        }

        public TestWorkOrderMainBreakFormBuilder WithIPage(IPage iPage)
        {
            _iPage = iPage;
            return this;
        }
        #endregion
    }

    internal class TestWorkOrderMainBreakForm : WorkOrderMainBreakForm
    {
        #region Exposed Methods

        public void SetODSMainBreak(IObjectDataSource ds)
        {
            odsMainBreak = ds;
        }

        public void SetGVMainBreak(IGridView gv)
        {
            gvMainBreak = gv;
        }

        public void SetLBLWorkDescriptionID(ILabel label)
        {
            lblWorkDescriptionID = label;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetIPage(IPage iPage)
        {
            _iPage = iPage;
        }

        #endregion
    }
}
