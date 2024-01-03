using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;

namespace MMSINC.Testing
{
    /// <summary>
    /// A helper class for performing common tests in view models. Like testing mapping.
    /// </summary>
    public class ViewModelTester<TViewModel, TEntity>
        where TViewModel : ViewModel<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly ITestDataFactoryService _testDataFactoryService;

        #endregion

        #region Properties

        public TViewModel ViewModel { get; private set; }
        public TEntity Entity { get; private set; }

        #endregion

        #region Constructor

        // Nullable parameter should be required when MC-2520 is started.
        public ViewModelTester(TViewModel viewModel, TEntity entity,
            ITestDataFactoryService testDataFactoryService = null)
        {
            viewModel.ThrowIfNull("viewModel");
            entity.ThrowIfNull("entity");
            if (ReferenceEquals(viewModel, entity))
            {
                throw new Exception(
                    "You really wanna test an object against itself? ViewModel and Entity must be different objects");
            }

            ViewModel = viewModel;
            Entity = entity;
            _testDataFactoryService = testDataFactoryService;
        }

        #endregion

        #region Private Methods

        private static void SetValue(PropertyInfo propInfo, object objectToSetOn, object value)
        {
            var setMeth = propInfo.GetSetMethod();
            if (setMeth != null)
            {
                setMeth.Invoke(objectToSetOn, new[] {value});
            }
            else
            {
                objectToSetOn.SetPropertyValueByName(propInfo.Name, value);
            }
        }

        private static object GetValue(PropertyInfo propInfo, object objectToGetFrom)
        {
            var getMeth = propInfo.GetGetMethod();
            if (getMeth != null)
            {
                return getMeth.Invoke(objectToGetFrom, new object[] { });
            }

            return objectToGetFrom.GetPropertyValueByName(propInfo.Name);
        }

        private object MapAndGetValue(PropertyInfo setter, object objectToSetOn, PropertyInfo getter,
            object objectToGetFrom, object expectedValue, Action mapperAction)
        {
            SetValue(setter, objectToSetOn, expectedValue);
            mapperAction();
            return GetValue(getter, objectToGetFrom);
        }

        private void CanMap(PropertyInfo setter, object objectToSetOn, PropertyInfo getter, object objectToGetFrom,
            object expectedValue, Action mapperAction, string message)
        {
            if (setter == null)
            {
                Assert.Fail("Property setter is null, so it can't map.");
            }

            if (getter == null)
            {
                Assert.Fail("Property getter is null, so it can't map. " + setter.Name);
            }

            var currentGetterValue = GetValue(getter, objectToGetFrom);
            Assert.AreNotEqual(currentGetterValue, expectedValue,
                $"Test for {setter.Name} can not continue when the current values and the expected value are equal.");
            var result = MapAndGetValue(setter, objectToSetOn, getter, objectToGetFrom, expectedValue, mapperAction);
            Assert.AreEqual(expectedValue, result, message ?? $"Could not map {setter.Name}");
        }

        private void DoesNotMap(PropertyInfo setter, object objectToSetOn, PropertyInfo getter, object objectToGetFrom,
            object expectedValue, Action mapperAction, string message)
        {
            var previousGetterValue = GetValue(getter, objectToGetFrom);
            Assert.AreNotEqual(previousGetterValue, expectedValue,
                "Test can not continue when the current values and the expected value are equal.");
            var result = MapAndGetValue(setter, objectToSetOn, getter, objectToGetFrom, expectedValue, mapperAction);
            Assert.AreEqual(previousGetterValue, result);
        }

        private static PropertyInfo FindMatchingViewModelGetter(PropertyInfo entitySetter)
        {
            var viewModelType = typeof(TViewModel); 

            if (viewModelType.TryGetPublicGetter(entitySetter.Name, out var getter))
            {
                return getter;
            }

            // see if TViewModel has a property which has an AutoMap attribute that can map back to the
            // entity property by name
            return viewModelType
                  .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                  .SingleOrDefault(p =>
                       p.GetCustomAttribute<AutoMapAttribute>(true)?.SecondaryPropertyName ==
                       entitySetter.Name);
        }

        #endregion

        #region Public Methods

        public TEntity MapToEntity()
        {
            return ViewModel.MapToEntity(Entity);
        }

        public void MapToViewModel()
        {
            ViewModel.Map(Entity);
        }

        #region CanMapBothWays

