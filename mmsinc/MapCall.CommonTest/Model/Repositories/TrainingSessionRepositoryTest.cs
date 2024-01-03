using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        TrainingSessionRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<TrainingSession, TrainingSessionRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        /// <summary>
        /// Cases
        /// http://stackoverflow.com/questions/325933/determine-whether-two-date-ranges-overlap
        /// </summary>

        #region Instructor

        [TestMethod]
        public void TestInstructorBookedReturnsFalseWhenAfter()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(7).AddMinutes(30)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsFalseWhenStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(8)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenStartInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(1)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenInsideStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(9).AddMinutes(1)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenEnclosingStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(30)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenEnclosing()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(10),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(20)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenEnclosingEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(10),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenExactMatch()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenInsideEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsTrueWhenEndInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(50),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsFalseWhenEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(9),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestInstructorBookedReturnsFalseWhenBefore()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {Instructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(10),
                EndDateTime = DateTime.Today.AddHours(11)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        #endregion

        #region SecondInstructor

        [TestMethod]
        public void TestSecondInstructorBookedReturnsFalseWhenAfter()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(7).AddMinutes(30)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsFalseWhenStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(8)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenStartInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(1)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenInsideStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(9).AddMinutes(1)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenEnclosingStartTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(30)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenEnclosing()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(10),
                EndDateTime = DateTime.Today.AddHours(8).AddMinutes(20)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenEnclosingEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(10),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenExactMatch()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenInsideEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(7),
                EndDateTime = DateTime.Today.AddHours(9)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsTrueWhenEndInside()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(8).AddMinutes(50),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsTrue(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsFalseWhenEndTouching()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(9),
                EndDateTime = DateTime.Today.AddHours(10)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        [TestMethod]
        public void TestSecondInstructorBookedReturnsFalseWhenBefore()
        {
            var instructor = GetEntityFactory<Employee>().Create();
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {SecondInstructor = instructor});
            var existingSession = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord,
                StartDateTime = DateTime.Today.AddHours(10),
                EndDateTime = DateTime.Today.AddHours(11)
            });
            Session.Flush();
            Session.Clear();

            Assert.IsFalse(Repository.InstructorBooked(instructor.Id, DateTime.Today.AddHours(8),
                DateTime.Today.AddHours(9)));
        }

        #endregion
    }
}
