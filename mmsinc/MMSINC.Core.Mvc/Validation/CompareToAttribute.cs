using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;

namespace MMSINC.Validation
{
    /// <summary>
    /// Expanded version of the CompareAttribute so that two properties can be compared outside
    /// of strict equality.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CompareToAttribute : ValidationAttribute, IClientValidatable
    {
        #region Fields

        private static readonly Dictionary<TypeCode, string> _convertedValueTypeClientParameters;

        #endregion

        #region Properties

        public string OtherProperty { get; private set; }

        public ComparisonType ComparisonType { get; private set; }

        /// <summary>
        /// If set, values will be converted to this type before being compared.
        /// </summary>
        public TypeCode ConvertedValueType { get; private set; }

        /// <summary>
        /// If true, this validator will not run if the value being tested is null. Use this
        /// if you need to compare values on properties that aren't required(like start dates that
        /// do not have their end dates set until later).
        /// </summary>
        public bool IgnoreNullValues { get; set; }

        #endregion

        #region Constructors

        static CompareToAttribute()
        {
            // There are only three possible client side comparison types:
            // integer, float, and date. 
            _convertedValueTypeClientParameters = new Dictionary<TypeCode, string> {
                {TypeCode.Int16, "integer"},
                {TypeCode.Int32, "integer"},
                {TypeCode.Int64, "integer"},
                {TypeCode.Byte, "integer"},
                {TypeCode.SByte, "integer"},
                {TypeCode.UInt16, "integer"},
                {TypeCode.UInt32, "integer"},
                {TypeCode.UInt64, "integer"},
                {TypeCode.Decimal, "float"},
                {TypeCode.Single, "float"},
                {TypeCode.Double, "float"},
                {TypeCode.DateTime, "date"},
                {TypeCode.String, "string"}
            };
        }

        public CompareToAttribute(string otherProperty, TypeCode valueType)
        {
            otherProperty.ThrowIfNullOrWhiteSpace("otherProperty");
            OtherProperty = otherProperty;

            if (!_convertedValueTypeClientParameters.ContainsKey(valueType))
            {
                throw new NotImplementedException("CompareToAttribute does not have an implementation for TypeCode." +
                                                  valueType.ToString());
            }

            ConvertedValueType = valueType;
        }

        public CompareToAttribute(string otherProperty, ComparisonType compType, TypeCode valueType) : this(
            otherProperty, valueType)
        {
            if (compType == ComparisonType.EqualToAny || compType == ComparisonType.NotEqualToAny)
            {
                throw new NotImplementedException(compType.ToString());
            }

            ComparisonType = compType;
        }

        #endregion

        #region Private Methods

        private string GetErrorMessage(string propName)
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                return ErrorMessage;
            }

            const string format = "{0} must be {1} {2}.";
            var words = Wordify.SpaceOutWordsFromCamelCase(ComparisonType.ToString()).ToLower();
            return string.Format(format, propName, words, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(OtherProperty);

            // Let's ensure this fails fast if we can't actually find
            // the dependent property.
            if (field == null)
            {
                throw new MissingMemberException(
                    containerType.AssemblyQualifiedName, OtherProperty);
            }

            // get the value of the dependent property
            var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

            if (IgnoreNullValues && value == null)
            {
                return ValidationResult.Success;
            }

            if (!DependentValueHasRequiredMatchForValue(value, dependentValue, ComparisonType, ConvertedValueType))
            {
                // compare the value against the target value
                // match => means we should try validating this field
                // validation failed - return an error
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName),
                    new[] {validationContext.MemberName});
            }

            return ValidationResult.Success;
        }

        private bool DependentValueHasRequiredMatchForValue(object targetValue, object actualDependentValue,
            ComparisonType comparison, TypeCode dependentTypeCode)
        {
            var target = (IComparable)targetValue;
            var actual = (IComparable)actualDependentValue;

            if (dependentTypeCode == TypeCode.String)
            {
                // We wanna replace null with string.Empty to make things
                // easier when comparing.
                target = target ?? string.Empty;
                actual = actual ?? string.Empty;
            }

            switch (comparison)
            {
                case ComparisonType.EqualTo:
                    return ComparisonEqualsTo(target, actual);
                case ComparisonType.NotEqualTo:
                    return !ComparisonEqualsTo(target, actual);
                case ComparisonType.GreaterThan:
                    return ComparisonGreaterThanAndMaybeEqualTo(target, actual, false);
                case ComparisonType.GreaterThanOrEqualTo:
                    return ComparisonGreaterThanAndMaybeEqualTo(target, actual, true);
                case ComparisonType.LessThan:
                    return ComparisonLessThanAndMaybeEqualTo(target, actual, false);
                case ComparisonType.LessThanOrEqualTo:
                    return ComparisonLessThanAndMaybeEqualTo(target, actual, true);
                default:
                    throw new NotSupportedException(comparison.ToString());
            }
        }

        private static bool ComparisonEqualsTo(IComparable targetValue, IComparable actualDependentValue)
        {
            if (targetValue == null && actualDependentValue == null)
            {
                return true;
            }

            if (targetValue == null || actualDependentValue == null)
            {
                return false;
            }

            return actualDependentValue.Equals(targetValue);
        }

        private static bool ComparisonGreaterThanAndMaybeEqualTo(IComparable targetValue,
            IComparable actualDependentValue, bool canBeEqual)
        {
            if (targetValue == null && actualDependentValue == null)
            {
                return false;
            }

            if (targetValue == null || actualDependentValue == null)
            {
                return false;
            }

            var result = targetValue.CompareTo(actualDependentValue);
            return (result > 0 || (canBeEqual && result == 0));
        }

        private static bool ComparisonLessThanAndMaybeEqualTo(IComparable targetValue, IComparable actualDependentValue,
            bool canBeEqual)
        {
            if (targetValue == null && actualDependentValue == null)
            {
                return false;
            }

            if (targetValue == null || actualDependentValue == null)
            {
                return false;
            }

            var result = targetValue.CompareTo(actualDependentValue);
            return (result < 0 || (canBeEqual && result == 0));
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule {
                ErrorMessage = GetErrorMessage(metadata.PropertyName),
                ValidationType = "compareto"
            };
            rule.ValidationParameters["other"] = OtherProperty;
            rule.ValidationParameters["comparison"] = ComparisonType.ToString().ToLower();
            rule.ValidationParameters["type"] = _convertedValueTypeClientParameters[ConvertedValueType];

            if (IgnoreNullValues)
            {
                rule.ValidationParameters["ignorenullvalues"] = IgnoreNullValues;
            }

            yield return rule;
        }

        #endregion
    }
}
