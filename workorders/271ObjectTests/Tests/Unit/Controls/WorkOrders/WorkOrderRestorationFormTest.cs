using System;
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
    /// Summary description for WorkOrderRestorationFormTest.
    /// </summary>
    [TestClass]
    public class WorkOrderRestorationFormTest : EventFiringTestClass
    {
        #region Private Members

        private IViewState _viewState;
        private IGridView _gvRestorations;
        private IGridViewRow _iFooterRow;
        private IObjectDataSource _odsRestorations;
        private ParameterCollection _odsRestorationsParameters;
        private TestWorkOrderRestorationForm _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _odsRestorations)
                .DynamicMock(out _viewState)
                .DynamicMock(out _gvRestorations)
                .DynamicMock(out _iFooterRow);

            _odsRestorationsParameters = new ParameterCollection();
            SetupResult.For(_odsRestorations.SelectParameters).Return(
                _odsRestorationsParameters);

            _target = new TestWorkOrderRestorationFormBuilder()
                .WithODSRestorations(_odsRestorations)
                .WithGVRestorations(_gvRestorations)
                .WithViewState(_viewState);

            SetupGridView();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void SetupGridView()
        {
            SetupResult.For(_gvRestorations.IFooterRow).Return(_iFooterRow);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSettingWorkOrderIDSetsSelectParameterForDataSource()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            _odsRestorationsParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadHidesSelectAndDeleteControlsWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_viewState.GetValue(
                    WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE)).
                    Return(DetailViewMode.ReadOnly);

                _gvRestorations.AutoGenerateSelectButton = false;
                _gvRestorations.AutoGenerateDeleteButton = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotHideSelectOrDeleteControlsWhenCurrentMvpModeIsNotReadOnly()
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

               //     _gvRestorations.AutoGenerateSelectButton = true;
                    _gvRestorations.AutoGenerateDeleteButton = true;
                }

                using (_mocks.Playback())
                {
                    InvokeEventByName(_target, "Page_Load");
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

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

        #endregion
    }

    internal class TestWorkOrderRestorationFormBuilder : TestDataBuilder<TestWorkOrderRestorationForm>
    {
        #region Private Members

        private IGridView _gvRestorations;
        private IViewState _viewState;
        private IObjectDataSource _odsRestorations;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderRestorationForm Build()
        {
            var obj = new TestWorkOrderRestorationForm();
            if (_odsRestorations != null)
                obj.SetODSRestorations(_odsRestorations);
            if (_gvRestorations != null)
                obj.SetGVRestorations(_gvRestorations);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            return obj;
        }

        public TestWorkOrderRestorationFormBuilder WithODSRestorations(IObjectDataSource ods)
        {
            _odsRestorations = ods;
            return this;
        }

        public TestWorkOrderRestorationFormBuilder WithGVRestorations(IGridView gridView)
        {
            _gvRestorations = gridView;
            return this;
        }

        public TestWorkOrderRestorationFormBuilder WithViewState(IViewState viewState)
        {
            _viewState = viewState;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderRestorationForm : WorkOrderRestorationForm
    {
        #region Exposed Methods

        public void SetODSRestorations(IObjectDataSource ds)
        {
            odsRestorations = ds;
        }

        public void SetGVRestorations(IGridView gv)
        {
            gvRestorations = gv;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        #endregion
    }
}