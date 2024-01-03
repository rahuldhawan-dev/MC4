using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    /// <summary>
    /// NOTE: There is no edit model for this as users are not supposed to edit these.
    /// </summary>
    public class CreateConfinedSpaceFormAtmosphericTest : ViewModel<ConfinedSpaceFormAtmosphericTest>
    {
        #region Constants

        public const string ERROR_CONFIRM_READINGS = "Please confirm the readings entered are accurate.";

        #endregion

        #region Properties

        [Required, DateTimePicker]
        public DateTime? TestedAt { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ConfinedSpaceFormReadingCaptureTime))]
        public int? ConfinedSpaceFormReadingCaptureTime { get; set; }

        [CheckBox]
        [RequiredWhenOxygenIsOutOfRange(nameof(OxygenPercentageBottom), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenOxygenIsOutOfRange(nameof(OxygenPercentageMiddle), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenOxygenIsOutOfRange(nameof(OxygenPercentageTop), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenLowerExplosiveLimitIsOutOfRange(nameof(LowerExplosiveLimitPercentageBottom), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenLowerExplosiveLimitIsOutOfRange(nameof(LowerExplosiveLimitPercentageMiddle), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenLowerExplosiveLimitIsOutOfRange(nameof(LowerExplosiveLimitPercentageTop), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenHydrogenSulfideIsOutOfRange(nameof(HydrogenSulfidePartsPerMillionBottom), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenHydrogenSulfideIsOutOfRange(nameof(HydrogenSulfidePartsPerMillionMiddle), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenHydrogenSulfideIsOutOfRange(nameof(HydrogenSulfidePartsPerMillionTop), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenCarbonMonoxideIsOutOfRange(nameof(CarbonMonoxidePartsPerMillionBottom), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenCarbonMonoxideIsOutOfRange(nameof(CarbonMonoxidePartsPerMillionMiddle), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [RequiredWhenCarbonMonoxideIsOutOfRange(nameof(CarbonMonoxidePartsPerMillionTop), ErrorMessage = ERROR_CONFIRM_READINGS)]
        [DoesNotAutoMap("This doesn't exist on the entity because the existence of a test record indicates that they accepted this.")]
        public bool? AcknowledgedValuesAreOutOfRange { get; set; }

        #region Top tests

        [Required, Min(0)]
        public decimal? OxygenPercentageTop { get; set; }
        
        [Required, Min(0)]
        public decimal? LowerExplosiveLimitPercentageTop { get; set; }
        
        [Required, Min(0)]
        public int? CarbonMonoxidePartsPerMillionTop { get; set; }
        
        [Required, Min(0)]
        public int? HydrogenSulfidePartsPerMillionTop { get; set; }

        #endregion

        #region Middle tests
        
        [Required, Min(0)]
        public decimal? OxygenPercentageMiddle { get; set; }
        
        [Required, Min(0)]
        public decimal? LowerExplosiveLimitPercentageMiddle { get; set; }
        
        [Required, Min(0)]
        public int? CarbonMonoxidePartsPerMillionMiddle { get; set; }
        
        [Required, Min(0)]
        public int? HydrogenSulfidePartsPerMillionMiddle { get; set; }

        #endregion

        #region Bottom tests
        
        [Required, Min(0)]
        public decimal? OxygenPercentageBottom { get; set; }
        
        [Required, Min(0)]
        public decimal? LowerExplosiveLimitPercentageBottom { get; set; }
        
        [Required, Min(0)]
        public int? CarbonMonoxidePartsPerMillionBottom { get; set; }
        
        [Required, Min(0)]
        public int? HydrogenSulfidePartsPerMillionBottom { get; set; }

        #endregion

        #endregion

        #region Constructor

        public CreateConfinedSpaceFormAtmosphericTest(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override ConfinedSpaceFormAtmosphericTest MapToEntity(ConfinedSpaceFormAtmosphericTest entity)
        {
            base.MapToEntity(entity);
            // TODO: Validation failure for user without employee.
            entity.TestedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: Do we add validation here for the acceptable constraints range? They're allowed to
            // enter whatever they want, it's just that they need a client-side prompt indicating they
            // accept that they're entering invalid values.
            //
            // Yes do validation. Also, you may be wondering "Why not use RequiredWhen for this?"
            // Well, for one, attributes don't support decimals, so I'd have to shove in some conversion
            // aspect to RequiredWhenAttribute. Two, RequiredWhen also does not currently support having
            // more than one RequiredWhenAttribute where the dependent property is used twice. This makes it
            // impossible to do ranges that way. Also, RequiredWhen doesn't support a ComparisonType.Between, which
            // would be pretty nice to have right about now.
            return base.Validate(validationContext);
        }

        #endregion

        #region Helper validation attributes
        
        private class RequiredWhenCarbonMonoxideIsOutOfRangeAttribute : RequiredWhenAttribute
        {
            public RequiredWhenCarbonMonoxideIsOutOfRangeAttribute(string dependentProperty)
                : base(dependentProperty, 
                       ComparisonType.GreaterThan,
                       ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX) { }
        }
        private class RequiredWhenHydrogenSulfideIsOutOfRangeAttribute : RequiredWhenAttribute
        {
            public RequiredWhenHydrogenSulfideIsOutOfRangeAttribute(string dependentProperty) 
                : base(dependentProperty,
                       ComparisonType.GreaterThan,
                       ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX) { }
        }

        private class RequiredWhenLowerExplosiveLimitIsOutOfRangeAttribute : RequiredWhenAttribute
        {
            public RequiredWhenLowerExplosiveLimitIsOutOfRangeAttribute(string dependentProperty) 
                : base(dependentProperty, 
                       ComparisonType.GreaterThan,
                       (double)ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX,
                       typeToConvertTargetValueTo: typeof(decimal)) { }
        }

        private class RequiredWhenOxygenIsOutOfRangeAttribute : RequiredWhenAttribute
        {
            public RequiredWhenOxygenIsOutOfRangeAttribute(string dependentProperty)
                : base(dependentProperty, 
                       ComparisonType.NotBetween,
                       (double)ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN,
                       (double)ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX,
                       convertToDecimal: true) { }
        }

        #endregion
    }
}