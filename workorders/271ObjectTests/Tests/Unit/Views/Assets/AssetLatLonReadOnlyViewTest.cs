using System;
using System.Web.Mvc;
using LINQTo271.Views.Assets;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Views.Assets
{
    /// <summary>
    /// Summary description for AssetLatLonReadOnlyViewTest
    /// </summary>
    [TestClass]
    public class AssetLatLonReadOnlyViewTest : EventFiringTestClass
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
            public const string ASSET_ID =
                AssetLatLonReadOnlyView.QueryStringKeys.ASSET_ID,
                                ASSET_TYPE_ID =
                AssetLatLonReadOnlyView.QueryStringKeys.ASSET_TYPE_ID;
        }

        #endregion

        #region Private Members

        private IQueryString _iQueryString;
        private IRequest _iRequest;
        private IHiddenField _hidLatitude, _hidLongitude;
        private IDetailPresenter<Asset> _presenter;
        private AssetLatLonReadOnlyView _target;
        private IContainer _container;

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(object obj)
        {
            InvokeEventByName(obj, "Page_Load", GetEventArgArray());
        }

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
            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
                .DynamicMock(out _presenter)
                .DynamicMock(out _hidLatitude)
                .DynamicMock(out _hidLongitude)
                .DynamicMock(out _iRequest)
                .DynamicMock(out _iQueryString);

            SetupResult.For(_iRequest.IQueryString).Return(_iQueryString);
            _container.Inject(_presenter);

            _target = new TestAssetLatLonReadOnlyViewBuilder()
                .WithHidLatitude(_hidLatitude)
                .WithHidLongitude(_hidLongitude)
                .WithIRequest(_iRequest);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Property Tests

        #region Structural Properties

        [TestMethod]
        public void TestChildResourceViewsPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.ChildResourceViews);
        }

        [TestMethod]
        public void TestCurrentModePropertyReturnsEdit()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(DetailViewMode.Edit, _target.CurrentMode);
        }

        [TestMethod]
        public void TestCurrentDataKeyPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.CurrentDataKey);
        }

        #endregion

        #region AssetTypeID

        [TestMethod]
        public void TestGetsAssetTypeIDFromQueryString()
        {
            var expected = 456;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _iQueryString.GetValue<int?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.ASSET_TYPE_ID)).
                    Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.AssetTypeID);
            }
        }

        [TestMethod]
        public void TestAssetTypeIDPropertyThrowsExceptionWhenValueNotProvidedInQueryString()
        {
            int throwAway;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _iQueryString.GetValue<int?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.ASSET_TYPE_ID)).
                    Return(null);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<NullReferenceException>(
                    () => throwAway = _target.AssetTypeID);
            }
        }

        #endregion

        #region AssetID

        [TestMethod]
        public void TestGetsAssetIDFromQueryStringWhenAssetTypeIsValveHydrantOrSewerOpening()
        {
            var assetTypeIDs = new[] {
                AssetTypeRepository.Indices.VALVE,
                AssetTypeRepository.Indices.HYDRANT,
                AssetTypeRepository.Indices.SEWER_OPENING,
                AssetTypeRepository.Indices.STORM_CATCH,
                AssetTypeRepository.Indices.EQUIPMENT
            };
            var expected = 123;

            foreach (var assetTypeID in assetTypeIDs)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.
                                ASSET_TYPE_ID))
                        .Return(assetTypeID);
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.ASSET_ID)).
                        Return(expected);
                }

                using (_mocks.Playback())
                {
                    Assert.AreEqual(expected, _target.AssetID);
                }
                
                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestAssetIDPropertyThrowsExceptionWhenValueNotProvidedAndAssetTypeIsValveHydrantOrSewerOpening()
        {
            var assetTypeIDs = new[] {
                AssetTypeRepository.Indices.VALVE,
                AssetTypeRepository.Indices.HYDRANT,
                AssetTypeRepository.Indices.SEWER_OPENING,
                AssetTypeRepository.Indices.STORM_CATCH,
                AssetTypeRepository.Indices.EQUIPMENT
            };
            int throwAway;

            foreach (var assetTypeID in assetTypeIDs)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.ASSET_TYPE_ID)).
                        Return(assetTypeID);
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.ASSET_ID)).
                        Return(null);
                }

                using (_mocks.Playback())
                {
                    MyAssert.Throws<NullReferenceException>(
                        () => throwAway = _target.AssetID);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestAssetIDPropertyReturnsDefaultValueWhenValueNotProvidedAndAssetTypeIsNotValveOrHydrant()
        {
            var assetTypeIDs = new[] {
                AssetTypeRepository.Indices.MAIN,
                AssetTypeRepository.Indices.SERVICE
            };

            foreach (var assetTypeID in assetTypeIDs)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.
                                ASSET_TYPE_ID)).
                        Return(assetTypeID);
                    SetupResult.For(
                        _iQueryString.GetValue<int?>(
                            AssetLatLonReadOnlyView.QueryStringKeys.ASSET_ID)).
                        Return(null);
                }

                using (_mocks.Playback())
                {
                    Assert.AreEqual(AssetLatLonReadOnlyView.DEFAULT_ASSET_ID,
                        _target.AssetID);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region Coordinates

        [TestMethod]
        public void TestCoordinateQueryStringPropertiesReturnValuesIfSetInQueryString()
        {
            const double expectedLat = 1.1, expectedLon = 2.2;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _iQueryString.GetValue<double?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.LATITUDE))
                    .Return(expectedLat);
                SetupResult.For(
                    _iQueryString.GetValue<double?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.LONGITUDE))
                    .Return(expectedLon);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expectedLat, _target.QueryStringLatitude);
                Assert.AreEqual(expectedLon, _target.QueryStringLongitude);
            }
        }

        [TestMethod]
        public void TestCoordinateQueryStringPropertiesReturnNullIfNotSetInQueryString()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _iQueryString.GetValue<double?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.LATITUDE))
                    .Return(null);
                SetupResult.For(
                    _iQueryString.GetValue<double?>(
                        AssetLatLonReadOnlyView.QueryStringKeys.LONGITUDE))
                    .Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.IsNull(_target.QueryStringLatitude);
                Assert.IsNull(_target.QueryStringLongitude);
            }
        }

        [TestMethod]
        public void TestCoordinatePropertiesReturnHiddenFieldValuesWhenSet()
        {
            const double expectedLat = 1.1, expectedLon = 2.2;
            var asset = new TestAssetBuilder()
                .WithValve(new Valve {
                    Coordinate = new Coordinate {
                        //Latitude = null,
                        //Longitude = null
                    }
                });
            _target =
                new TestAssetLatLonReadOnlyViewBuilder()
                    .WithHidLatitude(_hidLatitude)
                    .WithHidLongitude(_hidLongitude)
                    .WithAsset(asset);

            using (_mocks.Record())
            {
                SetupResult.For(_hidLatitude.Value).Return(
                    expectedLat.ToString());
                SetupResult.For(_hidLongitude.Value).Return(
                    expectedLon.ToString());
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expectedLat, _target.Latitude);
                Assert.AreEqual(expectedLon, _target.Longitude);
            }
        }

        [TestMethod]
        public void TestCoordinatePropertiesReturnAssetCoordinatesWhenSetAndHiddenFieldValuesNotSet()
        {
            const double expectedLat = 1.1, expectedLon = 2.2;
            var valve = new Valve {
                Coordinate = new Coordinate {
                    Latitude = expectedLat,
                    Longitude = expectedLon
                }
            };
            var asset = new TestAssetBuilder().WithValve(valve);
            _target =
                new TestAssetLatLonReadOnlyViewBuilder()
                    .WithHidLatitude(_hidLatitude)
                    .WithHidLongitude(_hidLongitude)
                    .WithAsset(asset);

            _mocks.ReplayAll();

            Assert.AreEqual(expectedLat, _target.Latitude);
            Assert.AreEqual(expectedLon, _target.Longitude);
        }

        [TestMethod]
        public void TestCoordinatePropertiesReturnDefaultCoordinatesWhenHiddenFieldsAreEmptyAndAssetHasNoCoordinateValues()
        {
            var valve = new Valve();
            var asset = new TestAssetBuilder().WithValve(valve);
            _target =
                new TestAssetLatLonReadOnlyViewBuilder()
                    .WithHidLatitude(_hidLatitude)
                    .WithHidLongitude(_hidLongitude)
                    .WithAsset(asset);

            _mocks.ReplayAll();

            Assert.AreEqual(AssetLatLonReadOnlyView.DefaultCoordinates.LATITUDE, _target.Latitude);
            Assert.AreEqual(AssetLatLonReadOnlyView.DefaultCoordinates.LONGITUDE, _target.Longitude);
        }

        [TestMethod]
        public void TestSettingCoordinateValuesSetsValuesOfHiddenInputs()
        {
            const double expectedLat = 1.1, expectedLon = 2.2;
            _target =
                new TestAssetLatLonReadOnlyViewBuilder()
                    .WithHidLatitude(_hidLatitude)
                    .WithHidLongitude(_hidLongitude);

            using (_mocks.Record())
            {
                _hidLatitude.Value = expectedLat.ToString();
                _hidLongitude.Value = expectedLon.ToString();
            }

            using (_mocks.Playback())
            {
                _target.Latitude = expectedLat;
                _target.Longitude = expectedLon;
            }
        }

        #endregion

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewInitializedWhenIsNotPostBack()
        {
            _target =
                new TestAssetLatLonReadOnlyViewBuilder().WithPostBack(false);

            using (_mocks.Record())
            {
                _presenter.OnViewInitialized();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotCallPresenterOnViewInitializedWhenIsPostBack()
        {
            _target =
                new TestAssetLatLonReadOnlyViewBuilder().WithPostBack(true);

            using (_mocks.Record())
            {
                DoNotExpect.Call(_presenter.OnViewInitialized);
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewLoaded()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewLoaded();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetViewControlsVisibleNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.SetViewControlsVisible(true),
                typeof(NotImplementedException));

            MyAssert.Throws(() => _target.SetViewControlsVisible(false),
                typeof(NotImplementedException));
        }

        [TestMethod]
        public void TestSetViewModeNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.SetViewMode(DetailViewMode.ReadOnly),
                typeof(NotImplementedException));

            MyAssert.Throws(() => _target.SetViewMode(DetailViewMode.Edit),
                typeof(NotImplementedException));

            MyAssert.Throws(() => _target.SetViewMode(DetailViewMode.Insert),
                typeof(NotImplementedException));
        }

        [TestMethod]
        public void TestShowEntityNotImplemented()
        {
            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.ShowEntity(null),
                typeof(NotImplementedException));
        }

        [TestMethod]
        public void TestSetChildResourceViewFilterExpressionsNotImplemented()
        {
             _mocks.ReplayAll();

            MyAssert.Throws(
                () => _target.SetChildResourceViewFilterExpressions(null),
                typeof(NotImplementedException));
        }

        #endregion

        #if !DEBUG

        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail("This test must be run in DEBUG mode.");
        }

        #endif
    }

    internal class TestAssetLatLonReadOnlyViewBuilder : TestDataBuilder<AssetLatLonReadOnlyView>
    {
        #region Private Members

        private bool? _postBack;
        private IHiddenField _hidLatitude, _hidLongitude;
        private Asset _asset;
        private IRequest _iRequest;

        #endregion

        #region Private Methods

        private void SetPostBack(AssetLatLonReadOnlyView view)
        {
            SetFieldValue(view, "_isMvpPostBack", _postBack.Value);
        }

        private void SetHidLatitude(AssetLatLonReadOnlyView view)
        {
            SetFieldValue(view, "hidLatitude", _hidLatitude);
        }

        private void SetHidLongitude(AssetLatLonReadOnlyView view)
        {
            SetFieldValue(view, "hidLongitude", _hidLongitude);
        }

        private void SetAsset(AssetLatLonReadOnlyView view)
        {
            SetFieldValue(view, "_asset", _asset);
        }

        private void SetIRequest(AssetLatLonReadOnlyView view)
        {
            SetFieldValue(view, "_iRequest", _iRequest);
        }

        #endregion

        #region Exposed Methods

        public override AssetLatLonReadOnlyView Build()
        {
            var view = new AssetLatLonReadOnlyView();
            if (_postBack != null)
                SetPostBack(view);
            if (_hidLatitude != null)
                SetHidLatitude(view);
            if (_hidLongitude != null)
                SetHidLongitude(view);
            if (_asset != null)
                SetAsset(view);
            if (_iRequest != null)
                SetIRequest(view);
            return view;
        }

        public TestAssetLatLonReadOnlyViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        public TestAssetLatLonReadOnlyViewBuilder WithHidLatitude(IHiddenField hidLatitude)
        {
            _hidLatitude = hidLatitude;
            return this;
        }

        public TestAssetLatLonReadOnlyViewBuilder WithHidLongitude(IHiddenField hidLongitude)
        {
            _hidLongitude = hidLongitude;
            return this;
        }

        public TestAssetLatLonReadOnlyViewBuilder WithAsset(Asset asset)
        {
            _asset = asset;
            return this;
        }

        public TestAssetLatLonReadOnlyViewBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        #endregion
    }
}
