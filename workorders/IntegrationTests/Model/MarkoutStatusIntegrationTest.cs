using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MarkoutStatusIntegrationTest
    /// </summary>
    [TestClass]
    public class MarkoutStatusIntegrationTest : WorkOrdersTestClass<MarkoutStatus>
    {
        #region Constants

        private const int MIN_EXPECTED_COUNT = 3;

        #endregion

        #region Exposed Static Methods

        public static MarkoutStatus GetValidMarkoutStatus()
        {
            return MarkoutStatusRepository.Pending;
        }

        public static void DeleteMarkoutStatus(MarkoutStatus entity)
        {
            MarkoutStatusRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override MarkoutStatus GetValidObject()
        {
            return GetValidMarkoutStatus();
        }

        protected override MarkoutStatus GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(MarkoutStatus entity)
        {
            DeleteMarkoutStatus(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void AssetTypeIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void AssetTypeIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                MarkoutStatus target, expected;
                Assert.AreEqual(new MarkoutStatusRepository().Count(), MIN_EXPECTED_COUNT);

                target = MarkoutStatusRepository.Pending;
                expected =
                    MarkoutStatusRepository.GetEntity(
                        MarkoutStatusRepository.Indices.PENDING);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutStatusRepository.Descriptions.PENDING, target.Description);

                target = MarkoutStatusRepository.Ready;
                expected =
                    MarkoutStatusRepository.GetEntity(
                        MarkoutStatusRepository.Indices.READY);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutStatusRepository.Descriptions.READY, target.Description);

                target = MarkoutStatusRepository.Overdue;
                expected =
                    MarkoutStatusRepository.GetEntity(
                        MarkoutStatusRepository.Indices.OVERDUE);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutStatusRepository.Descriptions.OVERDUE, target.Description);
            }
        }
        [TestMethod]
        public void TestCannotCreateNewMarkoutStatus()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new MarkoutStatus {
                    Description = "TestStatus"
                };

                MyAssert.Throws(() => MarkoutStatusRepository.Insert(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterMarkoutStatus()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.Description = "TestStatus";

                MyAssert.Throws(() => MarkoutStatusRepository.Update(target),
                    typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteMarkoutStatus()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => MarkoutStatusRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}