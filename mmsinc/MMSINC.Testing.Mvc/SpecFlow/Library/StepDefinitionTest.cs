using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MMSINC.Utilities.StructureMap;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace MMSINC.Testing.SpecFlow.Library
{
    [DoNotParallelize]
    public abstract class StepDefinitionTest<TAssemblyOf> : InMemoryDatabaseTest<TAssemblyOf>
    {
        #region Private Members

        //  protected Mock<ISlightlyBetterBrowser> _browser;

        #endregion

        #region Abstract Properties

        protected abstract Type StepDefinitionClass { get; }
        protected abstract Assembly ModelAssembly { get; }

        #endregion

        #region Private Methods

        [TestInitialize]
        public void StepDefinitionTestInitialize()
        {
            StepDefinitions.Data.SetModelAssembly(ModelAssembly);
            StepDefinitions.Data.NoDataReload = true;
            TestObjectCache.Reset();

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        protected void TestFor(string testString, Action<StepDefinitionTester> testFn)
        {
            testFn(new StepDefinitionTester(StepDefinitionClass, testString));
        }

        protected void TestFor(string testString, Func<StepDefinitionTester, bool> testFn)
        {
            testFn(new StepDefinitionTester(StepDefinitionClass, testString));
        }

        protected TEntity CreateCachedTestObject<TEntity>(string name, string typeName = null, object overrides = null)
            where TEntity : class, new()
        {
            overrides = overrides ?? new object();
            var obj = GetEntityFactory<TEntity>().Create(overrides);
            typeName = typeName ?? obj.GetType().Name.ToLowerSpaceCase();

            TestObjectCache.Instance.EnsureDictionary(typeName).Add(name, obj);

            return obj;
        }

        //protected Constraint ConstraintMatch(Constraint constraint)
        //{
        //    return It.Is<Constraint>(c => c.ToString() == constraint.ToString());
        //}

        #endregion
    }

    public class StepDefinitionTester
    {
        #region Private Members

        private readonly Regex STEP_REGEX =
            new Regex("^((?:Given|When|Then) ).+", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly Type _bindingClass;
        private readonly string _originalString;
        private readonly Regex _stepRegex;
        private readonly MethodInfo _stepDefinition;
        private Type _definitionAttribute;
        private string _conjunctiveAdverb, _testString;

        #endregion

        #region Constructors

        public StepDefinitionTester(Type bindingClass, string testString)
        {
            _bindingClass = bindingClass;
            _originalString = ValidateTestString(testString);
            var definition = EnsureSingleDefinition();
            _stepRegex = definition.Key;
            _stepDefinition = definition.Value;
        }

        #endregion

        #region Private Methods

        [DebuggerStepThrough]
        private void ShouldDo(Action setupFn, Action<Action> testFn)
        {
            setupFn();

            testFn(() => {
                try
                {
                    _stepDefinition.Invoke(null, GetParameters());
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.Unwind();
                }
            });
        }

        private string[] GetParameters()
        {
            return _stepRegex.Match(_testString).Groups.Map<Group, string>(g => g.ToString()).Skip(1).ToArray();
        }

        private string ValidateTestString(string testString)
        {
            var match = STEP_REGEX.Match(testString);

            if (match == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "Test string \"{0}\" does not appear to be valid. Test strings should start with \"Given\", \"When\", or \"Then\".",
                        testString), "testString");
            }

            _conjunctiveAdverb = match.Groups[1].ToString();

            switch (_conjunctiveAdverb)
            {
                case "Given ":
                    _definitionAttribute = typeof(GivenAttribute);
                    break;
                case "When ":
                    _definitionAttribute = typeof(WhenAttribute);
                    break;
                case "Then ":
                    _definitionAttribute = typeof(ThenAttribute);
                    break;
                default:
                    throw new InvalidOperationException(
                        "Somehow the test string didn't start with Given, When, or Then.");
            }

            _testString = testString.Replace(_conjunctiveAdverb, String.Empty);
            return testString;
        }

        private KeyValuePair<Regex, MethodInfo> EnsureSingleDefinition()
        {
            var definitions =
                _bindingClass.GetMethods(BindingFlags.Public | BindingFlags.Static)
                             .Where(t => t.HasAttribute(_definitionAttribute));

            if (!definitions.Any())
            {
                Assert.Fail("Cound not find any step definitions with {0} in binding class {1}.",
                    _definitionAttribute.Name, _bindingClass.FullName);
            }

            var matchingDefinitions = new Dictionary<Regex, MethodInfo>();

            foreach (var definition in definitions)
            {
                var attrs = definition.GetCustomAttributes(_definitionAttribute, false);
                foreach (var attr in attrs)
                {
                    var typedAttr = attr as StepDefinitionBaseAttribute;
                    if (typedAttr != null)
                    {
                        var rgx = new Regex(typedAttr.Regex);
                        if (rgx.IsMatch(_testString))
                        {
                            matchingDefinitions.Add(rgx, definition);
                            break;
                        }
                    }
                }
            }

            if (!matchingDefinitions.Any())
            {
                Assert.Fail("Cound not find any step definitions matching test string \"{0}\" in binding class {1}.",
                    _originalString, _bindingClass.FullName);
            }

            if (matchingDefinitions.Count > 1)
            {
                Assert.Fail("Found {0} definitions in binding class {1} which matched test string \"{2}\":\n{3}",
                    matchingDefinitions.Count, _bindingClass.FullName,
                    _originalString,
                    String.Join("\n",
                        matchingDefinitions.Map<KeyValuePair<Regex, MethodInfo>, string>(d => d.Value.Name)));
            }

            return matchingDefinitions.First();
        }

        #endregion

        #region Exposed Methods

        public bool ShallPass(params Action[] setupFns)
        {
            foreach (var setupFn in setupFns)
            {
                try
                {
                    ShouldDo(setupFn, MyAssert.DoesNotThrow<AssertionException>);
                }
                catch (Exception e)
                {
                    throw new AssertFailedException("Setup expected to pass did not pass: " + e.Message, e);
                }
            }

            return true;
        }

        public bool NoneShallPass(params Action[] setupFns)
        {
            foreach (var setupFn in setupFns)
            {
                try
                {
                    ShouldDo(setupFn, MyAssert.Throws<AssertionException>);
                }
                catch (Exception e)
                {
                    throw new AssertFailedException("Setup expected to fail threw exception instead: " + e.Message, e);
                }
            }

            return true;
        }

        #endregion
    }
}
