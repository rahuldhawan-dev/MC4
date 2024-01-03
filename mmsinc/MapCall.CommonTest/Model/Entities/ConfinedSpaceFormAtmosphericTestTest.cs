using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ConfinedSpaceFormAtmosphericTestTest
    {
        #region Private Methods

        private static ConfinedSpaceFormAtmosphericTest CreateNewValidInstance()
        {
            var target = new ConfinedSpaceFormAtmosphericTest();
            // NOTE: The Oxygen values are the only ones that need defaults for this
            // test. The rest can default to zero.
            target.OxygenPercentageBottom = 20m;
            target.OxygenPercentageMiddle = 20m;
            target.OxygenPercentageTop = 20m;

            return target;
        }

        private void AssertInvalidReadings(Action<ConfinedSpaceFormAtmosphericTest> invalidSetter)
        {
            var target = CreateNewValidInstance();
            invalidSetter(target);
            Assert.IsFalse(target.AllReadingsHaveAcceptableConcentrations);
        }

        private void AssertValidReadings(Action<ConfinedSpaceFormAtmosphericTest> validSetter)
        {
            var target = CreateNewValidInstance();
            validSetter(target);
            Assert.IsTrue(target.AllReadingsHaveAcceptableConcentrations);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestAllReadingsHaveAcceptableConcentrationsReturnsTrueWhenValuesAreAllWithinAcceptableConcentrations()
        {
            var target = CreateNewValidInstance();
            Assert.IsTrue(target.AllReadingsHaveAcceptableConcentrations);
        }

        [TestMethod]
        public void
            TestAllReadingsHaveAcceptableConcentrationsReturnsFalseWhenAnySingleCarbonMonoxideReadingIsOverTheMaximumValue()
        {
            var expectedMax = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX;
            var overTheLimit = expectedMax + 1;
            AssertInvalidReadings(x => x.CarbonMonoxidePartsPerMillionBottom = overTheLimit);
            AssertInvalidReadings(x => x.CarbonMonoxidePartsPerMillionMiddle = overTheLimit);
            AssertInvalidReadings(x => x.CarbonMonoxidePartsPerMillionTop = overTheLimit);

            AssertValidReadings(x => x.CarbonMonoxidePartsPerMillionBottom = expectedMax);
            AssertValidReadings(x => x.CarbonMonoxidePartsPerMillionMiddle = expectedMax);
            AssertValidReadings(x => x.CarbonMonoxidePartsPerMillionTop = expectedMax);
        }

        [TestMethod]
        public void
            TestAllReadingsHaveAcceptableConcentrationsReturnsFalseWhenAnySingleHydrogenSulfideReadingIsOverTheMaximumValue()
        {
            var expectedMax = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX;
            var overTheLimit = expectedMax + 1;
            AssertInvalidReadings(x => x.HydrogenSulfidePartsPerMillionBottom = overTheLimit);
            AssertInvalidReadings(x => x.HydrogenSulfidePartsPerMillionMiddle = overTheLimit);
            AssertInvalidReadings(x => x.HydrogenSulfidePartsPerMillionTop = overTheLimit);

            AssertValidReadings(x => x.HydrogenSulfidePartsPerMillionBottom = expectedMax);
            AssertValidReadings(x => x.HydrogenSulfidePartsPerMillionMiddle = expectedMax);
            AssertValidReadings(x => x.HydrogenSulfidePartsPerMillionTop = expectedMax);
        }

        [TestMethod]
        public void
            TestAllReadingsHaveAcceptableConcentrationsReturnsFalseWhenAnySingleLowerExplosiveLimitReadingIsOverTheMaximumValue()
        {
            var expectedMax = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX;
            var overTheLimit = expectedMax + 1m;
            AssertInvalidReadings(x => x.LowerExplosiveLimitPercentageBottom = overTheLimit);
            AssertInvalidReadings(x => x.LowerExplosiveLimitPercentageMiddle = overTheLimit);
            AssertInvalidReadings(x => x.LowerExplosiveLimitPercentageTop = overTheLimit);

            AssertValidReadings(x => x.LowerExplosiveLimitPercentageBottom = expectedMax);
            AssertValidReadings(x => x.LowerExplosiveLimitPercentageMiddle = expectedMax);
            AssertValidReadings(x => x.LowerExplosiveLimitPercentageTop = expectedMax);
        }

        [TestMethod]
        public void
            TestAllReadingsHaveAcceptableConcentrationsReturnsFalseWhenAnySingleOxygenReadingHasAValueOutsideOfTheAcceptableRange()
        {
            // NOTE: Oxygen values have both an upper and lower limit. The other values only have max value.
            AssertInvalidReadings(x => x.OxygenPercentageBottom = 0);
            AssertInvalidReadings(x => x.OxygenPercentageMiddle = 0);
            AssertInvalidReadings(x => x.OxygenPercentageTop = 0);
            AssertInvalidReadings(x => x.OxygenPercentageBottom = 50);
            AssertInvalidReadings(x => x.OxygenPercentageMiddle = 50);
            AssertInvalidReadings(x => x.OxygenPercentageTop = 50);

            // Ensure that the lower and upper limits are valid as well as anything in between.
            AssertValidReadings(x =>
                x.OxygenPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN);
            AssertValidReadings(x =>
                x.OxygenPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN);
            AssertValidReadings(x =>
                x.OxygenPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN);
            AssertValidReadings(x =>
                x.OxygenPercentageBottom = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX);
            AssertValidReadings(x =>
                x.OxygenPercentageMiddle = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX);
            AssertValidReadings(x =>
                x.OxygenPercentageTop = ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX);
            AssertValidReadings(x => x.OxygenPercentageBottom = 20m);
            AssertValidReadings(x => x.OxygenPercentageMiddle = 20m);
            AssertValidReadings(x => x.OxygenPercentageTop = 20m);
        }

        #endregion
    }
}
