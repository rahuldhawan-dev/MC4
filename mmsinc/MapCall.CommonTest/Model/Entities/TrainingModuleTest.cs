using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TrainingModuleTest
    {
        [TestMethod]
        public void TestDisplayReturnsIdAndTitle()
        {
            var target = new TrainingModule {Title = "I am a title"};

            Assert.AreEqual(String.Format("{0} - {1}", target.Id, target.Title), target.Display);
        }

        [TestMethod]
        public void TestEmployeesAttendedReturnsAllTheEmployeesAttendedRecordsFromTrainingRecords()
        {
            var target = new TrainingModule();
            var emp1 = new Employee();
            var emp2 = new Employee();

            var recordEmp1 = new TrainingRecordAttendedEmployee {Employee = emp1};
            var recordEmp2 = new TrainingRecordAttendedEmployee {Employee = emp2};

            var record1 = new TrainingRecord();
            record1.EmployeesAttended.Add(recordEmp1);
            target.TrainingRecords.Add(record1);

            var record2 = new TrainingRecord();
            record2.EmployeesAttended.Add(recordEmp2);
            target.TrainingRecords.Add(record2);

            var result = target.EmployeesAttended.ToArray();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(recordEmp1));
            Assert.IsTrue(result.Contains(recordEmp2));
        }
    }
}
