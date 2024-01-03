using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MaterialTestTest
    /// </summary>
    [TestClass]
    public class MaterialTest : WorkOrdersTestClass<Material>
    {
        #region Exposed Static Methods

        public static Material GetValidMaterial()
        {
            return new Material { PartNumber = "PN1234" };
        }

        public static void DeleteMaterial(Material entity)
        {
            MaterialRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override Material GetValidObject()
        {
            return GetValidMaterial();
        }

        protected override Material GetValidObjectFromDatabase()
        {
            var material = GetValidObject();
            MaterialRepository.Insert(material);
            return material;
        }

        protected override void DeleteObject(Material entity)
        {
            DeleteMaterial(entity);
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReturnsDescription()
        {
            var description = "this is the description";

            var target = new Material {
                Description = description
            };

            Assert.AreEqual(description, target.ToString());
        }
    }

    internal class TestMaterialBuilder : TestDataBuilder<TestMaterial>
    {
        #region Private Members

        private string _description,
                       _partNumber;

        #endregion

        #region Exposed Methods

        public override TestMaterial Build()
        {
            var material = new TestMaterial();
            if (_description != null)
                material.Description = _description;
            if (_partNumber != null)
                material.PartNumber = _partNumber;
            return material;
        }

        public TestMaterialBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestMaterialBuilder WithPartNumber(string partNumber)
        {
            _partNumber = partNumber;
            return this;
        }

        #endregion
    }

    internal class TestMaterial : Material
    {
    }
}
