using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MainCrossingTest
    {
        #region Fields

        private Mock<IIconSetRepository> _repo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _repo = new Mock<IIconSetRepository>();
            _container = new Container();
            _container.Inject(_repo.Object);
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsWithBodyOfWaterIfBodyOfWaterIsNotNull()
        {
            var target = new MainCrossing {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "XX1"},
                Town = new Town {ShortName = "Short Town Name"},
                BodyOfWater = new BodyOfWater {Name = "Body of Water Name"}
            };

            Assert.AreEqual("CR0 - XX1 - Short Town Name - Body of Water Name", target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsWithStreetIfStreetIsNotNull()
        {
            var target = new MainCrossing {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "XX1"},
                Town = new Town {ShortName = "Short Town Name"},
                Street = new Street {FullStName = "Some Street"}
            };

            Assert.AreEqual("CR0 - XX1 - Short Town Name - Some Street", target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsWithStreetXStreetIfStreetXStreetAreNotNull()
        {
            var target = new MainCrossing {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "XX1"},
                Town = new Town {ShortName = "Short Town Name"},
                Street = new Street {FullStName = "Some Street"},
                ClosestCrossStreet = new Street {FullStName = "Ohter St."}
            };

            Assert.AreEqual("CR0 - XX1 - Short Town Name - Some Street - Ohter St.", target.ToString());
        }

        [TestMethod]
        public void TestToStringDoesNotReturnExtraInfoAtTheEndWhenBodyOfWaterAndClosestCrossStreetAreBothNull()
        {
            var target = new MainCrossing {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "XX1"},
                Town = new Town {ShortName = "Short Town Name"},
            };

            Assert.AreEqual("CR0 - XX1 - Short Town Name", target.ToString());
        }

        [TestMethod]
        public void TestIconReturnsNullIfCoordinateIsNull()
        {
            var target = new MainCrossing();
            target.Coordinate = null;
            Assert.IsNull(target.Icon);
        }

        [TestMethod]
        public void TestIconReturnsRedIconIfRequiresInspectionGreenOtherwise()
        {
            var expectedIconSet = new IconSet();
            var redIcon = new MapIcon {FileName = "MapIcons/maincrossing-red.png"};
            var greenIcon = new MapIcon {FileName = "MapIcons/maincrossing-green.png"};
            expectedIconSet.Icons.Add(redIcon);
            expectedIconSet.Icons.Add(greenIcon);

            _repo.Setup(x => x.Find(IconSets.Assets)).Returns(expectedIconSet);

            var coordinate = new Coordinate();
            var target = new MainCrossing {
                Coordinate = coordinate,
            };
            _container.BuildUp(target);
            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(target,
                "RequiresInspection", true);
            Assert.AreSame(redIcon, target.Icon);

            MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions.SetPropertyValueByName(target,
                "RequiresInspection", false);
            Assert.AreSame(greenIcon, target.Icon);
        }
    }
}
