using System;
using System.Linq.Expressions;
using MMSINC.Testing.NHibernate;
// disable obsolete warnings, we know we're calling an obsolete class here.
#pragma warning disable CS0618

namespace MMSINC.Testing
{
    /// <inheritdoc />
    /// <remarks>
    /// This implementation is essentially a wrapper for the static <see cref="ValidationAssert"/> class.
    /// Having an instance rather than a static class implementation allows for internal state tracking
    /// and supplying arguments to the constructor rather than having to supply them for each method call.
    /// Eventually this should be replaced with a class which implements the
    /// <see cref="IValidationAsserter{TModel}"/> interface on its own.
    /// </remarks>
    public class ValidationAsserter<TModel> : IValidationAsserter<TModel>
    {
        #region Private Members

        private readonly TModel _model;
        private readonly ITestDataFactoryService _testDataFactoryService;

        #endregion

        #region Constructors

        public ValidationAsserter(TModel model, ITestDataFactoryService testDataFactoryService)
        {
            _model = model;
            _testDataFactoryService = testDataFactoryService;
        }

        #endregion
        
        #region Private Members

        private TEntity CreateEntity<TEntity>()
            where TEntity : class, new()
        {
            return _testDataFactoryService.GetEntityFactory<TEntity>().Create();
        }
        
        #endregion

        #region Exposed Methods
        
        #region EntityMustExist

        public IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            TEntity existingEntity,
            string error = null)
        {
            ValidationAssert.EntityMustExist(model, propExpression, existingEntity, error);
            return this;
        }

