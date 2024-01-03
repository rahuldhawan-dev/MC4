using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using System;
using System.Linq;
using MapCall.Common.Testing.Data;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RecurringProjectTest : InMemoryDatabaseTest<RecurringProject>
    {
        [TestMethod]
        public void TestEstimatedInServicePeriodReturnsCorrectlyFormattedString()
        {
            int year = 2013;
            var result = new RecurringProject {EstimatedInServiceDate = new DateTime(2013, 5, 14)};

            Assert.AreEqual(String.Format(RecurringProject.SERVICE_PERIOD_FORMAT, year, 2),
                result.EstimatedInServicePeriod);
        }

        [TestMethod]
        public void TestDecadeInstalledReturnsDecadeInstalledFromLargestPipe()
        {
            var expected = 2000;
            var target = new RecurringProject();
            target.RecurringProjectMains.Add(new RecurringProjectMain
                {Length = 10, DateInstalled = new DateTime(1990, 1, 1)});
            target.RecurringProjectMains.Add(new RecurringProjectMain
                {Length = 15, DateInstalled = new DateTime(2001, 1, 1)});
            target.RecurringProjectMains.Add(new RecurringProjectMain
                {Length = 12, DateInstalled = new DateTime(2011, 1, 1)});

            Assert.AreEqual(expected, target.MainsDecadeInstalled);
        }

        [TestMethod]
        public void TestDiameterReturnsExistingDiameterFromLargestPipe()
        {
            var expected = 1.5m;
            var target = new RecurringProject();
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 10, Diameter = 1.7m});
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 15, Diameter = expected});
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 12, Diameter = 1.7m});

            Assert.AreEqual(expected, target.MainsExistingDiameter);
        }

        [TestMethod]
        public void TestPipeMaterialReturnsExistingPipeMaterialFromLargestPipe()
        {
            var expected = "DI";
            var target = new RecurringProject();
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 10, Material = "CI"});
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 15, Material = expected});
            target.RecurringProjectMains.Add(new RecurringProjectMain {Length = 12, Material = "CI"});

            Assert.AreEqual(expected, target.MainsExistingPipeMaterial);
        }

        [TestMethod]
        public void TestRequiresScoringReturnsCorrectValueWhenScoreLessThanOrGreaterThan16()
        {
            var target = GetFactory<RecurringProjectFactory>().Create(new {ProjectTitle = "Ugh"});
            var type = GetEntityFactory<PipeDataLookupType>().Create(new
                {Description = RecurringProject.LookupTypeDescriptions.DECADE_INSTALLED});
            var value = GetEntityFactory<PipeDataLookupValue>().Create(new {
                Description = "Hello", PipeDataLookupType = type, VariableScore = 10m, PriorityWeightedScore = 100m,
                IsDefault = true, IsEnabled = true
            });

            //target.PipeDataLookupValues.Add(new RecurringProjectPipeDataLookupValue { RecurringProject = target, PipeDataLookupValue = value });
            target.PipeDataLookupValues.Add(value);

            Session.Save(target);
            Session.Flush();
            Session.Clear();
            target = Session.Get<RecurringProject>(target.Id);

            Assert.IsTrue(target.RequiresScoring);

            var newType = GetEntityFactory<PipeDataLookupType>().Create(new
                {Description = RecurringProject.LookupTypeDescriptions.PIPE_DIAMETER});
            var newValue = GetEntityFactory<PipeDataLookupValue>().Create(new {
                Description = "Hello", PipeDataLookupType = newType, VariableScore = 100m, PriorityWeightedScore = 100m,
                IsDefault = true, IsEnabled = true
            });
            //target.PipeDataLookupValues.Add(new RecurringProjectPipeDataLookupValue { RecurringProject = target, PipeDataLookupValue = newValue });
            target.PipeDataLookupValues.Add(newValue);
            Session.Save(target);
            Session.Flush();
            Session.Clear();
            target = Session.Get<RecurringProject>(target.Id);

            Assert.IsFalse(target.RequiresScoring);
        }

        [TestMethod]
        public void TestEstimatedVariableScoreReturnsCorrectValue()
        {
            var target = GetFactory<RecurringProjectFactory>().Create(new {ProjectTitle = "Ugh"});
            var type = GetEntityFactory<PipeDataLookupType>().Create(new
                {Description = RecurringProject.LookupTypeDescriptions.DECADE_INSTALLED});
            var value = GetEntityFactory<PipeDataLookupValue>().Create(new {
                Description = "Hello", PipeDataLookupType = type, VariableScore = 10m, PriorityWeightedScore = 100m,
                IsDefault = true, IsEnabled = true
            });

            Assert.AreEqual(0, target.EstimatedVariableScore);
            Assert.AreEqual(0, target.EstimatedPriorityWeightedScore);

            //target.PipeDataLookupValues.Add(new RecurringProjectPipeDataLookupValue { RecurringProject = target, PipeDataLookupValue = value });
            target.PipeDataLookupValues.Add(value);
            Session.Save(target);
            Session.Flush();
            Session.Clear();

            Assert.AreEqual(10m, target.EstimatedVariableScore);
            Assert.AreEqual(100m, target.EstimatedPriorityWeightedScore);

            var secondValue = GetEntityFactory<PipeDataLookupValue>().Create(new {
                Description = "Hello", PipeDataLookupType = type, VariableScore = 20m, PriorityWeightedScore = 200m,
                IsDefault = true, IsEnabled = true
            });

            //target.PipeDataLookupValues.Add(new RecurringProjectPipeDataLookupValue { RecurringProject = target, PipeDataLookupValue = secondValue });
            target.PipeDataLookupValues.Add(secondValue);

            Assert.AreEqual(15m, target.EstimatedVariableScore);
            Assert.AreEqual(150m, target.EstimatedPriorityWeightedScore);
        }

        [TestMethod]
        public void TestCorrectIconIsReturnedForStatusAPApproved()
        {
            var complete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var ap_approved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var statuses = new[] {complete, ap_approved};

            var target = GetFactory<RecurringProjectFactory>().Create(new {Status = ap_approved});
            Session.Clear();

            target = Session.Load<RecurringProject>(target.Id);

            Assert.IsNotNull(target.Icon);
            Assert.AreEqual(30, target.Icon.Id);
        }

        [TestMethod]
        public void TestCorrectIconIsReturnedForStatusComplete()
        {
            var complete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var ap_approved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var statuses = new[] {complete, ap_approved};
            var target = GetFactory<RecurringProjectFactory>().Create(new {Status = complete});
            Session.Clear();

            target = Session.Load<RecurringProject>(target.Id);

            Assert.IsNotNull(target.Icon);
            Assert.AreEqual(28, target.Icon.Id);
        }

        [TestMethod]
        public void TestCorrectIconIsReturnedForStatusNotCompletedOrAPApproved()
        {
            var ap_approved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var complete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var proposed = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var statuses = new[] {complete, ap_approved, proposed};

            foreach (var status in statuses.Where(x =>
                x.Id != RecurringProjectStatus.Indices.COMPLETE && x.Id != RecurringProjectStatus.Indices.AP_APPROVED))
            {
                var target = GetFactory<RecurringProjectFactory>().Create(new {Status = status});
                Session.Clear();

                target = Session.Load<RecurringProject>(target.Id);

                Assert.IsNotNull(target.Icon);
                Assert.AreEqual(29, target.Icon.Id);
            }
        }
    }
}
