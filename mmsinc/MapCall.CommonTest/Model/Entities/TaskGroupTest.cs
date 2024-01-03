using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TaskGroupTest
    {
        [TestMethod]
        public void TestToStringReturnsTaskGroupIdAndName()
        {
            // Arrange
            var target = new TaskGroup { TaskGroupId = "TG-01", TaskGroupName = "Test TaskGroupName Result" };

            // Act
            var result = target.ToString();

            // Assert
            Assert.AreEqual("TG-01 - Test TaskGroupName Result", result);
        }
    }
}
