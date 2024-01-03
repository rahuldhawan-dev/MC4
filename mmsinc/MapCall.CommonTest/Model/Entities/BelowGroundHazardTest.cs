using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class BelowGroundHazardTest : MapCallMvcInMemoryDatabaseTestBase<BelowGroundHazard>
    {
        #region Tests

        [TestMethod]
        public void TestToAssetCoordinateDoesNotSetLatitudeAndLongitudeIfCoordinateIsNull()
        {
            var target = new BelowGroundHazard {
                Coordinate = null,
                AssetStatus = new AssetStatus {
                    Id = AssetStatus.Indices.ACTIVE
                }
            };

            var result = target.ToAssetCoordinate();

            Assert.IsNull(result.Latitude);
            Assert.IsNull(result.Longitude);
            Assert.IsNull(result.Coordinate);
            Assert.IsTrue(result.IsActive);
            Assert.IsTrue(result.IsPublic);
            Assert.AreEqual(target.Id, result.Id);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsExpectedValueForIsActiveForActiveStatuses()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var target = new BelowGroundHazard {
                AssetStatus = new AssetStatus(),
                Coordinate = new Coordinate {
                    Latitude = 1,
                    Longitude = 2
                }
            };

            foreach (var assetStatus in assetStatusesById)
            {
                target.AssetStatus = assetStatus;

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Id);
                var result = target.ToAssetCoordinate();
                Assert.AreEqual(expectedResult, result.IsActive, $"Wrong result for asset status {assetStatus.Description}");
                Assert.AreEqual(target.Coordinate.Latitude, result.Latitude);
                Assert.AreEqual(target.Coordinate.Longitude, result.Longitude);
            }
        }

        #endregion
    }
}
