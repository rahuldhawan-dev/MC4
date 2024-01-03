using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Exceptions;
using StructureMap;
using IUnitOfWork = MMSINC.Data.V2.IUnitOfWork;

namespace MapCallImporter.Common
{
    [AlwaysUnique]
    public abstract class ExcelRecordBase<TEntity, TViewModel, TThis>
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TThis : ExcelRecordBase<TEntity, TViewModel, TThis>
    {
        #region Constants

        private static readonly ConcurrentDictionary<(Type RecordType, string Field, string Value), int?> EntityCache = new ConcurrentDictionary<(Type recordType, string Field, string Value), int?>();
        public const string SAP_RETRY_ERROR_CODE = "RETRY::NEWLY IMPORTED RECORD";

        #endregion

        #region Private Methods

        protected virtual TViewModel MapExtra(TViewModel viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper)
        {
            return viewModel;
        }

        protected virtual int? StringToEntity<TLookup>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string value, Expression<Func<TLookup, bool>> lookupFn)
            where TLookup : IEntity, new()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var key = (typeof(TThis), fieldName, value);

            if (EntityCache.ContainsKey(key))
            {
                return EntityCache[key];
            }

            var lookup = uow.GetRepository<TLookup>().Where(lookupFn).Select(x => new TLookup {Id = x.Id}).ToList();

            if (!lookup.Any())
            {
                helper.AddFailure($"Row {index}: Unable to find {fieldName} '{value}'");
                return null;
            }

            if (lookup.Count > 1)
            {
                helper.AddFailure($"Row {index}: More than one {fieldName} found for '{value}'");
                return null;
            }

