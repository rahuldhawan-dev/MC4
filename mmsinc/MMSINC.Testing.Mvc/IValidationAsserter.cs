using System;
using System.Linq.Expressions;

namespace MMSINC.Testing
{
    /// <summary>
    /// Basis for making assertions about models in unit/integration tests.
    /// </summary>
    public interface IValidationAsserter<TModel>
    {
        #region Abstract Methods
        
        #region EntityMustExist

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        [Obsolete("Use the overload without the model argument instead. You may also opt to use the overload without existingEntity, which will use a TestDataFactory to create the entity.")]
        IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            TEntity existingEntity,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            Expression<Func<TModel, TProp>> propExpression,
            TEntity existingEntity,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        IValidationAsserter<TModel> EntityMustExist<TEntity>(
            Expression<Func<TModel, int?>> propExpression,
            TEntity existingEntity,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            Expression<Func<TModel, TProp>> propExpression,
            string error = null)
            where TEntity : class, new();

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        IValidationAsserter<TModel> EntityMustExist<TEntity>(
            Expression<Func<TModel, int?>> propExpression,
            string error = null)
            where TEntity : class, new();

        /// <inheritdoc cref="ValidationAssert.EntityMustExist{T,TProp}(T,Expression{Func{T,TProp}},object,string)" />
        /// <remarks>
        /// The name of the property is assumed to match the name of the
        /// <typeparamref name="TEntity"/> class.
        /// </remarks>
        [Obsolete("This is just magic. It removes compile-time safety of the tests. This should include an expression parameter.")]
        IValidationAsserter<TModel> EntityMustExist<TEntity>(string error = null)
            where TEntity : class, new();
        
        #endregion
        
        #region ModelStateHasError

        /// <inheritdoc cref="ValidationAssert.ModelStateHasError{T,TProp}(T,Expression{Func{T,TProp}},string,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateHasError<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateHasError{T,TProp}(T,Expression{Func{T,TProp}},string,bool)" />
        IValidationAsserter<TModel> ModelStateHasError<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors = true);
        
