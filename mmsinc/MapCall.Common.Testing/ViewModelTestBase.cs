using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using System.Reflection;
using MMSINC.Testing.NHibernate;

namespace MapCall.Common.Testing
{
    public abstract class ViewModelTestBase<TEntity, TViewModel>
        : MapCallMvcInMemoryDatabaseTestBase<TEntity>
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
    {
        #region Fields

        protected ViewModelTester<TViewModel, TEntity> _vmTester;
        protected TViewModel _viewModel;
        protected TEntity _entity;
        private Type _thisTestType;

        #endregion
        
        #region Properties

        protected IValidationAsserter<TViewModel> ValidationAssert { get; private set; }
        
        #endregion

        #region Constructor

        protected ViewModelTestBase()
        {
            _thisTestType = GetType();
            EnsureTestsHaveTestMethodAttribute();
        }

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void ViewModelTestBaseTestInitialize()
        {
            InitializeViewModelAndTestersFromEntity(CreateEntity());
        }

        protected void InitializeViewModelAndTestersFromEntity(TEntity entity)
        {
            _entity = entity;
            _viewModel = CreateViewModel();
            ValidationAssert =
                _container.With(_viewModel).GetInstance<ValidationAsserter<TViewModel>>();
            _vmTester = new ViewModelTester<TViewModel, TEntity>(_viewModel, _entity,
                _container.GetInstance<ITestDataFactoryService>());
        }

        protected virtual TViewModel CreateViewModel()
        {
            return _viewModelFactory.Build<TViewModel, TEntity>(_entity);
        }

        protected virtual TEntity CreateEntity()
        {
            return GetEntityFactory<TEntity>().Create();
        }

        #endregion

        #region Private Methods

        private void EnsureTestsHaveTestMethodAttribute()
        {
            var methods = new[] {
                nameof(TestPropertiesCanMapBothWays),
                nameof(TestRequiredValidation),
                nameof(TestEntityMustExistValidation),
                nameof(TestStringLengthValidation),
            };

            foreach (var methodName in methods)
            {
                if (!_thisTestType.GetMethod(methodName).GetCustomAttributes<TestMethodAttribute>().Any())
                {
                    Assert.Fail($"{_thisTestType.FullName}.{methodName} is missing a TestMethod attribute.");
                }
            }
        }

        private void EnsureSingleMethodWithAttribute<T>(string methodToOverride) where T : Attribute
        {
            var methods =
                _thisTestType.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

            if (methods.Where(x => x.GetCustomAttributes<T>().Any()).Count() > 1)
            {
                Assert.Fail(
                    $"This test has a method with the {typeof(T).Name}. Do not do this. Override the {methodToOverride} method instead.");
            }
        }

        #endregion

        #region Tests

        /// <summary>
        /// Run tests for properties that can always map in both directions. DO NOT
        /// put tests in here that only run in one direction as those should require
        /// explicit logic setup.
        /// </summary>
        public abstract void TestPropertiesCanMapBothWays();

        /// <summary>
        /// Run tests for Required or RequiredWhen validators. DO NOT put tests
        /// in here that can't be handled by a single ValidationAssert line. Those
        /// should be tested as individual tests.
        /// </summary>
        public abstract void TestRequiredValidation();

        /// <summary>
        /// Run all tests for entity must exist validators.
        /// </summary>
        public abstract void TestEntityMustExistValidation();

        /// <summary>
        /// Run all tests for string length validators.
        /// </summary>
        public abstract void TestStringLengthValidation();

        #endregion
    }
}
