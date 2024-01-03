using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.ViewModels
{
    [TestClass]
    public class AssetCoordinateTest
    {
        #region Fields

        private MapIcon _redBlackIcon,
                        _grayIcon,
                        _greenBlackIcon,
                        _greenIcon,
                        _blueIcon,
                        _redIcon,
                        _yellowIcon,
                        _yellowBlackIcon,
                        _purpleWhiteIcon,
                        _purpleBlackIcon,
                        _orangeWhiteIcon,
                        _orangeBlackIcon;

        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            var repo = new Mock<IIconSetRepository>();
            _container = new Container();
            _container.Inject(repo.Object);

            _redBlackIcon = new MapIcon {FileName = "MapIcons/blowoff-redblack.png"};
            _blueIcon = new MapIcon {FileName = "MapIcons/blowoff-blue.png"};
            _greenIcon = new MapIcon {FileName = "MapIcons/blowoff-green.png"};
            _redIcon = new MapIcon {FileName = "MapIcons/blowoff-red.png"};
            _greenBlackIcon = new MapIcon {FileName = "MapIcons/blowoff-greenblack.png"};
            _grayIcon = new MapIcon {FileName = "MapIcons/blowoff-gray.png"};
            _yellowIcon = new MapIcon {FileName = "MapIcons/blowoff-yellow.png"};
            _yellowBlackIcon = new MapIcon {FileName = "MapIcons/blowoff-yellowblack.png"};
            _purpleWhiteIcon = new MapIcon {FileName = "MapIcons/blowoff-purplewhite.png"};
            _purpleBlackIcon = new MapIcon {FileName = "MapIcons/blowoff-purpleblack.png"};
            _orangeWhiteIcon = new MapIcon {FileName = "MapIcons/blowoff-orangewhite.png"};
            _orangeBlackIcon = new MapIcon {FileName = "MapIcons/blowoff-orangeblack.png"};

            var iconSet = new IconSet();
            iconSet.Icons.Add(_redBlackIcon);
            iconSet.Icons.Add(_blueIcon);
            iconSet.Icons.Add(_greenIcon);
            iconSet.Icons.Add(_redIcon);
            iconSet.Icons.Add(_greenBlackIcon);
            iconSet.Icons.Add(_grayIcon);
            iconSet.Icons.Add(_yellowBlackIcon);
            iconSet.Icons.Add(_yellowIcon);
            iconSet.Icons.Add(_purpleBlackIcon);
            iconSet.Icons.Add(_purpleWhiteIcon);
            iconSet.Icons.Add(_orangeBlackIcon);
            iconSet.Icons.Add(_orangeWhiteIcon);

            repo.Setup(x => x.Find(IconSets.Assets)).Returns(iconSet);
        }

        #endregion

        [TestMethod]
        public void TestCoordinatePropertyReturnsNullIfEitherLatitudeOrLongitudeAreNull()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.Latitude = null;
            target.Longitude = null;

            Assert.IsNull(target.Coordinate);

            // Reinitialize because Coordinate is a readonly property that's lazy-initialized.

            target = _container.GetInstance<TestAssetCoordinate>();
            target.Latitude = null;
            target.Longitude = 10;

            Assert.IsNull(target.Coordinate);

            target = _container.GetInstance<TestAssetCoordinate>();
            target.Latitude = 10;
            target.Longitude = null;

            Assert.IsNull(target.Coordinate);
        }

        [TestMethod]
        public void TestCoordinateReturnsCoordinateInstanceWithLatitudeAndLongitudeValues()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.Latitude = 10;
            target.Longitude = 20;

            var result = target.Coordinate;

            Assert.AreEqual(target.Latitude, result.Latitude);
            Assert.AreEqual(target.Longitude, result.Longitude);
        }

        [TestMethod]
        public void TestGrayIconIsReturnedForInactiveAssets()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = false;
            Assert.AreSame(_grayIcon, target.Icon);
            Assert.AreEqual(AssetIconType.Inactive, target.IconType);
        }

        [TestMethod]
        public void TestRedBlackIconIsReturnedForAssetsThatRequireInspectionsAndWorkOrders()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.RequiresInspection = true;
            target.HasOpenWorkOrder = true; // Just needs a value
            Assert.IsTrue(target.RequiresWorkOrder);
            Assert.AreSame(_redBlackIcon, target.Icon);
            Assert.AreEqual(AssetIconType.RequiresInspectionWithWorkOrder, target.IconType);
        }

        [TestMethod]
        public void TestGreenBlackIconIsReturnedForWorkOrderAssets()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.RequiresInspection = false;
            target.HasOpenWorkOrder = true; // Just needs to have a value
            // target.LastInspection = DateTime.Today.AddDays(-1);
            //  target.LastNonInspection = DateTime.Today;
            Assert.IsTrue(target.RequiresWorkOrder);
            Assert.AreSame(_greenBlackIcon, target.Icon);
            Assert.AreEqual(AssetIconType.WorkOrder, target.IconType);
        }

        [TestMethod]
        public void TestRedIconIsReturnedForAssetsThatRequireInspections()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.RequiresInspection = true;
            Assert.AreSame(_redIcon, target.Icon);
            Assert.AreEqual(AssetIconType.RequiresInspection, target.IconType);
        }

        [TestMethod]
        public void TestBlueIconIsReturnedForNonPublicAssets()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.IsPublic = false;
            Assert.AreSame(_blueIcon, target.Icon);
            Assert.AreEqual(AssetIconType.NonPublic, target.IconType);
        }

        [TestMethod]
        public void TestGreenIconIsReturnedForPublicActiveAssetsThatDoNotRequireInspectionOrWorkOrder()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.IsPublic = true;
            Assert.AreSame(_greenIcon, target.Icon);
            Assert.AreEqual(AssetIconType.Default, target.IconType);
        }

        [TestMethod]
        public void TestYellowIconIsReturnedForAssetsThatAreOutOfServiceWithoutAWorkOrder()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.OutOfService = true;
            Assert.AreSame(_yellowIcon, target.Icon);
            Assert.AreEqual(AssetIconType.OutOfService, target.IconType);
        }

        [TestMethod]
        public void TestYellowBlackIconIsReturnedForAssetsThatAreOutOfServiceWithAWorkOrder()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.OutOfService = true;
            target.HasOpenWorkOrder = true; // Just needs to have a value
            Assert.IsTrue(target.RequiresWorkOrder, "Sanity");
            Assert.AreSame(_yellowBlackIcon, target.Icon);
            Assert.AreEqual(AssetIconType.OutOfServiceWithWorkOrder, target.IconType);
        }

        [TestMethod]
        public void TestPurpleWhiteIconIsReturnedWhenValvePositionIsNormallyOpenButIsClosed()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.InNormalPosition = false;
            target.NormalPosition = new ValveNormalPosition { Id = ValveNormalPosition.Indices.OPEN };
            Assert.IsFalse(target.RequiresWorkOrder, "Sanity");
            Assert.AreSame(_purpleWhiteIcon, target.Icon);
            Assert.AreEqual(AssetIconType.NormallyOpenButClosed, target.IconType);
        }

        [TestMethod]
        public void TestPurpleBlackcIconIsReturnedWhenValvePositionIsNormallyOpenButIsClosedAndAlsoRequiresAWorkOrder()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.InNormalPosition = false;
            target.HasOpenWorkOrder = true;
            target.NormalPosition = new ValveNormalPosition { Id = ValveNormalPosition.Indices.OPEN };
            Assert.IsTrue(target.RequiresWorkOrder, "Sanity");
            Assert.AreSame(_purpleBlackIcon, target.Icon);
            Assert.AreEqual(AssetIconType.NormallyOpenButClosedWithWorkOrder, target.IconType);
        }

        [TestMethod]
        public void TestOrangeWhiteIconIsReturnedWhenValvePositionIsNormallyClosedButIsOpen()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.InNormalPosition = false;
            target.NormalPosition = new ValveNormalPosition { Id = ValveNormalPosition.Indices.CLOSED };
            Assert.IsFalse(target.RequiresWorkOrder, "Sanity");
            Assert.AreSame(_orangeWhiteIcon, target.Icon);
            Assert.AreEqual(AssetIconType.NormallyClosedButOpen, target.IconType);
        }

        [TestMethod]
        public void TestOrangeWhiteIconIsReturnedWhenValvePositionIsNormallyClosedButIsOpenAndRequiresAWorkOorder()
        {
            var target = _container.GetInstance<TestAssetCoordinate>();
            target.IsActive = true;
            target.InNormalPosition = false;
            target.HasOpenWorkOrder = true;
            target.NormalPosition = new ValveNormalPosition { Id = ValveNormalPosition.Indices.CLOSED };
            Assert.IsTrue(target.RequiresWorkOrder, "Sanity");
            Assert.AreSame(_orangeBlackIcon, target.Icon);
            Assert.AreEqual(AssetIconType.NormallyClosedButOpenWithWorkOrder, target.IconType);
        }

        private class TestAssetCoordinate : AssetCoordinate
        {
            public override AssetCoordinateType AssetType => AssetCoordinateType.BlowOff;

            public TestAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }
        }
    }
}
