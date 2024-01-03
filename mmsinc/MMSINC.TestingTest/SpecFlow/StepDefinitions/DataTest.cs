using System;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using StructureMap;
using DataClass = MMSINC.Testing.SpecFlow.StepDefinitions.Data;

namespace MMSINC.TestingTest.SpecFlow.StepDefinitions
{
    [TestClass]
    public class DataTest : StepDefinitionTest<TestUser>
    {
        #region Private Members

        private TestObjectCache _objectCache;
        private TestTypeDictionary _typeDictionary;

        // ReSharper disable once UnusedField.Compiler
        private SQLiteException _doNotUseThisException;

        #endregion

        #region Properties

        protected override Type StepDefinitionClass
        {
            get { return typeof(DataClass); }
        }

        protected override Assembly ModelAssembly
        {
            get { return typeof(TestUser).Assembly; }
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            ClearStaticState();
            _objectCache = new TestObjectCache();
            _typeDictionary = new TestTypeDictionary();
            DataClass.SetTypeDictionary(_typeDictionary);
            DataClass.SetFactoryAssembly(typeof(TestUser).Assembly);
            DataClass.SetModelAssembly(typeof(TestUser).Assembly);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ClearStaticState();
        }

        #endregion

        #region Private Methods

        private static void ClearStaticState()
        {
            DataClass.SetModelAssembly(null);
            DataClass.SetFactoryAssembly(null);
            DataClass.SetTypeDictionary(null);
        }

        #endregion

        #region CreateObject Tests

        #region CallTypeLambda

        [TestMethod]
        public void TestCallTypeLambdaCallsTheRegisteredLambdaForTheSpecifiedType()
        {
            var group = new TestGroup();
            _typeDictionary.Add("foo", typeof(TestGroup), (nvc, toc, s) => group);

            Assert.AreSame(group, DataClass.CallTypeLambda("foo", "", _objectCache));
        }

        [TestMethod]
        public void TestCallTypeLambdaReturnsNullWhenTypeDictionaryDoesNotContainKey()
        {
            Assert.IsNull(DataClass.CallTypeLambda("foo", "baz", _objectCache));
        }

        [TestMethod]
        public void TestCallTypeLambdaThrowsExceptionWhenTypeDictionaryIsNotSet()
        {
            DataClass.SetTypeDictionary(null);

            MyAssert.Throws<NullReferenceException>(
                () => DataClass.CallTypeLambda("foo", "baz", _objectCache));
        }

        #endregion

        #region BuildUsingReflection

        [TestMethod]
        public void TestBuildUsingReflectionAllowsOverrides()
        {
            MyAssert.CausesIncrease(() => {
                var group = GetFactory<TestGroupFactory>().Create();
                _objectCache.EnsureDictionary("test group").Add("group1", group);

                var user = DataClass.BuildUsingReflection("test user",
                    "main group: group1, email: eviluser@evilsite.evil", _objectCache) as TestUser;

                Assert.IsInstanceOfType(user, typeof(TestUser));
                Assert.AreEqual(group, user.MainGroup);
                Assert.AreEqual("eviluser@evilsite.evil", user.Email);
            }, () => _container.GetInstance<IRepository<TestUser>>().GetAll().Count());
        }

        [TestMethod]
        public void TestBuildUsingReflectionCreatesAFactoryIfNoneCanBeFound()
        {
            MyAssert.CausesIncrease(() => {
                var thing =
                    DataClass.BuildUsingReflection("test factoryless thing", "", _objectCache) as
                        TestFactorylessThing;

                Assert.IsNotNull(thing);
            }, () => _container.GetInstance<IRepository<TestFactorylessThing>>().GetAll().Count());
        }

        [TestMethod]
        public void TestBuildUsingReflectionThrowsExceptionWhenFactoryAssemblyIsNotSet()
        {
            DataClass.SetFactoryAssembly(null);

            MyAssert.Throws<NullReferenceException>(
                () => DataClass.BuildUsingReflection("foo", "baz", _objectCache));
        }

        [TestMethod]
        public void TestBuildUsingReflectionThrowsExceptionWhenFactoryTypeAndModelTypeCannotBeFound()
        {
            DataClass.SetFactoryAssembly(typeof(string).Assembly);

            MyAssert.Throws<ArgumentException>(
                () => DataClass.BuildUsingReflection("foo", "baz", _objectCache));
        }

        [TestMethod]
        public void TestBuildUsingReflectionThrowsExceptionWhenModelAssemblyIsNotSet()
        {
            DataClass.SetModelAssembly(null);

            MyAssert.Throws<NullReferenceException>(
                () => DataClass.BuildUsingReflection("foo", "baz", _objectCache));
        }

        [TestMethod]
        public void TestBuildUsingReflectionUsesFactoryToCreateObject()
        {
            MyAssert.CausesIncrease(() => {
                var group = DataClass.BuildUsingReflection("test group", "", _objectCache);

                Assert.IsInstanceOfType(group, typeof(TestGroup));
            }, () => _container.GetInstance<IRepository<TestGroup>>().GetAll().Count());
        }

        [TestMethod]
        public void TestBuildUsingReflectionWorksProperlyForEntityLookupsWithNoFactory()
        {
            MyAssert.CausesIncrease(() => {
                var thing = DataClass.BuildUsingReflection("test entity lookup", "", _objectCache) as TestEntityLookup;
                Assert.IsFalse(String.IsNullOrWhiteSpace(thing.Description));
            }, () => _container.GetInstance<IRepository<TestEntityLookup>>().GetAll().Count());
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class MagicalBuilderThingyTest : InMemoryDatabaseTest<TestUser>
    {
        private Assembly _oldFactoryAssembly, _oldModelAssembly;
        private TestObjectCache _objectCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _oldFactoryAssembly = DataClass.FactoryAssembly;
            DataClass.FactoryAssembly = typeof(TestUserFactory).Assembly;
            _oldModelAssembly = typeof(TestUser).Assembly;
            DataClass.ModelAssembly = _oldModelAssembly;
            _objectCache = new TestObjectCache();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DataClass.FactoryAssembly = _oldFactoryAssembly;
            DataClass.ModelAssembly = _oldModelAssembly;
        }

        [TestMethod]
        public void TestCreateCreatesObject()
        {
            var type = "test user";
            var target = new DataClass.MagicalBuilderThingy(type, string.Empty, _objectCache, _container);

            Assert.IsNotNull(target.Create());
        }

        [TestMethod]
        public void TestCreateCreatesObjectWithArguments()
        {
            var email = "meh whatever";
            var type = "test user";
            var target = new DataClass.MagicalBuilderThingy(type, String.Format("email: \"{0}\"", email), _objectCache,
                _container);

            var user = (TestUser)target.Create();

            Assert.AreEqual(email, user.Email);
        }

        [TestMethod]
        public void TestCreateCreatesObjectWithNullableDecimal()
        {
            var someNullableDecimal = 123.45m;
            var type = "test user";
            var target = new DataClass.MagicalBuilderThingy(type,
                String.Format("some nullable decimal: {0}", someNullableDecimal),
                _objectCache, _container);

            var user = (TestUser)target.Create();

            Assert.AreEqual(someNullableDecimal, user.SomeNullableDecimal);
        }

        [TestMethod]
        public void TestCreateReturnsNullWhenValueIsExplicitlySetToNullForReferenceProperties()
        {
            var type = "test user";
            var target = new DataClass.MagicalBuilderThingy(type, "main group: \"null\"", _objectCache, _container);

            var user = (TestUser)target.Create();

            Assert.IsNull(user.MainGroup);
        }
    }
}
