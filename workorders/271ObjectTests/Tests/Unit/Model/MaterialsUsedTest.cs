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
    /// Summary description for MaterialsUsedTestTest
    /// </summary>
    [TestClass]
    public class MaterialsUsedTest
    {
        #region Private Members

        private IRepository<MaterialsUsed> _repository;
        private TestMaterialsUsed _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MaterialsUsedTestInitialize()
        {
            _repository = new MockMaterialsUsedRepository();
            _target = new TestMaterialsUsedBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewMaterialsUsed()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target = new TestMaterialsUsedBuilder()
                .WithWorkOrder(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithQuantityLessThanOne()
        {
            _target = new TestMaterialsUsedBuilder()
                .WithQuantity(0);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSetQuantityToZeroOrLess()
        {
            MyAssert.Throws<DomainLogicException>(() => _target.Quantity = 0,
                "Attempting to set Quantity for a MaterialsUsed object to zero or less should throw an exception");

            MyAssert.Throws<DomainLogicException>(() => _target.Quantity = -1,
                "Attempting to set Quantity for a MaterialsUsed object to zero or less should throw an exception");
        }


        [TestMethod]
        public void TestDescriptionPropertyReturnsNonStockDescriptionWhenNoMaterialPresent()
        {
            var expected = "Test Description";
            _target = new TestMaterialsUsedBuilder()
                .WithNonStockDescription(expected)
                .WithMaterial(null);

            Assert.AreEqual(expected, _target.Description);
        }

        [TestMethod]
        public void TestDescriptionPropertyReturnsMaterialDescriptionWhenMaterialIsPresent()
        {
            var expectedSize = "2 1/2\"";
            var expectedDescription = "Test Description";
            var expected = expectedSize + " " + expectedDescription;
            var material = new TestMaterialBuilder()
                .WithDescription(expected);
            _target = new TestMaterialsUsedBuilder()
                .WithMaterial(material);

            Assert.AreEqual(expected, _target.Description);
        }

        [TestMethod]
        public void TestPartNumberPropertyReturnsNAWhenNoMaterialPresent()
        {
            var expected = "n/a";
            _target = new TestMaterialsUsedBuilder()
                .WithMaterial(null);

            Assert.AreEqual(expected, _target.PartNumber);
        }

        [TestMethod]
        public void TestPartNumberReturnsMaterialPartNumberWhenMaterialIsPresent()
        {
            var expected = "867-5309";
            var material = new TestMaterialBuilder()
                .WithPartNumber(expected);
            _target = new TestMaterialsUsedBuilder()
                .WithMaterial(material);

            Assert.AreEqual(expected, _target.PartNumber);
        }

        [TestMethod]
        public void TestCannotSaveWithoutStockLocationWhenMaterialIsNotNull()
        {
            _target = new TestMaterialsUsedBuilder()
                .WithStockLocation(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCanSaveWithoutStockLocationWhenMaterialIsNull()
        {
            _target = new TestMaterialsUsedBuilder()
                .WithMaterial(null)
                .WithStockLocation(null);

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestMaterialsUsedBuilder : TestDataBuilder<TestMaterialsUsed>
    {
        #region Constants

        private const short DEFAULT_QUANTITY = 1;

        #endregion

        #region Private Members

        private Material _material = new TestMaterialBuilder();
        private StockLocation _stockLocation = new TestStockLocationBuilder();
        private WorkOrder _workOrder = new TestWorkOrderBuilder();
        private string _nonStockDescription;
        private short? _quantity = DEFAULT_QUANTITY;

        #endregion

        #region Exposed Methods

        public override TestMaterialsUsed Build()
        {
            var obj = new TestMaterialsUsed();
            if (_nonStockDescription != null)
                obj.NonStockDescription = _nonStockDescription;
            if (_material != null)
                obj.Material = _material;
            if (_stockLocation != null)
                obj.StockLocation = _stockLocation;
            if (_quantity != null)
                obj.SetQuantity(_quantity.Value);
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            return obj;
        }

        public TestMaterialsUsedBuilder WithNonStockDescription(string description)
        {
            _nonStockDescription = description;
            return this;
        }

        public TestMaterialsUsedBuilder WithMaterial(Material material)
        {
            _material = material;
            return this;
        }

        public TestMaterialsUsedBuilder WithStockLocation(StockLocation stockLocation)
        {
            _stockLocation = stockLocation;
            return this;
        }

        public TestMaterialsUsedBuilder WithQuantity(short? quantity)
        {
            _quantity = quantity;
            return this;
        }

        public TestMaterialsUsedBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }

    internal class TestMaterialsUsed : MaterialsUsed
    {
        #region Exposed Methods

        public void SetQuantity(short quantity)
        {
            _quantity = quantity;
        }

        #endregion
    }

    internal class MockMaterialsUsedRepository : MockRepository<MaterialsUsed>
    {
    }
}