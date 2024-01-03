using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for OperatingCenterSpoilTypeTest.
    /// </summary>
    [TestClass]
    public class OperatingCenterSpoilRemovalCostTest
    {
        #region Private Members

        private MockRepository<OperatingCenterSpoilRemovalCost> _repository;
        private OperatingCenterSpoilRemovalCost _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void OperatingCenterSpoilTypeTestInitialize()
        {
            _repository = new MockRepository<OperatingCenterSpoilRemovalCost>();

            _target = new TestOperatingCenterSpoilTypeBuilder()
                .WithOperatingCenter(new OperatingCenter())
                .WithCost(1);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewOperatingCenterSpoilType()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target.OperatingCenter = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithCostOfZero()
        {
            _target.Cost = 0;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithCostLessThanZero()
        {
            _target.Cost = -1;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestOperatingCenterSpoilTypeBuilder : TestDataBuilder<OperatingCenterSpoilRemovalCost>
    {
        #region Private Members

        private OperatingCenter _center;
        private short? _cost;

        #endregion

        #region Exposed Methods

        public override OperatingCenterSpoilRemovalCost Build()
        {
            var obj = new OperatingCenterSpoilRemovalCost();
            if (_center != null)
                obj.OperatingCenter = _center;
            if (_cost != null)
                obj.Cost = _cost.Value;
            return obj;
        }

        public TestOperatingCenterSpoilTypeBuilder WithOperatingCenter(OperatingCenter center)
        {
            _center = center;
            return this;
        }

        public TestOperatingCenterSpoilTypeBuilder WithCost(short i)
        {
            _cost = i;
            return this;
        }

        #endregion
    }
}