        /// <inheritdoc cref="ValidationAssert.ModelStateHasError(object,string,string,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateHasError(
            TModel model,
            string property,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors = true);
        
        /// <inheritdoc cref="ValidationAssert.ModelStateHasError(object,string,string,bool)" />
        IValidationAsserter<TModel> ModelStateHasError(
            string property,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors = true);

        #endregion
        
        #region ModelStateHasNonPropertySpecificError

        /// <inheritdoc cref="ValidationAssert.ModelStateHasNonPropertySpecificError(object,string,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateHasNonPropertySpecificError(
            object model,
            string error,
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateHasNonPropertySpecificError(object,string,bool)" />
        IValidationAsserter<TModel> ModelStateHasNonPropertySpecificError(
            string error,
            bool callIValidatableObjectMethods = true);

        #endregion
        
        #region ModelStateIsValid

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid(object,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateIsValid(
            object model,
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid(object,bool)" />
        IValidationAsserter<TModel> ModelStateIsValid(
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid(object,string,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateIsValid(
            object model,
            string property,
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid(object,string,bool)" />
        IValidationAsserter<TModel> ModelStateIsValid(
            string property,
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid{T,TProp}(T,Expression{Func{T,TProp}},bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> ModelStateIsValid<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            bool callIValidatableObjectMethods = true);

        /// <inheritdoc cref="ValidationAssert.ModelStateIsValid{T,TProp}(T,Expression{Func{T,TProp}},bool)" />
        IValidationAsserter<TModel> ModelStateIsValid<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            bool callIValidatableObjectMethods = true);

        #endregion
        
        #region PropertyHasMaxStringLength

        /// <inheritdoc cref="ValidationAssert.PropertyHasMaxStringLength{T,TProp}(T,Expression{Func{T,TProp}},int,bool,string)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasMaxStringLength<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.PropertyHasMaxStringLength{T,TProp}(T,Expression{Func{T,TProp}},int,bool,string)" />
        IValidationAsserter<TModel> PropertyHasMaxStringLength<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.PropertyHasMaxStringLength(object,string,int,bool,string)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasMaxStringLength(
            object model,
            string property,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.PropertyHasMaxStringLength(object,string,int,bool,string)" />
        IValidationAsserter<TModel> PropertyHasMaxStringLength(
            string property,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        #endregion
        
        #region PropertyHasMinValueRequirement

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            string error = null);

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            string error = null);

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null);

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null);

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasMinValueRequirement(
            object model,
            string property,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null);

        /// <summary>
        /// Asserts that a value must have a value equal to above a given minimum. 
        /// </summary>
        IValidationAsserter<TModel> PropertyHasMinValueRequirement(
            string property,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null);

        #endregion
        
        #region PropertyHasStringLength
        
        /// <inheritdoc cref="ValidationAssert.PropertyHasStringLength{T,TProp}(T,Expression{Func{T,TProp}},int,int,bool,string)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasStringLength<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            int minLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        /// <inheritdoc cref="ValidationAssert.PropertyHasStringLength{T,TProp}(T,Expression{Func{T,TProp}},int,int,bool,string)" />
        IValidationAsserter<TModel> PropertyHasStringLength<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            int minLength,
            bool useCurrentPropertyValue = false,
            string error = null);

        #endregion
        
        #region PropertyHasRequiredRange

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified decimal
        /// range.
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified decimal
        /// range.
        /// </summary>
        IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified decimal
        /// range.
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasRequiredRange(
            object model,
            string property,
            decimal minValue,
            decimal maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified decimal
        /// range.
        /// </summary>
        IValidationAsserter<TModel> PropertyHasRequiredRange(
            string property,
            decimal minValue,
            decimal maxValue,
            string error = null);
        
        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified int
        /// range.
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            int maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified int
        /// range.
        /// </summary>
        IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            int maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified int
        /// range.
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyHasRequiredRange(
            object model,
            string property,
            int minValue,
            int maxValue,
            string error = null);

        /// <summary>
        /// Asserts that a property is required to have a value which falls within the specified int
        /// range.
        /// </summary>
        IValidationAsserter<TModel> PropertyHasRequiredRange(
            string property,
            int minValue,
            int maxValue,
            string error = null);
        
        #endregion
        
        #region PropertyIsNotRequired

        /// <summary>
        /// Asserts that a property does not throw any required field validation when its value is null.
        /// </summary>
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyIsNotRequired<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression);

        /// <summary>
        /// Asserts that a property does not throw any required field validation when its value is null.
        /// </summary>
        IValidationAsserter<TModel> PropertyIsNotRequired<TProp>(
            Expression<Func<TModel, TProp>> propExpression);
        
        #endregion
        
        #region PropertyIsRequired

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequired{T,TProp}(T,Expression{Func{T,TProp}},string,bool)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyIsRequired<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            string error = null,
            bool validationNotDoneByAttribute = false);

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequired{T,TProp}(T,Expression{Func{T,TProp}},string,bool)" />
        IValidationAsserter<TModel> PropertyIsRequired<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            string error = null,
            bool validationNotDoneByAttribute = false);
        
        #endregion
        
        #region PropertyIsRequiredWhen

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequiredWhen{T,TProp,TParentProp}(T,Expression{Func{T,TProp}},object,Expression{Func{T,TParentProp}},object,object,string)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            TModel model,
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired,
            object parentInvalidValue,
            string expectedRequiredErrorMessage = null);

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequiredWhen{T,TProp,TParentProp}(T,Expression{Func{T,TProp}},object,Expression{Func{T,TParentProp}},object,object,string)" />
        IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired,
            object parentInvalidValue,
            string expectedRequiredErrorMessage = null);

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequiredWhen{T,TProp,TParentProp}(T,Expression{Func{T,TProp}},object,Expression{Func{T,TParentProp}},object)" />
        [Obsolete("Use the overload without the model argument instead.")]
        IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            TModel model,
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired);

        /// <inheritdoc cref="ValidationAssert.PropertyIsRequiredWhen{T,TProp,TParentProp}(T,Expression{Func{T,TProp}},object,Expression{Func{T,TParentProp}},object)" />
        IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired);

        #endregion

        #endregion
    }
}
