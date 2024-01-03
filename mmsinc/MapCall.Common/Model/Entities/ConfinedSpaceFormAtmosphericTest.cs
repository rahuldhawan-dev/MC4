using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Confined space atmospheric tests are performed in three different
    /// locations of the confined space: Top, Middle, and Bottom. All of
    /// these tests are done at the same time.
    ///
    /// These should not be edited as they're considered signed information
    /// that's verified by the user at entry time.
    /// </summary>
    [Serializable]
    public class ConfinedSpaceFormAtmosphericTest : IEntity
    {
        #region Consts

        public struct AcceptableConcentrations
        {
            // NOTE: If you change ANY of these values, you need to update
            // the View Descriptions for the properties.
            public const decimal LEL_MAX = 10.0m,
                                 OXYGEN_MIN = 19.5m,
                                 OXYGEN_MAX = 23.5m;

            public const int CO_MAX = 35,
                             H2S_MAX = 10;

            // If the descriptions of these change, you may need to update
            // the formulas in the validation methods below.
            public const string
                CO = "Acceptable range: < 35ppm",
                H2S = "Acceptable range: < 10ppm",
                LEL = "Acceptable range: < 10%",
                OXYGEN = "Acceptable range: > 19.5% & < 23.5%";

            // If any of these validation methods get changed, the descriptions above
            // also need to be changed to reflect them.
            public static bool IsValidOxygenValue(decimal value) => OXYGEN_MIN <= value && value <= OXYGEN_MAX;
            public static bool IsValidLowerExplosiveLimitValue(decimal value) => value <= LEL_MAX;
            public static bool IsValidCarbonMonoxideValue(int value) => value <= CO_MAX;
            public static bool IsValidHydrogenSulfideValue(int value) => value <= H2S_MAX;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ConfinedSpaceForm ConfinedSpaceForm { get; set; }
        public virtual Employee TestedBy { get; set; }
        public virtual DateTime TestedAt { get; set; }

        [View(DisplayName = "When Was Reading Taken")]
        public virtual ConfinedSpaceFormReadingCaptureTime ConfinedSpaceFormReadingCaptureTime { get; set; }

        #region Top tests

        [View("Oxygen %", MMSINC.Utilities.FormatStyle.PercentageUnmodified,
            Description = AcceptableConcentrations.OXYGEN)]
        public virtual decimal OxygenPercentageTop { get; set; }

        [View("LEL %", MMSINC.Utilities.FormatStyle.PercentageUnmodified, Description = AcceptableConcentrations.LEL)]
        public virtual decimal LowerExplosiveLimitPercentageTop { get; set; }

        [View("CO ppm", Description = AcceptableConcentrations.CO)]
        public virtual int CarbonMonoxidePartsPerMillionTop { get; set; }

        [View("H2S ppm", Description = AcceptableConcentrations.H2S)]
        public virtual int HydrogenSulfidePartsPerMillionTop { get; set; }

        #endregion

        #region Middle tests

        [View("Oxygen %", MMSINC.Utilities.FormatStyle.PercentageUnmodified,
            Description = AcceptableConcentrations.OXYGEN)]
        public virtual decimal OxygenPercentageMiddle { get; set; }

        [View("LEL %", MMSINC.Utilities.FormatStyle.PercentageUnmodified, Description = AcceptableConcentrations.LEL)]
        public virtual decimal LowerExplosiveLimitPercentageMiddle { get; set; }

        [View("CO ppm", Description = AcceptableConcentrations.CO)]
        public virtual int CarbonMonoxidePartsPerMillionMiddle { get; set; }

        [View("H2S(ppm", Description = AcceptableConcentrations.H2S)]
        public virtual int HydrogenSulfidePartsPerMillionMiddle { get; set; }

        #endregion

        #region Bottom tests

        [View("Oxygen %", MMSINC.Utilities.FormatStyle.PercentageUnmodified,
            Description = AcceptableConcentrations.OXYGEN)]
        public virtual decimal OxygenPercentageBottom { get; set; }

        [View("LEL %", MMSINC.Utilities.FormatStyle.PercentageUnmodified, Description = AcceptableConcentrations.LEL)]
        public virtual decimal LowerExplosiveLimitPercentageBottom { get; set; }

        [View("CO ppm", Description = AcceptableConcentrations.CO)]
        public virtual int CarbonMonoxidePartsPerMillionBottom { get; set; }

        [View("H2S ppm", Description = AcceptableConcentrations.H2S)]
        public virtual int HydrogenSulfidePartsPerMillionBottom { get; set; }

        #endregion

        #region Logical

        private bool AllOxygenReadingsHaveAcceptableConcentrations =>
            AcceptableConcentrations.IsValidOxygenValue(OxygenPercentageBottom) &&
            AcceptableConcentrations.IsValidOxygenValue(OxygenPercentageMiddle) &&
            AcceptableConcentrations.IsValidOxygenValue(OxygenPercentageTop);

        private bool AllLowerExplosiveLimitReadingsHaveAcceptableConcentrations =>
            AcceptableConcentrations.IsValidLowerExplosiveLimitValue(LowerExplosiveLimitPercentageBottom) &&
            AcceptableConcentrations.IsValidLowerExplosiveLimitValue(LowerExplosiveLimitPercentageMiddle) &&
            AcceptableConcentrations.IsValidLowerExplosiveLimitValue(LowerExplosiveLimitPercentageTop);

        private bool AllCarbonMonoxideReadingsHaveAcceptableConcentrations =>
            AcceptableConcentrations.IsValidCarbonMonoxideValue(CarbonMonoxidePartsPerMillionBottom) &&
            AcceptableConcentrations.IsValidCarbonMonoxideValue(CarbonMonoxidePartsPerMillionMiddle) &&
            AcceptableConcentrations.IsValidCarbonMonoxideValue(CarbonMonoxidePartsPerMillionTop);

        private bool AllHydrogenSulfideReadingsHaveAcceptableConcentrations =>
            AcceptableConcentrations.IsValidHydrogenSulfideValue(HydrogenSulfidePartsPerMillionBottom) &&
            AcceptableConcentrations.IsValidHydrogenSulfideValue(HydrogenSulfidePartsPerMillionMiddle) &&
            AcceptableConcentrations.IsValidHydrogenSulfideValue(HydrogenSulfidePartsPerMillionTop);

        /// <summary>
        /// If this returns false, then Section 2 of the ConfinedSpaceForm is required.
        /// </summary>
        public virtual bool AllReadingsHaveAcceptableConcentrations =>
            AllOxygenReadingsHaveAcceptableConcentrations &&
            AllLowerExplosiveLimitReadingsHaveAcceptableConcentrations &&
            AllCarbonMonoxideReadingsHaveAcceptableConcentrations &&
            AllHydrogenSulfideReadingsHaveAcceptableConcentrations;

        #endregion

        #endregion
    }
}
