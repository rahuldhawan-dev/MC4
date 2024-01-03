using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SkillSetTest
    {
        [TestMethod]
        public void TestToStringReturnsName()
        {
            // Arrange
            var target = new SkillSet { Name = "Test Name Result" };

            // Act
            var result = target.ToString();

            // Assert
            Assert.AreEqual("Test Name Result", result);
        }
    }
}