        /// <summary>
        /// Asserts that a property can be mapped to both the view model and entity with the same property name.
        /// Make sure you use different properties.
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> CanMapBothWays<TProp>(
            Expression<Func<TEntity, TProp>> entitySetter,
            TProp expectedViewModelValue,
            TProp expectedEntityValue,
            string message = null)
        {
            if (expectedViewModelValue.Equals(expectedEntityValue))
                throw new Exception("Using the same value for this helper method will give false positives.");
            var setter = (PropertyInfo)Expressions.GetMember(entitySetter);
            var getter = FindMatchingViewModelGetter(setter);
            CanMapToViewModel(setter, expectedViewModelValue, getter, message);
            CanMapToEntity(getter, expectedEntityValue, setter, message);

            return this;
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, string>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, "X", "Y", message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, int>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1, 2, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, int?>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1, 2, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, long>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 9999999999, 9999999990, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, long?>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 9999999999, 9999999990, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, decimal>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1m, 2m, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, decimal?>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1m, 2m, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, bool>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, true, false, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, bool?>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, true, false, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, DateTime>> entitySetter, string message = null)
        {
            // This is potentially flukey when using an entity or viewmodel that had its date
            // set to DateTime.Now through other means, like when it's created by a factory that
            // sets the value to DateTime.Now. 
            return CanMapBothWays(entitySetter, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, DateTime?>> entitySetter, string message = null)
        {
            // This is potentially flukey when using an entity or viewmodel that had its date
            // set to DateTime.Now through other means, like when it's created by a factory that
            // sets the value to DateTime.Now. 
            var now = DateTime.Now.AddDays(1);
            var later = now.AddDays(1);
            return CanMapBothWays(entitySetter, now, later, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, float>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1f, 2f, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays(Expression<Func<TEntity, float?>> entitySetter, string message = null)
        {
            return CanMapBothWays(entitySetter, 1f, 2f, message);
        }

        public ViewModelTester<TViewModel, TEntity> CanMapBothWays<TOtherEntity>(Expression<Func<TEntity, TOtherEntity>> entitySetter,
            TOtherEntity valueToUse)
            where TOtherEntity : IEntity
        {
            var entityProp = (PropertyInfo)Expressions.GetMember(entitySetter);
            var modelProp = FindMatchingViewModelGetter(entityProp);

            entityProp.SetValue(Entity, valueToUse);

            MapToViewModel();

            Assert.AreEqual(valueToUse.Id, modelProp.GetValue(ViewModel),
                $"Failed to map property {entityProp.Name} to view model");

            entityProp.SetValue(Entity, null);

            MapToEntity();

            Assert.AreSame(valueToUse, entityProp.GetValue(Entity),
                $"Failed to map property {entityProp.Name} to entity. {valueToUse} is not the same as {entityProp.GetValue(Entity) ?? "null"}");

            return this;
        }

        /// <summary>
        /// Asserts that an entity property that references another entity can map to the view model property that's 
        /// usually an int. Asserts that the reverse can also be done.
        /// </summary>
        /// <typeparam name="TOtherEntity"></typeparam>
        public ViewModelTester<TViewModel, TEntity> CanMapBothWays<TOtherEntity>(Expression<Func<TEntity, TOtherEntity>> entitySetter)
            where TOtherEntity : class, IEntity, new()
        {
            Assert.IsNotNull(_testDataFactoryService,
                "TestDataFactoryService must be supplied to the ViewModelTester constructor in order to use this method.");
            return CanMapBothWays(entitySetter, _testDataFactoryService.GetEntityFactory<TOtherEntity>().Create());
        }

        [Obsolete(
            "Use the other CanMapBothWays method that doesn't require the factoryService. You will probably have to fix the ViewModelTester instance in your test. This should go away with MC-2520.")]
        public ViewModelTester<TViewModel, TEntity> CanMapBothWays<TOtherEntity>(Expression<Func<TEntity, TOtherEntity>> entitySetter,
            ITestDataFactoryService factoryService)
            where TOtherEntity : class, IEntity, new()
        {
            return CanMapBothWays(entitySetter, factoryService.GetEntityFactory<TOtherEntity>().Create());
        }

        #endregion

        #region CanMapToEntity

        /// <summary>
        /// Asserts that a property on the view model will be mapped to a property of the same name on the entity. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> CanMapToEntity<TProp>(Expression<Func<TViewModel, TProp>> viewModelSetter, TProp expectedValue,
            string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(viewModelSetter);
            var getter = typeof(TEntity).GetProperty(setter.Name);
            CanMapToEntity(setter, expectedValue, getter, message);

            return this;
        }

        /// <summary>
        /// Asserts that a property on the view model will be mapped to the entity property supplied. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> CanMapToEntity<TProp>(Expression<Func<TViewModel, TProp>> viewModelSetter, TProp expectedValue,
            Expression<Func<TEntity, TProp>> entityGetter, string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(viewModelSetter);
            var getter = (PropertyInfo)Expressions.GetMember(entityGetter);
            CanMapToEntity(setter, expectedValue, getter, message);

            return this;
        }

        private void CanMapToEntity(PropertyInfo viewModelSetter, object expectedValue, PropertyInfo entityGetter,
            string message)
        {
            CanMap(viewModelSetter, ViewModel, entityGetter, Entity, expectedValue, () => MapToEntity(), message);
        }

        #endregion

        #region CanMapToViewModel

        /// <summary>
        /// Asserts that a property on the entity will be mapped to a property of the same name on the view model. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> CanMapToViewModel<TProp>(Expression<Func<TEntity, TProp>> entitySetter, TProp expectedValue,
            string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(entitySetter);
            var getter = FindMatchingViewModelGetter(setter);
            CanMapToViewModel(setter, expectedValue, getter, message);

            return this;
        }

