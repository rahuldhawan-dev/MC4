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
    /// Summary description for WorkDescriptionTestTest
    /// </summary>
    [TestClass]
    public class WorkDescriptionTest : WorkOrdersTestClass<WorkDescription>
    {
        #region Constants

        public const short REFERENCE_INDEX_TO = 70;
        public const short REFERENCE_INDEX_FROM = 71;

        #endregion

        #region Private Members

        private IRepository<WorkDescription> _repository;
        private WorkDescription _target;

        #endregion

        #region Exposed Static Methods

        public static WorkDescription GetValidWorkDescription()
        {
            return WorkDescriptionRepository.GetEntity(REFERENCE_INDEX_TO);
        }

        public static WorkDescription GetValidToWorkDescription()
        {
            return GetValidWorkDescription();
        }

        public static WorkDescription GetValidFromWorkDescription()
        {
            return WorkDescriptionRepository.GetEntity(REFERENCE_INDEX_FROM);
        }

        public static void DeleteWorkDescription(WorkDescription entity)
        {
            WorkDescriptionRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override WorkDescription GetValidObject()
        {
            return new WorkDescription {
                Description = "Test",
                AssetType = AssetTypeTest.GetValidAssetType()
            };
        }

        protected override WorkDescription GetValidObjectFromDatabase()
        {
            return GetValidWorkDescription();
        }

        protected override void DeleteObject(WorkDescription entity)
        {
            DeleteWorkDescription(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkDescriptionTestInitialize()
        {
            _repository = new MockWorkDescriptionRepository();
            _target = new TestWorkDescriptionBuilder();
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            Assert.AreEqual(TestWorkDescriptionBuilder.TEST_DESCRIPTION,
                _target.ToString());
        }

        [TestMethod]
        public void TestCannotSetTimeToCompleteToZero()
        {
            MyAssert.Throws<DomainLogicException>(
                () => _target.TimeToComplete = 0);
        }

        [TestMethod]
        public void TestCreateNewWorkDescription()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutAssetType()
        {
            _target = new TestWorkDescriptionBuilder().WithAssetType(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDescription()
        {
            _target = new TestWorkDescriptionBuilder().WithDescription(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target),
                "Attempting to save a WorkDescription without a value for Description should throw an exception.");
        }

        [TestMethod]
        public void TestCannotSaveWithoutTimeToComplete()
        {
            _target = new TestWorkDescriptionBuilder().WithTimeToComplete(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target),
                "Attempting to save a WorkDescription without a value for TimeToComplete should throw an exception.");
        }

        [TestMethod]
        public void TestFirstAccountingStringReturnsCompiledFirstRowAccountingInformation()
        {
            var accountingCode = new RestorationAccountingCode {
                Code = "86753",
                SubCode = "09"
            };
            var productCode = new RestorationProductCode {
                Code = "ASDF"
            };

            _target = new WorkDescription {
                FirstRestorationAccountingCode = accountingCode,
                FirstRestorationProductCode = productCode
            };

            Assert.AreEqual("86753.09 ASDF",
                _target.FirstRestorationAccountingString);
        }

        [TestMethod]
        public void FirstRestorationCostBreakdownStringReturnsWithPercentSign()
        {
            Assert.AreEqual(_target.FirstRestorationCostBreakdownString,
                "0%");

            short firstRestorationCostBreakdown = 50;
            
            _target = new WorkDescription {
                FirstRestorationCostBreakdown = firstRestorationCostBreakdown
            };

            Assert.AreEqual(firstRestorationCostBreakdown + "%",
                _target.FirstRestorationCostBreakdownString);

        }

        [TestMethod]
        public void TestSecondAccountingStringReturnsCompiledSecondRowAccountingInformationIfAvailable()
        {
            var accountingCode = new RestorationAccountingCode {
                Code = "86753",
                SubCode = "09"
            };
            var productCode = new RestorationProductCode {
                Code = "ASDF"
            };

            _target = new WorkDescription
            {
                SecondRestorationAccountingCode = accountingCode,
                SecondRestorationProductCode = productCode
            };

            Assert.AreEqual("86753.09 ASDF",
                _target.SecondRestorationAccountingString);

            _target = new WorkDescription();

            Assert.IsNull(_target.SecondRestorationAccountingString);
        }

        [TestMethod]
        public void SecondRestorationCostBreakdownStringReturnsEmptyStringIfNotSet()
        {
            Assert.AreEqual(string.Empty,
                _target.SecondRestorationCostBreakdownString);
        }

        [TestMethod]
        public void SecondRestorationCostBreakdownStringReturnsWithPercentSignIfSet()
        {
            short secondRestorationCostBreakdown = 50;
            _target = new WorkDescription
            {
                SecondRestorationCostBreakdown = secondRestorationCostBreakdown
            };

            Assert.AreEqual(secondRestorationCostBreakdown + "%",
                _target.SecondRestorationCostBreakdownString);
        }
    }

    // TODO: Should this be internal?
    public class TestWorkDescriptionBuilder : TestDataBuilder<WorkDescription>
    {
        #region Constants

        public const string TEST_DESCRIPTION = "Test Description";

        #endregion

        #region Private Members

        private decimal? _timeToComplete = 1;
        private AssetType _assetType = new AssetType();
        private string _description = TEST_DESCRIPTION;

        #endregion

        #region Exposed Methods

        public override WorkDescription Build()
        {
            var desc = new WorkDescription();
            if (_assetType != null)
                desc.AssetType = _assetType;
            if (_description != null)
                desc.Description = _description;
            if (_timeToComplete != null)
                desc.TimeToComplete = _timeToComplete.Value;
            return desc;
        }

        public TestWorkDescriptionBuilder WithAssetType(AssetType assetType)
        {
            _assetType = assetType;
            return this;
        }

        public TestWorkDescriptionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestWorkDescriptionBuilder WithTimeToComplete(decimal? timeToComplete)
        {
            _timeToComplete = timeToComplete;
            return this;
        }

        #endregion
    }

    public class MockWorkDescriptionRepository : MockRepository<WorkDescription>
    {
    }
}
