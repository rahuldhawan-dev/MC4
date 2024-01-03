using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MMSINC.Utilities;
using StructureMap;

// ReSharper disable once CheckNamespace
namespace MMSINC.Validation
{
    /// <summary>
    /// Validator that indicates a property is only required when another property's value matches certain criteria.
    /// </summary>
    /// <remarks>
    ///
    /// The client-side script is located in MapCall.Common.Mvc/Content/JS/required-when-validation.js
    ///
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredWhenAttribute : ValidationAttribute, IClientValidatable
    {
        #region Type Codes

        /// <summary>
        /// This exists solely because TypeCode becomes unhelpful
        /// when dealing with specific types
        /// </summary>
        protected enum ValueTypeCode
        {
            Int16,
            Int32,
            Int64,
            Byte,
            SByte,
            UInt16,
            UInt32,
            UInt64,
            Single,
            Double,
            Decimal,
            DateTime,
            String,
            Boolean,
            RangeDecimal,
            RangeDouble,
            RangeInt32,
            Unknown
        }

        private static ValueTypeCode GetTypeCode(Type type)
        {
            // First attempt to map the System.TypeCode to our internal ValueTypeCode.

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return ValueTypeCode.Boolean;
                case TypeCode.Byte: return ValueTypeCode.Byte;
                case TypeCode.DateTime: return ValueTypeCode.DateTime;
                case TypeCode.Decimal: return ValueTypeCode.Decimal;
                case TypeCode.Double: return ValueTypeCode.Double;
                case TypeCode.Int16: return ValueTypeCode.Int16;
                case TypeCode.Int32: return ValueTypeCode.Int32;
                case TypeCode.Int64: return ValueTypeCode.Int64;
                case TypeCode.SByte: return ValueTypeCode.SByte;
                case TypeCode.Single: return ValueTypeCode.Single;
                case TypeCode.String: return ValueTypeCode.String;
                case TypeCode.UInt16: return ValueTypeCode.UInt16;
                case TypeCode.UInt32: return ValueTypeCode.UInt32;
                case TypeCode.UInt64: return ValueTypeCode.UInt64;
            }

            if (type == typeof(Range<decimal>))
            {
                return ValueTypeCode.RangeDecimal;
            }

            if (type == typeof(Range<double>))
            {
                return ValueTypeCode.RangeDouble;
            }
            else if (type == typeof(Range<int>))
            {
                return ValueTypeCode.RangeInt32;
            }

            throw new NotSupportedException(
                $"Unable to get a ValueTypeCode for type {type}. System.TypeCode was {Type.GetTypeCode(type)}.");
        }

        #endregion

        #region Private Members

        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();
        private readonly object _typeId = new object();
        private bool _hasDynamicValue;
        private readonly object _staticTargetValue;
        private ThreadSafeAttributeValue _staticThreadSafeValue; // Will only be set for non-dynamic values.
        private string _dynamicValueCallbackMethod;
        private Type _dynamicValueCallbackType;

        private static readonly Dictionary<ValueTypeCode, string> _javaScriptValueTypesByValueTypeCode;
        private static readonly ComparisonType[] _acceptableComparisonTypesForArrays;
        private static readonly ComparisonType[] _acceptableComparisonTypesForStringsAndBools;

        #endregion

        #region Properties

        public string DependentProperty { get; set; }

        // Apparently you're supposed to override TypeId when the
        // AttributeUsage is set to AllowMultiple = true and used
        // in an MVC scenario.
        public override object TypeId
        {
            get { return _typeId; }
        }

        // This is used in Permits if you're ever wondering where it's being used.
        /// <summary>
        /// Gets/sets whether validation is handled only on the client side. If true, you need to handle
        /// validation on the server yourself. Default is false.
        /// </summary>
        public bool ClientSideOnly { get; set; }

        public ComparisonType ComparisonType { get; private set; }

