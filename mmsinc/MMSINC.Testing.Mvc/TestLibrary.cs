using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing.ClassExtensions.StringExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Testing
{
    public static class TestLibrary
    {
        #region Constants

        public struct ControllerActions
        {
            public const string CREATE = "Create",
                                DELETE = "Delete",
                                DESTROY = "Destroy",
                                EDIT = "Edit",
                                INDEX = "Index",
                                NEW = "New",
                                SHOW = "Show",
                                SEARCH = "Search",
                                UPDATE = "Update";
        }

        public static readonly string[] DestructiveControllerActions = new[] {
            ControllerActions.CREATE, ControllerActions.DELETE,
            ControllerActions.DESTROY, ControllerActions.EDIT,
            ControllerActions.NEW, ControllerActions.UPDATE
        };

        public static readonly Type[] HTTP_METHODS = new[] {
            typeof(HttpDeleteAttribute), typeof(HttpGetAttribute),
            typeof(HttpPostAttribute), typeof(HttpPutAttribute)
        };

        public struct MigrationRegExes
        {
            public static readonly string FILE_NAME = @"\[Migration\((\d{14})\)\]",
                                          CLASS_NAME = @"public class (\w+) : Migration";
        }

        //TODO: Permits and Contractors - for RequiresSecureForm
        public static readonly string[] IGNORED_ASSEMBLIES = {"Permits,", "Contractors,"};

        #endregion

        #region Test Methods

        #region View Models

        private static IEnumerable<Type> GetViewModelTypes(Assembly underTest)
        {
            var viewModels = underTest
               .GetClassesByCondition(
                    t =>
                        t.IsSubclassOfRawGeneric(typeof(ViewModel<>)) &&
                        !t.IsAbstract);

            if (!viewModels.Any())
            {
                Assert.Fail(
                    "No concrete classes inheriting from ViewModel<TEntity> seem to exist in assembly '{0}'.",
                    underTest);
            }

            return viewModels;
        }

        public static void
            TestAllConcreteViewModelDerivedClassesHavePublicParameterlessConstructor
            (Assembly underTest)
        {
            var viewModels = GetViewModelTypes(underTest);

            foreach (var viewModel in viewModels)
            {
                Assert.IsTrue(viewModel.HasParameterlessConstructor(),
                    String.Format(
                        "'{0}' must have a public parameterless constructor in order to be used with MVC.",
                        viewModel.FullName));
            }
        }

        /// <summary>
        /// If we have a need for one, add it to the ignored types in your calling method
        /// and decorate the default one with the [DefaultConstructor] Attribute
        /// </summary>
        public static void TestAllConcreteViewModelsHaveOnlyASingleConstructorWithAContainerArgument(Assembly assemblies, IEnumerable<Type> ignoredTypes = null)
        {
            var viewModels = GetViewModelTypes(assemblies).Except(ignoredTypes ?? Enumerable.Empty<Type>());
            var badViewModels = new StringBuilder();
            foreach (var vm in viewModels)
            {
                if (vm.GetConstructors().Count(x => x.GetParameters().Any(p => p.ParameterType == typeof(IContainer))) > 1)
                {
                    badViewModels.AppendLine($"You had more than one constructor in {vm.Name} with the parameter type IContainer");
                }
            }

            if (badViewModels.Length > 0)
            {
                Assert.Fail(badViewModels.ToString());
            }
        }

        private static AutoMapAttribute GetAutoMapAttribute(PropertyInfo prop)
        {
            try
            {
                return
                    prop.GetCustomAttributes<AutoMapAttribute>(false).SingleOrDefault() ??
                    prop.GetCustomAttributes<AutoMapAttribute>(true).SingleOrDefault();
            }
            catch (AmbiguousMatchException e)
            {
                throw new InvalidOperationException(
                    $"{prop.DeclaringType}.{prop.Name} has multiple attributes derived from AutoMapAttribute. There can be only one.",
                    e);
            }
        }

        private static bool EntityHasAccessibleProperty(Type entityType, string propertyName)
        {
            // propertyName might be nested, so we need to account for that by returning the expected nested property.

            if (propertyName.Contains("."))
            {
                // I'm lazy and frustrated getting this test working right now. This will throw
                // an exception if a property can't be found.
                var dpa = new DeepPropertyAccessor(entityType, propertyName);
                return true;
            }

            var prop = entityType.IsInterface
                ? entityType.GetPropertyFromInterface(propertyName)
                : entityType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            return prop != null;
        }

        public static void TestAllViewModelPropertiesHaveMatchingPropertyOnEntity(Assembly underTest,
            IEnumerable<Type> ignoredTypes = null)
        {
            // This test exists to catch when properties get removed from an entity but not the view model.
            // While unit tests/regression tests *should* catch this, we aren't perfect. Example: When we replaced
            // the Contractors.Data entities with the MapCall.Common ones, the WorkOrder entity was missing a few
            // properties. The site tests continued to pass, but a few properties were not being set anymore
            // because the MapCall.Common entity was missing some of the properties referenced by the ViewModel.

            var badProperties = new StringBuilder();
            var viewModels = GetViewModelTypes(underTest).Except(ignoredTypes ?? Enumerable.Empty<Type>());
            var errors = 0;
            foreach (var viewModel in viewModels)
            {
                var entityType = GetEntityTypeForViewModel(viewModel);

                // Get all public properties. ViewModel mapping only works with public instance properties.
                var propertyFlags = BindingFlags.Instance | BindingFlags.Public;
                ;
                // var entityProperties = entityType.GetProperties(propertyFlags).ToDictionary(x => x.Name, x => x);

                // This is not a dictionary because some view models use the "new" keyword to override properties for some reason.
                // ex: ReplaceHydrant.Town in MVC.
                var viewModelProps = viewModel.GetProperties(propertyFlags);

                foreach (var vmProp in viewModelProps)
                {
                    var mapAttribute = GetAutoMapAttribute(vmProp);
                    var entityPropertyName = mapAttribute?.SecondaryPropertyName ?? vmProp.Name;

                    if (!EntityHasAccessibleProperty(entityType, entityPropertyName))
                        //if (!entityProperties.ContainsKey(entityPropertyName))
                    {
                        if (mapAttribute == null || mapAttribute.Direction != MapDirections.None)
                        {
                            badProperties.AppendLine(
                                $"'{viewModel.Name}.{vmProp.Name}' exists but '{entityType.FullName}' does not have this property.");
                            errors++;
                        }
                    }
                }
            }

            if (badProperties.Length > 0)
            {
                Assert.Fail("There was " + errors + " error(s)! " +
                            "At least one view model includes a property that does not exist on the parent entity. " +
                            "If this is intentional, ensure the view model property has the DoesNotAutoMapAttribute on it or the AutoMap/EntityMap attribute has MapDirection.None set. " +
                            "If unintentional, ensure the property names are both identical or that the SecondaryPropertyName value is correct. "
                            + Environment.NewLine + badProperties);
            }
        }

        private static Type GetEntityTypeForViewModel(Type viewModelType)
        {
            if (!viewModelType.IsGenericType)
            {
                return GetEntityTypeForViewModel(viewModelType.BaseType);
            }

            return viewModelType.GenericTypeArguments.Single();
        }

        #endregion

        #region NHibernate Maps

        private static IEnumerable<Type> GetMapTypes(Assembly underTest)
        {
            var types = underTest
               .GetClassesByCondition(
                    t =>
                        t.IsSubclassOfRawGeneric(typeof(ClassMap<>)) &&
                        !t.IsAbstract);

            if (!types.Any())
            {
                Assert.Fail(
                    "No concrete classes inheriting from ClassMap<TEntity> seem to exist in assembly '{0}'.",
                    underTest);
            }

            return types;
        }

        public static void TestAllNHibernateMapsHaveNullableSettingsConsistentWithTheMappedPropertyType(Assembly ass)
        {
            // If a property is mapped as Nullable, but a non-nullable value type is used, NHibernate will save the default(T) 
            // value for the property the moment Session.Flush() is called because it will see it as a value change(from null to, say, 0 
            // in the case of a int) if the original value was null.
            var privBinding = BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance;
            var types = GetMapTypes(ass);
            var sb = new StringBuilder();
            var count = 0;
            foreach (var mapType in types)
            {
                var instance = Activator.CreateInstance(mapType);
                var props = ((IEnumerable)MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions
                                                .GetHiddenPropertyValueByName(instance, "Properties"))
                   .Cast<PropertyPart>();
                foreach (var propVal in props)
                {
                    var pType = propVal.GetType();

                    var columnAttributes =
                        (AttributeStore)pType.GetField("columnAttributes", privBinding).GetValue(propVal);
                    var attributes = (AttributeStore)pType.GetField("attributes", privBinding).GetValue(propVal);
                    var member = (Member)pType.GetField("member", privBinding).GetValue(propVal);

                    // This test can only work with actual value types. Strings and foreign key refs can't be tested.
                    if (!member.PropertyType.IsValueType)
                    {
                        continue;
                    }

                    // Formulas are read-only so they don't have an effect on nhibernate's change tracking, so they can be ignored.
                    if (attributes.Get("Formula") != null)
                    {
                        continue;
                    }

                    var notNullable = (bool?)columnAttributes.Get("NotNull");

                    //Console.WriteLine("{0} - {1}", member.MemberInfo.ReflectedType.Name + "." + member.Name, notNullable);

                    // It seems it gets mapped as nullable/not-nullable based on if the value type is nullable when
                    // it isn't explicitly set in the mapping. I can't 100% prove that though.
                    if (notNullable == null)
                    {
                        continue;
                    }

                    if (notNullable.GetValueOrDefault())
                    {
                        if (member.PropertyType.IsNullable())
                        {
                            sb.AppendFormat(
                                   "{0}.{1} is mapped as not-nullable, but the entity property has a nullable value type.",
                                   member.MemberInfo.ReflectedType.FullName, member.Name)
                              .AppendLine();
                            count++;
                        }
                    }
                    else
                    {
                        if (!member.PropertyType.IsNullable())
                        {
                            sb.AppendFormat(
                                   "{0}.{1} is mapped as nullable, but the entity property has a non-nullable value type.",
                                   member.MemberInfo.ReflectedType.FullName, member.Name)
                              .AppendLine();
                            count++;
                        }
                    }
                }
            }

            if (count > 0)
            {
                Assert.Fail("{0} fails found. {1}", count, sb);
            }
        }

        #endregion

        #region Repositories

        public static void TestAllRepositoriesHaveAUnitTest(
            Assembly dataAssembly, Assembly testAssembly, string testNamespace,
            Func<Type, bool> extraDataAssemblyFilter = null)
        {
            TestAllThingsHaveAUnitTest(dataAssembly, testAssembly,
                typeof(RepositoryBase<>), testNamespace, "repositories",
                extraDataAssemblyFilter);
        }

        #endregion

        #region Controllers

        public static void TestAllControllersHaveAUnitTest(Assembly webSiteAssembly, Assembly testAssembly,
            string testNamespace, Type baseType = null)
        {
            baseType = baseType ?? typeof(Controller);
            TestAllThingsHaveAUnitTest(webSiteAssembly, testAssembly,
                baseType, testNamespace, "controllers",
                t => t.IsVisible && !t.IsNested && !t.IsGenericType);
        }

        public static void TestClassesInAssemblyNamespaceUseTheBaseClass(Assembly targetAssembly, string testNamespace,
            Type baseType, Type[] skipControllers)
        {
            var testClasses = targetAssembly.GetClassesByCondition(t => {
                return t.HasAttribute(typeof(TestClassAttribute)) && t.Namespace.StartsWith(testNamespace);
            });

            var failures = new List<string>();

            testClasses.Each(c => {
                if (!(c.BaseType.IsSubclassOfRawGeneric(baseType)) && !skipControllers.Contains(c))
                {
                    failures.Add(c.Name);
                }
            });

            if (failures.Count > 0)
            {
                Assert.Fail("The following classes do not use the proper base class - {0} :\r\n{1}", baseType,
                    String.Join("\r\n", failures));
            }
        }

        public static void TestRESTfulCRUDMethods(Assembly toCheck, Type baseType = null,
            Func<Type, bool> extraFilter = null, Dictionary<Type, string[]> skippableControllerActions = null)
        {
            new RESTFulCRUDTestHelper(toCheck, baseType, extraFilter, skippableControllerActions).DoTest();
        }

        public static void TestAllControllersInheritFromCustomControllerBase(Assembly ass)
        {
            // ReSharper disable RedundantNameQualifier
            var baseControllerClass = typeof(System.Web.Mvc.ControllerBase);
            var baseCustomControllerClass = typeof(MMSINC.Controllers.ControllerBase);
            // ReSharper restore RedundantNameQualifier

            var testClasses = ass.GetClassesByCondition(t => { return baseControllerClass.IsAssignableFrom(t); });

            if (!testClasses.Any())
            {
                Assert.Fail("Unable to find any controllers in the given assembly. {0}", ass.FullName);
            }

            var failures = new List<string>();

            testClasses.Each(c => {
                if (!baseCustomControllerClass.IsAssignableFrom(c))
                {
                    failures.Add(c.FullName);
                }
            });

            if (failures.Any())
            {
                Assert.Fail("The following classes do not use the proper base class - {0} :\r\n{1}",
                    baseCustomControllerClass.FullName, String.Join("\r\n", failures));
            }
        }

        #endregion

        #region Migrations

        public static void TestAllMigrationsAreNamedCorrectly(string path)
        {
            var timeStampRegEx = new Regex(MigrationRegExes.FILE_NAME);
            var classNameRegEx = new Regex(MigrationRegExes.CLASS_NAME);
            var failures = new List<string>();
            var di = new DirectoryInfo(path);
            if (!di.Exists)
                Assert.Fail("Migrations folder does not exist: {0}", path);

            foreach (var file in di.GetFiles("*.cs"))
            {
                string line, className = "", timeStamp = "";
                using (var stream = file.OpenRead())
                using (var reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        var match = timeStampRegEx.Match(line);
                        if (match.Length > 1)
                            timeStamp = match.Groups[1].Value;
                        match = classNameRegEx.Match(line);
                        if (match.Length > 1)
                        {
                            className = match.Groups[1].Value;
                            break;
                        }
                    }
                }

                if (String.IsNullOrWhiteSpace(className))
                    failures.Add(String.Format("Could not find class name in file : {0}", file.Name));
                if (String.IsNullOrWhiteSpace(timeStamp))
                    failures.Add(String.Format("Could not find time stamp in file : {0}", file.Name));
                if (String.Format("{0}_{1}.cs", timeStamp, className) != file.Name)
                    failures.Add(String.Format("File: {0} should be {1}_{2}.cs", file.Name, timeStamp, className));
            }

            if (failures.Count > 0)
                Assert.Fail("The following errors occurred with the migration files: {0}", String.Join(", ", failures));
        }

        #endregion

        #endregion

        #region Private Methods

        private static void TestAllThingsHaveAUnitTest(Assembly targetAssembly,
            Assembly testAssembly, Type thingType, string testNamespace,
            string thingName, Func<Type, bool> extraTargetAssemblyFilter = null,
            Func<Type, bool> extraTestAssemblyFilter = null)
        {
            extraTargetAssemblyFilter = extraTargetAssemblyFilter ?? (t => true);
            extraTestAssemblyFilter = extraTestAssemblyFilter ?? (t => true);
            var things = targetAssembly
               .GetClassesByCondition(
                    t => {
                        return t.IsSubclassOfRawGeneric(thingType) &&
                               extraTargetAssemblyFilter(t) &&
                               !t.IsAbstract;
                    });
            Assert.AreNotEqual(0, things.Count(), "No classes were found to test.");
            var tests = testAssembly
               .GetClassesByCondition(
                    t =>
                        t.Namespace != null &&
                        t.Namespace.StartsWith(testNamespace) &&
                        t.HasAttribute<TestClassAttribute>() &&
                        extraTestAssemblyFilter(t));
            var failures = new List<string>();

            things.Each(thing => {
                if (
                    !(from t in tests
                      where t.Name == thing.Name.ToTestName()
                      select t).Any())
                {
                    failures.Add(thing.Name);
                }
            });

            if (failures.Count > 0)
            {
                Assert.Fail(
                    "All {0} should have corresponding unit tests. The following {0} do not: {1}",
                    thingName, String.Join(", ", failures));
            }
        }

        #endregion

        #region Nested Classes

        internal class RESTFulCRUDTestHelper
        {
            #region Constants

            public static readonly Type DEFAULT_BASE_TYPE =
                typeof(Controller);

            #endregion

            #region Private Members

            private readonly Type _baseType;
            private readonly Assembly _toCheck;
            private readonly Func<Type, bool> _extraControllerFilter;
            private readonly Dictionary<Type, string[]> _skippableControllerActions;
            private IEnumerable<Type> _controllers;

            #endregion

            #region Properties

            public IEnumerable<Type> Controllers
            {
                get
                {
                    return _controllers ??
                           (_controllers =
                               _toCheck.GetClassesByCondition(
                                   t =>
                                       t.IsSubclassOfRawGeneric(_baseType) &&
                                       _extraControllerFilter(t)));
                }
            }

            #endregion

            #region Constructors

            public RESTFulCRUDTestHelper(Assembly toCheck, Type baseType = null,
                Func<Type, bool> extraFilter = null, Dictionary<Type, string[]> skippableControllerActions = null)
            {
                _baseType = baseType ?? DEFAULT_BASE_TYPE;
                _toCheck = toCheck;
                _extraControllerFilter = extraFilter ?? (t => true);
                _skippableControllerActions = skippableControllerActions ?? new Dictionary<Type, string[]>();
            }

            #endregion

            #region Exposed Methods

            public void DoTest()
            {
                if (!Controllers.Any())
                {
                    Assert.Fail(
                        "No concrete classes inheriting from ControllerBase<TEntity> seem to exist in assembly '{0}'.",
                        _toCheck);
                }

                foreach (var controller in Controllers)
                {
                    RunMethodAssertions(controller);
                }
            }

            #endregion

            #region Private Methods

            private bool CanAssertForAction(Type controller, string actionName)
            {
                if (_skippableControllerActions.ContainsKey(controller))
                {
                    return !_skippableControllerActions[controller].Contains(actionName);
                }

                return true;
            }

            private void RunMethodAssertions(Type controller)
            {
                controller
                   .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                   .Where(m => !new Regex("^(g|s)et_").IsMatch(m.Name))
                   .Where(m => CanAssertForAction(controller, m.Name))
                   .Each(method => {
                        switch (method.Name)
                        {
                            case ControllerActions.CREATE:
                                RunCreateAssertions(method);
                                break;
                            case ControllerActions.DELETE:
                                RunDeleteAssertions(method);
                                break;
                            case ControllerActions.DESTROY:
                                RunDestroyAssertions(method);
                                break;
                            case ControllerActions.EDIT:
                                RunEditAssertions(method);
                                break;
                            case ControllerActions.INDEX:
                                RunIndexAssertions(method);
                                break;
                            case ControllerActions.NEW:
                                RunNewAssertions(method);
                                break;
                            case ControllerActions.SEARCH:
                                RunSearchAssertions(method);
                                break;
                            case ControllerActions.SHOW:
                                RunShowAssertions(method);
                                break;
                            case ControllerActions.UPDATE:
                                RunUpdateAssertions(method);
                                break;
                            //default:
                            //    Debug.Print("Action '{0}' Controller '{1}'", method.Name, method.DeclaringType);
                            //    break;
                        }
                    });
            }

            private void RunSearchAssertions(MethodInfo method)
            {
                OnlyAllow<HttpGetAttribute>(method);
                var parameters = method.GetParameters();
                if (parameters.Length > 1)
                {
                    Assert.Fail("Controller 'Search' action should only accept 1 parameter at most. controller: {0}",
                        method.DeclaringType);
                }
            }

            private void RunShowAssertions(MethodInfo method)
            {
                OnlyAllow<HttpGetAttribute>(method);
                OnlyAllowSingleIntParameter(method);
            }

            private void RunIndexAssertions(MethodInfo method)
            {
                OnlyAllow<HttpGetAttribute>(method);
            }

            private void RunDestroyAssertions(MethodInfo method)
            {
                OnlyAllow(method, new[] {typeof(HttpDeleteAttribute)});
                // RequireSecureForm(method);
            }

            private void RunDeleteAssertions(MethodInfo method)
            {
                OnlyAllow<HttpGetAttribute>(method);
                OnlyAllowSingleIntParameter(method);
            }

            private void RunUpdateAssertions(MethodInfo method)
            {
                OnlyAllow<HttpPostAttribute>(method);
                // RequireSecureForm(method);
                var parameters = method.GetParameters();
                Assert.IsTrue(parameters.Length > 0,
                    "The 'Update' action on a controller should have at least one parameter. controller: '{0}'",
                    method.DeclaringType);
            }

            private void RunEditAssertions(MethodInfo method)
            {
                try
                {
                    OnlyAllow<HttpGetAttribute>(method);
                }
                catch (AssertFailedException ex)
                {
                    if (method.HasAttribute<HttpPostAttribute>() ||
                        method.HasAttribute<HttpPutAttribute>())
                    {
                        throw new AssertFailedException(
                            "The corresponding POST/PUT action for an 'Edit' is 'Update'. Perhaps the method is misnamed? " +
                            ex.Message, ex);
                    }

                    throw;
                }

                OnlyAllowSingleIntParameter(method);
            }

            private void RunNewAssertions(MethodInfo method)
            {
                OnlyAllow<HttpGetAttribute>(method);
            }

            private void RunCreateAssertions(MethodInfo method)
            {
                OnlyAllow<HttpPostAttribute>(method);
                //RequireSecureForm(method);
            }

            private static void OnlyAllowSingleIntParameter(MethodInfo method)
            {
                var parameters = method.GetParameters();
                Assert.AreEqual(1, parameters.Length,
                    "The '{0}' action a controller should accept a single int parameter. controller: '{1}'",
                    method.Name, method.DeclaringType);
                Assert.AreEqual(typeof(int), parameters[0].ParameterType,
                    "The '{0}' action a controller should accept a single int parameter. controller: '{1}'",
                    method.Name, method.DeclaringType);
            }

            private void OnlyAllow<TAttribute>(MethodInfo method)
                where TAttribute : ActionMethodSelectorAttribute
            {
                OnlyAllow(method, new[] {typeof(TAttribute)});
            }

            //private void RequireSecureForm(MethodInfo method)
            //{
            //    foreach (var assembly in IGNORED_ASSEMBLIES)
            //        if (_toCheck.FullName.StartsWith(assembly))
            //            return;
            //    MyAssert.MethodHasAttribute<RequiresSecureFormAttribute>(method);
            //}

            private void OnlyAllow(MethodInfo method, Type[] actions)
            {
                foreach (var action in actions)
                {
                    MyAssert.MethodHasAttribute(method, action);
                }

                foreach (var attribute in HTTP_METHODS)
                {
                    if (actions.Contains(attribute)) continue;
                    MyAssert.MethodDoesNotHaveAttribute(method, attribute);
                }
            }

            #endregion
        }

        #endregion
    }
}
