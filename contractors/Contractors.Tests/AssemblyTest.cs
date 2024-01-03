using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Controllers.WorkOrder;
using Contractors.Data.DesignPatterns.Mvc;

using Contractors.Data.Models.Repositories;
using MMSINC.Data;
using MMSINC.Testing;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Tests
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class AssemblyTest
    {
        #region Constants

        public struct Types
        {
            public static readonly Type VIEW_MODEL = typeof(ViewModel<>),
                                        REPOSITORY_BASE = typeof(RepositoryBase<>),
                                        SECURED_REPOSITORY_BASE = typeof(SecuredRepositoryBase<,>),
                                        CONTROLLER_BASE = typeof(Data.DesignPatterns.Mvc.ControllerBase<>),
                                        CONTROLLER_BASE_2 = typeof(Data.DesignPatterns.Mvc.ControllerBase<,>),
                                        CONTROLLER_BASE_WITH_VALIDATION = typeof(ControllerBaseWithValidation<>),
                                        CONTROLLER_BASE_WITH_VALIDATION_2 = typeof(ControllerBaseWithValidation<,>),
                                        SAP_CONTROLLER_BASE_WITH_VALIDATION = typeof(SapSyncronizedControllerBaseWithValidation<,>),
                                        SAP_CONTROLLER_BASE_WITH_VALIDATION_2 = typeof(SapControllerWithValidationBase<,>),
                                        OBJECT = typeof(object),
                                        STRING = typeof(string),
                                        STRING_LENGTH_ATTRIBUTE = typeof(StringLengthAttribute),
                                        DATA_TYPE_ATTRIBUTE = typeof(DataTypeAttribute);
        }

        public struct Assemblies
        {
            public static readonly Assembly CONTRACTORS =
                Assembly.GetAssembly(typeof(UserController)),
                                            CONTRACTORS_DATA =
                                                Assembly.GetAssembly(typeof(AsBuiltImageRepository)),
                                            CONTRACTORS_TESTS = Assembly.GetAssembly(typeof(AssemblyTest));
        }

        // special controller types that don't inherit from the regular controller base class
        public static readonly Type[] SPECIAL_CONTROLLER_TYPES = new[] {
            typeof(AuthenticationController),
            typeof(ErrorController),
            typeof(HomeController), typeof(SapSyncronizedControllerBaseWithValidation<,>)
        };

        #endregion

        #region Private Members

        // these aren't meant to be used, they're just so that the necessary
        // they come from get copied over.  if you have tests that pass in
        // visual studio but fail in TC and from the command line, you might
        // need these in your project also.
        #pragma warning disable 169
        private System.Data.SQLite.SQLiteException _doNotUseThisException;
        private log4net.LogManager _doNotUsethisLogManager;
        #pragma warning restore 169

        #endregion

        #region Private Methods

        private static IEnumerable<Type> GetRepositoryBaseDerivedTypes()
        {
            return Assemblies
                .CONTRACTORS_DATA
                .GetClassesByCondition(
                    t =>
                        t.IsSubclassOfRawGeneric(Types.REPOSITORY_BASE) &&
                            t != Types.SECURED_REPOSITORY_BASE);
        }

        private IEnumerable<Type> GetControllerBaseDerivedTypesTypes(Func<Type, bool> predicate = null)
        {
            if (predicate == null)
            {
                predicate = t => true;
            }
            return Assemblies.CONTRACTORS.GetClassesByCondition(
                t =>
                    t != Types.CONTROLLER_BASE && t != Types.CONTROLLER_BASE_2 && t.Namespace != null &&
                        t.Namespace.Contains("Contractors.Controllers") && t.IsVisible && !t.IsNested && predicate(t));
        }

        #endregion

        #region Repository Tests

        [TestMethod]
        public void TestAllRepositoriesAreSecuredExceptForAuthenticationAndAuthenticationLog()
        {
            var types = GetRepositoryBaseDerivedTypes();
            Assert.IsTrue(types.Count() > 1);

            var exclude = new[] {
                typeof(AuthenticationRepository),
                typeof(AuthenticationLogRepository),
            };

            foreach (var t in types)
            {
                if (exclude.Contains(t))
                {
                    continue;
                }

                Assert.IsTrue(t.IsSubclassOfRawGeneric(Types.SECURED_REPOSITORY_BASE),
                              String.Format(
                                  "Repository class '{0}' was expected to inherit from SecuredRepositoryBase, but does not.",
                                  t));
            }
        }

        [TestMethod]
        public void TestAllRepositoriesShouldHaveAUnitTest()
        {
            TestLibrary.TestAllRepositoriesHaveAUnitTest(
                Assemblies.CONTRACTORS_DATA,
                Assemblies.CONTRACTORS_TESTS,
                "Contractors.Tests.Models.Repositories",
                t => {
                   return t != Types.SECURED_REPOSITORY_BASE;
                });
        }

        #endregion

        #region Controller Tests

        [TestMethod]
        public void TestAllControllersShouldInheritFromContractorsControllerBase()
        {
            var controllers = GetControllerBaseDerivedTypesTypes();
            Assert.IsTrue(controllers.Count() > 1);

            foreach (var controller in controllers)
            {
                if (SPECIAL_CONTROLLER_TYPES.Contains(controller)) continue;

                Assert.IsTrue(
                    controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE) ||
                    controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE_2) ||
                    controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE_WITH_VALIDATION) ||
                    controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE_WITH_VALIDATION_2) ||
                    controller.IsSubclassOfRawGeneric(Types.SAP_CONTROLLER_BASE_WITH_VALIDATION) ||
                    controller.IsSubclassOfRawGeneric(Types.SAP_CONTROLLER_BASE_WITH_VALIDATION_2),
                    "Type '{0}' was not found to be a subclass of type '{1}' or '{2}'",
                    controller,
                    Types.CONTROLLER_BASE, Types.CONTROLLER_BASE_2);

                // special case
                if (controller == typeof(WorkOrderControllerBase<>)) continue;

                MyAssert.Matches(new Regex("Controller|BaseWithValidation$"), controller.Name,
                    "All controller class names should end with 'Controller'.");
            }
        }

        [TestMethod]
        public void TestAllControllersWithDestructiveActionsShouldInheritFromControllerBaseWithValidation()
        {
            var controllers = GetControllerBaseDerivedTypesTypes().Where(HasDestructiveActions);
            var failures = new List<string>();

            foreach (var controller in controllers)
            {
                if (controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE_WITH_VALIDATION) ||
                    controller.IsSubclassOfRawGeneric(Types.CONTROLLER_BASE_WITH_VALIDATION_2) ||
                    controller.IsSubclassOfRawGeneric(Types.SAP_CONTROLLER_BASE_WITH_VALIDATION) ||
                    controller.IsSubclassOfRawGeneric(Types.SAP_CONTROLLER_BASE_WITH_VALIDATION_2))
                {
                    continue;
                }
                failures.Add(controller.Name);
            }

            if (failures.Count > 0)
            {
                Assert.Fail(
                    "All controllers with destructive actions should inherit from ControllerBaseWithValidation.  The following controllers do not: {0}",
                    String.Join(", ", failures));
            }
        }

        [TestMethod]
        public void TestRESTfulCRUDMethods()
        {
            var ignorableControllers = new HashSet<Type>();
            ignorableControllers.Add(typeof(UserController));
            ignorableControllers.Add(typeof(ServiceController));
            TestLibrary
                .TestRESTfulCRUDMethods(Assemblies.CONTRACTORS,
                    extraFilter: t => !ignorableControllers.Contains(t));
        }

        [TestMethod]
        public void TestAllControllersShouldHaveAUnitTest()
        {
            TestLibrary.TestAllControllersHaveAUnitTest(
                Assemblies.CONTRACTORS,
                Assemblies.CONTRACTORS_TESTS,
                "Contractors.Tests.Controllers");
        }

        [TestMethod]
        public void TestAllViewModelPropertiesHaveMatchingPropertyOnEntity()
        {
            TestLibrary.TestAllViewModelPropertiesHaveMatchingPropertyOnEntity(Assemblies.CONTRACTORS);
        }

        #region Helper Methods

        private bool HasDestructiveActions(Type t)
        {
            return
                t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(
                        m => m.ReturnType.IsOrIsSubclassOf(typeof(ActionResult)))
                    .Any(
                        m => TestLibrary.DestructiveControllerActions.Contains(m.Name));
        }

        #endregion

        #endregion

        #region Model tests

        private static bool PropertyHasMultilineTextOrPasswordDataTypeAttribute(PropertyInfo prop)
        {
            // DataTypeAttribute can only be applied once on a property, so we can use
            // SingleOrDefault here.
            var dta = prop.GetCustomAttributes<DataTypeAttribute>(true).SingleOrDefault();
            if (dta == null) { return false; }
            return (dta.DataType == System.ComponentModel.DataAnnotations.DataType.MultilineText || dta.DataType == System.ComponentModel.DataAnnotations.DataType.Password);
        }

        private static bool PropertyHasStringLengthAttribute(PropertyInfo prop)
        {
            return prop.GetCustomAttributes<StringLengthAttribute>(true).Any();
        }

        private static bool MemberHasStringLengthNotRequiredAttribute(MemberInfo prop)
        {
            return prop.GetCustomAttributes<StringLengthNotRequiredAttribute>(true).Any();
        }

        private static void TestStringPropertiesForStringLengthAttributeOnTypes(IEnumerable<Type> types)
        {
            var missing = new StringBuilder();
            var count = 0;
            const string missingFormat = "{0}.{1}";
            foreach (var t in types)
            {
                // We can ignore this type.
                if (MemberHasStringLengthNotRequiredAttribute(t)) { continue; }

                foreach (var prop in t.GetProperties())
                {
                    // We only want setters, as readonly properties shouldn't be getting
                    // posted back. 
                    if (!prop.CanWrite) { continue; }

                    // These should be self explanatory
                    if (prop.PropertyType != Types.STRING) { continue; }
                    if (MemberHasStringLengthNotRequiredAttribute(prop)) { continue; }
                    if (PropertyHasStringLengthAttribute(prop)) { continue; }

                    // There wouldn't be a StringLength attribute on a property with
                    // MultilineText, because they'd be mapped to a text/ntext field
                    // which doesn't have a length restriction. If it's set to Password
                    // because we're hashing things that are passwords, string length
                    // doesn't matter either.
                    if (PropertyHasMultilineTextOrPasswordDataTypeAttribute(prop)) { continue; }

                    missing.AppendFormat(missingFormat, t.FullName, prop.Name).AppendLine();
                    count++;
                }
            }
            if (count > 0)
            {
                missing.Insert(0, "There are " + count + " string properties missing a StringLengthAttribute:");
                Assert.Fail(missing.ToString());
            }
        }

        [TestMethod]
        public void TestAllStringPropertiesOnModelsHaveStringLengthAttribute()
        {

            var modelNamespace = typeof(ContractorUser).Namespace;
            // ReSharper disable AssignNullToNotNullAttribute
            var types = Assemblies.CONTRACTORS_DATA.GetClassesByCondition(
                x => !x.IsAbstract && x.Namespace != null &&
                        x.Namespace.StartsWith(modelNamespace));
            // ReSharper restore AssignNullToNotNullAttribute
            TestStringPropertiesForStringLengthAttributeOnTypes(types);
        }
        
        #endregion

        #region Nested Classes

        private class ViewModelWithParameterlessConstructor : ViewModel<object>
        {
            // Shut up, Resharper.
            // ReSharper disable UnusedMember.Local
            public ViewModelWithParameterlessConstructor(IContainer container) : base(container) { }
            // ReSharper restore UnusedMember.Local

        }

        private class BadViewModel : ViewModel<object>
        {
            public BadViewModel(IContainer container) : base(container) { }
        }

        #endregion
    }
}
