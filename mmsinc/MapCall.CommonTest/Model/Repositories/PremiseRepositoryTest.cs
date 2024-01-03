using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class PremiseRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<Premise, PremiseRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            foreach (var id in new[] {
                         MapIcon.Indices.PREMISE_BLUE,
                         MapIcon.Indices.PREMISE_ORANGE,
                         MapIcon.Indices.PREMISE_MABLUE,
                         MapIcon.Indices.PREMISE_BLUE_ORANGE
                     })
            {
                GetEntityFactory<MapIcon>().Create(new {
                    Id = id
                });
            }
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Test_FindByPremiseNumber_ReturnsPremisesThatMatchByPremiseNumber()
        {
            var prem1 = GetEntityFactory<Premise>().Create(new {PremiseNumber = "123456789"});
            var prem2 = GetEntityFactory<Premise>().Create(new {PremiseNumber = "123456789"});
            var prem3 = GetEntityFactory<Premise>().Create(new {PremiseNumber = "000000000"});

            var result = Repository.FindByPremiseNumber("123456789");
            Assert.IsTrue(result.Contains(prem1));
            Assert.IsTrue(result.Contains(prem2));
            Assert.IsFalse(result.Contains(prem3));

            // Also make sure trimming works
            result = Repository.FindByPremiseNumber("  123456789  ");
            Assert.IsTrue(result.Contains(prem1));
            Assert.IsTrue(result.Contains(prem2));
            Assert.IsFalse(result.Contains(prem3));
        }

        [TestMethod]
        public void Test_FindActivePremiseByPremiseNumberAndInstallation_ReturnsActivePremisesThatMatchByPremiseNumberAndInstallation()
        {
            // all 3 matching
            var prem1 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "123456789", StatusCode = GetFactory<ActivePremiseStatusCodeFactory>().Create(), DeviceLocation = "device loc1", Installation = "install1" });
            var prem2 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "123456789", StatusCode = GetFactory<InactivePremiseStatusCodeFactory>().Create(), DeviceLocation = "device loc1", Installation = "install1" });
            var prem3 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "000000000" });

            var result = Repository.FindActivePremiseByPremiseNumberDeviceLocationAndInstallation("123456789", "device loc1", "install1");
            Assert.IsTrue(result.Contains(prem1));
            Assert.IsFalse(result.Contains(prem2));
            Assert.IsFalse(result.Contains(prem3));

            // Also make sure trimming works
            result = Repository.FindActivePremiseByPremiseNumberDeviceLocationAndInstallation("  123456789  ", "device loc1", "install1");
            Assert.IsTrue(result.Contains(prem1));
            Assert.IsFalse(result.Contains(prem2));
            Assert.IsFalse(result.Contains(prem3));

            // combination of DeviceLocation with PremiseNumber matching
            var prem4 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "9876", StatusCode = GetFactory<ActivePremiseStatusCodeFactory>().Create(), DeviceLocation = "device loc2" });
            var prem5 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "9876", StatusCode = GetFactory<InactivePremiseStatusCodeFactory>().Create(), DeviceLocation = "device loc2" });
            result = Repository.FindActivePremiseByPremiseNumberDeviceLocationAndInstallation("9876", "device loc2", null);
            Assert.IsTrue(result.Contains(prem4));
            Assert.IsFalse(result.Contains(prem5));
            Assert.IsFalse(result.Contains(prem3));

            // combination of Installation with PremiseNumber matching
            var prem6 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "4567", StatusCode = GetFactory<ActivePremiseStatusCodeFactory>().Create(), Installation = "install2" });
            var prem7 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "4567", StatusCode = GetFactory<InactivePremiseStatusCodeFactory>().Create(), Installation = "install2" });
            result = Repository.FindActivePremiseByPremiseNumberDeviceLocationAndInstallation("4567", null, "install2");
            Assert.IsTrue(result.Contains(prem6));
            Assert.IsFalse(result.Contains(prem7));
            Assert.IsFalse(result.Contains(prem3));

            // only PremiseNumber matching
            var prem8= GetEntityFactory<Premise>().Create(new { PremiseNumber = "6789", StatusCode = GetFactory<ActivePremiseStatusCodeFactory>().Create() });
            var prem9 = GetEntityFactory<Premise>().Create(new { PremiseNumber = "6789", StatusCode = GetFactory<InactivePremiseStatusCodeFactory>().Create() });
            result = Repository.FindActivePremiseByPremiseNumberDeviceLocationAndInstallation("6789", null, null);
            Assert.IsTrue(result.Contains(prem8));
            Assert.IsFalse(result.Contains(prem9));
            Assert.IsFalse(result.Contains(prem3));
        }

        [TestMethod]
        public void Test_SearchForMap_GathersPremisesWithTheirCoordinatesAndMapIconsAndMapIconOffsets()
        {
            var prem = GetEntityFactory<Premise>().Create(new {
                Coordinate = GetEntityFactory<Coordinate>().Create(new {
                    Icon = GetEntityFactory<MapIcon>().Create(new {
                        Offset = GetEntityFactory<MapIconOffset>().Create()
                    })
                })
            });

            var result = Repository.SearchForMap(new TestSearchPremiseForMap()).Single();
            
            Assert.AreEqual(prem.Id, result.Id);
            Assert.AreEqual(prem.Coordinate.Latitude, result.Coordinate.Latitude);
            Assert.AreEqual(prem.Coordinate.Longitude, result.Coordinate.Longitude);
            Assert.AreEqual(prem.Coordinate.Icon.FileName, result.Coordinate.Icon.FileName);
            Assert.AreEqual(prem.Coordinate.Icon.Height, result.Coordinate.Icon.Height);
            Assert.AreEqual(prem.Coordinate.Icon.Width, result.Coordinate.Icon.Width);
            Assert.AreEqual(
                prem.Coordinate.Icon.Offset.Description,
                result.Coordinate.Icon.Offset.Description);
        }

        [TestMethod]
        public void Test_SearchForMap_GathersPremisesWithTheirCoordinatesWithNoIcons()
        {
            var prem = GetEntityFactory<Premise>().Create(new {
                Coordinate = GetEntityFactory<Coordinate>().Create(new {
                    Icon = (MapIcon)null
                })
            });

            var result = Repository.SearchForMap(new TestSearchPremiseForMap()).Single();
            
            Assert.AreEqual(prem.Id, result.Id);
            Assert.AreEqual(prem.Coordinate.Latitude, result.Coordinate.Latitude);
            Assert.AreEqual(prem.Coordinate.Longitude, result.Coordinate.Longitude);
            Assert.IsNull(prem.Coordinate.Icon);
        }

        [TestMethod]
        public void Test_SearchForMap_FiltersPremisesWhichHaveNoCoordinates()
        {
            var prem = GetEntityFactory<Premise>().Create(new {
                Coordinate = (Coordinate)null
            });

            var result = Repository.SearchForMap(new TestSearchPremiseForMap());
            
            Assert.AreEqual(0, result.Count());
        }

        #endregion
        
        private class TestSearchPremiseForMap : SearchSet<PremiseCoordinate>, ISearchPremiseForMap { }
    }
}
