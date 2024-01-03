using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;
using TypeExtensions = MMSINC.ClassExtensions.TypeExtensions.TypeExtensions;

namespace MMSINC.Testing
{
    // TODO: Get this to actually create entities to search on to see if results are actually returned.
    //       right now it doesn't check the returned results. It only makes sure exceptions aren't thrown.

    public interface ISearchModelTesterForSearchSet
    {
        #region Properties

        Dictionary<string, object> TestPropertyValues { get; }
        HashSet<string> IgnoredPropertyNames { get; }

        // This is currently dynamic because it makes life so much easier in
        // the automatic controller tests.
        Action<dynamic> SearchCallback { get; set; }

        #endregion

        #region Methods

        void TestAllProperties(params string[] ignorePropertyNames);

        ISearchSet TestBlankSearchAndGetResult();

        #endregion
    }

    /// <summary>
    /// Tests that all properties on a search view model can be searched for without blowing up.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TSearchModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class SearchModelTesterForSearchSet<TSearchModel, TViewModel, TEntity> : ISearchModelTesterForSearchSet
        where TSearchModel : ISearchSet<TViewModel>
    {
        #region Fields

        // ReSharper disable StaticFieldInGenericType
        private static readonly string[] _ignoredPropertyNamesStatic;

        private static readonly Type _dateTimeType = typeof(DateTime);
        // ReSharper restore StaticFieldInGenericType

        private readonly IContainer _container;
        private readonly MMSINC.Data.NHibernate.IRepository<TEntity> _repository;
        private readonly ISession _session;
        private readonly ITestDataFactoryService _factoryService;
        private HashSet<string> _ignoredPropertyNames;
        private readonly IEnumerable<PropertyInfo> _allProperties = typeof(TSearchModel).GetProperties();
        private readonly IEnumerable<PropertyInfo> _requiredProperties;

        #endregion

        #region Properties

        public Action<dynamic> SearchCallback { get; set; }

        public HashSet<string> IgnoredPropertyNames
        {
            get { return _ignoredPropertyNames; }
        }

        /// <summary>
        /// Returns a dictionary of test values that can be used in place of the automatically generated values.
        /// If a value doesn't exist in this dictionary, then the automatically generated value will be used.
        ///
        /// NOTE: A value is only used for the specific property being tested. If a different property needs a value
        /// set then that value will not be set. These values *are* used when setting required properties
        /// on a model, though.
        /// 
        /// NOTE: If the value is an entity reference, you must use the entity's Id here.
        /// </summary>
        public Dictionary<string, object> TestPropertyValues { get; } = new Dictionary<string, object>();

        #endregion

        #region Constructors

        static SearchModelTesterForSearchSet()
        {
            _ignoredPropertyNamesStatic = typeof(ISearchSet).GetProperties().Select(x => x.Name)
                                                             // Results comes from ISearchSet<T>.
                                                            .Concat(new[] {"Results"}).ToArray();
        }

        public SearchModelTesterForSearchSet(
            IContainer container,
            MMSINC.Data.NHibernate.IRepository<TEntity> repository,
            ISession session,
            ITestDataFactoryService factoryService)
        {
            // Create a copy of the shared array so it can be modified.
            _requiredProperties = _allProperties.Where(x => x.GetCustomAttribute<RequiredAttribute>() != null).ToList();
            _ignoredPropertyNames = new HashSet<string>(_ignoredPropertyNamesStatic);
            _container = container;
            _repository = repository;
            _session = session;

            _factoryService = factoryService;
        }

        #endregion

        #region Private Methods

        private void TestProperty(PropertyInfo prop)
        {
            // NOTE: This does not test that expect results are returned. This is to test that the search
            // properties have the correct types/seach aliases so exceptions aren't thrown at runtime.
            var searchModel = _container.GetInstance<TSearchModel>();

            try
            {
                // Set any required values first
                foreach (var requiredProp in _requiredProperties)
                {
                    if (requiredProp != prop)
                    {
                        CreateValueAndSetProperty(searchModel, requiredProp);
                    }
                }

                CreateValueAndSetProperty(searchModel, prop);

                if (SearchCallback != null)
                {
                    SearchCallback(searchModel);
                }
                else
                {
                    _repository.Search(searchModel);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to perform search on '{0}' property of type '{1}'. Exception: {2}", prop.Name,
                    typeof(TSearchModel), ex);
            }
        }

        private void CreateValueAndSetProperty(TSearchModel searchModel, PropertyInfo prop)
        {
            if (prop.GetSetMethod() == null)
            {
                var actualValue = prop.GetValue(searchModel, new object[] { });
                if (actualValue == null)
                {
                    // If the readonly value is null then the property will not actually be
                    // used in the search query.
                    Assert.Fail(
                        $"Unable to perform search on '{prop.Name}' property of type '{typeof(TSearchModel).FullName}' because the property is read-only and has a null value.");
                }

                // Otherwise we can just test with the model as-is
            }
            else
            {
                var testValue = GenerateTestValue(prop);

                // Searches with MultiSelectAttribute have props that are int arrays.
                if (prop.PropertyType.IsAssignableFrom(typeof(int[])))
                {
                    testValue = new[] {(int)testValue};
                }
                else if (prop.PropertyType.IsAssignableFrom(typeof(string[])))
                {
                    testValue = (string[])testValue;
                }

                // NOTE: Don't set any of the base SearchSet properties on here.
                // The SearchCallback should always be calling Index() and that
                // will handle any EnablePaging or other stuff that should be set.
                // searchModel.EnablePaging = false; // to reduce the number of queries the test generates.
                prop.SetValue(searchModel, testValue, new object[] { });
            }
        }

        private Type GetUsablePropertyType(PropertyInfo prop)
        {
            var pType = prop.PropertyType;
            if (TypeExtensions.IsNullable(pType))
            {
                return pType.GetGenericArguments()[0];
            }

            return pType;
        }

        // TODO: Refactor this method in to a way that also usese SearchMapper so we're not having to scan
        //       for this twice.
        private bool RequiresEntityCreation(PropertyInfo viewModelProp, Type viewModelPropType,
            out PropertyInfo entityProp)
        {
            // Select/MultiSelect can be used with strings. These can't be entity mapped.
            if (viewModelPropType != typeof(int))
            {
                entityProp = null;
                return false;
            }

            var viewModelAttributes = viewModelProp.GetCustomAttributes(true);
            var isDropDown = viewModelAttributes.OfType<SelectAttribute>().Any();

            if (!isDropDown) // Means we're actually searching for integers or something else.
            {
                entityProp = null;
                return false;
            }

            if (typeof(TEntity).TryGetPublicGetter(viewModelProp.Name, out entityProp) &&
                viewModelPropType != entityProp.PropertyType)
            {
                return true;
            }

            return false;
        }

        private object GenerateTestValue(PropertyInfo prop)
        {
            if (TestPropertyValues.ContainsKey(prop.Name))
            {
                return TestPropertyValues[prop.Name];
            }

            // If this is generic, it'll return the generic constraint instead.
            var usablePropType = GetUsablePropertyType(prop);

            PropertyInfo entityProp = null;
            if (RequiresEntityCreation(prop, usablePropType, out entityProp))
            {
                return GenerateEntityAndTestValue(entityProp);
            }

            return GenerateTestValue(usablePropType);
        }

        private object GenerateTestValue(Type type)
        {
            // NOTE: DON'T RETURN NULLS! Our search mapping methods ignore null values on properties.

            if (type == _dateTimeType)
            {
                return default(DateTime);
            }

            if (type == typeof(DateRange))
            {
                // Some override repository methods manually implement the query for DateRanges.
                // The default Operator is Between which will cause errors if Start or End are null.
                return new DateRange {
                    End = DateTime.Today.EndOfDay(),
                    Start = DateTime.Today,
                    Operator = RangeOperator.Between
                };
            }

            if (type == typeof(RequiredDateRange))
            {
                // Some override repository methods manually implement the query for DateRanges.
                // The default Operator is Between which will cause errors if Start or End are null.
                return new RequiredDateRange {
                    End = DateTime.Today.EndOfDay(),
                    Start = DateTime.Today,
                    Operator = RangeOperator.Between
                };
            }

            if (type == typeof(string))
            {
                return "some string";
            }

            if (type == typeof(int[]))
            {
                type = typeof(int);
            }

            if (type == typeof(string[]))
            {
                return new[] {"test search string in an array"};
            }

            if (type == typeof(SearchString))
            {
                return new SearchString {
                    Value = "some search string"
                };
            }

            return Activator.CreateInstance(type);
        }

        protected object GenerateEntityAndTestValue(PropertyInfo entityProp)
        {
            dynamic factory = _factoryService.GetEntityFactory(entityProp.PropertyType);
            var entity = factory.Create();
            _session.Flush();
            return _session.GetIdentifier(entity);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// For each property, a value will be set, then a search will be performed
        /// to ensure that the generated query is valid.
        /// </summary>
        /// <param name="ignorePropertyNames"></param>
        public void TestAllProperties(params string[] ignorePropertyNames)
        {
            foreach (var p in _allProperties)
            {
                if (!_ignoredPropertyNames.Union(ignorePropertyNames).Contains(p.Name))
                {
                    //  Console.WriteLine();
                    //   Console.WriteLine("SearchModelTester: Testing property: {0}", p.Name);
                    TestProperty(p);
                }
            }
        }

        /// <summary>
        /// Attempts to perform a single search with a default, valid model.
        /// This means the only value set on the model are required fields.
        /// </summary>
        public ISearchSet TestBlankSearchAndGetResult()
        {
            // NOTE: This does not test that expect results are returned. This is to test that the search
            // properties have the correct types/search aliases so exceptions aren't thrown at runtime.
            var searchModel = _container.GetInstance<TSearchModel>();

            try
            {
                // Set any required values first
                foreach (var requiredProp in _requiredProperties)
                {
                    CreateValueAndSetProperty(searchModel, requiredProp);
                }

                if (SearchCallback != null)
                {
                    SearchCallback(searchModel);
                }
                else
                {
                    _repository.Search(searchModel);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unable to perform an empty search on type '{typeof(TSearchModel)}'. Exception: {ex}");
            }

            return searchModel;
        }

        #endregion
    }
}
