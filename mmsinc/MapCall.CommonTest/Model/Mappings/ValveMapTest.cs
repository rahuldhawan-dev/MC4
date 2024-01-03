using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using NHibernate.Linq;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class ValveMapTest : MapCallMvcInMemoryDatabaseTestBase<Valve>
    {
        #region Fields

        private Valve model;
        private List<ValveZone> valveZones;
        private ValveControl valveControlsMain, valveControlsBlowOff, valveControlsBlowOffWithFlushing;
        private ValveBilling valveBillingPublic, valveBillingMunicipal;
        private AssetStatus activeValveStatus, retiredValveStatus;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            valveControlsMain = GetFactory<MainValveControlFactory>().Create();
            valveControlsBlowOffWithFlushing = GetFactory<BlowOffWithFlushingValveControlFactory>().Create();
            valveControlsBlowOff = GetFactory<BlowOffValveControlFactory>().Create();
            valveBillingPublic = GetFactory<PublicValveBillingFactory>().Create();
            Assert.AreEqual(valveBillingPublic.Id, ValveBilling.Indices.PUBLIC);
            valveBillingMunicipal = GetFactory<MunicipalValveBillingFactory>().Create();
            activeValveStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            retiredValveStatus = GetFactory<RetiredAssetStatusFactory>().Create();

            model = GetEntityFactory<Valve>().Create(new {
                ValveControls = valveControlsMain,
                Status = activeValveStatus,
                ValveBilling = valveBillingPublic,
                // There's a matrix to figure out the valve zone due this year - ValveMap.cs
                // large zone 5 valves are due in odd years, zone 6 valves in even
                ValveZone = (DateTime.Now.Year % 2 == 1) ? valveZones[4] : valveZones[5]
            });
        }

        private Valve EvictAndRequery(Valve model)
        {
            Session.Evict(model);
            return Session.Query<Valve>().Single(x => x.Id == model.Id);
        }

        #endregion

        #region Tests

        /// <summary>
        /// These aren't tests of actual production code, just local sqlite dbspecific formulas
        /// If anyone decides to update one side and not the other, these just become irrelevant.
        /// </summary>

        #region Requires Inspection

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueIfValveStatusIsOneOfTheActiveStatuses()
        {
            var allStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var activeStatuses = allStatuses.Where(x => AssetStatus.ACTIVE_STATUSES.Contains(x.Id)).ToList();
            var inactiveStatuses = allStatuses.Except(activeStatuses).ToList();

            foreach (var status in activeStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                Assert.IsTrue(model.RequiresInspection,
                    $"RequiresInspection should be true for status '{status.Description}'");
            }

            foreach (var status in inactiveStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                Assert.IsFalse(model.RequiresInspection,
                    $"RequiresInspection should be false for status '{status.Description}'");
            }
        }

        [TestMethod]
        public void TestRequiresInspectionIfValveBillingIsNotPublicOrBlankReturnsFalse()
        {
            model.ValveBilling = GetFactory<MunicipalValveBillingFactory>().Create();
            Session.Flush();

            model = EvictAndRequery(model);
            Assert.IsFalse(model.RequiresInspection);
        }

        [TestMethod]
        public void TestRequiresInspectionIfValveControlsBlowoffWithFlushingReturnsFalse()
        {
            model.ValveControls = valveControlsBlowOffWithFlushing;
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresInspection,
                String.Format("Valve Controls: {0}:{1}", model.ValveControls.Id, model.ValveControls));
        }

        [TestMethod]
        public void TestRequiresInspectionIfValveBPUKPITrueReturnsFalse()
        {
            model.BPUKPI = true;
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresInspection);
        }

        [TestMethod]
        public void TestRequiresInspectionIfValveSizeIsLessThanTwoAndValveControlsABlowOffReturnsFalse()
        {
            model.ValveControls = valveControlsBlowOff;
            model.ValveSize = GetEntityFactory<ValveSize>().Create(new {Size = 1m});
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresInspection,
                string.Format("ValveSize: {0}, Valve Controls: {1}", model.ValveSize, model.ValveControls));
        }

        [TestMethod]
        public void TestRequiresInspectionSkipsValveSizeCheckIfValveSizeIsNull()
        {
            // This test will fail if the the ValvesDueInspection view switches back to 
            // an inner join on ValveSizes. It needs to be a left join or else valves
            // without valve sizes will not be included in the RequiresInspection query.
            model.ValveControls = valveControlsBlowOff;
            model.ValveSize = null;
            var valveInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now.AddYears(-1),
                Inspected = true
            });
            model.ValveZone = _container.GetInstance<IRepository<ValveZone>>().Find(7);
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsTrue(model.RequiresInspection);
        }

        /// <summary>
        /// This madness uses a seed year to determine which years have inspections 
        /// due based on the large or small valve zone. BEWARE YE THAT ENT3R HRE.
        /// | Small Zone | Large Zone | Year  |
        /// |          1 |          5 | 2011  |
        /// |          2 |          6 | 2012  |
        /// |          3 |          5 | 2013  |
        /// |          4 |          6 | 2014  | Repeat
        /// |		   7 |		      | Annual|
        /// 
        /// NOTE: If the year is 2016 and this test is now failing, Alex is a jerk or he misplaced his parens.
        /// </summary>
        [TestMethod]
        public void TestRequiresInspectionReturnsFalseIfSmallValveZoneIsNotRequiredForTheYearOfTheDateWeAreChecking()
        {
            var valveZoneRepository = _container.GetInstance<IRepository<ValveZone>>();
            var smallValveZones = new[] {1, 2, 3, 4};
            foreach (var valveZone in smallValveZones)
            {
                model.ValveZone = valveZoneRepository.Find(valveZone);
                Session.Flush();

                model = EvictAndRequery(model);
                if (valveZone != (((Math.Abs(2011 - DateTime.Now.Year)) % 4) + 1))
                {
                    Assert.IsFalse(model.RequiresInspection, String.Format("ValveZoneId: {0}", valveZone));
                }
                else
                {
                    Assert.IsTrue(model.RequiresInspection);
                }
            }
        }

        /// <summary>
        /// This madness uses a seed year to determine which years have inspections 
        /// due based on the large or small valve zone. BEWARE YE THAT ENT3R HRE.
        /// | Small Zone | Large Zone | Year  |
        /// |          1 |          5 | 2011  |
        /// |          2 |          6 | 2012  |
        /// |          3 |          5 | 2013  |
        /// |          4 |          6 | 2014  | Repeat
        /// |		   7 |		      | Annual|
        /// 
        /// NOTE: If the year is 2016 and this test is now failing, Alex is a jerk or he misplaced his parens.
        /// </summary>
        [TestMethod]
        public void TestRequiresInspectionReturnsFalseIfLargeValveZoneIsNotRequiredForTheYearOfTheDateWeAreChecking()
        {
            var valveZoneRepository = _container.GetInstance<IRepository<ValveZone>>();
            var smallValveZones = new[] {5, 6};
            foreach (var valveZone in smallValveZones)
            {
                model.ValveZone = valveZoneRepository.Find(valveZone);
                Session.Flush();
                model = EvictAndRequery(model);

                if (valveZone != (((Math.Abs(2011 - DateTime.Now.Year)) % 2) + 5))
                {
                    Assert.IsFalse(model.RequiresInspection,
                        String.Format("ValveZoneId: {0}, model valveZoneId: {1}", valveZone, model.ValveZone.Id));
                }
                else
                {
                    Assert.IsTrue(model.RequiresInspection);
                }
            }
        }

        [TestMethod]
        public void
            TestRequiresInspectionReturnsFalseIfUsesValveInspectionFrequencyIsTrueAndInspectionFrequencyUnitAndFrequencyExistAndYearIsIncorrect()
        {
            var inspectionFequency = 5;
            var annualFrequency = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            var valveZoneRepository = _container.GetInstance<IRepository<ValveZone>>();
            var theValveZones = new[] {1, 2, 3, 4, 5};
            model.OperatingCenter.UsesValveInspectionFrequency = true;

            foreach (var valveZone in theValveZones)
            {
                model.ValveZone = valveZoneRepository.Find(valveZone);
                model.InspectionFrequency = inspectionFequency;
                model.InspectionFrequencyUnit = annualFrequency;

                Session.Flush();

                model = EvictAndRequery(model);
                // From valvemap for REQUIRES_INSPECTION_FORMAT_STRING
                // 5 != 0
                if (valveZone % inspectionFequency != Math.Abs(2011 - DateTime.Now.Year) % inspectionFequency)
                {
                    Assert.IsFalse(model.RequiresInspection, $"ValveZoneId: {valveZone}");
                }
                else
                {
                    Assert.IsTrue(model.RequiresInspection);
                }
            }
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueIfInspectionRequired()
        {
            var valveInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now.AddYears(-1),
                Inspected = true
            });
            model.ValveZone = _container.GetInstance<IRepository<ValveZone>>().Find(7);
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsTrue(model.RequiresInspection);
        }

        [TestMethod]
        public void
            TestRequiresInspectionReturnsTrueIfNotPublicWithInspectionFrequencyAndInspectionFrequencyUnitAndInspectionRequired()
        {
            model.ValveBilling = valveBillingMunicipal;
            model.InspectionFrequency = 1;
            model.InspectionFrequencyUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();

            var valveInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now.AddYears(-1),
                Inspected = true
            });
            model.ValveZone = _container.GetInstance<IRepository<ValveZone>>().Find(7);
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsTrue(model.RequiresInspection);
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsFalseIfAnInspectionExistsForTheCurrentYearAlready()
        {
            var valveInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now,
                Inspected = true
            });
            model.ValveZone = _container.GetInstance<IRepository<ValveZone>>().Find(7);
            Session.Flush();

            model = EvictAndRequery(model);
            var valveInspections = _container.GetInstance<IRepository<ValveInspection>>().GetAll();

            Assert.IsFalse(model.RequiresInspection);
        }

        #endregion

        #region Requires BlowOff Inspection

        [TestMethod]
        public void TestRequiresBlowOffInspectionReturnsFalseIfValveDoesNotControlBlowOffWithFlushing()
        {
            model.ValveControls = valveControlsMain;
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresBlowOffInspection);
        }

        [TestMethod]
        public void TestRequiresBlowOffInspectionReturnsTrueIfValveStatusIsOneOfTheActiveStatuses()
        {
            var allStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var activeStatuses = allStatuses.Where(x => AssetStatus.ACTIVE_STATUSES.Contains(x.Id)).ToList();
            var inactiveStatuses = allStatuses.Except(activeStatuses).ToList();

            model.ValveControls = valveControlsBlowOffWithFlushing;

            foreach (var status in activeStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                Assert.IsTrue(model.RequiresBlowOffInspection,
                    $"RequiresBlowOffInspection should be true for status '{status.Description}'");
            }

            foreach (var status in inactiveStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                Assert.IsFalse(model.RequiresBlowOffInspection,
                    $"RequiresBlowOffInspection should be false for status '{status.Description}'");
            }
        }

        [TestMethod]
        public void TestRequiresBlowOffInspectionReturnsFalseIfValveIsNotPublic()
        {
            var municipalBilling = GetFactory<MunicipalValveBillingFactory>().Create();
            model.ValveBilling = municipalBilling;
            model.ValveControls = valveControlsBlowOffWithFlushing;
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresBlowOffInspection);
        }

        [TestMethod]
        public void TestRequiresBlowOffInspectionReturnsTrueIfActivePublicBlowOffWithFlushingAndNoInspectionsExist()
        {
            model.ValveControls = valveControlsBlowOffWithFlushing;
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsTrue(model.RequiresBlowOffInspection);
        }

        [TestMethod]
        public void
            TestRequiresBlowOffInspectionReturnsTrueIfActivePublicBlowOffWithFlushingAndInspectionsExistOverAYearAgo()
        {
            model.ValveControls = valveControlsBlowOffWithFlushing;
            var inspection = GetEntityFactory<BlowOffInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now.AddYears(-1),
            });
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsTrue(model.RequiresBlowOffInspection);
        }

        [TestMethod]
        public void TestRequiresBlowOffInspectionReturnsFalseWhenACurrentInspectionExists()
        {
            model.ValveBilling = valveBillingPublic;
            model.ValveControls = valveControlsBlowOffWithFlushing;
            var inspection = GetEntityFactory<BlowOffInspection>().Create(new {
                Valve = model,
                DateInspected = DateTime.Now,
            });
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresBlowOffInspection);
        }

        #endregion

        #region Last Inspected

        [TestMethod]
        public void TestLastInspectionReturnsLastInspection()
        {
            var previousInspection = GetEntityFactory<ValveInspection>().Create(new
                {Valve = model, Inspected = true, DateInspected = DateTime.Now.AddDays(-1)});
            var lastInspection = GetEntityFactory<ValveInspection>()
               .Create(new {Valve = model, Inspected = true, DateInspected = DateTime.Now});
            var lastInspectionWithIssue = GetEntityFactory<ValveInspection>().Create(new
                {Valve = model, Inspected = false, DateInspected = DateTime.Now.AddMilliseconds(1)});
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.AreEqual(lastInspection.DateInspected.ToString(), model.LastInspectionDate.Value.ToString());
            Assert.AreNotEqual(previousInspection.DateInspected.ToString(), model.LastInspectionDate.Value.ToString());
        }

        [TestMethod]
        public void TestLastNonInspectionExistsAndHasBeenTested()
        {
            var validInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                Inspected = true,
                DateInspected = DateTime.Now.AddHours(-1)
            });
            var nonInspection = GetEntityFactory<ValveInspection>().Create(new {
                Valve = model,
                Inspected = false,
                WorkOrderRequestOne = GetEntityFactory<ValveWorkOrderRequest>().Create(new {Description = "COVERED"}),
                DateInspected = DateTime.Now
            });
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.AreEqual(nonInspection.DateInspected.ToString(), model.LastNonInspectionDate.Value.ToString());
            Assert.AreNotEqual(validInspection.DateInspected.ToString(), model.LastNonInspectionDate.Value.ToString());
        }

        #endregion

        #region ValveSize

        [TestMethod]
        public void TestIsLargeValveReturnsTrueWhenGreaterThanOrEqualToTwelve()
        {
            var valveSize12 = GetEntityFactory<ValveSize>().Create(new {Size = 12m});
            var valveSize13 = GetEntityFactory<ValveSize>().Create(new {Size = 13m});
            model.ValveSize = valveSize12;
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsTrue(model.IsLargeValve);

            model.ValveSize = valveSize13;
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsTrue(model.IsLargeValve);
        }

        [TestMethod]
        public void TestIsLargeValveReturnsFalseWhenLessThanTwelve()
        {
            var valveSize10 = GetEntityFactory<ValveSize>().Create(new {Size = 10m});
            var valveSize11 = GetEntityFactory<ValveSize>().Create(new {Size = 11.99m});
            model.ValveSize = valveSize10;
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsFalse(model.IsLargeValve);

            model.ValveSize = valveSize11;
            Session.Flush();

            model = EvictAndRequery(model);

            Assert.IsFalse(model.IsLargeValve);
        }

        #endregion

        #region MainCrossings

        [TestMethod]
        public void TestSavingAndRemovingMainCrossingsFromValve()
        {
            var main = GetFactory<MainCrossingFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create();

            valve.MainCrossings.Add(main);
            Session.Save(valve);
            Session.Flush();

            Session.Evict(valve);
            Session.Evict(main);

            // Adding a valve to the main should also add the main to the valve.
            valve = Session.Query<Valve>().Single(x => x.Id == valve.Id);
            main = Session.Query<MainCrossing>().Single(x => x.Id == main.Id);
            Assert.IsTrue(valve.MainCrossings.Any(x => x.Id == main.Id));
            Assert.IsTrue(main.Valves.Any(x => x.Id == valve.Id));

            Session.Evict(main);
            Session.Delete(valve);
            Session.Flush();

            main = Session.Query<MainCrossing>().Single(x => x.Id == main.Id);
            Assert.IsNotNull(main, "The main should not have been deleted via cascading.");
        }

        #endregion

        #endregion
    }
}