        public IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            Expression<Func<TModel, TProp>> propExpression,
            TEntity existingEntity,
            string error = null)
        {
            return EntityMustExist(_model, propExpression, existingEntity, error);
        }

        public IValidationAsserter<TModel> EntityMustExist<TEntity>(
            Expression<Func<TModel, int?>> propExpression,
            TEntity existingEntity,
            string error = null)
        {
            return EntityMustExist(_model, propExpression, existingEntity, error);
        }

        public IValidationAsserter<TModel> EntityMustExist<TProp, TEntity>(
            Expression<Func<TModel, TProp>> propExpression,
            string error = null)
            where TEntity : class, new()
        {
            return EntityMustExist(_model, propExpression, CreateEntity<TEntity>(), error);
        }

        public IValidationAsserter<TModel> EntityMustExist<TEntity>(
            Expression<Func<TModel, int?>> propExpression,
            string error = null)
            where TEntity : class, new()
        {
            return EntityMustExist(_model, propExpression, CreateEntity<TEntity>(), error);
        }
        
        public IValidationAsserter<TModel> EntityMustExist<TEntity>(string error = null)
            where TEntity : class, new()
        {
            var entityTypeName = typeof(TEntity).Name;
            ValidationAssert.EntityMustExist(
                _model,
                entityTypeName,
                CreateEntity<TEntity>(),
                entityTypeName,
                error);

            return this;
        }

        #endregion
        
        #region ModelStateHasError

        public IValidationAsserter<TModel> ModelStateHasError<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors)
        {
            ValidationAssert.ModelStateHasError(
                model,
                propExpression,
                error,
                classClassLevelValidatorsOnPropertyErrors);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateHasError<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors)
        {
            return ModelStateHasError(
                _model,
                propExpression,
                error,
                classClassLevelValidatorsOnPropertyErrors);
        }

        public IValidationAsserter<TModel> ModelStateHasError(
            TModel model,
            string property,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors)
        {
            ValidationAssert.ModelStateHasError(
                model,
                property,
                error,
                classClassLevelValidatorsOnPropertyErrors);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateHasError(
            string property,
            string error,
            bool classClassLevelValidatorsOnPropertyErrors)
        {
            return ModelStateHasError(
                _model,
                property,
                error,
                classClassLevelValidatorsOnPropertyErrors);
        }
        
        #endregion
        
        #region ModelStateHasNonPropertySpecificError

        public IValidationAsserter<TModel> ModelStateHasNonPropertySpecificError(
            object model,
            string error,
            bool callIValidatableObjectMethods = true)
        {
            ValidationAssert.ModelStateHasNonPropertySpecificError(
                model,
                error,
                callIValidatableObjectMethods);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateHasNonPropertySpecificError(
            string error,
            bool callIValidatableObjectMethods = true)
        {
            return ModelStateHasNonPropertySpecificError(
                _model,
                error,
                callIValidatableObjectMethods);
        }

        #endregion
        
        #region ModelStateIsValid

        public IValidationAsserter<TModel> ModelStateIsValid(
            object model,
            bool callIValidatableObjectMethods = true)
        {
            ValidationAssert.ModelStateIsValid(model, callIValidatableObjectMethods);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateIsValid(bool callIValidatableObjectMethods = true)
        {
            return ModelStateIsValid(_model, callIValidatableObjectMethods);
        }

        public IValidationAsserter<TModel> ModelStateIsValid(
            object model,
            string property,
            bool callIValidatableObjectMethods = true)
        {
            ValidationAssert.ModelStateIsValid(model, property, callIValidatableObjectMethods);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateIsValid(
            string property,
            bool callIValidatableObjectMethods = true)
        {
            return ModelStateIsValid(_model, property, callIValidatableObjectMethods);
        }

        public IValidationAsserter<TModel> ModelStateIsValid<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            bool callIValidatableObjectMethods = true)
        {
            ValidationAssert.ModelStateIsValid(model, propExpression, callIValidatableObjectMethods);
            return this;
        }

        public IValidationAsserter<TModel> ModelStateIsValid<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            bool callIValidatableObjectMethods = true)
        {
            return ModelStateIsValid(_model, propExpression, callIValidatableObjectMethods);
        }

        #endregion
        
        #region PropertyHasMaxStringLength

        public IValidationAsserter<TModel> PropertyHasMaxStringLength<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null)
        {
            ValidationAssert.PropertyHasMaxStringLength(
                model,
                propExpression,
                maxLength,
                useCurrentPropertyValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasMaxStringLength<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null)
        {
            return PropertyHasMaxStringLength(
                _model,
                propExpression,
                maxLength,
                useCurrentPropertyValue,
                error);
        }

        public IValidationAsserter<TModel> PropertyHasMaxStringLength(
            object model,
            string property,
            int maxLength,
            bool useCurrentPropertyValue = false, 
            string error = null)
        {
            ValidationAssert.PropertyHasMaxStringLength(
                model,
                property,
                maxLength,
                useCurrentPropertyValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasMaxStringLength(
            string property,
            int maxLength,
            bool useCurrentPropertyValue = false,
            string error = null)
        {
            return PropertyHasMaxStringLength(
                _model,
                property,
                maxLength,
                useCurrentPropertyValue,
                error);
        }
        
        #endregion
        
        #region PropertyHasMinValueRequirement

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression, 
            int minValue,
            string error = null)
        {
            ValidationAssert.PropertyHasMinValueRequirement(
                model,
                propExpression,
                minValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            Expression<Func<TModel, TProp>> propExpression, 
            int minValue,
            string error = null)
        {
            return PropertyHasMinValueRequirement(
                _model,
                propExpression,
                minValue,
                error);
        }

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null)
        {
            ValidationAssert.PropertyHasMinValueRequirement(
                model,
                propExpression,
                minValue,
                valueLessThanMin,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null)
        {
            return PropertyHasMinValueRequirement(
                _model,
                propExpression,
                minValue,
                valueLessThanMin,
                error);
        }

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement(
            object model,
            string property,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null)
        {
            ValidationAssert.PropertyHasMinValueRequirement(
                model,
                property,
                minValue,
                valueLessThanMin,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasMinValueRequirement(
            string property,
            decimal minValue,
            decimal? valueLessThanMin = null,
            string error = null)
        {
            return PropertyHasMinValueRequirement(
                _model,
                property,
                minValue,
                valueLessThanMin,
                error);
        }

        #endregion
        
        #region PropertyHasStringLength

        public IValidationAsserter<TModel> PropertyHasStringLength<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            int minLength,
            bool useCurrentPropertyValue = false,
            string error = null)
        {
            ValidationAssert.PropertyHasStringLength(
                model,
                propExpression,
                maxLength,
                minLength,
                useCurrentPropertyValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasStringLength<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int maxLength,
            int minLength,
            bool useCurrentPropertyValue = false,
            string error = null)
        {
            return PropertyHasStringLength(
                _model,
                propExpression,
                maxLength,
                minLength,
                useCurrentPropertyValue,
                error);
        }
        
        #endregion
        
        #region PropertyHasRequiredRange

        public IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal maxValue,
            string error = null)
        {
            ValidationAssert.PropertyHasRequiredRange(
                model,
                propExpression,
                minValue,
                maxValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            decimal minValue,
            decimal maxValue,
            string error = null)
        {
            return PropertyHasRequiredRange(
                _model,
                propExpression,
                minValue,
                maxValue,
                error);
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange(
            object model,
            string property,
            decimal minValue,
            decimal maxValue,
            string error = null)
        {
            ValidationAssert.PropertyHasRequiredRange(
                model,
                property,
                minValue,
                maxValue);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange(
            string property,
            decimal minValue,
            decimal maxValue,
            string error = null)
        {
            return PropertyHasRequiredRange(
                _model,
                property,
                minValue,
                maxValue);
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            int maxValue,
            string error = null)
        {
            ValidationAssert.PropertyHasRequiredRange(
                model,
                propExpression,
                minValue,
                maxValue,
                error);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            int minValue,
            int maxValue,
            string error = null)
        {
            return PropertyHasRequiredRange(
                _model,
                propExpression,
                minValue,
                maxValue,
                error);
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange(
            object model,
            string property,
            int minValue,
            int maxValue,
            string error = null)
        {
            ValidationAssert.PropertyHasRequiredRange(
                model,
                property,
                minValue,
                maxValue);
            return this;
        }

        public IValidationAsserter<TModel> PropertyHasRequiredRange(
            string property,
            int minValue,
            int maxValue,
            string error = null)
        {
            return PropertyHasRequiredRange(
                _model,
                property,
                minValue,
                maxValue);
        }
        
        #endregion
        
        #region PropertyIsNotRequired

        public IValidationAsserter<TModel> PropertyIsNotRequired<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression)
        {
            ValidationAssert.PropertyIsNotRequired(model, propExpression);
            return this;
        }

        public IValidationAsserter<TModel> PropertyIsNotRequired<TProp>(
            Expression<Func<TModel, TProp>> propExpression)
        {
            return PropertyIsNotRequired(_model, propExpression);
        }

        #endregion
        
        #region PropertyIsRequired

        public IValidationAsserter<TModel> PropertyIsRequired<TProp>(
            TModel model,
            Expression<Func<TModel, TProp>> propExpression,
            string error = null,
            bool validationNotDoneByAttribute = false)
        {
            ValidationAssert.PropertyIsRequired(model, propExpression, error, validationNotDoneByAttribute);
            return this;
        }

        public IValidationAsserter<TModel> PropertyIsRequired<TProp>(
            Expression<Func<TModel, TProp>> propExpression,
            string error = null,
            bool validationNotDoneByAttribute = false)
        {
            return PropertyIsRequired(_model, propExpression, error, validationNotDoneByAttribute);
        }
        
        #endregion
        
        #region PropertyIsRequiredWhen

        public IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            TModel model,
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired,
            object parentInvalidValue,
            string expectedRequiredErrorMessage = null)
        {
            ValidationAssert.PropertyIsRequiredWhen(
                model,
                childExpression,
                validValueWhenChildIsRequired,
                parentExpression,
                parentValueThatMakesChildPropertyRequired,
                parentInvalidValue,
                expectedRequiredErrorMessage);
            return this;
        }

        public IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired,
            object parentInvalidValue,
            string expectedRequiredErrorMessage = null)
        {
            return PropertyIsRequiredWhen(
                _model,
                childExpression,
                validValueWhenChildIsRequired,
                parentExpression,
                parentValueThatMakesChildPropertyRequired,
                parentInvalidValue,
                expectedRequiredErrorMessage);
        }

        public IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            TModel model,
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired)
        {
            ValidationAssert.PropertyIsRequiredWhen(
                model,
                childExpression,
                validValueWhenChildIsRequired,
                parentExpression,
                parentValueThatMakesChildPropertyRequired);
            return this;
        }

        public IValidationAsserter<TModel> PropertyIsRequiredWhen<TProp, TParentProp>(
            Expression<Func<TModel, TProp>> childExpression,
            object validValueWhenChildIsRequired,
            Expression<Func<TModel, TParentProp>> parentExpression,
            object parentValueThatMakesChildPropertyRequired)
        {
            return PropertyIsRequiredWhen(
                _model,
                childExpression,
                validValueWhenChildIsRequired,
                parentExpression,
                parentValueThatMakesChildPropertyRequired);
        }

        #endregion

        #endregion
    }
}
