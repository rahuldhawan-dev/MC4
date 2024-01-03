using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WorkDescriptionTest
    {
        [TestMethod]
        public void Test_ToString_ReturnsDescription()
        {
            var description = "foozball";
            var target = new WorkDescription {Description = description};

            Assert.AreEqual(description, target.ToString());
        }

        [TestMethod]
        public void Test_IsMainReplaceOrRepair_ReturnsTrue_WhenReplaceOrRepair()
        {
            var target = new WorkDescription();

            target.SetPropertyValueByName("Id", 1);
            Assert.IsFalse(target.IsMainReplaceOrRepair);

            target.SetPropertyValueByName("Id", WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
            Assert.IsTrue(target.IsMainReplaceOrRepair);

            target.SetPropertyValueByName("Id", WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE);
            Assert.IsTrue(target.IsMainReplaceOrRepair);
        }

        [TestMethod]
        public void Test_WorkDescriptionToJson_ReturnsNewObject_WithSomePropertyValues()
        {
            var target = new WorkDescription {
                Id = 123,
                Description = "foo bar",
                DigitalAsBuiltRequired = true
            };

            var result = (dynamic)WorkDescription.WorkDescriptionToJson(target);
            
            Assert.AreEqual(target.Id, result.Id);
            Assert.AreEqual(target.Description, result.Description);
            Assert.AreEqual(target.DigitalAsBuiltRequired, result.DigitalAsBuiltRequired);
        }
    }
}
