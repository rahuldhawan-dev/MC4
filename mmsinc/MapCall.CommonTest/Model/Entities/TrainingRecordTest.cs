using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TrainingRecordTest : InMemoryDatabaseTest<TrainingRecord>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        [TestMethod]
        public void TestHasEnoughTrainingSessionsHoursForTrainingModuleReturnsFalseWhenNoTrainingModule()
        {
            var target = GetEntityFactory<TrainingRecord>().Create();

            Assert.IsFalse(target.HasEnoughTrainingSessionsHoursForTrainingModule);
        }

        [TestMethod]
        public void
            TestHasEnoughTrainingSessionsHoursForTrainingModuleReturnsFalseWhenTrainingModuleTotalHoursDoesNotHaveValue()
        {
            var trainingModule = GetEntityFactory<TrainingModule>().Create();
            var target = GetEntityFactory<TrainingRecord>().Create(new {TrainingModule = trainingModule});

            Assert.IsFalse(target.HasEnoughTrainingSessionsHoursForTrainingModule);
        }

        [TestMethod]
        public void TestHasEnoughTrainingSessionsHoursForTrainingModuleReturnsFalseWhenNoTrainingSessions()
        {
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new {TotalHours = 10f});
            var target = GetEntityFactory<TrainingRecord>().Create(new {TrainingModule = trainingModule});

            Assert.IsFalse(target.HasEnoughTrainingSessionsHoursForTrainingModule);
        }

        [TestMethod]
        public void TestHasEnoughTrainingSessionsHoursForTrainingModuleReturnsFalseWhenNotEnoughTrainingSessionHours()
        {
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new {TotalHours = 10f});
            var target = GetEntityFactory<TrainingRecord>().Create(new {TrainingModule = trainingModule});
            var session = GetEntityFactory<TrainingSession>().Create(new {
                StartDateTime = Lambdas.GetNow(), EndDateTime = Lambdas.GetNow().AddHours(1), TrainingRecord = target
            });
            target.TrainingSessions.Add(session);

            Assert.IsFalse(target.HasEnoughTrainingSessionsHoursForTrainingModule);
        }

        [TestMethod]
        public void TestHasEnoughTrainingSessionsHoursForTrainingModuleReturnsTrueWhenEnoughTrainingSessionHours()
        {
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new {TotalHours = 10f});
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {TrainingModule = trainingModule});
            var session = GetEntityFactory<TrainingSession>().Create(new {
                StartDateTime = Lambdas.GetNow(), EndDateTime = Lambdas.GetNow().AddHours(10),
                TrainingRecord = trainingRecord
            });
            trainingRecord.TrainingSessions.Add(session);
            Session.Save(trainingRecord);
            Session.Flush();
            Session.Clear();
            var target = Session.Load<TrainingRecord>(trainingRecord.Id);

            Assert.IsTrue(target.HasEnoughTrainingSessionsHoursForTrainingModule);
        }
    }
}
