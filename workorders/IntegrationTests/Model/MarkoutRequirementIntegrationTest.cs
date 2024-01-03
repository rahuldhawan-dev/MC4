using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for MarkoutRequirementIntegrationTest
    /// </summary>
    [TestClass]
    public class MarkoutRequirementIntegrationTest
    {
        #region Constants

        private const short MIN_EXPECTED_COUNT = 3;

        #endregion

        #region Private Members

        private HttpSimulator _simulator;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void MarkoutRequirementIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void MarkoutRequirementIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                MarkoutRequirement target, expected;
                Assert.AreEqual(MIN_EXPECTED_COUNT,
                                new MarkoutRequirementRepository().Count());

                target = MarkoutRequirementRepository.None;
                expected =
                    MarkoutRequirementRepository.GetEntity(
                        MarkoutRequirementRepository.Indices.NONE);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutRequirementRepository.Descriptions.NONE, target.Description);
                Assert.AreEqual(MarkoutRequirementEnum.None, target.RequirementEnum);

                target = MarkoutRequirementRepository.Routine;
                expected =
                    MarkoutRequirementRepository.GetEntity(
                        MarkoutRequirementRepository.Indices.ROUTINE);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutRequirementRepository.Descriptions.ROUTINE, target.Description);
                Assert.AreEqual(MarkoutRequirementEnum.Routine, target.RequirementEnum);

                target = MarkoutRequirementRepository.Emergency;
                expected =
                    MarkoutRequirementRepository.GetEntity(
                        MarkoutRequirementRepository.Indices.EMERGENCY);
                Assert.AreEqual(expected, target);
                Assert.AreEqual(MarkoutRequirementRepository.Descriptions.EMERGENCY, target.Description);
                Assert.AreEqual(MarkoutRequirementEnum.Emergency, target.RequirementEnum);
            }
        }
    }
}