using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SewerOpeningTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpening>
    {
        #region Tests

        [TestMethod]
        public void TestToStringReturnsAbbreviation()
        {
            var expected = "11-22";
            var target = new SewerOpening {OpeningNumber = expected};

            Assert.AreEqual(expected, target.ToString());
        }

        [TestMethod]
        public void TestIsActiveReturnsTrueForAllActiveStatusIds()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll().ToDictionary(x => x.Id, x => x);
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var sm = new SewerOpening();

            foreach (var assetStatus in assetStatusesById)
            {
                sm.Status = assetStatus.Value;

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Key);
                Assert.AreEqual(expectedResult, sm.IsActive);
            }
        }

        [TestMethod]
        public void TestIsActiveReturnsFalseWhenStatusIsNull()
        {
            var sm = new SewerOpening();
            sm.Status = null;
            Assert.IsFalse(sm.IsActive);
        }

        [TestMethod]
        public void TestLastInspectionAndManholeCleaningDate()
        {
            var so = GetEntityFactory<SewerOpening>().Create();

            Assert.IsNull(so.LastInspectionDate);
            Assert.IsNull(so.LastManholeCleaningDate);

            var inspection1 = GetFactory<SewerOpeningInspectionFactory>().Create(new {
                SewerOpening = so
            });
            var inspection2 = GetFactory<SewerOpeningInspectionFactory>().Create(new {
                SewerOpening = so,
                DateInspected = DateTime.Now
            });
            var inspection3 = GetFactory<SewerOpeningInspectionFactory>().Create(new {
                SewerOpening = so,
                DateInspected = DateTime.Now.AddDays(-1)
            });

            var smc1 = GetFactory<SewerMainCleaningFactory>().Create(new {
                Opening1 = so
            });
            var smc2 = GetFactory<SewerMainCleaningFactory>().Create(new {
                Opening1 = so,
                InspectedDate = DateTime.Now.AddDays(-3)
            });
            var smc3 = GetFactory<SewerMainCleaningFactory>().Create(new {
                Opening2 = so,
                InspectedDate = DateTime.Now.AddDays(-2)
            });

            Session.Refresh(so);

            Assert.AreEqual(inspection2.DateInspected, so.LastInspectionDate);
            Assert.AreEqual(smc3.InspectedDate, so.LastManholeCleaningDate);
        }

        [TestMethod]
        public void TestIsInActiveReturnsTrueForAllInActiveStatusIds()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll().ToDictionary(x => x.Id, x => x);
            var expectedAssetStatusIds = AssetStatus.INACTIVE_STATUSES;

            var sm = new SewerOpening();

            foreach (var assetStatus in assetStatusesById)
            {
                sm.Status = assetStatus.Value;

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Key);
                Assert.AreEqual(expectedResult, sm.IsInactive);
            }
        }

        [TestMethod]
        public void TestIsInspectableReturnsExpectedValues()
        {
            var target = new SewerOpening {
                Status = new AssetStatus()
            };

            foreach (var status in SewerOpening.UNINSPECTABLE_STATUSES)
            {
                target.Status.Id = status;
                Assert.IsFalse(target.IsInspectable);
            }

            target.Status.Id = AssetStatus.Indices.ACTIVE;
            Assert.IsTrue(target.IsInspectable);
        }

        #endregion
    }
}
