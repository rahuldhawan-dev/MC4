using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities;
using MMSINC.Validation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace MMSINC.Testing
{
    [Obsolete("Use IValidationAsserter (rather an implementation thereof) instead.")]
    public static class ValidationAssert
    {
        #region Fields

        /// <summary>
        /// Reference to the internal DataAnnotationsModelValidatorProvider.AttributeFactories field.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, DataAnnotationsModelValidationFactory>
            _previousAttributeFactories = new ConcurrentDictionary<Type, DataAnnotationsModelValidationFactory>();

        #endregion

        #region Private Methods

        private static string GetModelStatePropertyNamesThatHaveErrors(ModelStateDictionary msd)
        {
            return string.Join(", ", msd.Where(x => x.Value.Errors.Any()).Select(x => x.Key != string.Empty ? x.Key : "Empty String - ValidationResult did not include a property name."));
        }

        private static string GenerateMaxLengthString(int maxLength)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < maxLength; i++)
            {
                sb.Append("a");
            }

            return sb.ToString();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Asserts that the validated ModelState has an error for the given property.
        /// </summary>
        public static void ModelStateHasError<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            string error, bool classClassLevelValidatorsOnPropertyErrors = true)
        {
            var prop = Expressions.GetMember(propExpression);
            ModelStateHasError(model, prop.Name, error, classClassLevelValidatorsOnPropertyErrors);
        }

        /// <summary>
        /// Asserts that the validated ModelState has an error for the given property.
        /// </summary>
        /// <remarks>
        /// 
        /// BIG OL' NOTE: If you're trying to search for an error that would be added
        /// by IValidatableObject.Validate, the rest of the model's properties must already
        /// be valid for whatever validation attributes they might have. MVC does not call
        /// IValidatableObject unless all the properties are already valid in that sense.
        /// 
        /// BIG OL' NOTE 2: If you don't have a property send String.Empty, not null (duh)
        /// 
        /// So set classClassLevelValidatorsOnPropertyErrors to true to force the Validate method to be called.
        /// 
        /// </remarks>
        public static void ModelStateHasError(object model, string property, string error,
            bool classClassLevelValidatorsOnPropertyErrors = true)
        {
            var modelStateDict = ValidateModel(model, classClassLevelValidatorsOnPropertyErrors);
            if (!modelStateDict.ContainsKey(property))
            {
                Assert.Fail("Unable to find property '{0}' in ModelState. Properties with errors: {1}", property,
                    GetModelStatePropertyNamesThatHaveErrors(modelStateDict));
            }

            var modelState = modelStateDict[property];

            var results = modelState.Errors.Select(x => x.ErrorMessage).ToArray();
            if (!results.Any())
            {
                Assert.Fail("No validation errors were found for '{0}'", property);
            }

            if (!results.Contains(error))
            {
                Assert.Fail("Unable to find error '{0}'. Did find these: {1}", error, string.Join(", ", results));
            }
        }

        /// <summary>
        /// Asserts that a model has an error that isn't specific to a single property.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="error"></param>
        public static void ModelStateHasNonPropertySpecificError(object model, string error,
            bool callIValidatableObjectMethods = true)
        {
            var c = new ValidationController();
            c.TryValidateModelImpl(model, callIValidatableObjectMethods);

            var errors = c.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();

            if (!errors.Contains(error))
            {
                Assert.Fail(
                    $"No validation errors were found that matched: \"{error}\". Did find the following: {string.Join(", ", errors)} ");
            }
        }

        /// <summary>
        /// Returns true if the model is valid for all properties.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="callIValidatableObjectMethods"></param>
        public static void ModelStateIsValid(object model, bool callIValidatableObjectMethods = true)
        {
            var modelStateDict = ValidateModel(model, callIValidatableObjectMethods);

            var withErrors = modelStateDict.Where(x => x.Value.Errors.Any()).Select(x => x).ToArray();

            if (withErrors.Any())
            {
                var sb = new StringBuilder();
                sb.Append("Model was not valid. The following errors were found.")
                  .AppendLine();
                foreach (var modelState in withErrors)
                {
                    sb.AppendFormat("Errors were found on property '{0}'. {1}", modelState.Key,
                        string.Join(", ", modelState.Value.Errors.Select(x => x.ErrorMessage)));
                    sb.AppendLine();
                }

                Assert.Fail(sb.ToString());
            }
        }

        /// <summary>
        /// Asserts that a model's property is valid. 
        /// </summary>
        public static void ModelStateIsValid(object model, string property, bool callIValidatableObjectMethods = true)
        {
            var modelStateDict = ValidateModel(model, callIValidatableObjectMethods);

            // A key won't exist if it has no validation errors. 
            if (modelStateDict.ContainsKey(property))
            {
                var modelState = modelStateDict[property];
                if (modelState.Errors.Any())
                {
                    Assert.Fail("Errors were found on property '{0}'. {1}", property,
                        string.Join(", ", modelState.Errors.Select(x => x.ErrorMessage)));
                }
            }
        }

        /// <summary>
        /// Asserts that a model's property is valid. 
        /// </summary>
        public static void ModelStateIsValid<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            bool callIValidatableObjectMethods = true)
        {
            var prop = Expressions.GetMember(propExpression);
            ModelStateIsValid(model, prop.Name, callIValidatableObjectMethods);
        }

        /// <summary>
        /// Asserts that a model is completely valid, and says why if its not.
        /// </summary>
        [DebuggerStepThrough]
        [Obsolete("Use ModelStateIsValid overload that doesn't take a property.")]
        public static void WholeModelIsValid(object model)
        {
            var modelState = ValidateModel(model, true);

            if (!modelState.IsValid)
            {
                Assert.Fail("Model is not valid for the following reasons:\n{0}", string
                   .Join("\n",
                        (from x in modelState
                         from y in x.Value.Errors
                         select y.ErrorMessage).ToArray()));
            }
        }

        public static void SomethingAboutModelIsNotValid(object model)
        {
            var x = ValidateModel(model, true);
            Assert.IsFalse(x.IsValid,
                "Model is valid and if you don't know why I'm not going to tell you");
        }

        /// <summary>
        /// Takes an arbitrary object and validates it. Useful for testing that
        /// properties have the correct validation attributes on them when there's
        /// no controller being tested.
        /// </summary>
        /// <returns></returns>
        public static ModelStateDictionary ValidateModel(object model, bool classClassLevelValidatorsOnPropertyErrors)
        {
            var c = new ValidationController();
            c.TryValidateModelImpl(model, classClassLevelValidatorsOnPropertyErrors);
            return c.ModelState;
        }

        #region PropertyHasStringLength

        // TODO: This method is barely used. Should consider swapping the max/min parameters because
        // that's a little confusing.

        public static void PropertyHasStringLength<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            int maxLength, int minLength, bool useCurrentPropertyValue = false, string error = null)
        {
            PropertyHasStringLength(model, Expressions.GetMember(propExpression).Name, maxLength, minLength,
                useCurrentPropertyValue, error);
        }

        public static void PropertyHasStringLength(object model, string property, int maxLength, int minLength,
            bool useCurrentPropertyValue = false, string error = null)
        {
            var stringToUse = GenerateMaxLengthString(maxLength);
            if (useCurrentPropertyValue)
            {
                stringToUse = (string)model.GetPropertyValueByName(property);
                if (stringToUse.Length > maxLength || stringToUse.Length < minLength)
                {
                    Assert.Fail(
                        $"When useCurrentPropertyValue is true, the model property({property}) must already have a value with a length within the expected range before the validation test can run. Expected length is between {minLength} and {maxLength}, but actual length is {stringToUse?.Length}).");
                }
            }

            PropertyHasStringLength(model, property, maxLength, minLength, stringToUse, error);
        }

        private static void PropertyHasStringLength(object model, string property, int maxLength, int minLength,
            string goodLengthString, string error = null)
        {
            if (error == null)
            {
                var actualProperty = string.Empty;

                var attribute =
                    (DisplayNameAttribute)Attribute.GetCustomAttribute(model.GetType().GetProperty(property),
                        typeof(DisplayNameAttribute));

                error =
                    $"The field {(attribute == null ? property : attribute.DisplayName)} must be a string with a minimum length of {minLength} and a maximum length of {maxLength}.";
            }

            var tooLong = GenerateMaxLengthString(maxLength + 1);
            var tooShort = GenerateMaxLengthString(minLength - 1);

            model.SetPropertyValueByName(property, goodLengthString);
            ModelStateIsValid(model, property, false);

            model.SetPropertyValueByName(property, tooLong);
            ModelStateHasError(model, property, error, false);

            model.SetPropertyValueByName(property, tooShort);
            ModelStateHasError(model, property, error, false);
        }

        #endregion

        #region PropertyHasMaxStringLength

        /// <summary>
        /// Tests a model's validation by setting its property value to the max length specified and
        /// then again with a value that's max length + 1.
        /// </summary>
        public static void PropertyHasMaxStringLength<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            int maxLength, bool useCurrentPropertyValue = false, string error = null)
        {
            PropertyHasMaxStringLength(model, Expressions.GetMember(propExpression).Name, maxLength,
                useCurrentPropertyValue, error);
        }

        /// <summary>
        /// Tests a model's validation by setting its property value to the max length specified and
        /// then again with a value that's max length + 1.
        /// </summary>
        public static void PropertyHasMaxStringLength(object model, string property, int maxLength,
            bool useCurrentPropertyValue = false, string error = null)
        {
            var almostMaxLengthString = GenerateMaxLengthString(maxLength);
            if (useCurrentPropertyValue)
            {
                almostMaxLengthString = (string)model.GetPropertyValueByName(property);
                if (almostMaxLengthString.Length != maxLength)
                {
                    Assert.Fail(
                        $"When useCurrentPropertyValue is true, the model property({property}) must already have a value with a length equal to the maximum allowed length before the validation test can run. Expected length is {maxLength}, but actual length is {almostMaxLengthString?.Length}).");
                }
            }

            PropertyHasMaxStringLength(model, property, maxLength, almostMaxLengthString, error);
        }

        private static void PropertyHasMaxStringLength(object model, string property, int maxLength,
            string almostMaxLengthString, string error)
        {
            if (error == null)
            {
                var actualProperty = string.Empty;

                var attribute =
                    (DisplayNameAttribute)Attribute.GetCustomAttribute(model.GetType().GetProperty(property),
                        typeof(DisplayNameAttribute));

                error =
                    $"The field {(attribute == null ? property : attribute.DisplayName)} must be a string with a maximum length of {maxLength}.";
            }

            var maxLengthString = almostMaxLengthString + "1"; // add that additional character!

            model.SetPropertyValueByName(property, almostMaxLengthString);
            ModelStateIsValid(model, property, false);

            // Add one more character to fail
            model.SetPropertyValueByName(property, maxLengthString + "1");
            ModelStateHasError(model, property, error, false);
        }

        #endregion

        #region PropertyHasRequiredRange - decimal

        public static void PropertyHasRequiredRange<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            decimal minValue, decimal maxValue, string error = null)
        {
            PropertyHasRequiredRange(model, Expressions.GetMember(propExpression).Name, minValue, maxValue, error);
        }

        public static void PropertyHasRequiredRange(object model, string property, decimal minValue, decimal maxValue,
            string error = null)
        {
            if (error == null)
            {
                error = string.Format("The field {0} must be between {1} and {2}.", property, minValue, maxValue);
            }

            // In range
            model.SetPropertyValueByName(property, maxValue - 1m);
            ModelStateIsValid(model, property, false);

            // Max range
            model.SetPropertyValueByName(property, maxValue);
            ModelStateIsValid(model, property, false);

            // min range
            model.SetPropertyValueByName(property, minValue);
            ModelStateIsValid(model, property, false);

            // TODO: If this is failing, it may potentially be related to the Range attribute only
            // accepting double as a value. This needs to be investigated further.

            // Above the range
            model.SetPropertyValueByName(property, maxValue + 0.01m);
            ModelStateHasError(model, property, error, false);

            // below the range
            model.SetPropertyValueByName(property, minValue - 0.01m);
            ModelStateHasError(model, property, error, false);
        }

        #endregion

        #region PropertyHasRequiredRange - int

        public static void PropertyHasRequiredRange<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            int minValue, int maxValue, string error = null)
        {
            PropertyHasRequiredRange(model, Expressions.GetMember(propExpression).Name, minValue, maxValue, error);
        }

        public static void PropertyHasRequiredRange(object model, string property, int minValue, int maxValue,
            string error = null)
        {
            error = error ?? $"The field {property} must be between {minValue} and {maxValue}.";

            // In range
            model.SetPropertyValueByName(property, maxValue - 1);
            ModelStateIsValid(model, property, false);

            // Max range
            model.SetPropertyValueByName(property, maxValue);
            ModelStateIsValid(model, property, false);

            // min range
            model.SetPropertyValueByName(property, minValue);
            ModelStateIsValid(model, property, false);

            // Above the range
            model.SetPropertyValueByName(property, maxValue + 1);
            ModelStateHasError(model, property, error, false);

            // below the range
            model.SetPropertyValueByName(property, minValue - 1);
            ModelStateHasError(model, property, error, false);
        }

        #endregion

        #region PropertyIsRequired

        private static void PropertyIsRequiredForReal(object model, string property, string displayName,
            string error = null)
        {
            if (error == null)
            {
                // MVC's RequiredAttribute uses the DisplayNameAttribute value instead of the property name
                // if one exists.
                error = string.Format("The {0} field is required.", displayName ?? property);
            }

            model.SetPropertyValueByName(property, null);
            ModelStateHasError(model, property, error, false);
        }

        /// <summary>
        /// Asserts that a property throws the expected required field validation when its value is null. The property
        /// must have a RequiredAttribute, unless the validationNotDoneByAttribute parameter is set to true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="model"></param>
        /// <param name="propExpression"></param>
        /// <param name="error"></param>
        /// <param name="validationNotDoneByAttribute">Set this to true to bypass the attribute check. This should only be done when validation
        /// is being done through IValidatableObject callbacks or the rare occasion that PropertyIsRequiredWhen can not be used.</param>
        public static void PropertyIsRequired<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            string error = null, bool validationNotDoneByAttribute = false)
        {
            var prop = Expressions.GetMember(propExpression);
            var dispNameAttr = prop.GetCustomAttributes<DisplayNameAttribute>(true).SingleOrDefault();
            var dispName = (dispNameAttr != null ? dispNameAttr.DisplayName : prop.Name);

            if (!validationNotDoneByAttribute && prop.HasAttribute(typeof(RequiredAttribute)))
            {
                Assert.IsTrue(prop.HasAttribute(typeof(RequiredAttribute)),
                    $"{prop.Name} does not have a Required attribute");
            }
            else
            {
                Assert.IsFalse(prop.HasAttribute(typeof(RequiredAttribute)),
                    $"{prop.Name} does have a Required attribute");
            }

            PropertyIsRequiredForReal(model, prop.Name, dispName, error);
        }

        public static void PropertyIsNotRequired<T, TProp>(T model, Expression<Func<T, TProp>> propExpression)
        {
            var prop = Expressions.GetMember(propExpression);
            model.SetPropertyValueByName(prop.Name, null);

            ModelStateIsValid(model, propExpression);
        }

        public static void PropertyWithValueIsNotRequired<T, TProp>(T model, Expression<Func<T, TProp>> propExpression)
        {
            ModelStateIsValid(model, propExpression);
        }

        public static void PropertyIsRequired(object model, string property, string error = null)
        {
            PropertyIsRequiredForReal(model, property, null, error);
        }

        #endregion

        #region PropertyIsRequiredWhen

        /// <summary>
        /// Helper for testing RequiredWhen attributes when you need to set an invalid parent value
        /// </summary>
        /// <param name="model">ViewModel to be tested</param>
        /// <param name="childExpression">Child Property to check. E.g. x => x.ChildProperty</param>
        /// <param name="validValueWhenChildIsRequired">A value value to set on the child property when the child property is required.</param>
        /// <param name="parentExpression">Parent Property E.g. x => x.ParentProperty</param>
        /// <param name="parentValueThatMakesChildPropertyRequired">A valid value to set the parent to for the test. This value should make the child property required.</param>
        /// <param name="parentInvalidValue">value for the parent that would make the child not required</param>
        /// <param name="expectedRequiredErrorMessage">The expected error message that should be displayed when the child property is required.</param>
        public static void PropertyIsRequiredWhen<T, TProp, TParentProp>(T model,
            Expression<Func<T, TProp>> childExpression, object validValueWhenChildIsRequired,
            Expression<Func<T, TParentProp>> parentExpression, object parentValueThatMakesChildPropertyRequired,
            object parentInvalidValue, string expectedRequiredErrorMessage = null)
        {
            var childProp = Expressions.GetMember(childExpression);
            var parentProp = Expressions.GetMember(parentExpression);
            // Make sure the parent property is null before we start
            model.SetPropertyValueByName(parentProp.Name, parentInvalidValue);

            // Parent does not have child requirement, so child is not required and validation passes
            PropertyIsNotRequired(model, childExpression);

            // Parent has child required, child does not have required value, so child is required and validation fails
            model.SetPropertyValueByName(parentProp.Name, parentValueThatMakesChildPropertyRequired);
            PropertyIsRequired(model, childExpression, expectedRequiredErrorMessage,
                validationNotDoneByAttribute: true);

            // Parent has child required, child does have required value, validation passes
            model.SetPropertyValueByName(childProp.Name, validValueWhenChildIsRequired);
            ModelStateIsValid(model, childExpression);
        }

        /// <summary>
        /// Helper for testing RequiredWhen attributes
        /// </summary>
        /// <param name="model">ViewModel to be tested</param>
        /// <param name="childExpression">Child Property to check. E.g. x => x.ChildProperty</param>
        /// <param name="validValueWhenChildIsRequired">A value value to set on the child property when the child property is required.</param>
        /// <param name="parentExpression">Parent Property E.g. x => x.ParentProperty</param>
        /// <param name="parentValueThatMakesChildPropertyRequired">A valid value to set the parent to for the test</param>
        public static void PropertyIsRequiredWhen<T, TProp, TParentProp>(T model,
            Expression<Func<T, TProp>> childExpression, object validValueWhenChildIsRequired,
            Expression<Func<T, TParentProp>> parentExpression, object parentValueThatMakesChildPropertyRequired)
        {
            var prop = Expressions.GetMember(childExpression);
            Assert.IsTrue(prop.HasAttribute<RequiredWhenAttribute>(),
                $"{prop.Name} does not have a RequiredWhen attribute");
            PropertyIsRequiredWhen(model, childExpression, validValueWhenChildIsRequired, parentExpression,
                parentValueThatMakesChildPropertyRequired, null);
        }

        #endregion

        #region PropertyMustBeEmailAddress

        /// <summary>
        /// Tests that a property must be an email address.
        /// </summary>
        public static void PropertyMustBeEmailAddress<T, TProp>(T model, Expression<Func<T, TProp>> propExpression)
        {
            PropertyMustBeEmailAddress(model, Expressions.GetMember(propExpression).Name);
        }

        /// <summary>
        /// Tests that a property must be an email address.
        /// </summary>
        public static void PropertyMustBeEmailAddress(object model, string property)
        {
            model.SetPropertyValueByName(property, "notanemail");
            ModelStateHasError(model, property, "Invalid email address.");
            model.SetPropertyValueByName(property, "notanemail@");
            ModelStateHasError(model, property, "Invalid email address.");
            model.SetPropertyValueByName(property, "notanemail@.com");
            ModelStateHasError(model, property, "Invalid email address.");

            model.SetPropertyValueByName(property, "is@an.email");
            ModelStateIsValid(model, property);
        }

        #endregion

        #region PropertyMustBeUrl

        /// <summary>
        /// Tests that a property must be a url.
        /// </summary>
        public static void PropertyMustBeUrl<T, TProp>(T model, Expression<Func<T, TProp>> propExpression)
        {
            PropertyMustBeUrl(model, Expressions.GetMember(propExpression).Name);
        }

        /// <summary>
        /// Tests that a property must be a url.
        /// </summary>
        public static void PropertyMustBeUrl(object model, string property)
        {
            var expectedError = $"The {property} field is not a valid fully-qualified http, https, or ftp URL.";

            model.SetPropertyValueByName(property, "not a url");
            ModelStateHasError(model, property, expectedError);
            model.SetPropertyValueByName(property, "http");
            ModelStateHasError(model, property, expectedError);
            model.SetPropertyValueByName(property, "http://www.");
            ModelStateHasError(model, property, expectedError);

            model.SetPropertyValueByName(property, "http://www.google.com?thing=value%20thing&wow=neat");
            ModelStateIsValid(model, property);
        }

        #endregion

        #region PropertyHasMinValueRequirement

        public static void PropertyHasMinValueRequirement<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            int minValue, string error = null)
        {
            PropertyHasMinValueRequirement(model, Expressions.GetMember(propExpression).Name, minValue, error);
        }

        public static void PropertyHasMinValueRequirement(object model, string property, int minValue,
            string error = null)
        {
            PropertyHasMinValueRequirementImpl(model, property, minValue, minValue - 1, error);
        }

        /// <summary>
        /// Asserts that a property has a minimum requirement value.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propExpression"></param>
        /// <param name="minValue"></param>
        /// <param name="valueLessThanMin">By default, this value is minValue + 0.01. If this needs to be something more precise, set this parameter.</param>
        /// <param name="error"></param>
        public static void PropertyHasMinValueRequirement<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            decimal minValue, decimal? valueLessThanMin = null, string error = null)
        {
            PropertyHasMinValueRequirement(model, Expressions.GetMember(propExpression).Name, minValue,
                valueLessThanMin, error);
        }

        /// <summary>
        /// Asserts that a property has a minimum requirement value.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="property"></param>
        /// <param name="minValue"></param>
        /// <param name="valueLessThanMin">By default, this value is minValue + 0.01. If this needs to be something more precise, set this parameter.</param>
        /// <param name="error"></param>
        public static void PropertyHasMinValueRequirement(object model, string property, decimal minValue,
            decimal? valueLessThanMin = null, string error = null)
        {
            // 0.1m was chosen to satisfy the typical requirements for decimal values.
            // but it could be made smaller if necessary.
            valueLessThanMin = valueLessThanMin ?? minValue - 0.01m;
            PropertyHasMinValueRequirementImpl(model, property, minValue, valueLessThanMin, error);
        }

        private static void PropertyHasMinValueRequirementImpl(object model, string property, object minValue,
            object valueLessThanMin, string error)
        {
            if (error == null)
            {
                // The annotations that generates this error message forgot to put a period at the end. :(
                error = string.Format("The field {0} must be greater than or equal to {1}", property, minValue);
            }

            model.SetPropertyValueByName(property, valueLessThanMin);
            ModelStateHasError(model, property, error, false);

            model.SetPropertyValueByName(property, minValue);
            ModelStateIsValid(model, property, false);
        }

        #endregion

        [Obsolete("Do not use this. Use PropertyHasRequiredRange.")]
        public static void PropertyHasRangeValueRequirement(object model, string property, Action setter, int minValue,
            int maxValue, string error = null)
        {
            if (error == null)
            {
                error = string.Format("The field {0} must be between {1} and {2}.", property, minValue, maxValue);
            }

            setter();
            ModelStateHasError(model, property, error, false);
        }

        #region EntityMustExist

        /// <summary>
        /// Asserts that a property value must match to an existing entity.
        /// </summary>
        /// <param name="existingEntity">An existing entity that can be used for getting a valid id value from.</param>
        public static void EntityMustExist<T, TProp>(T model, Expression<Func<T, TProp>> propExpression,
            object existingEntity, string error = null)
        {
            var prop = Expressions.GetMember(propExpression);
            var dispAttr =
                prop.GetCustomAttributes<DisplayNameAttribute>()
                    .FirstOrDefault(); // TODO: This should probably work with ViewAttribute too.
            var dispName = (dispAttr != null ? dispAttr.DisplayName : prop.Name);
            EntityMustExist(model, prop.Name, existingEntity, dispName, error);
        }

        /// <summary>
        /// Asserts that a property value must match to an existing entity.
        /// </summary>
        /// <param name="existingEntity">An existing entity that can be used for getting a valid id value from.</param>
        public static void EntityMustExist(object model, string property, object existingEntity, string displayName,
            string error = null)
        {
            if (error == null)
            {
                error = string.Format(EntityMustExistAttribute.DEFAULT_ERROR_MESSAGE_FORMAT, displayName);
            }

            model.SetPropertyValueByName(property, -1);
            ModelStateHasError(model, property, error,
                false); // No need to call the IValidatableObject methods because this validation is always at attribute level.

            var validId = existingEntity.GetPropertyValueByName("Id");
            model.SetPropertyValueByName(property, validId);

            ModelStateIsValid(model, property,
                false); // No need to call the IValidatableObject methods because this validation is always at attribute level.
        }

        #endregion

        private static void OverrideFactory(Type attributeType)
        {
            var factories = GetAttributeFactories();
            if (factories.ContainsKey(attributeType))
            {
                _previousAttributeFactories[attributeType] = factories[attributeType];
            }
        }

        /// <summary>
        /// Register a data annotations validator adapter. Use this if validation requires custom adapters in order to work.
        /// </summary>
        /// <typeparam name="TValidatorAttribute"></typeparam>
        /// <typeparam name="TAdapter"></typeparam>
        public static void RegisterValidatorAdapter<TValidatorAttribute, TAdapter>()
        {
            var vType = typeof(TValidatorAttribute);
            OverrideFactory(vType);
            DataAnnotationsModelValidatorProvider.RegisterAdapter(vType, typeof(TAdapter));
        }

        public static void RegisterValidatorAdapterWithDependencyInjection<TValidatorAttribute, TAttributeAdapter>(
            global::StructureMap.IContainer container)
            where TValidatorAttribute : ValidationAttribute
            where TAttributeAdapter : DataAnnotationsModelValidator<TValidatorAttribute>
        {
            var vType = typeof(TValidatorAttribute);
            OverrideFactory(vType);
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(vType,
                (metadata, context, attribute) =>
                    container.With(metadata).With(context).With(attribute as TValidatorAttribute)
                             .GetInstance<TAttributeAdapter>());
        }

        /// <summary>
        /// Removes a registered validator adapter and replaces it with the previous one if it existed.
        /// </summary>
        /// <typeparam name="TValidatorAttribute"></typeparam>
        public static void UnregisterValidatorAdapter<TValidatorAttribute>()
        {
            var vType = typeof(TValidatorAttribute);
            var factories = GetAttributeFactories();
            factories.Remove(vType);
            if (_previousAttributeFactories.ContainsKey(vType))
            {
                factories[vType] = _previousAttributeFactories[vType];
                DataAnnotationsModelValidationFactory throwAway;
                _previousAttributeFactories.TryRemove(vType, out throwAway);
            }
        }

        private static Dictionary<Type, DataAnnotationsModelValidationFactory> GetAttributeFactories()
        {
            var t = typeof(DataAnnotationsModelValidatorProvider);
            var field = t.GetField("AttributeFactories",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return (Dictionary<Type, DataAnnotationsModelValidationFactory>)field.GetValue(null);
        }

        #endregion

        #region Helper controller

        private sealed class ValidationController : Controller
        {
            public ValidationController()
            {
                ControllerContext = new ControllerContext {Controller = this};
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="model"></param>
            /// <param name="callClassLevelValidatorsOnPropertyErrors">Set to true if class validators should be forced if the property validators fail.</param>
            public void TryValidateModelImpl(object model, bool callClassLevelValidatorsOnPropertyErrors)
            {
                ViewData.Model = model;
                ViewData = new ViewDataDictionary(model);

                // Use TryValidateModel since regular ValidateModel
                // will throw an exception if the model is invalid.
                var isValid = TryValidateModel(model);

                // The class level validators will have been called if isValid is true,
                // so no need to call them all twice.
                if (!isValid && callClassLevelValidatorsOnPropertyErrors)
                {
                    var classLevelValidators = ViewData.ModelMetadata.GetValidators(ControllerContext);

                    foreach (var validator in classLevelValidators)
                    {
                        foreach (var result in validator.Validate(model))
                        {
                            // If the initial TryValidateModel call failed validation during a class level validator,
                            // then running the class level validators will end up with duplicate validation errors.
                            // We want to be sure we're not adding duplicates, otherwise it makes the test results confusing.
                            if (!ModelState.ContainsKey(result.MemberName) || !ModelState[result.MemberName].Errors.Any(x => x.ErrorMessage == result.Message))
                            {
                                ModelState.AddModelError(result.MemberName, result.Message);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