        /// <summary>
        /// Asserts that a property on the entity will be mapped to the view model property supplied. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> CanMapToViewModel<TProp>(Expression<Func<TEntity, TProp>> entitySetter, TProp expectedValue,
            Expression<Func<TViewModel, TProp>> viewModelGetter, string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(entitySetter);
            var getter = (PropertyInfo)Expressions.GetMember(viewModelGetter);
            CanMapToViewModel(setter, expectedValue, getter, message);

            return this;
        }

        private void CanMapToViewModel(PropertyInfo entitySetter, object expectedValue, PropertyInfo viewModelGetter,
            string message)
        {
            CanMap(entitySetter, Entity, viewModelGetter, ViewModel, expectedValue, MapToViewModel, message);
        }

        #endregion

        #region DoesNotMapToEntity

        /// <summary>
        /// Asserts that a property on the view model will NOT be mapped to a property of the same name on the entity. 
        /// </summary>
        /// <param name="expectedValue">The value set on the view model before attempting to map to entity. This value must be different from the entity property's value.</param>
        public ViewModelTester<TViewModel, TEntity> DoesNotMapToEntity<TProp>(Expression<Func<TViewModel, TProp>> viewModelSetter, TProp expectedValue,
            string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(viewModelSetter);
            var getter = typeof(TEntity).GetProperty(setter.Name);
            DoesNotMapToEntity(setter, expectedValue, getter, message);

            return this;
        }

        /// <summary>
        /// Asserts that a property on the view model will NOT be mapped to the given entity property.
        /// </summary>
        /// <param name="expectedValue">The value set on the view model before attempting to map to entity. This value must be different from the entity property's value.</param>
        public ViewModelTester<TViewModel, TEntity> DoesNotMapToEntity<TProp>(Expression<Func<TViewModel, TProp>> viewModelSetter, TProp expectedValue,
            Expression<Func<TEntity, TProp>> entityGetter, string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(viewModelSetter);
            var getter = (PropertyInfo)Expressions.GetMember(entityGetter);
            DoesNotMapToEntity(setter, expectedValue, getter, message);

            return this;
        }

        /// <param name="expectedValue">The value set on the view model before attempting to map to entity. This value must be different from the entity property's value.</param>
        private void DoesNotMapToEntity(PropertyInfo viewModelSetter, object expectedValue, PropertyInfo entityGetter,
            string message)
        {
            DoesNotMap(viewModelSetter, ViewModel, entityGetter, Entity, expectedValue, () => MapToEntity(), message);
        }

        #endregion

        #region DoesNotMapToViewModel

        /// <summary>
        /// Asserts that a property on the entity will NOT be mapped to a property of the same name on the view model. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> DoesNotMapToViewModel<TProp>(Expression<Func<TEntity, TProp>> entitySetter, TProp expectedValue,
            string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(entitySetter);
            var getter = FindMatchingViewModelGetter(setter);
            DoesNotMapToViewModel(setter, expectedValue, getter, message);

            return this;
        }

        /// <summary>
        /// Asserts that a property on the entity will NOT be mapped to the given entity property. 
        /// </summary>
        public ViewModelTester<TViewModel, TEntity> DoesNotMapToViewModel<TProp>(Expression<Func<TEntity, TProp>> entitySetter, TProp expectedValue,
            Expression<Func<TViewModel, TProp>> viewModelGetter, string message = null)
        {
            var setter = (PropertyInfo)Expressions.GetMember(entitySetter);
            var getter = (PropertyInfo)Expressions.GetMember(viewModelGetter);
            DoesNotMapToViewModel(setter, expectedValue, getter, message);

            return this;
        }

        private void DoesNotMapToViewModel(PropertyInfo entitySetter, object expectedValue,
            PropertyInfo viewModelGetter, string message)
        {
            DoesNotMap(entitySetter, Entity, viewModelGetter, ViewModel, expectedValue, MapToViewModel, message);
        }

        #endregion

        #endregion
    }
}
