using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MaintenancePlanRepositoryTest : InMemoryDatabaseTest<MaintenancePlan, MaintenancePlanRepository>
    {
        #region Private Members

        private readonly DateTime _today = DateTime.Parse("01/15/2030 12:00AM");

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_today));
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetOnlyScheduledPlansReturnsOnlyListOfPlansThatShouldBeScheduledToday()
        {
            var startDate = _today.AddMonths(-6);
            
            var frequencyFactory = GetFactory<ProductionWorkOrderFrequencyFactory>();
            var dailyFreq = frequencyFactory.Create();

            var planFactory = GetFactory<MaintenancePlanFactory>();

            var plans = new List<MaintenancePlan> {
                // Plans that should be scheduled (Start date on or before _today, IsActive, Daily Frequency, is not paused)
                planFactory.Create(new {
                    Start = startDate,
                    IsActive = true,
                    ProductionWorkOrderFrequency = dailyFreq,
                }),
                planFactory.Create(new {
                    Start = startDate,
                    IsActive = true,
                    ProductionWorkOrderFrequency = dailyFreq,
                }),

                // This plan isn't active and thus shouldn't be scheduled
                planFactory.Create(new {
                    Start = startDate,
                    IsActive = false,
                    ProductionWorkOrderFrequency = dailyFreq,
                }),

                // This plan is paused and thus shouldn't be scheduled
                planFactory.Create(new {
                    Start = startDate,
                    IsActive = true,
                    IsPlanPaused = true,
                    ProductionWorkOrderFrequency = dailyFreq,
                }),

                // This plan starts after _today and thus shouldn't be scheduled
                planFactory.Create(new {
                    Start = _today.AddDays(1),
                    IsActive = true,
                    ProductionWorkOrderFrequency = dailyFreq,
                }),
            };

            Session.Flush();

            var scheduledPlans = Repository.GetOnlyScheduledMaintenancePlans().Select(x => x.MaintenancePlanId).ToList();
            var underlyingPlans = Repository.Where(x => scheduledPlans.Contains(x.Id)).ToList();
            Assert.IsNotNull(scheduledPlans);
            Assert.AreEqual(2, scheduledPlans.Count);
            Assert.IsTrue(underlyingPlans.All(x => x.IsActive));
            Assert.IsTrue(underlyingPlans.All(x => !x.IsPlanPaused));
            Assert.IsTrue(underlyingPlans.All(x => _today >= x.Start.Date));
            Assert.IsTrue(underlyingPlans.All(x => x.ProductionWorkOrderFrequency.Id == dailyFreq.Id));
        }

        [TestMethod]
        public void TestTrimScheduledAssignmentsUpToRemovesOldScheduledAssignments()
        {
            var startDate = _today.AddMonths(-6);

            var employee = GetFactory<EmployeeFactory>().Create();
            var frequencyFactory = GetFactory<ProductionWorkOrderFrequencyFactory>();
            var dailyFreq = frequencyFactory.Create();

            var planFactory = GetFactory<MaintenancePlanFactory>();

            var plan = planFactory.Create(new {
                Start = startDate,
                IsActive = true,
                ProductionWorkOrderFrequency = dailyFreq,
            });

            plan.ScheduledAssignments.Add(new ScheduledAssignment {
                MaintenancePlan = plan,
                AssignedTo = employee,
                AssignedFor = _today.AddDays(1),
                ScheduledDate = _today.AddDays(-1)
            });

            plan.ScheduledAssignments.Add(new ScheduledAssignment {
                MaintenancePlan = plan,
                AssignedTo = employee,
                AssignedFor = _today.AddDays(1),
                ScheduledDate = _today
            });

            plan.ScheduledAssignments.Add(new ScheduledAssignment {
                MaintenancePlan = plan,
                AssignedTo = employee,
                AssignedFor = _today.AddDays(1),
                ScheduledDate = _today.AddDays(1)
            });
            
            Session.Flush();
            Session.Clear();

            Repository.TrimScheduledAssignmentsUpTo(_today);
            var actual = Repository.Find(plan.Id);

            // One of the assignments is for "tomorrow" so it shouldn't have been deleted
            Assert.AreEqual(1, actual.ScheduledAssignments.Count);
        }

        #endregion
    }
}
