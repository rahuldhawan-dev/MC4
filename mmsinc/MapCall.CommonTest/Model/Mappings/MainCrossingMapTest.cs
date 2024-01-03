using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class MainCrossingMapTest : InMemoryDatabaseTest<MainCrossing>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        private MainCrossing EvictAndRequery(MainCrossing model)
        {
            // Evict and requery to ensure the database is being queried.
            Session.Evict(model);
            return Session.Query<MainCrossing>().Single(x => x.Id == model.Id);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDeletingAMainCrossingAlsoDeletesItsInspections()
        {
            var main = GetFactory<MainCrossingFactory>().Create();
            var inspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = main
            });

            Session.Clear();
            Session.Delete(main);
            Session.Flush();

            Session.Clear();

            Assert.IsNull(Session.Query<MainCrossing>().SingleOrDefault(x => x.Id == main.Id));
            Assert.IsNull(Session.Query<MainCrossingInspection>().SingleOrDefault(x => x.Id == inspection.Id));
        }

        [TestMethod]
        public void TestSavingAndRemovingImpactToTypesFromMainCrossing()
        {
            var main = GetFactory<MainCrossingFactory>().Create();
            var impact = GetFactory<MainCrossingImpactToTypeFactory>().Create();

            main.ImpactTo.Add(impact);
            Session.Save(main);
            Session.Flush();

            Session.Evict(impact);
            Session.Evict(main);

            // Adding a valve to the main should also add the main to the valve.
            impact = Session.Query<MainCrossingImpactToType>().Single(x => x.Id == impact.Id);
            main = Session.Query<MainCrossing>().Single(x => x.Id == main.Id);
            Assert.IsTrue(main.ImpactTo.Any(x => x.Id == impact.Id));

            Session.Evict(main);
            Session.Delete(impact);
            Session.Flush();

            main = Session.Query<MainCrossing>().Single(x => x.Id == main.Id);
            Assert.IsNotNull(main, "The main should not have been deleted via cascading.");
        }

        #region Unit tests that don't test production code because it uses DbSpecificFormula.

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueIfThereAreNoInspectionsAttached()
        {
            var model = GetFactory<MainCrossingFactory>().Create();
            model = EvictAndRequery(model);
            Assert.AreEqual(0, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection);
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForYearlyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var model = GetFactory<MainCrossingFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = model,
                InspectedOn = DateTime.Today.AddYears(-1) // Gives us last year
            });

            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            Assert.AreEqual(1, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection, "Requires inspection because last inspection was last year.");

            lastInspection.InspectedOn = new DateTime(DateTime.Today.Year, 1, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            Assert.IsFalse(model.RequiresInspection,
                "Does not require inspection because last inspection was this year.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForMonthlyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var today = DateTime.Today;

            var model = GetFactory<MainCrossingFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(MonthlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = model,
                InspectedOn = today.AddMonths(-1) // Gives us last month
            });

            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            Assert.AreEqual(1, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection, "Requires inspection because last inspection was last month.");

            lastInspection.InspectedOn = new DateTime(today.Year, today.Month, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            Assert.IsFalse(model.RequiresInspection,
                "Does not require inspection because last inspection was this month.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForMonthlyINspectionFrequencyWhenMonthsAreSameButInDifferentYears()
        {
            var today = DateTime.Today;
            var model = GetFactory<MainCrossingFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(MonthlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = model,
                InspectedOn = today.AddYears(-1) // Gives us last year but the same month
            });

            model = EvictAndRequery(model);
            Assert.AreEqual(1, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection, "Requires inspection because last inspection was last year.");

            lastInspection.InspectedOn = new DateTime(today.Year, today.Month, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);

            Assert.IsFalse(model.RequiresInspection,
                "Does not require inspection because last inspection was this year.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForWeeklyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var today = DateTime.Today;

            var model = GetFactory<MainCrossingFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(WeeklyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = model,
                InspectedOn = today.AddDays(-7) // Gives us last week
            });

            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            Assert.AreEqual(1, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection, "Requires inspection because last inspection was last week.");

            // We don't want this test to fail on the first day of the week.
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                lastInspection.InspectedOn = today.AddDays(1);
            }
            else
            {
                lastInspection.InspectedOn = today.AddDays(-1);
            }

            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            Assert.IsFalse(model.RequiresInspection,
                "Does not require inspection because last inspection was this week.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForDayInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var today = DateTime.Today;

            var model = GetFactory<MainCrossingFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(DailyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = model,
                InspectedOn = today.AddDays(-1) // Gives us yesterday
            });

            model = EvictAndRequery(model);
            Assert.AreEqual(1, model.Inspections.Count);
            Assert.IsTrue(model.RequiresInspection, "Requires inspection because last inspection was yesterday.");

            lastInspection.InspectedOn = today;

            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            Assert.IsFalse(model.RequiresInspection, "Does not require inspection because last inspection was today.");
        }

        [TestMethod]
        public void TestLastInspectedOnReturnsNullIfThereAreNoInspectionsForAMainCrossing()
        {
            var mc = GetFactory<MainCrossingFactory>().Create();
            Assert.IsFalse(mc.Inspections.Any());
            Session.Evict(mc);
            mc = EvictAndRequery(mc);
            Assert.IsNull(mc.LastInspectedOn);
        }

        [TestMethod]
        public void TestLastInspectedOnReturnsTheLatestInspectionByInspectedOnDate()
        {
            var mc = GetFactory<MainCrossingFactory>().Create();
            var expectedDate = DateTime.Today;
            var expected = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = mc,
                InspectedOn = expectedDate
            });
            Session.Clear();
            var unexpected = GetFactory<MainCrossingInspectionFactory>().Create(new {
                MainCrossing = mc,
                InspectedOn = expectedDate.AddDays(-2)
            });

            mc = EvictAndRequery(mc);
            Assert.AreEqual(expectedDate, mc.LastInspectedOn);
        }

        #endregion

        #endregion
    }
}