        private bool IsRangeComparisonType =>
            ComparisonType == ComparisonType.Between || ComparisonType == ComparisonType.NotBetween;

        /// <summary>
        /// When true, the field is only visible when it's required. False by default.
        /// </summary>
        public bool FieldOnlyVisibleWhenRequired { get; set; }

        #endregion

        #region Constructors

        static RequiredWhenAttribute()
        {
            _javaScriptValueTypesByValueTypeCode = new Dictionary<ValueTypeCode, string> {
                {ValueTypeCode.Int16, "integer"},
                {ValueTypeCode.Int32, "integer"},
                {ValueTypeCode.Int64, "integer"},
                {ValueTypeCode.Byte, "integer"},
                {ValueTypeCode.SByte, "integer"},
                {ValueTypeCode.UInt16, "integer"},
                {ValueTypeCode.UInt32, "integer"},
                {ValueTypeCode.UInt64, "integer"},
                {ValueTypeCode.Single, "float"},
                {ValueTypeCode.Decimal, "float"},
                {ValueTypeCode.Double, "float"},
                {ValueTypeCode.DateTime, "date"},
                {ValueTypeCode.String, "string"},
                {ValueTypeCode.Boolean, "boolean"},
                {ValueTypeCode.RangeDecimal, "range-float"},
                {ValueTypeCode.RangeDouble, "range-float"},
                {ValueTypeCode.RangeInt32, "range-integer"}
            };

            _acceptableComparisonTypesForStringsAndBools = new[] {ComparisonType.EqualTo, ComparisonType.NotEqualTo};
            _acceptableComparisonTypesForArrays = new[] {ComparisonType.EqualToAny, ComparisonType.NotEqualToAny};
        }

        /// <summary>
        /// Defines a validator that a property is a required if the dependent is equal to a specific value.
        /// </summary>
        public RequiredWhenAttribute(string dependentProperty, object targetValue)
            : this(dependentProperty, ComparisonType.EqualTo, targetValue) { }

