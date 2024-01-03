using System;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for AssetStatusTest.
    /// </summary>
    [TestClass]
    public class AssetStatusTest : WorkOrdersTestClass<AssetStatus>
    {
        #region Private Members

        private IRepository<AssetStatus> _repository;
        private TestAssetStatus _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AssetStatusTestInitialize()
        {
            _repository = new MockAssetStatusRepository();
            _target = new TestAssetStatusBuilder();
        }

        #endregion

        #region Private Methods

        protected override AssetStatus GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteObject(AssetStatus entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewAssetStatus()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var description = "ACTIVE";
            var target = new AssetStatus
            {
                Description = description
            };

            Assert.AreEqual(description, target.ToString());
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }

    internal class TestAssetStatusBuilder : TestDataBuilder<TestAssetStatus>
    {
        #region Exposed Methods

        public override TestAssetStatus Build()
        {
            var obj = new TestAssetStatus();
            return obj;
        }

        #endregion
    }

    internal class TestAssetStatus : AssetStatus { }

    internal class MockAssetStatusRepository : MockRepository<AssetStatus> { }
}
