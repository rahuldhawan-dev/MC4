using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using System;
using StructureMap;
using System.Linq;
using MMSINC.Testing.Utilities;
using MapCall.Common.Model.Repositories;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class WorkOrderMapTest : InMemoryDatabaseTest<WorkOrder>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestHasJobSiteCheckLists()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var jobSiteCheckList1 = GetFactory<JobSiteCheckListFactory>().Create(new
            {
                MapCallWorkOrder = wo,
                HasExcavation = true,
                AnyPotentialOverheadHazards = false
            });

            wo.JobSiteCheckLists.Add(jobSiteCheckList1);
            Session.Save(wo);
            Session.Flush();
            Session.Evict(wo);


            var woAgain = Session.Get<WorkOrder>(wo.Id);
            Assert.AreEqual(1, woAgain.JobSiteCheckLists.Count);
            Assert.IsNotNull(woAgain.HasJobSiteCheckLists);
            Assert.IsTrue(woAgain.HasJobSiteCheckLists.Value);
            Assert.IsNotNull(woAgain.HasPreJobSafetyBriefs);
            Assert.IsTrue(woAgain.HasPreJobSafetyBriefs.Value);
        }

        [TestMethod]
        public void TestHasPreJobSafetyBriefs()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var jobSiteCheckList1 = GetFactory<JobSiteCheckListFactory>().Create(new
            {
                MapCallWorkOrder = wo,
                HasExcavation = true,
                AnyPotentialOverheadHazards = true
            });

            wo.JobSiteCheckLists.Add(jobSiteCheckList1);
            Session.Save(wo);
            Session.Flush();
            Session.Evict(wo);

            var woAgain = Session.Get<WorkOrder>(wo.Id);
            Assert.AreEqual(1, woAgain.JobSiteCheckLists.Count);
            Assert.IsNotNull(woAgain.HasPreJobSafetyBriefs);
            Assert.IsTrue(woAgain.HasPreJobSafetyBriefs.Value);
        }

        [TestMethod]
        public void TestAlertIssued()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            wo.AlertIssued = true;

            Session.Save(wo);
            Session.Flush();
            Session.Evict(wo);

            var woAgain = Session.Get<WorkOrder>(wo.Id);
            Assert.IsTrue(woAgain.AlertIssued);
        }

        [TestMethod]
        public void Test_HasPendingAssignments_ReturnsFalseWhenOrderHasAssignmentsInThePast()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var ca = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo,
                AssignedFor = DateTime.Today.AddDays(-2)
            });

            var woAgain = Session.Get<WorkOrder>(wo.Id);

            Assert.AreEqual(1, woAgain.CrewAssignments.Count);
            Assert.AreEqual(ca.Id, woAgain.CrewAssignments.First().Id);
            Assert.IsFalse(woAgain.HasPendingAssignments);
        }

        [TestMethod]
        public void Test_HasPendingAssignments_ReturnsTrueWhenPendingAssignments()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var ca = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo,
                AssignedFor = DateTime.Today.AddDays(1)
            });

            var woAgain = Session.Get<WorkOrder>(wo.Id);

            Assert.AreEqual(1, woAgain.CrewAssignments.Count);
            Assert.AreEqual(ca.Id, woAgain.CrewAssignments.First().Id);
            Assert.IsTrue(woAgain.HasPendingAssignments);
        }

        #endregion
    }
}
