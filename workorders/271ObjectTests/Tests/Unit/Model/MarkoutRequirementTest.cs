using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MarkoutRequirementTest
    /// </summary>
    [TestClass]
    public class MarkoutRequirementTest
    {
        [TestMethod]
        public void TestIsRequiredReturnsFalseWhenRequirementIsNone()
        {
            var target = TestMarkoutRequirementBuilder.None;

            Assert.IsFalse(target.IsRequired);
        }

        [TestMethod]
        public void TestIsRequiredReturnsTrueWhenRequirementIsNotNone()
        {
            var target = TestMarkoutRequirementBuilder.Routine;

            Assert.IsTrue(target.IsRequired);

            target = TestMarkoutRequirementBuilder.Emergency;

            Assert.IsTrue(target.IsRequired);
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var expected = "Test";
            var target = new MarkoutRequirement {
                Description = expected
            };

            Assert.AreEqual(expected, target.ToString());
        }
    }

    public class TestMarkoutRequirementBuilder : TestDataBuilder<MarkoutRequirement>
    {
        #region Static Properties

        public static MarkoutRequirement None
        {
            get
            {
                return new MarkoutRequirement
                {
                    MarkoutRequirementID = MarkoutRequirementRepository.Indices.NONE,
                    Description = MarkoutRequirementRepository.Descriptions.NONE
                };
            }
        }

        public static MarkoutRequirement Routine
        {
            get
            {
                return new MarkoutRequirement
                {
                    MarkoutRequirementID = MarkoutRequirementRepository.Indices.ROUTINE,
                    Description = MarkoutRequirementRepository.Descriptions.ROUTINE
                };
            }
        }

        public static MarkoutRequirement Emergency
        {
            get
            {
                return new MarkoutRequirement
                {
                    MarkoutRequirementID = MarkoutRequirementRepository.Indices.EMERGENCY,
                    Description = MarkoutRequirementRepository.Descriptions.EMERGENCY
                };
            }
        }

        #endregion

        #region Exposed Methods

        public override MarkoutRequirement Build()
        {
            return null;
        }

        #endregion
    }
}