            return EntityCache[key] = lookup.Single().Id;
        }

        public virtual int? StringToEntityLookup<TLookup>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string value)
            where TLookup : IEntityLookup, new()
        {
            return StringToEntity<TLookup>(uow, index, helper, fieldName, value, tl => tl.Description == value);
        }

        protected virtual int[] StringValuesToEntities<TLookup>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string commaSeparatedValues, Expression<Func<TLookup, string>> propertyToLookup)
            where TLookup : IEntity, new()
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedValues))
            {
                return null;
            }

            return commaSeparatedValues
                  .SplitCommaSeparatedValues()
                  .Select(value => StringToEntity(uow, index, helper, fieldName, value, GenerateLookupExpression(propertyToLookup, value)) ?? 0)
                  .Where(id => id > 0)
                  .ToArray();
        }

        protected virtual int[] IntValuesToEntities<TLookup>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string commaSeparatedValues, Expression<Func<TLookup, int>> propertyToLookup)
            where TLookup : IEntity, new()
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedValues))
            {
                return null;
            }

            var repo = uow.GetRepository<TLookup>();

            var inputValues = ParseIntValues(index, helper, fieldName, commaSeparatedValues);

            var result = new List<int>();
            foreach (var value in inputValues)
            {
                var found = repo.Where(GenerateLookupExpression(propertyToLookup, value)).Select(x => x.Id);
                
                if (!found.Any())
                {
                    helper.AddFailure($"Row {index}: Unable to find {fieldName} '{value}'");
                }
                else if (found.Count() > 1)
                {
                    helper.AddFailure($"Row {index}: More than one {fieldName} found for '{value}'");
                }
                else
                {
                    result.Add(found.Single());
                }
            }

            return result.ToArray();
        }

        protected virtual int[] IdsToEntities<TLookup>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string commaSeparatedValues)
            where TLookup : IEntity, new()
        {
            return IntValuesToEntities<TLookup>(uow, index, helper, fieldName, commaSeparatedValues, x => x.Id);
        }

        private static Expression<Func<TLookup, bool>> GenerateLookupExpression<TLookup, TInput>(Expression<Func<TLookup, TInput>> propertyToLookup, TInput value)
            where TLookup : IEntity, new()
        {
            if (propertyToLookup is null || propertyToLookup.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException($"The body of {nameof(propertyToLookup)} must be an expression that consists of a single property from the {typeof(TLookup).Name} class.");
            }

            // The following code translates to the literal expression x => x.MyProperty == MyValue
            // where "x => x.MyProperty" is the propertyToLookup parameter, and "MyValue" is the current value we're iterating over
            return Expression.Lambda<Func<TLookup, bool>>(
                Expression.Equal(
                    propertyToLookup.Body,
                    Expression.Constant(value)),
                propertyToLookup.Parameters);
        }

        private static IEnumerable<int> ParseIntValues(int index, ExcelRecordItemHelperBase<TEntity> helper, string fieldName, string commaSeparatedValues)
        {
            var values = commaSeparatedValues.SplitCommaSeparatedValues();

            var result = new List<int>();
            foreach (var value in values)
            {
                if (int.TryParse(value, out var number))
                {
                    result.Add(number);
                }
                else
                {
                    helper.AddFailure($"Row {index}: Invalid data for the {fieldName} column. '{value}' is not a number.");
                }
            }

            return result;
        }

        #endregion

        #region Abstract Methods

        // use the base class so Validate can call MapToEntity and cut down some redundancy
        public abstract TEntity MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<TEntity, TViewModel, TThis> helper);

        #endregion

        protected virtual bool SkipValidationError(string errorMessage)
        {
            return errorMessage.Contains("Coordinate is required");
        }

        #region Exposed Methods

        public virtual TEntity MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, TEntity entity)
        {
            var createModel = MapToViewModel(uow, index, helper, entity);

            var state = Validate(createModel, uow);
            IEnumerable<ModelState> errors = new ModelState[0];

            // we used to only check state.IsValid (skipping ones we don't care about),
            // but if MapToEntity were called after the helper saw errors we'd potentially
            // see null reference and other weird errors rather than showing the user
            // what issues the helper saw.  thus now we check helper.LastErrors along with
            // model state from the view model validation
            if ((!helper.LastErrors?.Any() ?? true) &&
                (state.IsValid ||
                 // this filters out certain validation errors we don't care about
                 // the rules for which are defined in SkipValidationError
                !(errors = state.Values.Where(x => x.Errors.Any(e => !SkipValidationError(e.ErrorMessage)))).Any()))
            {
                return createModel.MapToEntity(entity);
            }

            foreach (var value in errors)
            {
                foreach (var error in value.Errors)
                {
                    helper.AddFailure($"Row {index}: {error.ErrorMessage}");
                }
            }

            return null;
        }

        public virtual TEntity MapToEntity(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper)
        {
            return MapToEntity(uow, index, helper, new TEntity());
        }

        public TViewModel MapToViewModel(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<TEntity> helper, TEntity entity)
        {
            var viewModel = uow.Container.GetInstance<TViewModel>();
            viewModel.Map(entity);
            uow.GetMapper(GetType(), typeof(TViewModel)).MapToSecondary(this, viewModel);
            return MapExtra(viewModel, uow, index, helper);
        }

        public virtual ModelStateDictionary Validate(TViewModel viewModel, IUnitOfWork uow)
        {
            var c = new ValidationController();
            c.TryValidateModelImpl(viewModel, true, uow.GetInstance<ISession>() as SessionWrapper);
            return c.ModelState;
        }

        /// <summary>
        /// When overriding, do NOT rely on internal state of your actual ExcelRecord instance, because it is not guaranteed to be there!
        /// </summary>
        public virtual void InsertEntity(IUnitOfWork uow, TEntity entity)
        {
            uow.Insert(entity);
        }

        /// <summary>
        /// When overriding, do NOT rely on internal state of your actual ExcelRecord instance, because it is not guaranteed to be there!
        /// </summary>
        public virtual void PreImport(TEntity[] entities) { }

        #endregion

        #region Nested Type: ValidationController

        protected class ValidationController : Controller
        {
            #region Exposed Methods

            /// <summary>
            /// 
            /// </summary>
            /// <param name="model"></param>
            /// <param name="callClassLevelValidatorsOnPropertyErrors">Set to true if class validators should be forced if the property validators fail.</param>
            public void TryValidateModelImpl(object model, bool callClassLevelValidatorsOnPropertyErrors, SessionWrapper session)
            {
                ViewData.Model = model;
                ViewData = new ViewDataDictionary(model);
                bool valid;

                try
                {
                    valid = TryValidateModel(model);
                }
                catch (GenericADOException e)
                {
                    throw new ObjectDisposedException(session?.MyDisposalMessage, e);
                }
                catch (ObjectDisposedException e)
                {
                    throw new ObjectDisposedException(session?.MyDisposalMessage, e);
                }

                // The class level validators will have been called if isValid is true,
                // so no need to call them all twice.
                if (!valid && callClassLevelValidatorsOnPropertyErrors)
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

            #endregion

            public ValidationController()
            {
                ControllerContext = new ControllerContext {Controller = this};
            }
        }

        #endregion
    }
}
