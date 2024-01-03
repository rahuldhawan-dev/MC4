using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.Utilities;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class HydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Private Members

        private DateTime _now;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now = DateTime.Now);
        }

        #endregion

        #region Nested Type: TestHydrantDueInspection

        private class TestHydrantDueInspection
        {
            #region Properties

            public string OpCntr { get; set; }
            public string OpCntrUnit { get; set; }
            public int? OpCntrFreq { get; set; }
            public string Number { get; set; }
            public string Unit { get; set; }
            public int? Freq { get; set; }
            public DateTime? LastInspection { get; set; }
            public int? Zone1Year { get; set; }
            public int? Zone { get; set; }
            public object Billing { get; set; } = typeof(PublicHydrantBillingFactory);
            public object Status { get; set; } = typeof(ActiveAssetStatusFactory);
            public bool IsNonBPUKPI { get; set; }
            public bool DueInspection { get; set; }
            public string Note { get; set; }

            #endregion
        }

        #endregion

        #region Private Methods

        private Hydrant GetHydrant(TestHydrantDueInspection obj)
        {
            // create recurring inspection frequency units
            var opCntrFreqUnit = obj.OpCntrUnit == "Y" ? GetFactory<YearlyRecurringFrequencyUnitFactory>().Create() :
                obj.OpCntrUnit == "M" ? GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create() :
                GetFactory<DailyRecurringFrequencyUnitFactory>().Create();
            var hydFreqUnit = obj.Unit == "Y" ? GetFactory<YearlyRecurringFrequencyUnitFactory>().Create() :
                obj.Unit == "M" ? GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create() :
                GetFactory<DailyRecurringFrequencyUnitFactory>().Create();

            //create operating center
            var opCntr = GetEntityFactory<OperatingCenter>().Create(new {
                OperatingCenterCode = obj.OpCntr,
                HydrantInspectionFrequencyUnit = opCntrFreqUnit,
                HydrantInspectionFrequency = obj.OpCntrFreq,
                ZoneStartYear = obj.Zone1Year
            });

            //create hydrant
            var hydrant = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = opCntr,
                HydrantNumber = obj.Number,
                InspectionFrequencyUnit = hydFreqUnit,
                InspectionFrequency = obj.Freq,
                HydrantBilling = obj.Billing,
                Status = obj.Status ?? typeof(ActiveAssetStatusFactory),
                obj.IsNonBPUKPI,
                obj.Zone
            });

            //create inspection
            if (obj.LastInspection != null)
            {
                GetEntityFactory<HydrantInspection>()
                   .Create(new {Hydrant = hydrant, DateInspected = obj.LastInspection});
            }

            // clear out the hydrant
            Session.Evict(hydrant);

            //return a fresh copy of it
            // Don't use Session.Load here. It returns a proxy object that causes
            // weird issues with every property being null if you try to debug the
            // test.
            return Session.Get<Hydrant>(hydrant.Id);
        }

        #endregion

        [TestMethod]
        public void TestCurrentOpenOutOfServiceRecordReturnsNullIfTheHydrantIsInService()
        {
            var target = new Hydrant();
            var oos = new HydrantOutOfService {OutOfServiceDate = DateTime.Today, BackInServiceDate = DateTime.Today};
            target.OutOfServiceRecords.Add(oos);
            Assert.IsNull(target.CurrentOpenOutOfServiceRecord);
        }

        [TestMethod]
        public void TestCurrentOpenOutOfServiceRecordReturnsNullIfThereAreNoRecords()
        {
            var target = new Hydrant();
            Assert.AreEqual(0, target.OutOfServiceRecords.Count);
            Assert.IsNull(target.CurrentOpenOutOfServiceRecord);
        }

        [TestMethod]
        public void TestCurrentOpenOutOfServiceRecordReturnsRecordThatIsCurrentlyOpen()
        {
            var target = new Hydrant();
            var early = new HydrantOutOfService {OutOfServiceDate = DateTime.Today};
            var middle = new HydrantOutOfService {OutOfServiceDate = DateTime.Today.AddDays(1)};
            var expected = new HydrantOutOfService {OutOfServiceDate = DateTime.Today.AddDays(2)};

            // Add these out of order to ensure they get sorted.
            target.OutOfServiceRecords.Add(middle);
            target.OutOfServiceRecords.Add(expected);
            target.OutOfServiceRecords.Add(early);

            Assert.AreSame(expected, target.CurrentOpenOutOfServiceRecord);
        }

        [TestMethod]
        public void TestHydrantDueInspectionReturnsCorrectlyForTruthTable()
        {
            // TODO: This test is failing bceause the view needs to be updated.
            GetFactory<MunicipalHydrantBillingFactory>().Create();
            GetFactory<PublicHydrantBillingFactory>().Create();
            var now = DateTime.Now;
            var theTruth = new[] {
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = null, Freq = null,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1,
                    Billing = typeof(MunicipalHydrantBillingFactory), DueInspection = false,
                    Note = "Billing not public or private"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1,
                    Billing = typeof(MunicipalHydrantBillingFactory), DueInspection = true,
                    Note = "Billing not public or private but frequency set"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, Status = typeof(ActiveAssetStatusFactory),
                    DueInspection = true, Note = "Active assets are active"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1,
                    Status = typeof(RequestRetirementAssetStatusFactory), DueInspection = true,
                    Note = "Request retirement assets are active"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1,
                    Status = typeof(RequestCancellationAssetStatusFactory), DueInspection = true,
                    Note = "Request cancellation assets are active"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, Status = typeof(InstalledAssetStatusFactory),
                    DueInspection = false, Note = "Installed asset status is not active"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, Status = typeof(CancelledAssetStatusFactory),
                    DueInspection = false, Note = "Status not active"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, IsNonBPUKPI = true, DueInspection = false,
                    Note = "Non-BPUKPI"
                },

                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-1", Unit = "Y", Freq = 1,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, DueInspection = true,
                    Note = "Yearly, never inspected"
                },

                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-2", Unit = "Y", Freq = 1,
                    LastInspection = new DateTime(now.Year, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = false, Note = "Yearly, inspected this year"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-3", Unit = "Y", Freq = 1,
                    LastInspection = new DateTime(now.Year - 1, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = true, Note = "Yearly, not inspected this year"
                },

                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-4", Unit = "M", Freq = 1,
                    LastInspection = now.AddMonths(-1), Zone1Year = now.Year, Zone = 1, DueInspection = true,
                    Note = "Monthly, not inspected this month"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-4", Unit = "M", Freq = 1,
                    LastInspection = now, Zone1Year = now.Year, Zone = 1, DueInspection = false,
                    Note = "Monthly, inspected this month"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-4", Unit = "M", Freq = 6,
                    LastInspection = now.AddMonths(-7), Zone1Year = now.Year, Zone = 1, DueInspection = true,
                    Note = "Every 6 Months, not inspected in the last 6 months"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NJ7", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "HAB-4", Unit = "M", Freq = 6,
                    LastInspection = now.AddMonths(-5), Zone1Year = now.Year, Zone = 1, DueInspection = false,
                    Note = "Every 6 Months, inspected in the last 6 months"
                },

                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-1", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = false, Note = "Zone 1 - in 5 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-2", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 1, 1, 1), Zone1Year = now.Year, Zone = 2,
                    DueInspection = false, Note = "Zone 2 - in 5 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-3", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 2, 1, 1), Zone1Year = now.Year, Zone = 3,
                    DueInspection = false, Note = "Zone 3 - in 5 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-4", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 3, 1, 1), Zone1Year = now.Year, Zone = 4,
                    DueInspection = false, Note = "Zone 4 - in 5 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-5", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 4, 1, 1), Zone1Year = now.Year, Zone = 5,
                    DueInspection = false, Note = "Zone 5 - in 5 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-6", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 5, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = true, Note = "Zone 1 - in 5 year without valid inspection"
                },

                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-7", Unit = null, Freq = null,
                    LastInspection = null, Zone1Year = now.Year, Zone = 1, DueInspection = true,
                    Note = "Zone 1 - in 5 year with no previous inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-7", Unit = null, Freq = null,
                    LastInspection = null, Zone1Year = now.Year, Zone = 2, DueInspection = false,
                    Note = "Zone 2 - in 5 year with no previous inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA30", OpCntrUnit = "Y", OpCntrFreq = 5, Number = "CAB-8", Unit = null, Freq = null,
                    LastInspection = null, Zone1Year = now.Year, Zone = null, DueInspection = true,
                    Note = "Zone unset - in 5 year"
                },

                new TestHydrantDueInspection {
                    OpCntr = "CA31", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "CAB-2", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year, 1, 1), Zone1Year = null, Zone = null, DueInspection = false,
                    Note = "Operating Center yearly, inspected this year"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA31", OpCntrUnit = "Y", OpCntrFreq = 1, Number = "CAB-3", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 1, 1, 1), Zone1Year = null, Zone = null,
                    DueInspection = true, Note = "Operating Center yearly, not inspected this year"
                },

                new TestHydrantDueInspection {
                    OpCntr = "CA32", OpCntrUnit = "M", OpCntrFreq = 1, Number = "CAB-4", Unit = null, Freq = null,
                    LastInspection = now.AddMonths(-1), Zone1Year = null, Zone = null, DueInspection = true,
                    Note = "Operating Center monthly, not inspected this month"
                },
                new TestHydrantDueInspection {
                    OpCntr = "CA32", OpCntrUnit = "M", OpCntrFreq = 1, Number = "CAB-4", Unit = null, Freq = null,
                    LastInspection = now, Zone1Year = null, Zone = null, DueInspection = false,
                    Note = "Operating Center monthly, inspected this month"
                },

                new TestHydrantDueInspection {
                    OpCntr = "NY1", OpCntrUnit = "Y", OpCntrFreq = 3, Number = "NAB-1", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = false, Note = "Zone 1 - in 3 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NY1", OpCntrUnit = "Y", OpCntrFreq = 3, Number = "NAB-2", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 1, 1, 1), Zone1Year = now.Year, Zone = 2,
                    DueInspection = false, Note = "Zone 2 - in 3 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NY1", OpCntrUnit = "Y", OpCntrFreq = 3, Number = "NAB-3", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 2, 1, 1), Zone1Year = now.Year, Zone = 3,
                    DueInspection = false, Note = "Zone 3 - in 3 year with valid inspection"
                },
                new TestHydrantDueInspection {
                    OpCntr = "NY1", OpCntrUnit = "Y", OpCntrFreq = 3, Number = "NAB-4", Unit = null, Freq = null,
                    LastInspection = new DateTime(now.Year - 3, 1, 1), Zone1Year = now.Year, Zone = 1,
                    DueInspection = true, Note = "Zone 1 - in 3 year with valid inspection"
                },
            };

            string StringPad(string value, int totalWidth)
            {
                return value.PadRight(totalWidth, ' ');
            }

            string BoolPad(bool value)
            {
                return StringPad(value.ToString(), 5);
            }

            var errors = new List<string>();
            foreach (var item in theTruth)
            {
                var hydrant = GetHydrant(item);
                var result =
                    new HydrantInspectionRequirementHelper(new TestDateTimeProvider(DateTime.Now)).GetStatus(hydrant);

                if (item.DueInspection != hydrant.HydrantDueInspection.RequiresInspection)
                {
                    errors.Add(
                        $"Expected {BoolPad(item.DueInspection)}, Actual {BoolPad(hydrant.HydrantDueInspection.RequiresInspection)} - {StringPad(item.OpCntr, 4)} {StringPad(item.Number, 6)} {item.Note}");
                }
                else if (item.DueInspection != result.IsRequired)
                {
                    errors.Add(
                        $"{item.Note}: Validator says {result.IsRequired}, test says {item.DueInspection}, hydrant (view) says {hydrant.HydrantDueInspection.RequiresInspection}. Validator's reasoning: {string.Join(", ", result.Reasons.Where(r => r.IsRequired == result.IsRequired))}");
                }
            }

            var spacer = $"{Environment.NewLine}  ";
            if (errors.Any())
            {
                Assert.Fail(spacer + string.Join(spacer, errors));
            }
        }

        [TestMethod]
        public void TestIsCopyableRetursCopyableCorrectly()
        {
            var target = new Hydrant {
                Status = new AssetStatus()
            };

            for (var x = 0; x < 20; x++)
            {
                target.Status.Id = x;
                if (AssetStatus.CanBeCopiedStatuses.Contains(x))
                    Assert.IsTrue(target.CanBeCopied);
                else
                    Assert.IsFalse(target.CanBeCopied);
            }
        }

        [TestMethod]
        public void TestIsInspectableReturnsExpectedValues()
        {
            var target = new Hydrant {
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
        public void TestToAssetCoordinateDoesNotSetLatitudeAndLongitudeIfCoordinateIsNull()
        {
            var target = new Hydrant {
                HydrantBilling = new HydrantBilling {Id = HydrantBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                Coordinate = null
            };

            var result = target.ToAssetCoordinate();
            Assert.IsNull(result.Latitude);
            Assert.IsNull(result.Longitude);
            Assert.IsNull(result.Coordinate);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsExpectedValues()
        {
            var target = new Hydrant {
                HydrantBilling = new HydrantBilling {Id = HydrantBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                HydrantDueInspection = new HydrantDueInspection {
                    LastInspectionDate = DateTime.Now,
                    RequiresInspection = true,
                },
                LastNonInspectionDate = DateTime.Today,
                Coordinate = new Coordinate {
                    Latitude = 1,
                    Longitude = 2
                }
            };
            target.SetPropertyValueByName("Id", 3);
            target.SetPropertyValueByName("OutOfService", true);
            var result = target.ToAssetCoordinate();

            Assert.AreEqual(3, result.Id);
            Assert.AreEqual(target.HydrantDueInspection.LastInspectionDate, result.LastInspection);
            Assert.IsTrue(result.IsPublic);
            Assert.IsTrue(result.OutOfService);
            Assert.AreEqual(target.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(target.Coordinate.Longitude, result.Longitude);
        }

        [TestMethod]
        public void TestToAssetCoordinateReturnsExpectedValueForIsActiveForActiveStatuses()
        {
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll();
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var target = new Hydrant {
                HydrantBilling = new HydrantBilling {Id = HydrantBilling.Indices.PUBLIC},
                Status = new AssetStatus(),
                HydrantDueInspection = new HydrantDueInspection {
                    LastInspectionDate = DateTime.Now,
                    RequiresInspection = true,
                },
                LastNonInspectionDate = DateTime.Today,
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
                Assert.AreEqual(expectedResult, result, $"Wrong result for asset status {assetStatus.Description}");
            }
        }

        [TestMethod]
        public void TestToStringReturnsHydrantNumber()
        {
            var expected = "HYD-1";
            var target = new Hydrant {HydrantNumber = expected};

            Assert.AreEqual(expected, target.ToString());
        }

        [TestMethod]
        public void TestIsActiveReturnsTrueForSpecificActiveStatuses()
        {
            // MC-1725: For inspection purposes, a hydrant is active if the status is "ACTIVE", "REQUEST RETIREMENT", or "REQUEST CANCELLATION"
            var assetStatusesById = GetFactory<AssetStatusFactory>().CreateAll().ToDictionary(x => x.Id, x => x);
            var expectedAssetStatusIds = AssetStatus.ACTIVE_STATUSES;

            var hydrant = new Hydrant();

            foreach (var assetStatus in assetStatusesById)
            {
                hydrant.Status = assetStatus.Value;

                var expectedResult = expectedAssetStatusIds.Contains(assetStatus.Key);
                Assert.AreEqual(expectedResult, hydrant.IsActive);
            }
        }

        [TestMethod]
        public void Test_PaintedToday_ReturnsFalse_WhenLastPaintedAtIsNotToday()
        {
            var target = new Hydrant {
                HydrantDuePainting = new HydrantDuePainting {
                    LastPaintedAt = _now.AddDays(-2)
                }
            };
            _container.BuildUp(target);

            Assert.IsFalse(target.PaintedToday);
        }

        [TestMethod]
        public void Test_PaintedToday_ReturnsFalse_WhenLastPaintedAtIsNull()
        {
            var target = new Hydrant();
            _container.BuildUp(target);

            Assert.IsFalse(target.PaintedToday);

            target.HydrantDuePainting = new HydrantDuePainting();

            Assert.IsFalse(target.PaintedToday);
        }

        [TestMethod]
        public void Test_PaintedToday_ReturnsTrue_WhenLastPaintedAtIsToday()
        {
            var target = new Hydrant {
                HydrantDuePainting = new HydrantDuePainting {
                    LastPaintedAt = _now.Date.AddHours(3)
                }
            };
            _container.BuildUp(target);

            Assert.IsTrue(target.PaintedToday);
        }
    }
}
