using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MainBreakMaterialTest.
    /// </summary>
    [TestClass]
    public class MainBreakMaterialTest
    {
        #region Private Members

        private TestMainBreakMaterial _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MainBreakMaterialTestInitialize()
        {
            _target = new TestMainBreakMaterialBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReflectsNameProperty()
        {
            const string expected = "foo";
            var target = new MainBreakMaterial()
            {
                Description = expected
            };

            Assert.AreEqual(expected, target.ToString(),
                "MainBreakMaterial#ToString() should reflect the Name property.");
        }
    }

    internal class TestMainBreakMaterialBuilder : TestDataBuilder<TestMainBreakMaterial>
    {
        #region Exposed Methods

        public override TestMainBreakMaterial Build()
        {
            var obj = new TestMainBreakMaterial();
            return obj;
        }

        #endregion
    }

    internal class TestMainBreakMaterial : MainBreakMaterial
    {
    }
}
