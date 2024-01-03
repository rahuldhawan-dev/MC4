using System;
using System.Web.Mvc;
using System.Web.UI;
using LINQTo271.Views.Assets;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.Assets
{
    /// <summary>
    /// Summary description for AssetLatLonPickerViewTest
    /// </summary>
    [TestClass]
    public class AssetLatLonPickerViewTest : EventFiringTestClass
    {
        #region Constants

        private const string TEST_QUERY_STRING_BASE = "http://localhost/foo?";

        private struct QUERY_STRING_FORMATS
        {
            public const string WITH_VALUE = "{0}={1}&",
                                NO_VALUE = "{0}=&";
        }

        private struct TEST_QUERY_STRING_KEYS
        {
            public const string LOCATION =
                AssetLatLonReadOnlyView.QueryStringKeys.LOCATION;
        }

        #endregion

        #region Private Members

        private IDetailPresenter<Asset> _presenter;
        private AssetLatLonPickerView _target;
        private IContainer _container;

        #endregion

        #region Private Static Methods

        private static HttpSimulator BuildRequestWithQueryString(string queryString)
        {
            var simulator = new HttpSimulator();
            var url = TEST_QUERY_STRING_BASE + queryString;
            simulator =
                simulator.SimulateRequest(
                    new Uri(url.Substring(0, url.Length - 1)));
            return simulator;
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            _container = new Container();
            _mocks = new MockRepository();
            _presenter = _mocks.DynamicMock<IDetailPresenter<Asset>>();
            _container.Inject(_presenter);
            _target = new AssetLatLonPickerView();
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestGetsLocationFromQueryString()
        {
            _mocks.ReplayAll();
            var expected = "Dead End St.";

            using (BuildRequestWithQueryString(String.Format(
                QUERY_STRING_FORMATS.WITH_VALUE, TEST_QUERY_STRING_KEYS.LOCATION, expected)))
            {
                Assert.AreEqual(expected, _target.Location);
            }
        }

        [TestMethod]
        public void TestLocationPropertyReturnsNullWhenNotProvidedInQueryString()
        {
            _mocks.ReplayAll();

            using (BuildRequestWithQueryString(String.Format(
                QUERY_STRING_FORMATS.NO_VALUE, TEST_QUERY_STRING_KEYS.LOCATION)))
            {
                Assert.AreEqual(String.Empty, _target.Location);
            }
        }

        [TestMethod]
        public void TestBtnSaveClickFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            using (_target = new TestAssetLatLonPickerViewBuilder()
                .WithUpdatingEventHandler((sender, e) => _called = true)
                .WithAsset(new TestAssetBuilder().WithValve(new Valve { Coordinate = new Coordinate() })))
            {
                InvokeEventByName(_target, "btnFormSave_Click");

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsClientScriptManagerWhenPostBack()
        {
            var _clientScriptManager = _mocks.DynamicMock<IClientScriptManager>();
            _target = new TestAssetLatLonPickerViewBuilder().WithPostBack(true)
                .WithClientScriptManager(_clientScriptManager);

            using (_mocks.Record())
            {
                _clientScriptManager
                    .Expect(x => x.TryRegisterClientScriptInclude("foo", "~/Views/Assets/AssetLatLongPickerViewClose.js")).Return(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load", GetEventArgArray());
            }
        }

        #if !DEBUG

        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail("This test must be run in DEBUG mode.");
        }

        #endif
    }

    internal class TestAssetLatLonPickerViewBuilder : TestDataBuilder<TestAssetLatLonPickerView>
    {
        #region Private Members

        private bool? _postBack;
        private EventHandler<EntityEventArgs<Asset>> _onUpdating;
        private Asset _asset;
        private IClientScriptManager _clientScriptManager;

        #endregion

        #region Private Methods

        private void SetPostBack(TestAssetLatLonPickerView view)
        {
            SetFieldValue(view, "_isMvpPostBack", _postBack.Value);
        }

        private void Picker_OnDispose(TestAssetLatLonPickerView view)
        {
            if (_onUpdating != null)
                view.Updating -= _onUpdating;
        }

        #endregion

        #region Exposed Methods

        public override TestAssetLatLonPickerView Build()
        {
            var view = new TestAssetLatLonPickerView();
            if (_postBack != null)
                SetPostBack(view);
            if (_asset != null)
                view.SetAsset(_asset);
            if (_onUpdating != null)
                view.Updating += _onUpdating;
            if (_clientScriptManager != null)
            {
                SetFieldValue(view, "_iClientScript", _clientScriptManager);
                //SetFieldValue(view, "_iClientScriptManager", _clientScriptManager);
                //view.SetHiddenFieldValueByName("_iClientScript", _clientScriptManager);
                //view.SetPropertyValueByName("ClientScriptManager", _clientScriptManager);
            }
            view._onDispose = Picker_OnDispose;
            return view;
        }

        public TestAssetLatLonPickerViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        public TestAssetLatLonPickerViewBuilder WithUpdatingEventHandler(EventHandler<EntityEventArgs<Asset>> onUpdating)
        {
            _onUpdating = onUpdating;
            return this;
        }

        public TestAssetLatLonPickerViewBuilder WithAsset(Asset asset)
        {
            _asset = asset;
            return this;
        }

        public TestAssetLatLonPickerViewBuilder WithClientScriptManager(IClientScriptManager clientScriptManager)
        {
            _clientScriptManager = clientScriptManager;
            return this;
        }

        #endregion
    }

    internal class TestAssetLatLonPickerView : AssetLatLonPickerView
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestAssetLatLonPickerView view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Exposed Methods

        public override void Dispose()
        {
            base.Dispose();
            if (_onDispose != null)
                _onDispose(this);
        }

        public void SetAsset(Asset asset)
        {
            _asset = asset;
        }

        #endregion
    }
}
