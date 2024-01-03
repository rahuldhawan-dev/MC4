using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for OperatingCenterStockedMaterialTest
    /// </summary>
    [TestClass]
    public class OperatingCenterStockedMaterialTest
    {
        private IRepository<OperatingCenterStockedMaterial> _repository;
        private OperatingCenterStockedMaterial _target;

        [TestInitialize]
        public void OperatingCenterStockedMaterialTestInitialize()
        {
            _repository = new MockOperatingCenterStockedMaterialRepository();
            _target = new TestOperatingCenterStockedMaterialBuilder();
        }

        [TestMethod]
        public void TestCreateNewOperatingCenterStockedMaterial()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));

            Assert.IsNotNull(_target);
            Assert.IsInstanceOfType(_target,
                                    typeof(OperatingCenterStockedMaterial));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target = new TestOperatingCenterStockedMaterialBuilder()
                    .WithOperatingCenter(null);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                            typeof(DomainLogicException),
                            "Attempting to save an OperatingCenterStockedMaterial without an OperatingCenter should throw an exception");
        }

        [TestMethod]
        public void TestCannotSaveWithoutMaterial()
        {
            _target = new TestOperatingCenterStockedMaterialBuilder()
                    .WithMaterial(null);

            MyAssert.Throws(() => _repository.InsertNewEntity(_target),
                            typeof(DomainLogicException),
                            "Attempting to save an OperatingCenterStockedMaterial without a Material should throw an exception");
        }
    }

    internal class MockOperatingCenterStockedMaterialRepository : MockRepository<OperatingCenterStockedMaterial>
    {
    }

    internal class TestOperatingCenterStockedMaterialBuilder : TestDataBuilder<OperatingCenterStockedMaterial>
    {
        #region Private Members

        private Material _material = new Material();
        //MaterialTest.GetValidMaterial();
        private OperatingCenter _operatingCenter = new OperatingCenter();
        //OperatingCenterIntegrationTest.GetValidOperatingCenter();

        #endregion

        #region Exposed Methods

        public override OperatingCenterStockedMaterial Build()
        {
            var ocsm = new OperatingCenterStockedMaterial();
            if (_material != null)
                ocsm.Material = _material;
            if (_operatingCenter != null)
                ocsm.OperatingCenter = _operatingCenter;
            return ocsm;
        }

        public TestOperatingCenterStockedMaterialBuilder WithMaterial(Material material)
        {
            _material = material;
            return this;
        }

        public TestOperatingCenterStockedMaterialBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        #endregion
    }

}
