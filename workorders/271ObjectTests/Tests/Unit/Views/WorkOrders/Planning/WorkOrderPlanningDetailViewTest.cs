using System;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.Planning;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Planning
{
    /// <summary>
    /// Summary description for WorkOrderPlanningDetailViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderPlanningDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl, _inputFormViewWorkOrder;
        private IWorkOrderInputFormView _wofvWorkOrder;
        private ILabel _lblWorkOrderID, _lblMarkoutError, _lbMarkoutRequirement;

        private ITextBox _txtMarkoutNumber,
                         _txtRadius,
                         _ccDateOfRequest;

        private IButton _btnEditInitialInfo, _btnCancelInitialInfo;

        private IObjectContainerDataSource _odsWorkOrder;
        private IObjectDataSource _odsMarkouts;
        private TestWorkOrderPlanningDetailView _target;
        private IUpdatePanel _upMarkouts;

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

        #endregion

        #region Private Methods

        private void MockUsing(Func<Type, object[], object> mock)
        {
            _odsMarkouts = (IObjectDataSource)mock(typeof(IObjectDataSource), null);
            _lblWorkOrderID = (ILabel)mock(typeof(ILabel), null);
            //_lblCurrentNotes = (ILabel)mock(typeof(ILabel), null);
            _txtMarkoutNumber = (ITextBox)mock(typeof(ITextBox), null);
            _txtRadius = (ITextBox)mock(typeof(ITextBox), null);
            _ccDateOfRequest = (ITextBox)mock(typeof(ITextBox), null);
            //_txtAppendNotes = (ITextBox)mock(typeof(ITextBox), null);
            _detailControl = (IDetailControl)mock(typeof(IDetailControl), null);
            _odsWorkOrder =
                (IObjectContainerDataSource)
                mock(typeof(IObjectContainerDataSource), null);
            _wofvWorkOrder = (IWorkOrderInputFormView)mock(typeof(IWorkOrderInputFormView), null);
            _upMarkouts = (IUpdatePanel)mock(typeof(IUpdatePanel), null);
            _lblMarkoutError = (ILabel)mock(typeof(ILabel), null);
            _lbMarkoutRequirement = (ILabel)mock(typeof(ILabel), null);

            _inputFormViewWorkOrder = (IDetailControl)mock(typeof(IDetailControl), null);
            _mocks
                .DynamicMock(out _btnEditInitialInfo)
                .DynamicMock(out _btnCancelInitialInfo);

            _target = new TestWorkOrderPlanningDetailViewBuilder()
                .WithODSMarkouts(_odsMarkouts)
                .WithLBLWorkOrderID(_lblWorkOrderID)
                .WithTXTMarkoutNumber(_txtMarkoutNumber)
                .WithTXTRadius(_txtRadius)
                .WithCCDateOfRequest(_ccDateOfRequest)
                .WithDetailControl(_detailControl)
                .WithODSWorkOrder(_odsWorkOrder)
                .WithWOFVWorkOrder(_wofvWorkOrder)
                .WithBTNEditInitialInfo(_btnEditInitialInfo)
                .WithBTNCancelInitialInfo(_btnCancelInitialInfo);

            SetupResult.For(_detailControl.FindIControl<IUpdatePanel>("upMarkouts")).Return(_upMarkouts);
            SetupResult.For(_upMarkouts.FindIControl<ILabel>("lblMarkoutError")).Return(_lblMarkoutError);
            SetupResult.For(_wofvWorkOrder.FindIControl<IDetailControl>("fvWorkOrder")).Return(_inputFormViewWorkOrder);
            SetupResult.For(_inputFormViewWorkOrder.FindIControl<ILabel>("lbMarkoutRequirement")).Return(_lbMarkoutRequirement);
            SetupResult.For(_lbMarkoutRequirement.Text).Return("Routine");
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestCancelButtonPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.CancelButton);
        }

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_odsWorkOrder, _target.DataSource);
        }

        [TestMethod]
        public void TestEditButtonPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.EditButton);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesPlanning()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Planning, _target.Phase);
        }

        #endregion

        #region Exposed Method Tests

        //TODO: Finish testing this
        //[TestMethod]
        //public void TestBTNCancelInitialInfoClick()
        //{
            //using (_mocks.Record())
            //{
            //    _btnEditInitialInfo.Visible = true;
            //    _btnCancelInitialInfo.Visible = false;
            //    _wofvWorkOrder.ChangeMvpMode(DetailViewMode.ReadOnly);
            //}
            //using (_mocks.Playback())
            //{
            //    InvokeEventByName(_target, "btnCancelInitialInfo_Click");
            //}
        //}

        [TestMethod]
        public void TestSetViewControlsVisibleDoesNothing()
        {
            MockUsing(_mocks.CreateMock);

            using (_mocks.Record())
            {
            }

            using (_mocks.Playback())
            {
                _target.SetViewControlsVisible(false);
                _target.SetViewControlsVisible(true);
            }
        }

        [TestMethod]
        public void TestSetViewModeOnlySetsDetailControlModeToEditDespiteItsArgument()
        {
            using (_mocks.Record())
            {
                _detailControl.ChangeMvpMode(DetailViewMode.Edit);
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(DetailViewMode.ReadOnly);
                _target.SetViewMode(DetailViewMode.Insert);
                _target.SetViewMode(DetailViewMode.Edit);
            }
        }

        [TestMethod]
        public void TestShowEntitySetsDataSourceAndInitialFormDataSourceToWorkOrderInstance()
        {
            var expected = new WorkOrder();

            using (_mocks.Record())
            {
                _odsWorkOrder.DataSource = expected;
            }

            using (_mocks.Playback())
            {
                _target.ShowEntity(expected);
            }
        }

        #endregion
    }

    internal class TestWorkOrderPlanningDetailViewBuilder : TestDataBuilder<TestWorkOrderPlanningDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectDataSource _odsMarkouts;
        private ILabel _lblWorkOrderID, _lblCurrentNotes;
        private ITextBox _txtMarkoutNumber,
                         _txtRadius,
                         _ccDateOfRequest,
                         _txtAppendNotes;

        private IObjectContainerDataSource _odsWorkOrder;
        private IWorkOrderInputFormView _wofvWorkOrder;
        private IButton _btnEditInitialInfo, _btnCancelInitialInfo;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderPlanningDetailView Build()
        {
            var obj = new TestWorkOrderPlanningDetailView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_odsWorkOrder != null)
                obj.SetODSWorkOrder(_odsWorkOrder);
            if (_wofvWorkOrder != null)
                obj.SetWOFVWorkOrder(_wofvWorkOrder);
            if (_btnEditInitialInfo != null)
                obj.SetBTNEditInitialInfo(_btnEditInitialInfo);
            if (_btnCancelInitialInfo != null)
                obj.SetBTNCancelInitialInfo(_btnCancelInitialInfo);
            return obj;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithODSMarkouts(IObjectDataSource markouts)
        {
            _odsMarkouts = markouts;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithLBLWorkOrderID(ILabel id)
        {
            _lblWorkOrderID = id;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithTXTMarkoutNumber(ITextBox number)
        {
            _txtMarkoutNumber = number;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithTXTRadius(ITextBox radius)
        {
            _txtRadius = radius;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithCCDateOfRequest(ITextBox request)
        {
            _ccDateOfRequest = request;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithDetailControl(IDetailControl control)
        {
            _detailControl = control;
            return this;
        }

        //public TestWorkOrderPlanningDetailViewBuilder WithTXTAppendNotes(ITextBox notes)
        //{
        //    _txtAppendNotes = notes;
        //    return this;
        //}

        //public TestWorkOrderPlanningDetailViewBuilder WithLBLCurrentNotes(ILabel notes)
        //{
        //    _lblCurrentNotes = notes;
        //    return this;
        //}

        public TestWorkOrderPlanningDetailViewBuilder WithODSWorkOrder(IObjectContainerDataSource order)
        {
            _odsWorkOrder = order;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithWOFVWorkOrder(IWorkOrderInputFormView order)
        {
            _wofvWorkOrder = order;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithBTNEditInitialInfo(IButton button)
        {
            _btnEditInitialInfo = button;
            return this;
        }

        public TestWorkOrderPlanningDetailViewBuilder WithBTNCancelInitialInfo(IButton button)
        {
            _btnCancelInitialInfo = button;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderPlanningDetailView : WorkOrderPlanningDetailView
    {
        #region Exposed Methods

        public void SetDetailControl(IDetailControl control)
        {
            fvWorkOrder = control;
        }

        public void SetODSWorkOrder(IObjectContainerDataSource order)
        {
            odsWorkOrder = order;
        }

        public void SetWOFVWorkOrder(IWorkOrderInputFormView order)
        {
            _wofvInitialInformation = order;
        }

        public void SetBTNEditInitialInfo(IButton button)
        {
            _btnEditInitialInfo = button;
        }
        
        public void SetBTNCancelInitialInfo(IButton button)
        {
            _btnCancelInitialInfo = button;
        }
        #endregion
    }
}
