using System;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Utility.Permissions
{
    [TestClass]
    public class ModulePermissionsHelperTest
    {
        private Type _expectedType = typeof(TestStaticApplicationClass);
        private string _expectedApplicationname = "Test Static Application Class";

        private IModulePermissions _expectedAlreadySetModPerm =
            TestStaticApplicationClass.AlreadySet;

        [TestInitialize]
        public void InitializeModulePermissionsInitializerTest()
        {
            ModulePermissionsHelper.InitType(_expectedType);
        }

        #region Dilbert tests

        [TestMethod]
        public void TestDilbertFieldIsNotNull()
        {
            Assert.IsNotNull(TestStaticApplicationClass.Dilbert);
        }

        [TestMethod]
        public void TestDilbertFieldHasExpectedApplication()
        {
            Assert.AreEqual(_expectedApplicationname, TestStaticApplicationClass.Dilbert.Application);
        }

        [TestMethod]
        public void TestDilbertFieldHasExpectedModule()
        {
            Assert.AreEqual("Dilbert", TestStaticApplicationClass.Dilbert.Module);
        }

        #endregion

        #region AlreadySet tests

        [TestMethod]
        public void TestAlreadySetFieldIsNotNull()
        {
            Assert.IsNotNull(TestStaticApplicationClass.AlreadySet);
        }

        [TestMethod]
        public void TestAlreadySetFieldWasNotChangedByInitType()
        {
            Assert.AreSame(_expectedAlreadySetModPerm,
                TestStaticApplicationClass.AlreadySet);
        }

        #endregion

        #region AFieldWithMultipleWords tests

        [TestMethod]
        public void TestAFieldWithMultipleWordsGetsModuleNameSplitUp()
        {
            var expected = "A Field With Multiple Words";

            Assert.AreEqual(expected,
                TestStaticApplicationClass.AFieldWithMultipleWords.Module);
        }

        #endregion

        #region TestStaticApplicationClassWithAttribute tests

        [TestMethod]
        public void TestGetApplicationNameFromTypeRoleApplicationNameAttributeWhenItExists()
        {
            var expected = "Piece Of Junk";
            Assert.AreEqual(expected,
                ModulePermissionsHelper.GetApplicationNameFromType(typeof(TestStaticApplicationClassWithAttribute)));
        }

        #endregion

        #region GetFields

        [TestMethod]
        public void TestGetFieldsGetsStaticPublicFields()
        {
            var expectedFieldCount = 3;
            var expectedFieldNames = new[] {
                "Dilbert",
                "AlreadySet",
                "AFieldWithMultipleWords"
            };

            var fields = ModulePermissionsHelper.GetIModulePermissionFields(_expectedType);
            Assert.AreEqual(expectedFieldCount, fields.Count());

            var fieldNames = (from f in fields select f.Name);
            foreach (var name in expectedFieldNames)
            {
                Assert.IsTrue(fieldNames.Contains(name));
            }
        }

        [TestMethod]
        public void TestGetFieldsDoesNotReturnNonPublicfields()
        {
            var fields = ModulePermissionsHelper.GetIModulePermissionFields(_expectedType);

            var fieldNames = (from f in fields select f.Name);

            Assert.IsFalse(fieldNames.Contains("APrivateField"));
        }

        [TestMethod]
        public void TestGetFieldsReturnsOnlyFieldsWithIModulePermissionsFieldTypes()
        {
            var fields = ModulePermissionsHelper.GetIModulePermissionFields(_expectedType);

            var impType = typeof(IModulePermissions);
            foreach (var f in fields)
            {
                Assert.AreEqual(impType, f.FieldType);
            }
        }

        #endregion

        #region GetApplicationAndModuleNames

        [TestMethod]
        public void TestGetApplicationAndModuleNamesDoesThingsCorrectly()
        {
            var test = "TestStaticApplicationClass.Dilbert";
            var expectedAppName = "TestStaticApplicationClass";
            var expectedModFieldName = "Dilbert";

            var result =
                ModulePermissionsHelper.GetApplicationAndModuleNames(test);

            Assert.AreEqual(expectedAppName, result.Item1);
            Assert.AreEqual(expectedModFieldName, result.Item2);
        }

        [TestMethod]
        public void TestGetApplicationAndModuleNamesThrowsForNullArguments()
        {
            MyAssert.Throws<NullReferenceException>(() => ModulePermissionsHelper.GetApplicationAndModuleNames(null));
            MyAssert.Throws<NullReferenceException>(() =>
                ModulePermissionsHelper.GetApplicationAndModuleNames(string.Empty));
            MyAssert.Throws<NullReferenceException>(() =>
                ModulePermissionsHelper.GetApplicationAndModuleNames("     "));
        }

        [TestMethod]
        public void TestGetApplicationAndModuleNamesThrowsForInvalidNameArguments()
        {
            MyAssert.Throws(() => ModulePermissionsHelper.GetApplicationAndModuleNames("IDontWorkWithoutAPeriod"));
            MyAssert.Throws(() => ModulePermissionsHelper.GetApplicationAndModuleNames("Multiple.Periods.Lose"));
        }

        #endregion

        #region FindModulePermissionsByName

        [TestMethod]
        public void TestFindModulePermissionsByNameReturnsExpectedIModulePermissionsObject()
        {
            ModulePermissionsHelper.InitType(typeof(FieldServices));
            var expected = FieldServices.WorkManagement;
            // Need to use FieldServices at the moment because the namespace is hardcoded in. 
            var result = ModulePermissionsHelper.GetModulePermissionsByName("FieldServices.WorkManagement");

            Assert.IsNotNull(expected);
            Assert.IsNotNull(result);
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void TestTestFindModulePermissionsByNameThrowsExceptionIfCantFindField()
        {
            ModulePermissionsHelper.InitType(typeof(FieldServices));
            MyAssert.Throws<InvalidOperationException>(() =>
                ModulePermissionsHelper.GetModulePermissionsByName("FieldServices.IDONOTEXIST!"));
        }

        #endregion

        #region GetTypeByName

        [TestMethod]
        public void TestGetTypeByNameThrowsExceptionForInvalidType()
        {
            MyAssert.Throws<TypeLoadException>(
                () => ModulePermissionsHelper.GetTypeByName("BOOGAH BOOGAH!"));
        }

        #endregion
    }

    #region Test Classes

    public static class TestStaticApplicationClass
    {
        public static readonly IModulePermissions Dilbert;
        public static readonly IModulePermissions AlreadySet = new ModulePermissions("Application", "Module");
        public static readonly IModulePermissions AFieldWithMultipleWords;
        private static readonly IModulePermissions APrivateField;
        public static readonly object AFieldThatIsntIModulePermissions;
    }

    [RoleApplicationName("Piece Of Junk")]
    public static class TestStaticApplicationClassWithAttribute
    {
        public static readonly IModulePermissions SomeField;
    }

    #endregion
}
