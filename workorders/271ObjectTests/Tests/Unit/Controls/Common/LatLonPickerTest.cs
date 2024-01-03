using System;
using LINQTo271.Common;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Controls.Common
{
    /// <summary>
    /// Summary description for LatLonPickerTest.
    /// </summary>
    [TestClass]
    public class LatLonPickerTest : EventFiringTestClass
    {
        #region Private Members

        private IImageButton _imgShowPicker;
        private IHiddenField _hidState, _hidLatitude, _hidLongitude;
        private TestLatLonPicker _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _hidState)
                .DynamicMock(out _hidLatitude)
                .DynamicMock(out _hidLongitude)
                .DynamicMock(out _imgShowPicker);

            _target = new TestLatLonPickerBuilder()
                .WithHIDState(_hidState)
                .WithHIDLatitude(_hidLatitude)
                .WithHIDLongitude(_hidLongitude)
                .WithIMGShowPicker(_imgShowPicker);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestAssetPropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();

            var asset = new TestAssetBuilder()
                .WithValve(new Valve())
                .Build();
            _target = new TestLatLonPickerBuilder()
                .WithAsset(asset);

            Assert.AreSame(asset, _target.Asset);
        }

        [TestMethod]
        public void TestLatitudePropertyReturnsLatitudeValueFromAssetIfPresent()
        {
            _mocks.ReplayAll();

            var latitude = 1.5;
            var asset = new TestAssetBuilder()
                .WithValve(new Valve {
                    Coordinate = new Coordinate {Latitude = latitude}
                })
                .Build();
            _target = new TestLatLonPickerBuilder()
                .WithAsset(asset);

            Assert.AreEqual(latitude, _target.Latitude);
        }

        [TestMethod]
        public void TestLatitudePropertyReturnsLatitudeValueFromHiddenFieldIfNoAssetPresent()
        {
            var expected = 1.5;

            using (_mocks.Record())
            {
                SetupResult.For(_hidLatitude.Value).Return(expected.ToString());
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Latitude);
            }
        }

        [TestMethod]
        public void TestSettingLatitudeSetsStringValueOfHiddenLatitudeField()
        {
            var expected = 1.1;

            using (_mocks.Record())
            {
                _hidLatitude.Value = expected.ToString();
            }

            using (_mocks.Playback())
            {
                _target.Latitude = expected;
            }
        }

        [TestMethod]
        public void TestLongitudePropertyReturnsLongitudeValueFromAssetIfPresent()
        {
            _mocks.ReplayAll();

            var longitude = 1.5;
            var asset = new TestAssetBuilder()
                .WithValve(new Valve {
                    Coordinate = new Coordinate { Longitude = longitude }
                })
                .Build();
            _target = new TestLatLonPickerBuilder()
                .WithAsset(asset);

            Assert.AreEqual(longitude, _target.Longitude);
        }

        [TestMethod]
        public void TestLongitudePropertyReturnsLongitudeValueFromHiddenFieldIfNoAssetPresent()
        {
            var expected = 2.5;

            using (_mocks.Record())
            {
                SetupResult.For(_hidLongitude.Value).Return(expected.ToString());
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Longitude);
            }
        }

        [TestMethod]
        public void TestSettingLongitudeSetsStringValueOfHiddenLongitudeField()
        {
            var expected = 2.1;

            using (_mocks.Record())
            {
                _hidLongitude.Value = expected.ToString();
            }

            using (_mocks.Playback())
            {
                _target.Longitude = expected;
            }
        }

        [TestMethod]
        public void TestStatePropertyReturnsValueFromHiddenStateFieldIfNotNullOrEmpty()
        {
            var state = "Disarray";

            using (_mocks.Record())
            {
                SetupResult.For(_hidState.Value).Return(state);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(state, _target.State);
            }
        }

        [TestMethod]
        public void TestStatePropertySetsHiddenFieldToAndReturnsDefaultStateValueIfNoValueSet()
        {
            var values = new[] {
                null, string.Empty
            };

            foreach (var value in values)
            {
                _target = new TestLatLonPickerBuilder()
                    .WithHIDState(_hidState)
                    .WithMockSetState(true);

                using (_mocks.Record())
                {
                    SetupResult.For(_hidState.Value).Return(value);
                }

                using (_mocks.Playback())
                {
                    // should test against 'expected' here, but mocking
                    // won't allow that to happen exactly.  however,
                    // the fact that we're setting up the expectation
                    // for _hidState.Value to be set, we should be ok
                    Assert.AreEqual(value, _target.State);
                    Assert.IsTrue(_target.SetStateCalled);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSettingStatePropertySetsValueOnHiddenStateField()
        {
            var expected = "TX";

            using (_mocks.Record())
            {
                _hidState.Value = expected;
            }

            using (_mocks.Playback())
            {
                _target.State = expected;
            }
        }

        [TestMethod]
        public void TestClientIDPropertyReturnsIDValueFormatted()
        {
            _mocks.ReplayAll();

            var id = "iddqd";
            var expected = String.Format(LatLonPicker.OUTER_DIV_ID_FORMAT, id);

            _target.ID = id;

            Assert.AreEqual(expected, _target.ClientID);
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetCoordinatesSetsHiddenLatAndLonFieldValuesIfValuesArePresent()
        {
            double latitude = 1.5, longitude = 2.5;
            var asset = new TestAssetBuilder()
                .WithValve(new Valve {
                    Coordinate = new Coordinate { 
                        Latitude = latitude,
                        Longitude = longitude
                    }
                })
                .Build();
            _target = new TestLatLonPickerBuilder()
                .WithAsset(asset)
                .WithHIDLatitude(_hidLatitude)
                .WithHIDLongitude(_hidLongitude);

            using (_mocks.Record())
            {
                _hidLatitude.Value = latitude.ToString();
                _hidLongitude.Value = longitude.ToString();
            }

            using (_mocks.Playback())
            {
                _target.ExposedSetCoordinateFields();
            }
        }

        [TestMethod]
        public void TestSetCoordinatesDoesNotSetHiddenLatAndLonFieldValuesIfNoValuesArePresent()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _hidLatitude.Value = null);
                LastCall.IgnoreArguments();
                DoNotExpect.Call(() => _hidLongitude.Value = null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.ExposedSetCoordinateFields();
            }
        }

        [TestMethod]
        public void TestSetIconUrlSetsIconUrlToGoodImageWhenHasCoordinates()
        {
            double latitude = 1.5, longitude = 2.5;
            var asset = new TestAssetBuilder()
                .WithValve(new Valve {
                    Coordinate = new Coordinate {
                        Latitude = latitude,
                        Longitude = longitude
                    }
                })
                .Build();
            _target = new TestLatLonPickerBuilder()
                .WithAsset(asset)
                .WithIMGShowPicker(_imgShowPicker);
            var expected = String.Format(LatLonPicker.IMAGE_PATH_FORMAT,
                LatLonPicker.ImageFileNames.GOOD);

            using (_mocks.Record())
            {
                _imgShowPicker.ImageUrl = expected;
            }

            using (_mocks.Playback())
            {
                _target.ExposedSetIconUrl();
            }
        }

        [TestMethod]
        public void TestSetIconUrlSetsIconUrlToBadImageWhenDoesNotHaveCoordinates()
        {
            var expected = String.Format(LatLonPicker.IMAGE_PATH_FORMAT,
                LatLonPicker.ImageFileNames.BAD);

            using (_mocks.Record())
            {
                _imgShowPicker.ImageUrl = expected;
            }

            using (_mocks.Playback())
            {
                _target.ExposedSetIconUrl();
            }
        }

        [TestMethod]
        public void TestSetStateSetsStateHiddenFieldValueToDefaultStateValue()
        {
            using (_mocks.Record())
            {
                _hidState.Value = LatLonPicker.DEFAULT_STATE_ABBREV;
            }

            using (_mocks.Playback())
            {
                _target.ExposedSetState();
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadSetsStateCoordinateFieldsIconUrl()
        {
            _mocks.ReplayAll();
            _target = new TestLatLonPickerBuilder()
                .WithMockSetState(true)
                .WithMockSetCoordinateFields(true)
                .WithMockSetIconUrl(true);

            InvokeEventByName(_target, "Page_Load");

            Assert.IsTrue(_target.SetStateCalled);
            Assert.IsTrue(_target.SetCoordinateFieldsCalled);
            Assert.IsTrue(_target.SetIconUrlCalled);
        }

        #endregion
    }

    internal class TestLatLonPickerBuilder : TestDataBuilder<TestLatLonPicker>
    {
        #region Private Members

        private Asset _asset;
        private IHiddenField _hidState, _hidLongitude, _hidLatitude;
        private bool _useMockSetState,
                     _useMockSetCoordinateFields,
                     _useMockSetIconUrl,
                     _useMockDataBind;
        private IImageButton _imgShowPicker;

        #endregion

        #region Exposed Methods

        public override TestLatLonPicker Build()
        {
            var obj = new TestLatLonPicker();
            if (_asset != null)
                obj.SetAsset(_asset);
            if (_hidState != null)
                obj.SetHIDState(_hidState);
            if (_hidLatitude != null)
                obj.SetHIDLatitude(_hidLatitude);
            if (_hidLongitude != null)
                obj.SetHIDLongitude(_hidLongitude);
            if (_imgShowPicker != null)
                obj.SetIMGShowPicker(_imgShowPicker);
            obj.UseMockSetState = _useMockSetState;
            obj.UseMockSetCoordinateFields = _useMockSetCoordinateFields;
            obj.UseMockSetIconUrl = _useMockSetIconUrl;
            obj.UseMockDataBind = _useMockDataBind;
            return obj;
        }

        public TestLatLonPickerBuilder WithAsset(Asset asset)
        {
            _asset = asset;
            return this;
        }

        public TestLatLonPickerBuilder WithHIDState(IHiddenField field)
        {
            _hidState = field;
            return this;
        }

        public TestLatLonPickerBuilder WithMockSetState(bool useMockSetState)
        {
            _useMockSetState = useMockSetState;
            return this;
        }

        public TestLatLonPickerBuilder WithMockSetCoordinateFields(bool b)
        {
            _useMockSetCoordinateFields = b;
            return this;
        }

        public TestLatLonPickerBuilder WithMockSetIconUrl(bool b)
        {
            _useMockSetIconUrl = b;
            return this;
        }

        public TestLatLonPickerBuilder WithMockDataBind(bool b)
        {
            _useMockDataBind = b;
            return this;
        }

        public TestLatLonPickerBuilder WithHIDLatitude(IHiddenField latitude)
        {
            _hidLatitude = latitude;
            return this;
        }

        public TestLatLonPickerBuilder WithHIDLongitude(IHiddenField longitude)
        {
            _hidLongitude = longitude;
            return this;
        }

        public TestLatLonPickerBuilder WithIMGShowPicker(IImageButton button)
        {
            _imgShowPicker = button;
            return this;
        }

        #endregion
    }

    internal class TestLatLonPicker : LatLonPicker
    {
        #region Properties

        public bool UseMockSetCoordinateFields { get; set; }
        public bool UseMockSetIconUrl { get; set; }
        public bool UseMockGetAsset { get; set; }
        public bool UseMockSetState { get; set; }
        public bool UseMockDataBind { get; set; }

        public bool SetCoordinateFieldsCalled { get; protected set; }
        public bool SetIconUrlCalled { get; protected set; }
        public bool GetAssetCalled { get; protected set; }
        public bool SetStateCalled { get; protected set; }
        public bool DataBindCalled { get; protected set; }

        public Asset MockAsset { get; set; }

        #endregion

        #region Private Methods

        protected override void SetCoordinateFields()
        {
            SetCoordinateFieldsCalled = true;
            if (!UseMockSetCoordinateFields)
                base.SetCoordinateFields();
        }

        protected override void SetIconUrl()
        {
            SetIconUrlCalled = true;
            if (!UseMockSetIconUrl)
                base.SetIconUrl();
        }

        protected override Asset GetAsset()
        {
            GetAssetCalled = true;
            return (UseMockGetAsset) ? MockAsset : base.GetAsset();
        }

        protected override void SetState()
        {
            SetStateCalled = true;
            if (!SetStateCalled)
                base.SetState();
        }

        #endregion

        #region Exposed Methods

        public override void DataBind()
        {
            DataBindCalled = true;
            if (!UseMockDataBind)
                base.DataBind();
        }

        public void SetAsset(Asset asset)
        {
            _asset = asset;
        }

        public void SetHIDState(IHiddenField field)
        {
            hidState = field;
        }

        public void ExposedSetCoordinateFields()
        {
            base.SetCoordinateFields();
        }

        public void ExposedSetIconUrl()
        {
            base.SetIconUrl();
        }

        public void ExposedSetState()
        {
            base.SetState();
        }

        public void SetHIDLatitude(IHiddenField latitude)
        {
            hidLatitude = latitude;
        }

        public void SetHIDLongitude(IHiddenField longitude)
        {
            hidLongitude = longitude;
        }

        public void SetIMGShowPicker(IImageButton button)
        {
            imgShowPicker = button;
        }

        #endregion
    }
}