        /// <summary>
        /// Defines a validator that requires a property value when the dependent has a value that falls
        /// within or outside of a specified range. This must be used with the ComparisonType.Between or
        /// ComparisonType.NotBetween values.
        /// </summary>
        /// <param name="dependentProperty"></param>
        /// <param name="comparisonType">Must be ComparisonType.Between or ComparisonType.NotBetween</param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="convertToDecimal">Set to true if the min/max values need to be converted from double to decimals due to the limitations of C# attributes.</param>
        public RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, double minValue,
            double maxValue, bool convertToDecimal = false)
            : this(dependentProperty, comparisonType)
        {
            if (convertToDecimal)
            {
                _staticTargetValue = new Range<decimal>(Convert.ToDecimal(minValue), Convert.ToDecimal(maxValue));
            }
            else
            {
                _staticTargetValue = new Range<double>(minValue, maxValue);
            }
        }

        /// <summary>
        /// Defines a validator that requires a property value when the dependent has a value that falls
        /// within or outside of a specified range. This must be used with the ComparisonType.Between or
        /// ComparisonType.NotBetween values.
        /// </summary>
        /// <param name="dependentProperty"></param>
        /// <param name="comparisonType">Must be ComparisonType.Between or ComparisonType.NotBetween</param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, int minValue,
            int maxValue)
            : this(dependentProperty, comparisonType, new Range<int>(minValue, maxValue)) { }

        /// <summary>
        /// Defines a validator that a property is a required if the dependent is equal to a specific date.
        /// </summary>
        public RequiredWhenAttribute(string dependentProperty, int year, int month, int day)
            : this(dependentProperty, new DateTime(year, month, day)) { }

        /// <summary>
        /// Defines a validator that a property is a required if the dependent can be compared to a specific date.
        /// </summary>
        public RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, int year, int month,
            int day)
            : this(dependentProperty, comparisonType, new DateTime(year, month, day)) { }

        /// <summary>
        /// Creates a validator that makes a property required when the dependent property matches the comparison
        /// to the given static value.
        /// </summary>
        /// <param name="dependentProperty"></param>
        /// <param name="comparisonType"></param>
        /// <param name="targetValue"></param>
        /// <param name="typeToConvertTargetValueTo">Change the targetValue to the given type. This really only exists
        /// so that we can convert doubles to decimals as you can't use decimals in attribute constructors.
        /// </param>
        public RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, object targetValue,
            Type typeToConvertTargetValueTo = null)
            : this(dependentProperty, comparisonType)
        {
            if (typeToConvertTargetValueTo != null)
            {
                targetValue = Convert.ChangeType(targetValue, typeToConvertTargetValueTo);
            }

            _staticTargetValue = targetValue;
        }

        /// <summary>
        /// ALL constructors should eventually filter down to using this one.
        /// </summary>
        /// <param name="dependentProperty"></param>
        /// <param name="comparisonType"></param>
        private RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType)
            : base(() => "The {0} field is required.")
        {
            DependentProperty = dependentProperty;
            ComparisonType = comparisonType;
        }

        /// <summary>
        /// Defines a validator by getting a target value by calling a method on a type.
        /// </summary>
        /// <param name="dynamicValueCallbackMethod">The method name that will be called to find the target value for this validator.</param>
        /// <param name="dynamicValueCallbackType">The type the method belongs to.</param>
        public RequiredWhenAttribute(string dependentProperty, string dynamicValueCallbackMethod,
            Type dynamicValueCallbackType)
            : this(dependentProperty, ComparisonType.EqualTo, dynamicValueCallbackMethod, dynamicValueCallbackType) { }

        /// <summary>
        /// Defines a validator by getting a target value by calling a method on a type.
        /// </summary>
        /// <param name="dynamicValueCallbackMethod">The method name that will be called to find the target value for this validator.</param>
        /// <param name="dynamicValueCallbackType">The type the method belongs to.</param>
        public RequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType,
            string dynamicValueCallbackMethod,
            Type dynamicValueCallbackType) : this(dependentProperty, comparisonType)
        {
            _dynamicValueCallbackMethod = dynamicValueCallbackMethod;
            _dynamicValueCallbackType = dynamicValueCallbackType;
            _hasDynamicValue = true;
        }

        protected ThreadSafeAttributeValue GetThreadSafeValue(IContainer container)
        {
            // Only skip the initialization when there isn't a dynamic value. Dynamic values need to be
            // checked everytime since it's implied they can change.
            if (_staticThreadSafeValue != null)
            {
                return _staticThreadSafeValue;
            }

            // DO NOT use the TargetValue getter here as it will cause a stack overflow.
            var tv = _hasDynamicValue
                ? GetDynamicTargetValue(container, _dynamicValueCallbackMethod, _dynamicValueCallbackType)
                : _staticTargetValue;

            var safeValue = new ThreadSafeAttributeValue();
            safeValue.TargetValue = tv;

            if (tv is Array) // Don't check for IEnumerable since that'll include strings.
            {
                var targetArray = ((Array)tv).Cast<object>().ToArray();
                if (!targetArray.Any())
                {
                    throw new InvalidOperationException(
                        "Arrays must have at least one item. Really it should have more than one item, but whatever floats your boat.");
                }

                if (!_acceptableComparisonTypesForArrays.Contains(ComparisonType))
                {
                    throw new NotSupportedException(string.Format("Unable to use {0} with arrays.", ComparisonType));
                }

                safeValue.DependentTypeCode = GetTypeCode(targetArray.First().GetType());
            }
            else
            {
                // For now, using TypeCode.String for any null values. 
                safeValue.DependentTypeCode = (tv != null ? GetTypeCode(tv.GetType()) : ValueTypeCode.String);
                if (safeValue.DependentTypeCode == ValueTypeCode.String ||
                    safeValue.DependentTypeCode == ValueTypeCode.Boolean)
                {
                    if (!_acceptableComparisonTypesForStringsAndBools.Contains(ComparisonType))
                    {
                        throw new NotSupportedException(string.Format("Unable to use {0} with {1}.", ComparisonType,
                            safeValue.DependentTypeCode));
                    }
                }
            }

            if (!_javaScriptValueTypesByValueTypeCode.ContainsKey(safeValue.DependentTypeCode))
            {
                throw new NotImplementedException(
                    "RequiredWhenAttribute does not have an implementation for TypeCode." +
                    safeValue.DependentTypeCode);
            }

            safeValue.DependentClientSideTypeCode = _javaScriptValueTypesByValueTypeCode[safeValue.DependentTypeCode];

            if (!_hasDynamicValue)
            {
                _staticThreadSafeValue = safeValue;
            }

            return safeValue;
        }

        #endregion

        #region Private Methods

        private static object GetDynamicTargetValue(IContainer container, string methodName, Type type)
        {
            var method = type.GetMethod(methodName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Static);
            if (method == null)
            {
                throw new MissingMethodException("Unable to find method '" + methodName + "' on type '" +
                                                 type.FullName + "'.");
            }

            if (!method.IsStatic)
            {
                throw new InvalidOperationException("Method '" + methodName + "' must be static.");
            }

            var parameters = method.GetParameters();

            if (parameters.Length == 0)
            {
                return method.Invoke(null, new object[0]);
            }

            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IContainer))
            {
                return method.Invoke(null, new object[] {container});
            }

            throw new InvalidOperationException("Method '" + methodName +
                                                "' must accept a single IContainer argument or no argument.");
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property

            var depProp =
                viewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(DependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + ".";
            if (depProp.StartsWith(thisField))
            {
                // This deals with GetFullHtmlFieldName returning Property.DependentProperty
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            }

            else
            {
                var possibleEnding = string.Format(".{0}.{1}", metadata.PropertyName, DependentProperty);
                if (depProp.EndsWith(possibleEnding))
                {
                    // This deals with GetFullHtmlFieldName returning ParentObject.Property.DependentProperty
                    var root = depProp.Substring(0, depProp.Length - possibleEnding.Length);
                    depProp = root + "." + DependentProperty;
                }
            }

            return depProp;
        }

        #region Methods for checking that the actual dependency value matches the target value

        private bool DependentValueHasRequiredMatch(IContainer container, object actualDependentValue,
            string dependentPropertyName)
        {
            var safeVal = GetThreadSafeValue(container);
            if (safeVal.TargetValue is Array)
            {
                var targetVal = ((IEnumerable)safeVal.TargetValue).OfType<object>().ToArray();
                return DependentValueHasRequiredMatchForArray(targetVal, actualDependentValue, ComparisonType,
                    safeVal.DependentTypeCode, dependentPropertyName);
            }
            else if (actualDependentValue is IList)
            {
                return ((IList)actualDependentValue).Contains(safeVal.TargetValue);
            }
            else
            {
                return DependentValueHasRequiredMatchForSingleValue(safeVal.TargetValue, actualDependentValue,
                    ComparisonType, safeVal.DependentTypeCode, dependentPropertyName);
            }
        }

        private static bool DependentValueHasRequiredMatchForArray(IEnumerable<object> targetValue,
            object actualDependentValue, ComparisonType comparison, ValueTypeCode dependentTypeCode,
            string dependentPropertyName)
        {
            switch (comparison)
            {
                case ComparisonType.EqualToAny:
                    return targetValue.Any(x => DependentValueHasRequiredMatchForSingleValue(x, actualDependentValue,
                        ComparisonType.EqualTo, dependentTypeCode, dependentPropertyName));

                case ComparisonType.NotEqualToAny:
                    return !targetValue.Any(x => DependentValueHasRequiredMatchForSingleValue(x, actualDependentValue,
                        ComparisonType.EqualTo, dependentTypeCode, dependentPropertyName));

                default:
                    throw new NotSupportedException(comparison.ToString());
            }
        }

        private static bool DependentValueHasRequiredMatchForSingleValue(object targetValue,
            object actualDependentValue, ComparisonType comparison, ValueTypeCode dependentTypeCode,
            string dependentPropertyName)
        {
            // We're checking the types here because comparisons will fail if the types aren't the same. 
            // ex: If the targetValue supplied is a short, but the actualDependentValue is an int, they
            // will not be equal even if they are both the same numerical value.
            //
            // We can't do this check if the values are null, and it shouldn't be an issue in that case anyway.
            //
            // If you're using an enum value, either cast it to whatever type is necessary, or change the dependent property from an int to the enum value instead.
            if (actualDependentValue != null && targetValue != null)
            {
                // An IRange will be the only time that the types do not match.
                if (targetValue.GetType() != actualDependentValue.GetType() && !(targetValue is IRange))
                {
                    // My apologies that this error message only contains the dependent property name 
                    // and not the type or the property that's actually being validated. -Ross 1/26/2018
#if DEBUG
                    throw new InvalidOperationException(
                        $"RequiredWhen that depends on the property '{dependentPropertyName}'({actualDependentValue.GetType()}) was initialized with an inconsistent value({targetValue.GetType()}).");
#endif
                }
            }

            var actual = (IComparable)actualDependentValue;

            // NOTE: This is broken out to its own switch statement
            // because targetValue needs to be cast to IRange. All of the
            // other comparisons afterwards need to be cast to IComparable.
            // Doing this instead of repeatedly casting to IComparable
            // all over the place.
            switch (comparison)
            {
                case ComparisonType.Between:
                    return ComparisonBetween((IRange)targetValue, actual);
                case ComparisonType.NotBetween:
                    return ComparisonNotBetween((IRange)targetValue, actual);
            }

            var target = (IComparable)targetValue;

            if (dependentTypeCode == ValueTypeCode.String)
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

        private static bool ComparisonBetween(IRange targetValue, IComparable actualDependentValue)
        {
            // If there isn't a dependent value actually set then there's no way for it
            // to be inside or outside of the range. targetValue can't be null because it
            // is a Range created by the constructor.
            if (actualDependentValue == null)
            {
                return false;
            }

            // This is the reverse of what the other methods do because the target value
            // is a Range<T> rather than the same type as the actual dependant value.
            return ((IRange)targetValue).IsInRange(actualDependentValue);
        }

        private static bool ComparisonNotBetween(IRange targetValue, IComparable actualDependentValue)
        {
            // If there isn't a dependent value actually set then there's no way for it
            // to be inside or outside of the range. targetValue can't be null because it
            // is a Range created by the constructor.
            if (actualDependentValue == null)
            {
                return false;
            }

            // This is the reverse of what the other methods do because the target value
            // is a Range<T> rather than the same type as the actual dependant value.
            return !((IRange)targetValue).IsInRange(actualDependentValue);
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

            var result = actualDependentValue.CompareTo(targetValue);
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

            var result = actualDependentValue.CompareTo(targetValue);
            return (result < 0 || (canBeEqual && result == 0));
        }

        #endregion

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (ClientSideOnly)
            {
                return ValidationResult.Success;
            }

            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentProperty);

            // Let's ensure this fails fast if we can't actually find
            // the dependent property.
            if (field == null)
            {
                throw new MissingMemberException(
                    containerType.AssemblyQualifiedName, DependentProperty);
            }

            // get the value of the dependent property
            var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

            if (DependentValueHasRequiredMatch((IContainer)validationContext.GetService(typeof(IContainer)),
                dependentValue, DependentProperty))
            {
                // compare the value against the target value
                // match => means we should try validating this field
                if (!_innerAttribute.IsValid(value))
                {
                    // validation failed - return an error
                    return new ValidationResult(ErrorMessage, new[] {validationContext.MemberName});
                }
            }

            return ValidationResult.Success;
        }

        #region Methods related to GetClientValidationRules

        /// <summary>
        /// The serialized value that ends up in the unobtrusive attributes.
        /// This value needs to be parsed on the client.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private object GetRawValueForClientSide(object val)
        {
            if (val == null)
            {
                return string.Empty;
            }

            if (val.GetType().IsEnum)
            {
                // We don't wanna ToString() the enum value because
                // we'll end up with the name instead of the int value.
                return (int)val;
            }

            return val;
        }

        private string GetSerializableClientTargetValueForSingleValue(ThreadSafeAttributeValue safeValue)
        {
            var rawTarget = GetRawValueForClientSide(safeValue.TargetValue);

            var stringTarg = rawTarget.ToString();

            if (rawTarget is bool)
            {
                // This needs to be lowercased or else javascript gets sad.
                stringTarg = stringTarg.ToLower();
            }

            return stringTarg;
        }

        /// <summary>
        /// Returns a serialized value for the client-side html attributes for non-ranged values.
        /// </summary>
        /// <param name="safeValue"></param>
        /// <returns></returns>
        private string GetSerializableClientTargetValue(ThreadSafeAttributeValue safeValue)
        {
            if (safeValue.TargetValue is Array)
            {
                return GetSerializableClientTargetValueForArray(safeValue);
            }

            return GetSerializableClientTargetValueForSingleValue(safeValue);
        }

        /// <summary>
        /// Returns a serialized value for the client-side html attributes for ranged values.
        /// </summary>
        /// <param name="safeValue"></param>
        /// <returns></returns>
        private (string Min, string Max) GetSerializableClientTargetValueForRange(ThreadSafeAttributeValue safeValue)
        {
            var range = (IRange)safeValue.TargetValue;
            var min = range.GetMin().ToString();
            var max = range.GetMax().ToString();
            return (Min: min, Max: max);
        }

        private string GetSerializableClientTargetValueForArray(ThreadSafeAttributeValue safeValue)
        {
            var rawArray = ((IEnumerable)safeValue.TargetValue)
                          .Cast<object>().Select(GetRawValueForClientSide).ToList();
            var serializer = JavaScriptSerializerFactory.Build();
            return serializer.Serialize(rawArray);
        }

        #endregion

        #endregion

        #region Public Methods

        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var propName = DependentProperty.ToLower();
            dynamic controller = context.Controller;

            var rule = new ModelClientValidationRule {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredwhen" + propName
            };
            var viewContext = (ViewContext)context;
            var depProp = BuildDependentPropertyId(metadata, viewContext);
            ThreadSafeAttributeValue safeVal = GetThreadSafeValue(controller.Container);
            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("comparison", ComparisonType.ToString().ToLower());
            if (IsRangeComparisonType)
            {
                var (min, max) = GetSerializableClientTargetValueForRange(safeVal);
                rule.ValidationParameters.Add("targetvaluemin", min);
                rule.ValidationParameters.Add("targetvaluemax", max);
            }
            else
            {
                rule.ValidationParameters.Add("targetvalue", GetSerializableClientTargetValue(safeVal));
            }

            rule.ValidationParameters.Add("targettype", safeVal.DependentClientSideTypeCode);

            if (FieldOnlyVisibleWhenRequired)
            {
                rule.ValidationParameters.Add("togglevisibility", "true"); // string, otherwise it serializes as "True"
            }

            // Don't use yield return here. It'll make Ross sad for performance reasons.
            return new[] {rule};
        }

        #endregion

        #region Private classes

        // Represents a read-only view of the target value and any related values. This is
        // needed for the dynamic values so that two simultaneous requests for a similar
        // resource does not cause the wrong attribute value to be sent to the wrong user.
        protected class ThreadSafeAttributeValue
        {
            public object TargetValue { get; set; }
            public ValueTypeCode DependentTypeCode { get; set; }
            public string DependentClientSideTypeCode { get; set; }
        }

        #endregion
    }
}
