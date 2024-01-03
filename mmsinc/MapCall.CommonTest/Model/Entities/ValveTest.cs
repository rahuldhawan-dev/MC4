using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ValveTest : MapCallMvcInMemoryDatabaseTestBase<Valve>
    {
        [TestMethod]
        public void TestToStringReturnsValNum()
        {
            var expected = "Val-1";
            var target = new Valve {ValveNumber = expected};

            Assert.AreEqual(expected, target.ToString());
        }

        [TestMethod]
        public void TestDescriptionReturnsValNum()
        {
            var expected = "Val-1";
            var target = new Valve {ValveNumber = expected};

            Assert.AreEqual(expected, target.Description);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsExpectedValues()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                LastInspectionDate = DateTime.Now,
                LastNonInspectionDate = DateTime.Today,
                RequiresInspection = true,
                RequiresBlowOffInspection = false,
                Coordinate = new Coordinate {
                    Latitude = 1,
                    Longitude = 2
                }
            };
            target.SetPropertyValueByName("Id", 3);
            target.SetPropertyValueByName("CanHaveBlowOffInspections", false);
            var result = target.ToAssetCoordinate();

            Assert.AreEqual(3, result.Id);
            Assert.AreEqual(target.LastInspectionDate, result.LastInspection);
            Assert.AreEqual(target.RequiresInspection, result.RequiresInspection);
            Assert.IsTrue(result.IsPublic);
            Assert.AreEqual(target.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(target.Coordinate.Longitude, result.Longitude);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsExpectedValueForIsActiveForActiveStatuses()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                LastInspectionDate = DateTime.Now,
                LastNonInspectionDate = DateTime.Today,
                RequiresInspection = true,
                RequiresBlowOffInspection = false,
                Coordinate = new Coordinate {
                    Latitude = 1,
                    Longitude = 2
                }
            };

            foreach (var assetStatus in assetStatusesById)
            {
                target.Status = assetStatus;

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Id);
                var result = target.ToAssetCoordinate().IsActive;
                Assert.AreEqual(expectedResult, result, $"Wrong result for asset status: {assetStatus.Description}");
            }
        }

        [TestMethod]
        public void TestToAssetCoordinateDoesNotSetLatitudeAndLongitudeIfCoordinateIsNull()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                Coordinate = null
            };

            var result = target.ToAssetCoordinate();
            Assert.IsNull(result.Latitude);
            Assert.IsNull(result.Longitude);
            Assert.IsNull(result.Coordinate);
        }

        [TestMethod]
        public void TestToAssetCoordinateUsesRequiresBlowOffInspectionIfValveCanHaveBlowOffInspections()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                RequiresInspection = false,
                RequiresBlowOffInspection = true
            };
            target.SetPropertyValueByName("CanHaveBlowOffInspections", true);

            var expectedDate = DateTime.Now.AddDays(1);
            var otherExpectedDate = expectedDate.AddDays(10);
            target.LastBlowOffInspectionDate = expectedDate;
            target.LastBlowOffNonInspectionDate = otherExpectedDate;
            var result = target.ToAssetCoordinate(false);
            Assert.IsTrue(result.RequiresInspection);
            Assert.AreEqual(expectedDate, result.LastInspection);
            //  Assert.AreEqual(otherExpectedDate, result.LastNonInspection);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsValveAssetCoordinateWhenCanHaveBlowOffInspectionsIsFalse()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
            };
            target.SetPropertyValueByName("CanHaveBlowOffInspections", false);

            var result = target.ToAssetCoordinate();
            Assert.IsInstanceOfType(result, typeof(ValveAssetCoordinate));
        }

        [TestMethod]
        public void TestToAssetCoordinateHandlesNormalPositionCorrectly()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
            };
            target.NormalPosition = null;

            var result = target.ToAssetCoordinate();
            Assert.IsNull(result.InNormalPosition, "If NormalPosition is not set, InNormalPosition must be null.");
            Assert.IsNull(result.NormalPosition, "This should not have been set since there is not a NormalPosition.");

            target.NormalPosition = new ValveNormalPosition {Description = "Open"};
            target.InNormalPosition = true;

            result = target.ToAssetCoordinate();
            Assert.IsTrue(result.InNormalPosition.Value,
                "InNormalPosition should be equal to Valve.InNormalPosition when NormalPosition has a value.");
            Assert.AreEqual(result.NormalPosition.Description, target.NormalPosition.Description,
                "NormalPosition.Description should also be set.");

            target.InNormalPosition = false;
            result = target.ToAssetCoordinate();
            Assert.IsFalse(result.InNormalPosition.Value,
                "Sanity in case something weird happens to this formula property.");
        }

        [TestMethod]
        public void TestToAssetCoordinateDoesNotDoAnythingWithNormalPositionIfBlowOffValve()
        {
            var target = new Valve {
                ValveBilling = new ValveBilling {Id = ValveBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
            };
            target.SetPropertyValueByName("CanHaveBlowOffInspections", true);
            target.NormalPosition = new ValveNormalPosition {Description = "Open"};
            target.InNormalPosition = true;

            var result = target.ToAssetCoordinate();
            Assert.IsNull(result.InNormalPosition);
            Assert.IsNull(result.NormalPosition);
        }

        [TestMethod]
        public void TestDefaultImagesLoadsTheDefaultImageOrTheLatestImage()
        {
            var target = new Valve();
            var valveImage = new ValveImage();
            valveImage.SetPropertyValueByName("Id", 1);
            // no image yet, make sure it's null
            Assert.IsNull(target.DefaultValveImage);

            target.ValveImages.Add(valveImage);
            // added image, not default but only one so it should be the default
            Assert.AreEqual(valveImage, target.DefaultValveImage);

            var defaultValveImage = new ValveImage {IsDefaultImageForValve = true};
            target.ValveImages.Add(defaultValveImage);
            // added default one, make sure it's the one
            Assert.AreEqual(defaultValveImage, target.DefaultValveImage);

            var newerValveImage = new ValveImage();
            newerValveImage.SetPropertyValueByName("Id", 3);
            target.ValveImages.Add(newerValveImage);
            // added another one, to make sure default is still the one
            Assert.AreEqual(defaultValveImage, target.DefaultValveImage);

            target.ValveImages.Remove(defaultValveImage);
            //Removed the default image, leaving only the two and the latest one should be the default now
            Assert.AreEqual(newerValveImage, target.DefaultValveImage);
            Assert.AreNotEqual(defaultValveImage, target.DefaultValveImage);
        }

        [TestMethod]
        public void TestMinimumRequiredTurnsReturnsTurnsIfLessThanOrEqualToOne()
        {
            var turns = 1;
            var target = new Valve {Turns = turns};

            Assert.AreEqual(turns, target.MinimumRequiredTurns);

            target.Turns = .25m;
            Assert.AreEqual(.25m, target.MinimumRequiredTurns);
        }

        [TestMethod]
        public void TestMinimumRequiredTurnsReturnsTwentyPercentIfMoreThanOne()
        {
            var turns = 51;
            var target = new Valve {Turns = turns};

            Assert.AreEqual(11, target.MinimumRequiredTurns);

            target.Turns = 592;
            Assert.AreEqual(119, target.MinimumRequiredTurns);

            target.Turns = 350;
            Assert.AreEqual(70, target.MinimumRequiredTurns);

            target.Turns = 271;
            Assert.AreEqual(55, target.MinimumRequiredTurns);

            target.Turns = 49;
            Assert.AreEqual(10, target.MinimumRequiredTurns);
        }

        [TestMethod]
        public void TestIsInspectableReturnsExpectedValues()
        {
            var target = new Valve {
                Status = new AssetStatus()
            };

            target.Status.Id = AssetStatus.Indices.CANCELLED;
            Assert.IsFalse(target.IsInspectable);
            target.Status.Id = AssetStatus.Indices.REMOVED;
            Assert.IsFalse(target.IsInspectable);
            target.Status.Id = AssetStatus.Indices.RETIRED;
            Assert.IsFalse(target.IsInspectable);
            target.Status.Id = AssetStatus.Indices.INACTIVE;
            Assert.IsFalse(target.IsInspectable);
            target.Status.Id = AssetStatus.Indices.ACTIVE;
            Assert.IsTrue(target.IsInspectable);
        }

        [TestMethod]
        public void TestCanBeCopiedReturnsCorrectly()
        {
            foreach (var x in typeof(AssetStatus.Indices).GetFields())
            {
                var valveStatusId = (int)x.GetValue(x);
                var valveStatus = new AssetStatus {Id = valveStatusId};
                var valve = new Valve {Status = valveStatus, Turns = 1, ValveSize = new ValveSize()};

                if (AssetStatus.CanBeCopiedStatuses.Contains(valveStatusId))
                    Assert.IsTrue(valve.CanBeCopied);
                else
                    Assert.IsFalse(valve.CanBeCopied);
            }
        }

        [TestMethod]
        public void TestCanBeCopiedReturnsFalseWhenTurnsIsMissing()
        {
            var target = new Valve
                {ValveSize = new ValveSize(), Status = new AssetStatus {Id = AssetStatus.CanBeCopiedStatuses.First()}};

            Assert.IsFalse(target.CanBeCopied);
        }

        [TestMethod]
        public void TestCanBeCopiedReturnsFalseWhenValveSizeIsMissing()
        {
            var target = new Valve {
                Turns = 21,
                Status = new AssetStatus {Id = AssetStatus.CanBeCopiedStatuses.First()}
            };

            Assert.IsFalse(target.CanBeCopied);
        }

        [TestMethod]
        public void TestIsActiveReturnsTrueForActiveAssetStatuses()
        {
            var status = new AssetStatus {Id = 0};
            var target = new Valve {
                Status = status
            };

            Assert.IsFalse(target.IsActive);

            foreach (var active in AssetStatus.ACTIVE_STATUSES)
            {
                status.Id = active;
                Assert.IsTrue(target.IsActive);
            }
        }
    }
}
