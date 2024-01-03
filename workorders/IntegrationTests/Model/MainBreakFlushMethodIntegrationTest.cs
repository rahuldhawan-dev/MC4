using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MainBreakFlushMethodIntegrationTest
    /// </summary>
    [TestClass]
    public class MainBreakFlushMethodIntegrationTest : WorkOrdersTestClass<MainBreakFlushMethod>
    {
        public static MainBreakFlushMethod GetValidMainBreakFlushMethod()
        {
            return MainBreakFlushMethodRepository.Blowoff;
        }

        #region Additional Test Attributes

        [TestInitialize]
        public void MainBreakFlushMethodIntegrationTestInitialize()
        {
            var mockUser = new MockUser();
        }

        [TestCleanup]
        public void MainBreakFlushMethodIntegrationTestTestCleanup()
        {
        }

        #endregion

        #region Overrides of LinqUnitTestClass<MainBreakFlushMethod>

        protected override MainBreakFlushMethod GetValidObject()
        {
            return GetValidMainBreakFlushMethod();
        }

        protected override MainBreakFlushMethod GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MainBreakFlushMethod entity)
        {
            throw new NotImplementedException();
        }

        #endregion
        [TestMethod]
        public void TestCannotCreateNewMainBreakFlushMethodWithoutDescription()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MainBreakFlushMethod
                {
                    Description = string.Empty
                };

                MyAssert.Throws(
                    () => MainBreakFlushMethodRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }
}
