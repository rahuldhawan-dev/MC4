using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.V2;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class HydrantMapTest : InMemoryDatabaseTest<Hydrant>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        private TEntity EvictAndRequery<TEntity>(TEntity model)
            where TEntity : IEntity
        {
            // Evict and requery to ensure the database is being queried.
            Session.Evict(model);
            return Session.Query<TEntity>().Single(x => x.Id == model.Id);
        }

        #endregion

        #region Tests

        #region OutOfService

        [TestMethod]
        public void TestOutOfServiceReturnsFalseWhenThereAreNoOutOfServiceRecords()
        {
            var target = GetFactory<HydrantFactory>().Create();
            target = EvictAndRequery(target);
            Assert.AreEqual(0, target.OutOfServiceRecords.Count);
            Assert.IsFalse(target.OutOfService);
        }

        [TestMethod]
        public void TestOutOfServiceReturnsFalseWhenThereAreNoOPENOutOfServiceRecords()
        {
            var target = GetFactory<HydrantFactory>().Create();
            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new {
                Hydrant = target,
                BackInServiceDate = DateTime.Now,
                BackInServiceByUser = typeof(UserFactory)
            });
            target = EvictAndRequery(target);
            Assert.AreEqual(1, target.OutOfServiceRecords.Count);
            Assert.IsFalse(target.OutOfService);
        }

        [TestMethod]
        public void TestOutOfServiceReturnsTrueWhenThereIsAnOPENOutOfServiceRecord()
        {
            var target = GetFactory<HydrantFactory>().Create();
            var oos = GetFactory<HydrantOutOfServiceFactory>().Create(new {Hydrant = target});
            Assert.IsNull(oos.BackInServiceDate, "Sanity check, date must be null.");
            target = EvictAndRequery(target);
            Assert.AreEqual(1, target.OutOfServiceRecords.Count);
            Assert.IsTrue(target.OutOfService);
        }

        #endregion

        #region Unit tests that don't test production code because it uses DbSpecificFormula.

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueIfThereAreNoInspectionsAttached()
        {
            var model = GetFactory<HydrantFactory>().Create();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(0, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection);
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForActiveAssets()
        {
            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = DateTime.Today.AddYears(-1) // Gives us last year
            });

            Session.Flush();
            Session.Evict(lastInspection);

            var allStatuses = GetFactory<AssetStatusFactory>().CreateAll();
            var activeStatuses = allStatuses.Where(x => AssetStatus.ACTIVE_STATUSES.Contains(x.Id)).ToList();
            var inactiveStatuses = allStatuses.Except(activeStatuses).ToList();

            foreach (var status in activeStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
                Assert.AreEqual(status.Id, model.Status.Id);
                Assert.AreEqual(1, model.HydrantInspections.Count);
                Assert.IsTrue(model.HydrantDueInspection.RequiresInspection,
                    $"Requires inspection because last inspection was last year and status is {status.Description}.");
            }

            foreach (var status in inactiveStatuses)
            {
                model.Status = status;
                Session.Save(model);
                Session.Flush();
                model = EvictAndRequery(model);
                model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
                Assert.AreEqual(status.Id, model.Status.Id);
                Assert.AreEqual(1, model.HydrantInspections.Count);
                Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                    $"Must be false because the hydrant status is not active. Status is {status.Description}.");
            }
        }

        [TestMethod]
        public void
            TestRequiresInspectionReturnsFalseIfHydrantIsNotPublicAndHasNoInspectionFrequencyOrInspectionFrequencyUnit()
        {
            var model = GetFactory<HydrantFactory>().Create(new {
                HydrantBilling = typeof(MunicipalHydrantBillingFactory)
            });

            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);

            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection, "Must be false because the hydrant status is not active.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsFalseIfHydrantIsNotPublicAndHasNoInspectionFrequency()
        {
            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                HydrantBilling = typeof(MunicipalHydrantBillingFactory)
            });

            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);

            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection, "Must be false because the hydrant status is not active.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsFalseIfHydrantIsNotPublicAndHasNoInspectionFrequencyUnit()
        {
            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                HydrantBilling = typeof(MunicipalHydrantBillingFactory)
            });

            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);

            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection, "Must be false because the hydrant status is not active.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForYearlyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = DateTime.Today.AddYears(-1) // Gives us last year
            });

            Session.Flush();
            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(1, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection, "Requires inspection because last inspection was last year.");

            lastInspection.DateInspected = new DateTime(DateTime.Today.Year, 1, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                "Does not require inspection because last inspection was this year.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForMonthlyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.
            var today = DateTime.Today;

            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(MonthlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = today.AddMonths(-1) // Gives us last month
            });

            Session.Flush();
            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(1, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection, "Requires inspection because last inspection was last month.");

            lastInspection.DateInspected = new DateTime(today.Year, today.Month, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                "Does not require inspection because last inspection was this month.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForMonthlyINspectionFrequencyWhenMonthsAreSameButInDifferentYears()
        {
            var today = DateTime.Today;
            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(MonthlyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = today.AddYears(-1) // Gives us last year but the same month
            });

            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(1, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection, "Requires inspection because last inspection was last year.");

            lastInspection.DateInspected = new DateTime(today.Year, today.Month, 1);
            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);

            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                "Does not require inspection because last inspection was this year.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForWeeklyInspectionFrequencies()
        {
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var today = DateTime.Today;

            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(WeeklyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = today.AddDays(-7) // Gives us last week
            });

            Session.Flush();
            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(1, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection, "Requires inspection because last inspection was last week.");

            // We don't want this test to fail on the first day of the week.
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                lastInspection.DateInspected = today.AddDays(1);
            }
            else
            {
                lastInspection.DateInspected = today.AddDays(-1);
            }

            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                "Does not require inspection because last inspection was this week.");
        }

        [TestMethod]
        public void TestRequiresInspectionReturnsTrueForDayInspectionFrequencies()
        {
            DateTime GetSqlNow()
            {
                var query = _container.GetInstance<ISession>()
                                      .CreateSQLQuery("select datetime('now', 'localtime') as TheDate");
                var wrappedQuery = new SqlQueryWrapper(query);
                return wrappedQuery
                      .AddScalar("TheDate", typeof(DateTime))
                      .UniqueResult<DateTime>();
            }
            // These formulas all use getdate() or some other variation of it, so we need to
            // actually use DateTime.Today/Now in these tests.

            var startTime = DateTime.Now;
            var sqlStartTime = GetSqlNow();
            var today = DateTime.Today;

            var model = GetFactory<HydrantFactory>().Create(new {
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(DailyRecurringFrequencyUnitFactory)
            });
            var lastInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = model,
                DateInspected = today.AddDays(-1) // Gives us yesterday
            });

            Session.Flush();
            Session.Evict(lastInspection);
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);
            Assert.AreEqual(1, model.HydrantInspections.Count);
            Assert.IsTrue(model.HydrantDueInspection.RequiresInspection, $"Requires inspection because last inspection was yesterday. DateInspected - {lastInspection.DateInspected}");

            lastInspection.DateInspected = DateTime.Today;

            Session.Save(lastInspection);
            Session.Flush();
            model = EvictAndRequery(model);
            model.HydrantDueInspection = EvictAndRequery(model.HydrantDueInspection);

            Assert.IsFalse(model.HydrantDueInspection.RequiresInspection,
                $@"Does not require inspection because last inspection was today.
  DateInspected: {lastInspection.DateInspected}
  StartTime: {startTime}
  SqlStartTime: {sqlStartTime}
  EndTime: {DateTime.Now}
  SqlEndTime: {GetSqlNow()}");
        }

        [TestMethod]
        public void TestLastInspectedOnReturnsNullIfThereAreNoInspectionsForAHydrant()
        {
            var mc = GetFactory<HydrantFactory>().Create();
            Assert.IsFalse(mc.HydrantInspections.Any());
            Session.Evict(mc);
            mc = EvictAndRequery(mc);
            mc.HydrantDueInspection = EvictAndRequery(mc.HydrantDueInspection);
            Assert.IsNull(mc.HydrantDueInspection.LastInspectionDate);
        }

        [TestMethod]
        public void TestLastInspectedOnReturnsTheLatestInspectionByInspectedOnDate()
        {
            var mc = GetFactory<HydrantFactory>().Create();
            var expectedDate = DateTime.Today;
            var expected = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = mc,
                DateInspected = expectedDate
            });
            var unexpected = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = mc,
                DateInspected = expectedDate.AddDays(-2)
            });

            mc = EvictAndRequery(mc);
            mc.HydrantDueInspection = EvictAndRequery(mc.HydrantDueInspection);
            Assert.AreEqual(expectedDate, mc.HydrantDueInspection.LastInspectionDate);
        }

        #endregion

        #endregion
    }
}
