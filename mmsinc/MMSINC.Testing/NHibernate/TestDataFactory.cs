using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.ClassExtensions.DictionaryExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Exceptions;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    public class TestDataFactory<TEntity> : TestDataFactory, ITestDataFactory<TEntity>
        where TEntity : class, new()
    {
        #region Private Members

        protected Type _type;
        protected readonly IContainer _container;

        #endregion

        #region Static Properties

        public static DefaultValueDictionary DefaultValues { get; private set; }

        public static TypedActionDictionary SavingActions { get; protected set; }
        public static TypedActionDictionary SavedActions { get; protected set; }

        #endregion

        #region Exposed Properties

        public ISession Session => _container.GetInstance<ISession>();

        public Type Type
        {
            get { return _type ?? (_type = GetType()); }
        }

        public bool Creating { get; protected set; }

        #endregion

        #region Constructors

        static TestDataFactory()
        {
            DefaultValues = new DefaultValueDictionary();
            SavingActions = new TypedActionDictionary();
            SavedActions = new TypedActionDictionary();
        }

        // ReSharper disable PublicConstructorInAbstractClass
        public TestDataFactory(IContainer container)
        {
            _container = container;
        }
        // ReSharper restore PublicConstructorInAbstractClass

        #endregion

        #region Static API Methods

        protected static void Defaults(object defaults)
        {
            var callingType = GetTypeOfCallingMethod();
            DefaultValues.Add(callingType, defaults);
        }

        protected static void OnSaving(Action<TEntity, IContainer> fn)
        {
            var callingType = GetTypeOfCallingMethod();
            SavingActions.Add(callingType, fn);
        }

        protected static void OnSaved(Action<TEntity, IContainer> fn)
        {
            var callingType = GetTypeOfCallingMethod();
            SavedActions.Add(callingType, fn);
        }

        #endregion

        #region Private Methods

        protected virtual TEntity Save(TEntity entity)
        {
            Session.Save(entity);
            return entity;
        }

        private TEntity PerformSave(TEntity entity)
        {
            PerformPreSaveActions(entity);
            var ret = Save(entity);
            PerformPostSaveActions(entity);
            return ret;
        }

        private IDictionary<string, object> FlattenDefaultsAndOverrides(object overrides)
        {
            // First create a dictionary of the default value getters
            var dict = new Dictionary<string, object>();
            foreach (var source in DefaultValues.GetInOrderByType(Type))
            {
                foreach (var item in GetReadableProperties(source.Value))
                {
                    dict[item.Property.Name] = item.Value;
                }
            }

            // If there are overrides, then we want to replace the default value getters.
            if (overrides != null)
            {
                // I believe this only happens when regression tests run and we hit MagicBuilderThingy
                if (overrides is IDictionary<string, object>)
                {
                    var overrideDict = (IDictionary<string, object>)overrides;
                    foreach (var kvPair in overrideDict)
                    {
                        dict[kvPair.Key] = kvPair.Value;
                    }
                }
                else
                {
                    foreach (var item in GetReadableProperties(overrides))
                    {
                        dict[item.Property.Name] = item.Value;
                    }
                }
            }

            return dict;
        }

        private void TryApplyValues(object source, TEntity entity)
        {
            if (source == null) return;
            //MethodInfo setter = null;

            if (source is IDictionary<string, object>)
            {
                ApplyValuesDictionary(source as IDictionary<string, object>, entity);
            }
            else
            {
                ApplyValuesObject(source, entity);
            }
        }

        private void ApplyValuesDictionary(IEnumerable<KeyValuePair<string, object>> source, TEntity entity)
        {
            foreach (var pair in source)
            {
                ApplyValue(pair.Key, pair.Value, entity);
            }
        }

        private void ApplyValuesObject(object source, TEntity entity)
        {
            foreach (var item in GetReadableProperties(source))
            {
                ApplyValue(item.Property.Name, item.Value, entity);
            }
        }

        private void ApplyValue(string key, object value, TEntity entity)
        {
            // Throw an exception if a property doesn't exist, cause it 
            // should mean that there's something gone wrong(like a property
            // being renamed in a model but not being updated in the anonymous
            // overrides object)
            if (!entity.HasPublicProperty(key))
            {
                throw new InvalidOperationException(
                    string.Format("The class '{0}' does not have a property named '{1}'", entity.GetType().FullName,
                        key));
            }

            MethodInfo setter;
            if (entity.HasPublicSetter(key, out setter))
            {
                try
                {
                    setter.Invoke(entity, new[] {ProcessDefaultOrOverrideValue(value)});
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException(
                        $"Error setting property '{key}' on class '{entity.GetType().FullName}' to the value '{value}'.",
                        e);
                }
            }
        }

        private object ProcessDefaultOrOverrideValue(object obj)
        {
            var type = obj as Type;
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(TestDataFactory)))
                {
                    // ReSharper disable PossibleNullReferenceException
                    // duh, we'll get a null reference exception telling us
                    // it's null if it's null
                    var factory = (ITestDataFactory<object>)_container.GetInstance(type);
                    // ReSharper restore PossibleNullReferenceException

                    return Creating ? factory.Create() : factory.Build();
                }
            }

            if (obj == null || !obj.GetType().IsGenericType)
            {
                return obj;
            }

            // handle values which are callable functions
            switch (obj)
            {
                case Func<string> f:
                    return f();
                case Func<byte[]> f:
                    return f();
                case Func<DateTime> f:
                    return f();
                case Func<Guid> f:
                    return f();
                case Func<decimal> f:
                    return f();
                case Func<int> f:
                    return f();
                case Func<IContainer, object> f:
                    return f(_container);
                default:
                    return obj;
            }
        }

        private void PerformPreSaveActions(TEntity entity)
        {
            foreach (var value in SavingActions.GetInOrderByType(Type))
            {
                value.Value(entity, _container);
            }
        }

        private void PerformPostSaveActions(TEntity entity)
        {
            foreach (var value in SavedActions.GetInOrderByType(Type))
            {
                value.Value(entity, _container);
            }
        }

        #endregion

        #region Private Static Methods

        private static IEnumerable<(PropertyInfo Property, object Value)> GetReadableProperties(object obj)
        {
            return obj.GetType().GetPublicPropertiesAndValues(obj);
        }

        protected static Type GetTypeOfCallingMethod()
        {
            return new StackTrace().GetFrame(2).GetMethod().DeclaringType;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Does not save to the database.
        /// You may discover some tests don't work as expected when using Build
        /// vs Create because Build will not cause a session flush. This can be an
        /// issue with factories that are using the OnSaving override, even if it's
        /// a factory you're using to build a dependency.
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public virtual TEntity Build(object overrides = null)
        {
            var ret = new TEntity();
            var flattened = FlattenDefaultsAndOverrides(overrides);
            TryApplyValues(flattened, ret);
            _container.BuildUp(ret);
            return ret;
        }

        /// <summary>
        /// Use this when you need to create but don't want extra objects created
        /// for objects you are replacing.
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns></returns>
        [Obsolete("This is used by one spot. Fix that spot and ditch this.")]
        public virtual TEntity BuildThenSave(object overrides = null)
        {
            var entity = Build(overrides);
            entity = PerformSave(entity);
            return entity;
        }

        public virtual IEnumerable<TEntity> BuildList(int count = 1,
            object overrides = null)
        {
            var ret = new List<TEntity>(count);

            for (var i = 0; i < count; ++i)
            {
                ret.Add(Build(overrides));
            }

            return ret;
        }

        public virtual TEntity[] BuildArray(int count = 1, object overrides = null)
        {
            return BuildList(count, overrides).ToArray();
        }

        /// <summary>
        /// Builds an instance of TEntity without saving to the database, but
        /// any referenced factory-supplied objects will be persisted.
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public virtual TEntity BuildWithConcreteDependencies(object overrides = null)
        {
            Creating = true;
            var ret = Build(overrides);
            Creating = false;
            return ret;
        }

        public virtual IEnumerable<TEntity> BuildListWithConcreteDependencies(int count = 1, object overrides = null)
        {
            Creating = true;
            var ret = BuildList(count, overrides);
            Creating = false;
            return ret;
        }

        public virtual TEntity[] BuildArrayWithConcreteDependencies(int count = 1, object overrides = null)
        {
            return BuildListWithConcreteDependencies(count, overrides).ToArray();
        }

        /// <summary>
        /// Saves entity to the database.
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public virtual TEntity Create(object overrides = null)
        {
            Creating = true;
            var ret = PerformSave(Build(overrides));
            Creating = false;

            return ret;
        }

        public virtual List<TEntity> CreateList(int count = 1,
            object overrides = null)
        {
            var ret = new List<TEntity>(count);

            for (var i = 0; i < count; ++i)
            {
                ret.Add(Create(overrides));
            }

            return ret;
        }

        public virtual ISet<TEntity> CreateSet(int count = 1, object overrides = null)
        {
            return new HashSet<TEntity>(CreateArray(count, overrides));
        }

        public virtual TEntity[] CreateArray(int count = 1, object overrides = null)
        {
            return CreateList(count, overrides).ToArray();
        }

        // TODO: Is this used anywhere?
        public override void Reset()
        {
            //Sequences.Reset();
        }

        #endregion

        #region Assertion Methods

        public TestDataFactory<TEntity> AssertCannotBeNull<TMember>(Expression<Func<TEntity, TMember>> exp)
        {
            var memberName = Expressions.GetMember(exp).Name;
            var entity = Build();
            entity.SetPropertyValueByName(memberName, null);

            MyAssert.Throws<PropertyValueException>(() => Session.Save(entity),
                String.Format(
                    "Attempting to save a {0} instance with a null value for '{1}' should throw an NHibernate.PropertyValueException based on the ClassMap for {2}.",
                    typeof(TEntity).FullName, memberName, typeof(TEntity).Name));

            return this;
        }

        public TestDataFactory<TEntity> AssertMustBeUnique<TMember>(Expression<Func<TEntity, TMember>> exp,
            TMember valueToUse)
        {
            var memberName = Expressions.GetMember(exp).Name;
            var options = new ExpandoObject();
            ((IDictionary<string, object>)options).Add(memberName, valueToUse);
            Create(options);

            MyAssert.Throws<GenericADOException>(() => Create(options),
                String.Format(
                    "Attempting to save a {0} instance with a non-unique value for '{1}' should throw an NHibernate.Exceptions.GenericADOException based on the ClassMap for {2}.",
                    typeof(TEntity).FullName, memberName, typeof(TEntity).Name));

            return this;
        }

        #endregion

        #region Nested Types

        public class DefaultValueDictionary : Dictionary<Type, object> { }

        public class TypedActionDictionary : Dictionary<Type, Action<TEntity, IContainer>> { }

        #endregion
    }

    public abstract class TestDataFactory
    {
        #region Static Properties

        public static readonly List<TestDataFactory> TestDataFactories =
            new List<TestDataFactory>();

        #endregion

        #region Constructors

        protected TestDataFactory()
        {
            TestDataFactories.Add(this);
        }

        #endregion

        #region Abstract Methods

        public abstract void Reset();

        #endregion

        #region Static Methods

        public static void ResetAll()
        {
            foreach (var factory in TestDataFactories)
            {
                factory.Reset();
            }
        }

        #endregion
    }

    public interface ITestDataFactory<out TEntity>
    {
        TEntity Build(object overrides = null);
        TEntity Create(object overrides = null);
    }

    public static class Extensions
    {
        public static TEntity EnsureEntityLookupValue<TEntity>(this TestDataFactory<TEntity> that, int id,
            string description, string tableName = null, string idColumn = "Id", string descColumn = "Description")
            where TEntity : class, IEntityLookup, new()
        {
            tableName = tableName ?? typeof(TEntity).Name + "s";

            return that.EnsureSpecificThing<TEntity>(tableName, id,
                new Dictionary<string, object> {{idColumn, id}, {descColumn, description}});
        }

        public static TEntity EnsureSpecificThing<TEntity>(this TestDataFactory<TEntity> that, string tableName, int id,
            Dictionary<string, object> columnValues)
            where TEntity : class, IEntity, new()
        {
            tableName = tableName ?? typeof(TEntity).Name + "s";

            var entity = that.Session.Get<TEntity>(id);

            if (entity != null)
            {
                return entity;
            }

            var columns = new List<string>();
            var values = new List<string>();

            foreach (var item in columnValues)
            {
                columns.Add(item.Key);
                values.Add(item.Value is int i ? i.ToString() : $"'{item.Value}'");
            }

            that.Session
                .CreateSQLQuery(
                     $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})")
                .ExecuteUpdate();

            return that.Session.Get<TEntity>(id);
        }
    }
}
