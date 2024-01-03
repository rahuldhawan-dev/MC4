using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class TrainingRecordTest : MapCallMvcInMemoryDatabaseTestBase<TrainingRecord>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITrainingSessionRepository>().Use<TrainingSessionRepository>();
        }

        #endregion

        #region AddTrainingSession
        
        [TestMethod]
        public void TestAddTrainingSessionInstructorBookedReturnsValidationError()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { Instructor = instructor });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create( new  {
                TrainingRecord = trainingRecord1,
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });
            Session.Clear();
            var trainingSession2 = _viewModelFactory.BuildWithOverrides<AddTrainingSession, TrainingRecord>(trainingRecord2, new {
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });

            ValidationAssert.ModelStateHasNonPropertySpecificError(trainingSession2, AddTrainingSession.INSTRUCTOR_BOOKED);
        }

        [TestMethod]
        public void TestAddTrainingSessionInstructorBookedAsSecondInstructorReturnsValidationError()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new { SecondInstructor = instructor });
            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { Instructor = instructor });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord1,
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });
            Session.Clear();
            var trainingSession2 = _viewModelFactory.BuildWithOverrides<AddTrainingSession, TrainingRecord>(trainingRecord2, new {
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });

            ValidationAssert.ModelStateHasNonPropertySpecificError(trainingSession2, AddTrainingSession.INSTRUCTOR_BOOKED);
        }

        [TestMethod]
        public void TestAddTrainingSessionSecondInstructorBookedAsSecondInstructorReturnsValidationError()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new { SecondInstructor = instructor });
            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { SecondInstructor = instructor });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord1,
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });
            Session.Clear();
            var trainingSession2 = _viewModelFactory.BuildWithOverrides<AddTrainingSession, TrainingRecord>(trainingRecord2, new {
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });

            ValidationAssert.ModelStateHasNonPropertySpecificError(trainingSession2, AddTrainingSession.SECOND_INSTRUCTOR_BOOKED);
        }

        [TestMethod]
        public void TestAddTrainingSessionSecondInstructorBookedAsInstructorReturnsValidationError()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new { Instructor = instructor });
            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new { SecondInstructor = instructor });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord1,
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });
            Session.Clear();
            var trainingSession2 = _viewModelFactory.BuildWithOverrides<AddTrainingSession, TrainingRecord>(trainingRecord2, new {
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddHours(1)
            });

            ValidationAssert.ModelStateHasNonPropertySpecificError(trainingSession2, AddTrainingSession.SECOND_INSTRUCTOR_BOOKED);
        }

        #endregion
    }
}