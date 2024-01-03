using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TaskGroupCategoryTest
    {
        [TestMethod]
        public void TestToStringReturnsType()
        {
            // Arrange
            var target = new TaskGroupCategory { Type = "Task Group Test Type Result" };

            // Act
            var result = target.ToString();

            // Assert
            Assert.AreEqual("Task Group Test Type Result", result);
        }
    }
}
