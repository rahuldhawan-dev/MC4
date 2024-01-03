using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels

{
    [TestClass]
    public class TrainingModuleTest : MapCallMvcInMemoryDatabaseTestBase<TrainingModule>
    {
        
        [TestMethod]
        public void TestDisablingTrainingModuleRemovesFromTrainingRequirementIfIsActiveInitialTrainingModule()
        {
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement });
            trainingRequirement.ActiveInitialTrainingModule = trainingModule;
            Session.Clear();

            // Sanity Check
            Assert.AreEqual(trainingRequirement.ActiveInitialTrainingModule, trainingModule);

            trainingModule.IsActive = false;
            Session.Save(trainingModule);

            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            Assert.IsNull(trainingRequirement.ActiveInitialTrainingModule);
        }

        [TestMethod]
        public void TestDisablingTrainingModuleRemovesFromTrainingRequirementIfIsActiveInitialAndRecurringTrainingModule()
        {
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement });
            trainingRequirement.ActiveInitialAndRecurringTrainingModule = trainingModule;
            Session.Clear();

            // Sanity Check
            Assert.AreEqual(trainingRequirement.ActiveInitialAndRecurringTrainingModule, trainingModule);

            trainingModule.IsActive = false;
            Session.Save(trainingModule);

            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            Assert.IsNull(trainingRequirement.ActiveInitialAndRecurringTrainingModule);
        }
        
        [TestMethod]
        public void TestDisablingTrainingModuleRemovesFromTrainingRequirementIfIsActiveRecurringTrainingModule()
        {
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create();
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new { IsActive = true, TrainingRequirement = trainingRequirement });
            trainingRequirement.ActiveRecurringTrainingModule = trainingModule;
            Session.Clear();

            // Sanity Check
            Assert.AreEqual(trainingRequirement.ActiveRecurringTrainingModule, trainingModule);

            trainingModule.IsActive = false;
            Session.Save(trainingModule);

            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            Assert.IsNull(trainingRequirement.ActiveRecurringTrainingModule);
        }
    }
}
