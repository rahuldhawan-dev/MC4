using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DeleporterCore.Client;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Infrastructure;
using MMSINC.Testing.SpecFlow.Library;
using NHibernate;
using StructureMap;
using TechTalk.SpecFlow;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.ClassExtensions.RegexExtensions;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Utilities;
using NUnit.Framework;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    [Binding]
    public static class Data
    {
        #region Properties

#pragma warning disable 169
        private static System.Data.SQLite.SQLiteException _doNotUseThisException;
#pragma warning restore 169
        public static TestTypeDictionary TypeDictionary;
        public static Assembly FactoryAssembly, ModelAssembly;
        private static Action PreCreateObjectFn, PostCreateObjectFn;

        public static ISession Session
        {
            get { return DependencyResolver.Current.GetService<ISession>(); }
        }

        /// <summary>
        /// If true then named-data lookups should just use the versions in the
        /// TestObjectCache, rather than re-getting them through deleporter
        /// from the site's persistence layer.
        /// </summary>
        public static bool NoDataReload { get; set; }

        #endregion

        #region Step Definitions

        [Given("^an? ([^\"]+) exists with (.+)$")]
        public static void GivenAThingExistsWithValues(string type, string values)
        {
            CreateObject(type, null, values, TestObjectCache.Instance);
        }

        [Given("^an? ([^\"]+) \"([^\"]+)\" exists")]
        public static void GivenANamedThingExists(string type, string name)
        {
            GivenANamedThingExistsWithValues(type, name, String.Empty);
        }

        [Given("^an? ([^\"]+) \"([^\"]+)\" exists with (.+)$")]
        public static void GivenANamedThingExistsWithValues(string type, string name, string values)
        {
            var objectCache = TestObjectCache.Instance;
            var dictionary = objectCache.EnsureDictionary(type);

            if (dictionary.ContainsKey(name))
            {
                throw new Exception(String.Format("Object Cache already contains named instance '{0}' of type '{1}'.",
                    name, type));
            }

            CreateObject(type, name, values, objectCache);
        }

        [Then(
            "^the currently shown ([^\"]+) (?:shall|will) (?:henceforth|now) be (?:known throughout the land|referred to) as \"([^\"]+)\"$")]
        public static void ThenAssociateTheCurrentThingWithTheTestObjectCache(string type, string name)
        {
            var rgx = new Regex(@"[^\d]+(\d+)$");
            var url = WebDriverHelper.Current.CurrentUri.ToString();

            if (!rgx.TryMatch(url, out var match))
            {
                WebDriverHelper.Current.CaptureScreenshot();
                throw new Exception(
                    $"Current url '{url}' does not end in an integer to indicate a shown record:" 
                    + Environment.NewLine 
                    + WebDriverHelper.Current.PageSource);
            }

            var id = int.Parse(match.Groups[1].Value);
            var entityType = GetModelType(type);
            var entity = GetEntityFromPersistence(entityType, id);

            if (entity == null)
            {
                throw new Exception($"Could not load entity of type '{type}' with the id '{id}'.");
            }

            TestObjectCache.Instance.EnsureDictionary(type).Add(name, entity);
        }

        [Then("([^\"]+) \"([^\"]+)\" and \"([^\"]+)\" should not be the same record")]
        public static void ThisSpyIsNotOurSpy(string type, string nameLeft, string nameRight)
        {
            var objectCache = TestObjectCache.Instance;
            var leftItem = objectCache.Lookup(type, nameLeft);
            var rightItem = objectCache.Lookup(type, nameRight);
            NUnit.Framework.Assert.AreNotSame(leftItem, rightItem);
            NUnit.Framework.Assert.AreNotEqual(leftItem.GetPropertyValueByName("Id"),
                rightItem.GetPropertyValueByName("Id"));
        }

        [Given("^the test flag \"([^\"]+)\" exists")]
        public static void GivenTheRegressionTestFlagExists(string flagName)
        {
#if DEBUG
            Deleporter.Run(() => { TestHelperProxy.AddRegressionTestFlag(flagName); });
#endif
        }

        [Then(@"I should delete ([^\""]+) ""([^""]+)""'s file to clean up after this test")]
        public static void IDeleteTheAsBuiltImagesUploadedFile(string type, string name)
        {
            var namedItem = (dynamic)GetCachedEntity(type, name);
            var imageRoot = Deleporter.Run(() => ConfigurationManager.AppSettings["ImageUploadRootDirectory"]);

            var filePath = Path.Combine(imageRoot, namedItem.Directory, namedItem.FileName);
            FileIO.DeleteIfFileExists(filePath);
        }

        #endregion

        #region Event-Driven Functionality

        private static void PerformPreStartUp()
        {
            var startUpAttr = typeof(SpecFlowInitializeAttribute);
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var attr in a.GetCustomAttributes(startUpAttr, false).OfType<SpecFlowInitializeAttribute>())
                {
                    attr.Method.Invoke(null, null);
                }
            }
        }

        // NOTE: you'll need something like this in your project's Data step
        // definitions
        [BeforeTestRun]
        public static void Start()
        {
            PerformPreStartUp();

            if (!WebServer.IsInitialized)
            {
                WebServer.Open();
            }

            // We need a local copy of these to send through Deleporter.
            // Otherwise, it won't get set. -Ross 3/1/2012
            var td = TypeDictionary;
            var fa = FactoryAssembly;
            var ma = ModelAssembly;

            Deleporter.Run(() => {
                SetTypeDictionary(td);
                SetFactoryAssembly(fa);
                SetModelAssembly(ma);
                TestHelperProxy.EnableTestModeOnGlobal();
                TestHelperProxy.InitializeTestDatabase();
            });
        }

        [AfterTestRun]
        public static void Finish()
        {
            Deleporter.Run(() => {
                TestHelperProxy.DisableTestModeOnGlobal();
                TestHelperProxy.DestroyTestDatabase();
            });
        }

        private static Stopwatch _stop = new Stopwatch();

        // this needs to happen before any other potential BeforeScenario hooks so that the database and
        // such will be there for any that need it
        [BeforeScenario(Order = 0)]
        public static void Open()
        {
            _stop.Restart();
            Deleporter.Run(() => {
                TestHelperProxy.CreateSystemWideSession();
                TestHelperProxy.ResetNHibernateSessionMessages();
                TestHelperProxy.EnableRequestProcessing();
            });
        }

        [AfterScenario]
        public static void Close()
        {
            WebDriverHelper.Current.PerformTerribleHackToClearUpChromeMemoryLeak();
            // Ideally, when we rollback, the site should not be responding to anymore requests.
            // That's most likely what's causing all the various nhibernate flukes. 
            try
            {
                Deleporter.Run(() => {
                    TestHelperProxy.DisableRequestProcessing();
                    TestHelperProxy.DestroySystemWideSession();
#if DEBUG
                    TestHelperProxy.ClearAllRegressionTestFlags();
#endif
                });
            }
            finally { }

            var sessionDisposalMessage =
                Deleporter.Run<String>(TestHelperProxy.GetNHibernateSessionMessage);
            var sessionFactoryDisposalMessage =
                Deleporter.Run<String>(
                    TestHelperProxy.GetNHibernateSessionFactoryMessage);
            TestObjectCache.Reset();
            _stop.Stop();
            Console.WriteLine("TIME FOR TEST: {0}ms", _stop.ElapsedMilliseconds);
            Console.WriteLine(String.IsNullOrWhiteSpace(sessionDisposalMessage)
                ? "NO SESSION DISPOSAL MESSAGE"
                : sessionDisposalMessage);
            Console.WriteLine(String.IsNullOrWhiteSpace(sessionFactoryDisposalMessage)
                ? "NO SESSION FACTORY DISPOSAL MESSAGE"
                : sessionFactoryDisposalMessage);
        }

        [BeforeScenario("@no_data_reload")]
        public static void PreNoDataReload()
        {
            NoDataReload = true;
        }

        [AfterScenario("@no_data_reload")]
        public static void PostNoDataReload()
        {
            NoDataReload = false;
        }

        #endregion

        #region Helper Methods

        public static void SetTypeDictionary(TestTypeDictionary dictionary)
        {
            TypeDictionary = dictionary;
        }

        public static void SetFactoryAssembly(Assembly factoryAssembly)
        {
            FactoryAssembly = factoryAssembly;
        }

        public static void SetModelAssembly(Assembly ma)
        {
            ModelAssembly = ma;
        }

        public static void SetPreCreateObjectFn(Action fn)
        {
            PreCreateObjectFn = fn;
        }

        public static void SetPostCreateObjectFn(Action fn)
        {
            PostCreateObjectFn = fn;
        }

        /// <summary>
        /// Throws exception if model type cannot be found.
        /// </summary>
        public static Type GetModelType(string name)
        {
            if (TypeDictionary == null && ModelAssembly == null)
            {
                throw new NullReferenceException(
                    "Both TypeDictionary and ModelAssembly are null, no way to find type.");
            }

            if (TypeDictionary != null && TypeDictionary.ContainsKey(name))
            {
                return TypeDictionary.GetTypeRegistration(name).Type;
            }

            var typeName = name.ToPascalCase();
            var type = ModelAssembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            if (type != null)
            {
                return type;
            }

            throw new ArgumentException(String.Format("Cound not find model type with name '{0}'", name), "name");
        }

        public static NameValueCollection ConvertToNameValueCollection(string data)
        {
            var pairs = data.Split(new[] {", "}, StringSplitOptions.None);
            var ret = new NameValueCollection();

            foreach (var pair in pairs)
            {
                var split = pair.Split(new[] {": "}, StringSplitOptions.None);

                var key = split[0];
                if (key != key.Trim())
                {
                    throw new Exception("You probably put a space somewhere between '" + key +
                                        "' and ':' in the feature file. Data: " + data);
                }

                ret.Add(split[0],
                    String.Join("", split.Skip(1).ToArray()).StripQuotes());
            }

            foreach (var key in ret.AllKeys)
            {
                // ReSharper disable AssignNullToNotNullAttribute
                if (ret.GetValues(key).Count() > 1)
                    // ReSharper restore AssignNullToNotNullAttribute
                {
                    throw new InvalidOperationException(
                        String.Format("The key '{0}' appears more than once in '{1}'. I'm sure you didn't mean it.",
                            key,
                            data));
                }
            }

            return ret;
        }

        public static object CreateObject(string type, string name, string values, TestObjectCache objectCache = null)
        {
            // Is there a need for the objectCache parameter for this method? It's immediately overwritten.
            objectCache = TestObjectCache.Instance;
            object obj = null;
            string error = null;
            PreCreateObjectFn?.Invoke();

            Deleporter.Run(() => {
                try
                {
                    obj = CallTypeLambda(type, values, objectCache) ??
                          BuildUsingReflection(type, values, objectCache);
                }
                catch (Exception e)
                {
                    error = e.ToString();
                }
            });

            if (error != null)
            {
                Assert.Fail(
                    $"The following error was encountered creating object of type {type}:{Environment.NewLine}{error}");
            }

            if (obj == null)
            {
                throw new NullReferenceException(String.Format("Could not build object of type {0}.", type));
            }

            if (name != null)
            {
                objectCache.EnsureDictionary(type).Add(name, obj);
            }

            PostCreateObjectFn?.Invoke();

            // objectCache is replaced with the the deserialized version
            // returned from 
            TestObjectCache.Instance = objectCache;
            return obj;
        }

        // TODO: It'd be nice to come back to this, but at the moment it runs slower than making each
        // item individually for some reason.
        //[Given("^the following objects exist")]
        //public static void GivenThisBulkedThingExists(Table table)
        //{
        //    var serializedObjects = new List<SerializedObject>();
        //    foreach (var row in table.Rows)
        //    {
        //        var so = new SerializedObject();
        //        so.Type = row[0];
        //        so.Name = row[1];
        //        so.Values = row[2];
        //        serializedObjects.Add(so);
        //    }

        //    var objectCache = TestObjectCache.Instance;
        //    Deleporter.Run(() => {
        //        foreach (var so in serializedObjects)
        //        {
        //            var result = CallTypeLambda(so.Type, so.Values, objectCache) ?? BuildUsingReflection(so.Type, so.Values, objectCache);
        //            so.Object = result;
        //            objectCache.EnsureDictionary(so.Type).Add(so.Name, so.Object);
        //        }

        //    });

        //    //if (returnObjectsDictionary.Any(x => x.Value == null))
        //    //{
        //    //    throw new NullReferenceException(String.Format("Could not build object of type {0}.", type));
        //    //}

        //    //foreach (var so in serializedObjects)
        //    //{
        //    //    objectCache.EnsureDictionary(so.Type).Add(so.Name, so.Object);
        //    //}

        //    //foreach (var kvPair in returnObjectsDictionary)
        //    //{
        //    //    objectCache.EnsureDictionary(type).Add(kvPair.Key, kvPair.Value);
        //    //}

        //    // objectCache is replaced with the the deserialized version
        //    // returned from 
        //    TestObjectCache.Instance = objectCache;
        //  //  return returnObjectsDictionary;
        //}

        //[Serializable]
        //private class SerializedObject
        //{
        //    public string Type { get; set; }
        //    public string Name { get; set; }
        //    public string Values { get; set; }
        //    public object Object { get; set; }
        //}

        /// <summary>
        /// Use this if you're calling CreateObject in a loop a lot. This saves a lot of time. 
        /// ex: GivenWorkDescriptionsExist went from taking 15.6 seconds to 6.2 seconds.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="namesAndValues"></param>
        /// <returns></returns>
        public static object CreateObjectsInBulk(string type, Dictionary<string, string> namesAndValues)
        {
            var objectCache = TestObjectCache.Instance;
            var returnObjectsDictionary = new Dictionary<string, object>();
            Deleporter.Run(() => {
                foreach (var kvPair in namesAndValues)
                {
                    var values = kvPair.Value;
                    var result = CallTypeLambda(type, values, objectCache) ??
                                 BuildUsingReflection(type, values, objectCache);
                    returnObjectsDictionary.Add(kvPair.Key, result);
                }
            });

            if (returnObjectsDictionary.Any(x => x.Value == null))
            {
                throw new NullReferenceException(String.Format("Could not build object of type {0}.", type));
            }

            foreach (var kvPair in returnObjectsDictionary)
            {
                objectCache.EnsureDictionary(type).Add(kvPair.Key, kvPair.Value);
            }

            // objectCache is replaced with the the deserialized version
            // returned from 
            TestObjectCache.Instance = objectCache;
            return returnObjectsDictionary;
        }

        public static object CallTypeLambda(string type, string values, TestObjectCache objectCache)
        {
            if (TypeDictionary == null)
            {
                throw new NullReferenceException(
                    "The type dictionary has not been set.  You need to call MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetTypeDictionary(TYPE_DICTIONARY)");
            }

            if (!TypeDictionary.ContainsKey(type))
            {
                return null;
            }

            return TypeDictionary.GetTypeRegistration(type)
                                 .RetrievalFn(ConvertToNameValueCollection(values), objectCache,
                                      DependencyResolver.Current.GetService<IContainer>());
        }

        public static object BuildUsingReflection(string type, string values, TestObjectCache objectCache)
        {
            return new MagicalBuilderThingy(type, values, objectCache,
                DependencyResolver.Current.GetService<IContainer>()).Create();
        }

        public static DateTime? GetDateTime(string dateValue)
        {
            return (String.IsNullOrWhiteSpace(dateValue) ? (DateTime?)null : dateValue.ToDateTime());
        }

        public static Int32 GetEntityId(object entity)
        {
            return DependencyResolver.Current?.GetService<IContainer>()?.TryGetInstance<ISessionFactory>() == null
                ? Deleporter.Run(() => TestHelperProxy.GetEntityId(entity))
                : TestHelperProxy.GetEntityId(entity);
        }

        public static bool EntityExists(int id, Type entityType)
        {
            return Deleporter.Run(() => TestHelperProxy.EntityExists(entityType, id));
        }

        public static object GetCurrentEntity(object entity, Type type)
        {
            return Deleporter.Run(() =>
                TestHelperProxy.GetEntityFromPersistence(entity, type));
        }

        public static object GetCurrentEntityPropertyValue(object entity, Type type, string propertyName)
        {
            return Deleporter.Run(() =>
                TestHelperProxy.GetEntityPropertyValueFromPersistence(entity, type, propertyName));
        }

        public static object GetCurrentEntityPropertyValue(string type, string name, string propertyName)
        {
            var cachedEntity = GetCachedEntity(type, name);
            return GetCurrentEntityPropertyValue(cachedEntity, cachedEntity.GetType(), propertyName);
        }

        public static object GetEntityFromPersistence(Type entityType, object id)
        {
            return Deleporter.Run(() => TestHelperProxy.GetEntityFromPersistence(entityType, id));
        }

        public static object GetCachedEntity(string type, string name)
        {
            return TestObjectCache.Instance.Lookup(type, name);
        }

        public static string GetCachedEntityPropertyValue(string type, string name, string property)
        {
            var namedItem = GetCachedEntity(type, name);
            var data = property == "ToString"
                ? namedItem.ToString()
                : namedItem.GetPropertyValueByName(property)?.ToString();
            return data;
        }

        #endregion

        #region Magical Builder Thingy

        public class MagicalBuilderThingy
        {
            #region Private Members

            private readonly string _typeName, _values;
            private readonly TestObjectCache _objectCache;
            private readonly IContainer _container;

            #endregion

            #region Properties

            public String TypeName
            {
                get { return _typeName; }
            }

            public String Values
            {
                get { return _values; }
            }

            public TestObjectCache ObjectCache
            {
                get { return _objectCache; }
            }
            public Type FactoryType { get; protected set; }
            public Type ModelType { get; protected set; }

            #endregion

            #region Constructor

            public MagicalBuilderThingy(string type, string values, TestObjectCache objectCache, IContainer container)
            {
                EnsureAssemblies();

                _typeName = type;
                _values = values;
                _objectCache = objectCache;
                _container = container;
            }

            #endregion

            #region Private Methods

            private void EnsureAssemblies()
            {
                if (FactoryAssembly == null)
                {
                    throw new NullReferenceException(
                        "The factory assembly has not been set.  You need to call MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetFactoryAssembly(typeof(UserFactory).Assembly)");
                }

                if (ModelAssembly == null)
                {
                    throw new NullReferenceException(
                        "The model assembly has not been set.  You need to call MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetModelAssembly(typeof(User).Assembly)");
                }
            }

            private void EnsureFactoryAndModelTypes()
            {
                var factoryName = (TypeName + " factory").ToPascalCase();
                FactoryType =
                    FactoryAssembly.GetTypes()
                                   .FirstOrDefault(t => t.IsSubclassOf(typeof(TestDataFactory)) &&
                                                        t.Name == factoryName);

                if (FactoryType != null)
                {
                    ModelType = FactoryType.GetMethod("Build").ReturnType;
                }
                else
                {
                    ModelType = GetModelType(TypeName);
                    FactoryType =
                        (ModelType.IsSubclassOf(typeof(EntityLookup))
                            ? typeof(EntityLookupTestDataFactory<>)
                            : typeof(TestDataFactory<>)).MakeGenericType(ModelType);
                }
            }

            private IDictionary<string, object> ProcessValues()
            {
                var overrides = new Dictionary<string, object>();

                if (Values == string.Empty)
                {
                    return overrides;
                }

                var nvc = ConvertToNameValueCollection(Values);
                foreach (var key in nvc.AllKeys)
                {
                    var propName = key.ToPascalCase();
                    var pi = ModelType.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
                    dynamic value;

                    // TODO: refactor this, maybe it's possible to use a switch statement pattern matching?

                    if (pi == null)
                    {
                        throw new ArgumentException(
                            $"Could not find property '{propName}' on model class {ModelType.Name} from the key {key}.");
                    }

                    // If the type of the model member we found comes from the
                    // model assembly, we know it's a model and can get it from
                    // the object cache
                    if (pi.PropertyType.Assembly == ModelAssembly)
                    {
                        value = GetValueFromSession(pi, nvc, key);
                    } // Convert and urlDecode strings
                    else if (pi.PropertyType == typeof(string))
                    {
                        value = Uri.UnescapeDataString(nvc[key]);
                    }
                    // Otherwise assume it's a type we can convert to, and hope for the best
                    else if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(Nullable<DateTime>))
                    {
                        value = nvc[key].ToDateTime();
                    }
                    else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(Nullable<int>))
                    {
                        value = Convert.ToInt32(nvc[key]);
                    }
                    else if (pi.PropertyType == typeof(long) || pi.PropertyType == typeof(Nullable<long>))
                    {
                        value = Convert.ToInt64(nvc[key]);
                    }
                    else if (pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(Nullable<decimal>))
                    {
                        value = Convert.ToDecimal(nvc[key]);
                    }
                    else if (pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(Nullable<bool>))
                    {
                        value = Convert.ToBoolean(nvc[key]);
                    }
                    else
                    {
                        try
                        {
                            value = Convert.ChangeType(nvc[key], pi.PropertyType);
                        }
                        catch (InvalidCastException e)
                        {
                            throw new Exception(
                                String.Format("Key: {0}, PropertyType: {1}, RawValue: {2}\n{3}", key,
                                    pi.PropertyType.Name, nvc[key], e.Message), e);
                        }
                    }

                    overrides.Add(propName, value);
                }

                return overrides;
            }

            private dynamic GetValueFromSession(PropertyInfo pi, NameValueCollection nvc, string key)
            {
                Dictionary<string, object> dictionary;
                var dictionaryName = pi.PropertyType.Name.ToLowerSpaceCase();
                dynamic value;

                try
                {
                    dictionary = ObjectCache.EnsureDictionary(dictionaryName);
                }
                catch (KeyNotFoundException e)
                {
                    throw new Exception(
                        String.Format("Cound not find existing dictionary by name '{0}'.", dictionaryName), e);
                }

                try
                {
                    var namedValueKey = nvc[key];
                    value = namedValueKey == "null" ? null : dictionary[namedValueKey];
                }
                catch (KeyNotFoundException e)
                {
                    throw new Exception(
                        String.Format("Could not find '{0}' named '{1}' using key '{2}'.", dictionaryName, nvc[key],
                            key), e);
                }

                return value == null ? value : _container.GetInstance<ISession>().Load(value.GetType(), value.Id);
            }

            #endregion

            #region Public Methods

            public object Create()
            {
                EnsureFactoryAndModelTypes();
                var overrides = ProcessValues();
                dynamic factory = _container.GetInstance(FactoryType);
                return factory.Create(overrides);
            }

            #endregion
        }

        #endregion
    }
}
