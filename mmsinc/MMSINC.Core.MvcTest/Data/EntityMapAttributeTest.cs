using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class EntityMapAttributeTest
    {
        [TestMethod]
        public void TestConstructorSetsDirectionProperty()
        {
            foreach (var direction in EnumExtensions.GetValues<MapDirections>())
            {
                Assert.AreEqual(direction, new EntityMapAttribute(direction).Direction);
            }
        }

        [TestMethod]
        public void TestConstructorSetsSecondaryPropertyNameToPassedInEntityPropertyNameParameter()
        {
            Assert.AreEqual("Prop", new EntityMapAttribute("Prop").SecondaryPropertyName);
        }
    }
}
