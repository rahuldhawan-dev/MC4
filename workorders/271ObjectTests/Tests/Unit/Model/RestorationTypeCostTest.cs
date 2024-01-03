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
    /// Summary description for RestorationTypeCostTest
    /// </summary>
    [TestClass]
    public class RestorationTypeCostTest
    {
        private IRepository<RestorationTypeCost> _repository;
        
        RestorationTypeCost _target;

        #region Additional test attributes
        [TestInitialize]
        public void RestorationTypeCostTestTestInitialize()
        {
            _repository = new MockRestorationTypeCostRepository();
            _target = new TestRestorationTypeCostBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewRestorationTypeCost()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target = new TestRestorationTypeCostBuilder()
                .WithOperatingCenter(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutRestorationType()
        {
            _target = new TestRestorationTypeCostBuilder()
                .WithRestorationType(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutCost()
        {
            _target = new TestRestorationTypeCostBuilder()
                .WithCost(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutFinalCost()
        {
            _target = new TestRestorationTypeCostBuilder()
                .WithFinalCost(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class MockRestorationTypeCostRepository : MockRepository<RestorationTypeCost>
    {}

    internal class TestRestorationTypeCostBuilder : TestDataBuilder<RestorationTypeCost>
    {
        private OperatingCenter _operatingCenter = new OperatingCenter();

        private RestorationType _restorationType = new RestorationType();

        private double? _cost = 1.0;

        private int? _finalCost = 2;

        public override RestorationTypeCost Build()
        {
            var obj = new TestRestorationTypeCost();
            if (_operatingCenter != null)
                obj.OperatingCenter = _operatingCenter;
            if (_restorationType != null)
                obj.RestorationType = _restorationType;
            if (_cost != null)
                obj.Cost = _cost.Value;
            if (_finalCost != null)
                obj.FinalCost = _finalCost.Value;
            return obj;
        }

        public TestRestorationTypeCostBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        public TestRestorationTypeCostBuilder WithRestorationType(RestorationType restorationType)
        {
            _restorationType = restorationType;
            return this;
        }

        public TestRestorationTypeCostBuilder WithCost(double? cost)
        {
            _cost = cost;
            return this;
        }

        public TestRestorationTypeCostBuilder WithFinalCost(int? finalCost)
        {
            _finalCost = finalCost;
            return this;
        }
    }
    
    internal class TestRestorationTypeCost : RestorationTypeCost
    {
    }
}


