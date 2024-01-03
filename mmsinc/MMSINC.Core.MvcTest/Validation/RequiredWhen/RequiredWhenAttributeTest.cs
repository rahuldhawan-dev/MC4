using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.Core.MvcTest.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Validation;
using Moq;
using NHibernate;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Container = StructureMap.Container;
using IContainer = StructureMap.IContainer;

// ReSharper disable once CheckNamespace
namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class RequiredWhenAttributeTest
    {
        #region Fields

        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            // Need to reset this for each test since it's static.
            DynamicTargetValueModel.GetMyValueValue = "this is a value";

            _container = new Container(e => {
                e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
                e.For<ISession>().Use(new Mock<ISession>().Object);
                e.For<IViewModelFactory>().Use<ViewModelFactory>();
            });
        }

        #endregion

        #region Private Methods

        private static void PropertyIsRequired<T, TProp>(T model, Expression<Func<T, TProp>> propExpression)
        {
            // This test needs to bypass the RequiredAttribute check 
            ValidationAssert.PropertyIsRequired(model, propExpression, validationNotDoneByAttribute: true);
        }

        #endregion

        #region Tests

        #region Constructors

        [TestMethod]
        public void TestConstructorDefaultsToEqualToComparisonType()
        {
            var target = new TestRequiredWhenAttribute("Dep", "value");
            Assert.AreEqual(ComparisonType.EqualTo, target.ComparisonType);
        }

        [TestMethod]
        public void TestConstructorForDatesDefaultsToEqualToComparisonType()
        {
            var target = new TestRequiredWhenAttribute("Dep", 2014, 3, 18);
            Assert.AreEqual(ComparisonType.EqualTo, target.ComparisonType);
        }

        [TestMethod]
        public void TestConstructorWithComparisonTypeSetsComparisonType()
        {
            var target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualTo, "blah");
            Assert.AreEqual(ComparisonType.NotEqualTo, target.ComparisonType);
        }

        [TestMethod]
        public void TestConstructorSetsTargetValue()
        {
            var target = new TestRequiredWhenAttribute("Dep", "some value");
            Assert.AreEqual("some value", target.TargetValue);
        }

        [TestMethod]
        public void TestConstructorForDatesSetsTargetValueToParsedDate()
        {
            var target = new TestRequiredWhenAttribute("Dep", 2014, 3, 18);
            Assert.AreEqual(new DateTime(2014, 3, 18), target.TargetValue);
        }

        [TestMethod]
        public void TestConstructorSetsFieldOnlyVisibleWhenRequiredToFalseByDefault()
        {
            var target = new TestRequiredWhenAttribute("Dep", "some value");
            Assert.IsFalse(target.FieldOnlyVisibleWhenRequired);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsIfArrayIsEmpty()
        {
            var target = new TestRequiredWhenAttribute("Dep", new object[] { });
            object thing = null;
            MyAssert.Throws(() => thing = target.TargetValue);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsIfTargetTypeIsArrayButComparisonTypeIsNotValid()
        {
            var comparisonTypes = EnumExtensions.GetValues<ComparisonType>().ToList();
            comparisonTypes.Remove(ComparisonType.EqualToAny);
            comparisonTypes.Remove(ComparisonType.NotEqualToAny);
            Assert.IsTrue(comparisonTypes.Any(), "Sanity check");
            object thing = null;
            var arr = new[] {"blah"};
            TestRequiredWhenAttribute target;

            foreach (var ct in comparisonTypes)
            {
                var cur = ct;
                target = new TestRequiredWhenAttribute("Dep", cur, arr);
                MyAssert.Throws<NotSupportedException>(() => thing = target.TargetValue);
            }

            target = new TestRequiredWhenAttribute("Dep", ComparisonType.EqualToAny, arr);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
            target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualToAny, arr);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsIfComparisonTypeIsMeantForArraysAndTargetTypeIsNotArray()
        {
            object thing;
            var target = new TestRequiredWhenAttribute("Dep", ComparisonType.EqualToAny, "i am not an array");
            MyAssert.Throws<NotSupportedException>(() => thing = target.TargetValue);
            target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualToAny, "i am not an array");
            MyAssert.Throws<NotSupportedException>(() => thing = target.TargetValue);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsForStringTypesWhenComparisonIsNotValidForStrings()
        {
            var comparisonTypes = EnumExtensions.GetValues<ComparisonType>().ToList();
            comparisonTypes.Remove(ComparisonType.EqualTo);
            comparisonTypes.Remove(ComparisonType.NotEqualTo);
            object thing = null;
            TestRequiredWhenAttribute target;

            foreach (var ct in comparisonTypes)
            {
                var cur = ct;
                target = new TestRequiredWhenAttribute("Dep", cur, "");
                MyAssert.Throws<NotSupportedException>(() => thing = target.TargetValue);
            }

            target = new TestRequiredWhenAttribute("Dep", ComparisonType.EqualTo, "");
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
            target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualTo, "");
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsForBoolTypesWhenComparisonIsNotValidForBools()
        {
            var comparisonTypes = EnumExtensions.GetValues<ComparisonType>().ToList();
            comparisonTypes.Remove(ComparisonType.EqualTo);
            comparisonTypes.Remove(ComparisonType.NotEqualTo);
            object thing = null;
            TestRequiredWhenAttribute target;
            foreach (var ct in comparisonTypes)
            {
                var cur = ct;
                target = new TestRequiredWhenAttribute("Dep", cur, true);
                MyAssert.Throws(() => thing = target.TargetValue);
            }

            target = new TestRequiredWhenAttribute("Dep", ComparisonType.EqualTo, true);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);

            target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualTo, true);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
        }

        [TestMethod]
        public void TestTargetValueGetterThrowsForNullTargetValueWhenComparisonTypeIsBadForNulls()
        {
            var comparisonTypes = EnumExtensions.GetValues<ComparisonType>().ToList();
            comparisonTypes.Remove(ComparisonType.EqualTo);
            comparisonTypes.Remove(ComparisonType.NotEqualTo);
            object thing = null;
            TestRequiredWhenAttribute target;

            foreach (var ct in comparisonTypes)
            {
                var cur = ct;
                target = new TestRequiredWhenAttribute("Dep", cur, null);
                MyAssert.Throws(() => thing = target.TargetValue);
            }

            target = new TestRequiredWhenAttribute("Dep", ComparisonType.EqualTo, null);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
            target = new TestRequiredWhenAttribute("Dep", ComparisonType.NotEqualTo, null);
            MyAssert.DoesNotThrow(() => thing = target.TargetValue);
        }

        #endregion

        #region GetClientValidationRules

        private ModelClientValidationRule GetClientValidationRule(object targetValue,
            ComparisonType comparison = ComparisonType.EqualTo,
            Action<TestRequiredWhenAttribute> attributeInitializer = null)
        {
            // The metadata and viewcontext are only here so an exception doesn't get thrown.
            var metadata =
                new EmptyModelMetadataProvider().GetMetadataForProperty(null, typeof(EqualsToModel), "ChildString");
            var viewcontext = new ViewContext {
                Controller = _container.GetInstance<TestController>(),
                ViewData = new ViewDataDictionary()
            };
            var target = new TestRequiredWhenAttribute("JohnnyDeppendent", comparison, targetValue);
            attributeInitializer?.Invoke(target);
            var rule = target.GetClientValidationRules(metadata, viewcontext).Single();
            return rule;
        }

        [TestMethod]
        public void
            TestGetClientValidationRulesReturnsRuleWithValidationTypeSetToDependentPropertyInAllLowerCaseAndStuff()
        {
            var rule = GetClientValidationRule("rule");
            Assert.AreEqual("requiredwhenjohnnydeppendent", rule.ValidationType);
        }

        [TestMethod]
        public void
            TestGetClientValidationRulesReturnsRuleWithValidationParameterForTargetValueSetToEmptyForNullValues()
        {
            var rule = GetClientValidationRule("value");
            Assert.AreEqual("value", rule.ValidationParameters["targetvalue"]);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsIntegerValueForEnumForTargetType()
        {
            var rule = GetClientValidationRule(SomeEnum.EqualsOne);
            Assert.AreEqual("1", rule.ValidationParameters["targetvalue"]);
        }

        [TestMethod]
        public void TestGetClientValidationRulesSetsExpectedTargetTypeForClientSideStuff()
        {
            Action<object, string> test = (value, expectedClientTargetType) => {
                var rule = GetClientValidationRule(value);
                Assert.AreEqual(expectedClientTargetType, rule.ValidationParameters["targettype"]);
            };

            // Stuff that are integers
            test(Convert.ToInt32(1), "integer");
            test(Convert.ToInt16(1), "integer");
            test(Convert.ToInt64(1), "integer");
            test(Convert.ToUInt32(1), "integer");
            test(Convert.ToUInt16(1), "integer");
            test(Convert.ToUInt64(1), "integer");
            test(Convert.ToByte(1), "integer");
            test(Convert.ToSByte(1), "integer");

            // Stuff that are floats
            test(Convert.ToDouble(1), "float");
            test(Convert.ToSingle(1), "float");

            test(true, "boolean");
            test(DateTime.Now, "date");
            test("string", "string");
        }

        [TestMethod]
        public void TestGetClientValidationRuleSetsTargetTypeToFirstElementOfArrayType()
        {
            var rule = GetClientValidationRule(new[] {1, 2}, ComparisonType.EqualToAny);
            Assert.AreEqual("integer", rule.ValidationParameters["targettype"]);
        }

        [TestMethod]
        public void TestGetClientValidationRulesSetsExpectedTargetValue()
        {
            Action<object, string> test = (value, expectedClientTargetValue) => {
                var rule = GetClientValidationRule(value);
                Assert.AreEqual(expectedClientTargetValue, rule.ValidationParameters["targetvalue"]);
            };

            test(1, "1");
            test("a string", "a string");
            test(true, "true");
            test(false, "false");
        }

        [TestMethod]
        public void TestGetClientValidationRuleReturnsJsonSerializedArrayForTargetValueWhenTargetValueIsArray()
        {
            var rule = GetClientValidationRule(new[] {1, 2, 3}, ComparisonType.EqualToAny);
            Assert.AreEqual("[1,2,3]", rule.ValidationParameters["targetvalue"]);
        }

        [TestMethod]
        public void
            TestGetClientValidationRuleReturnsJsonSerializedArrayForTargetValueWhenTargetValueIsArrayAndItsAnArrayOfEnums()
        {
            var rule = GetClientValidationRule(new[] {SomeEnum.EqualsOne, SomeEnum.EqualsTwo, SomeEnum.EqualsThree},
                ComparisonType.EqualToAny);
            Assert.AreEqual("[1,2,3]", rule.ValidationParameters["targetvalue"]);
        }

        [TestMethod]
        public void TestGetClientValidationRuleReturnsToggleVisibilityWhenFieldOnlyVisibleWhenRequiredIsTrue()
        {
            var rule = GetClientValidationRule("This doesn't matter for this test", ComparisonType.EqualTo,
                x => x.FieldOnlyVisibleWhenRequired = true);
            Assert.AreEqual("true", rule.ValidationParameters["togglevisibility"]);

            rule = GetClientValidationRule("This doesn't matter for this test", ComparisonType.EqualTo,
                x => x.FieldOnlyVisibleWhenRequired = false);
            Assert.IsFalse(rule.ValidationParameters.ContainsKey("togglevisibility"));
        }

        #endregion

        #region ComparisonType.Between

        [TestMethod]
        public void Test_IsRequired_Between_Decimal()
        {
            var model = new BetweenModel();
            // Not required when value less than the minimum
            model.DependentDecimal = -5.1m;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Not required when value is greater than the maximum
            model.DependentDecimal = 24.8400000001m;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Nulls are not considered in or out of range.
            model.DependentDecimal = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Value that falls in the range is required
            model.DependentDecimal = 5.1m;
            PropertyIsRequired(model, x => x.ChildDecimal);

            // Value that equals the minimum value is required
            model.DependentDecimal = 4.5m;
            PropertyIsRequired(model, x => x.ChildDecimal);

            // Value that equals the maximum value is required
            model.DependentDecimal = 24.81m;
            PropertyIsRequired(model, x => x.ChildDecimal);

            model.ChildDecimal = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);
        }

        [TestMethod]
        public void Test_IsRequired_Between_Double()
        {
            var model = new BetweenModel();
            // Not required when value less than the minimum
            model.DependentDouble = -5.1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Not required when value is greater than the maximum
            model.DependentDouble = 24.8400000001;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Nulls are not considered in or out of range.
            model.DependentDouble = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Value that falls in the range is required
            model.DependentDouble = 5.1;
            PropertyIsRequired(model, x => x.ChildDouble);

            // Value that equals the minimum value is required
            model.DependentDouble = 4.5;
            PropertyIsRequired(model, x => x.ChildDouble);

            // Value that equals the maximum value is required
            model.DependentDouble = 24.81;
            PropertyIsRequired(model, x => x.ChildDouble);

            model.ChildDouble = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);
        }

        [TestMethod]
        public void Test_IsRequired_Between_Int()
        {
            var model = new BetweenModel();
            // Not required when value less than the minimum
            model.DependentInt = -5;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Not required when value is greater than the maximum
            model.DependentInt = 25;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered in or out of range.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Value that falls in the range is required
            model.DependentInt = 5;
            PropertyIsRequired(model, x => x.ChildInt);

            // Value that equals the minimum value is required
            model.DependentInt = 4;
            PropertyIsRequired(model, x => x.ChildInt);

            // Value that equals the maximum value is required
            model.DependentInt = 24;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.NotBetween

        [TestMethod]
        public void Test_IsRequired_NotBetween_Decimal()
        {
            var model = new NotBetweenModel();
            // Not required when value is greater than the minimum
            // and less than the maximum
            model.DependentDecimal = 12;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Not required when value is equal to the minimum
            model.DependentDecimal = 4.5m;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Not required when value is equal to the maximum
            model.DependentDecimal = 24.84m;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Nulls are not considered in or out of range.
            model.DependentDecimal = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);

            // Value that is less than minimum is required.
            model.DependentDecimal = 4.49m;
            PropertyIsRequired(model, x => x.ChildDecimal);

            // Value that is greater than maximum is required.
            model.DependentDecimal = 24.84000001m;
            PropertyIsRequired(model, x => x.ChildDecimal);

            model.ChildDecimal = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDecimal);
        }

        [TestMethod]
        public void Test_IsRequired_NotBetween_Double()
        {
            var model = new NotBetweenModel();
            // Not required when value is greater than the minimum
            // and less than the maximum
            model.DependentDouble = 12;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Not required when value is equal to the minimum
            model.DependentDouble = 4.5;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Not required when value is equal to the maximum
            model.DependentDouble = 24.84;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Nulls are not considered in or out of range.
            model.DependentDouble = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);

            // Value that is less than minimum is required.
            model.DependentDouble = 4.49;
            PropertyIsRequired(model, x => x.ChildDouble);

            // Value that is greater than maximum is required.
            model.DependentDouble = 24.84000001;
            PropertyIsRequired(model, x => x.ChildDouble);

            model.ChildDouble = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDouble);
        }

        [TestMethod]
        public void Test_IsRequired_NotBetween_Int()
        {
            var model = new NotBetweenModel();
            // Not required when value is greater than the minimum
            // and less than the maximum
            model.DependentInt = 12;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Not required when value is equal to the minimum
            model.DependentInt = 4;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Not required when value is equal to the maximum
            model.DependentInt = 24;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered in or out of range.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Value that is less than minimum is required.
            model.DependentInt = 3;
            PropertyIsRequired(model, x => x.ChildInt);

            // Value that is greater than maximum is required.
            model.DependentInt = 251;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.EqualsTo

        [TestMethod]
        public void Test_IsRequired_EqualsTo_String()
        {
            var model = new EqualsToModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);

            model.DependentString = "some value";
            PropertyIsRequired(model, x => x.ChildString);

            model.ChildString = string.Empty;
            PropertyIsRequired(model, x => x.ChildString);

            model.ChildString = "anything";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_String_DependentIsNull()
        {
            var model = new EqualsToModel();
            model.DependentString = "not null";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildStringNull);

            model.DependentString = null;
            PropertyIsRequired(model, x => x.ChildStringNull);

            model.DependentString = string.Empty;
            PropertyIsRequired(model, x => x.ChildStringNull);

            model.ChildStringNull = "anything";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildStringNull);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_Bool()
        {
            var model = new EqualsToModel();
            model.DependentBool = false;
            model.ChildBool = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildBool);

            model.DependentBool = true;
            PropertyIsRequired(model, x => x.ChildBool);

            model.ChildBool = true;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildBool);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_DateTime()
        {
            var model = new EqualsToModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2014, 3, 18);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_DateTime_Null()
        {
            var model = new EqualsToModel();
            model.DependentDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDateNull);

            model.DependentDate = null;
            PropertyIsRequired(model, x => x.ChildDateNull);

            model.ChildDateNull = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDateNull);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_Int()
        {
            var model = new EqualsToModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = 42;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        [TestMethod]
        public void Test_IsRequired_EqualsTo_Int_Null()
        {
            var model = new EqualsToModel();
            model.DependentInt = 42;

            ValidationAssert.ModelStateIsValid(model, x => x.ChildIntNull);

            model.DependentInt = null;
            PropertyIsRequired(model, x => x.ChildIntNull);

            model.ChildIntNull = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildIntNull);
        }

        #endregion

        #region ComparisonType.EqualToAny

        [TestMethod]
        public void Test_IsRequired_EqualToAny_Int()
        {
            var model = new EqualsToAnyModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = 1;
            PropertyIsRequired(model, x => x.ChildInt);

            model.DependentInt = 2;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 237234;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.NotEqualTo

        [TestMethod]
        public void Test_IsRequired_NotEqualTo_String()
        {
            var model = new NotEqualsToModel();
            model.DependentString = "some value";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);

            model.DependentString = "some other value";
            PropertyIsRequired(model, x => x.ChildString);

            model.ChildString = "anything";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);
        }

        [TestMethod]
        public void Test_IsRequired_NotEqualTo_StringNullDependent()
        {
            var model = new NotEqualsToModel();
            model.DependentString = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildStringNull);

            model.DependentString = string.Empty;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildStringNull);

            model.DependentString = "some other value";
            PropertyIsRequired(model, x => x.ChildStringNull);

            model.ChildStringNull = "anything";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildStringNull);
        }

        [TestMethod]
        public void Test_IsRequired_NotEqualTo_Bool()
        {
            var model = new NotEqualsToModel();
            model.DependentBool = true;
            model.ChildBool = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildBool);

            model.DependentBool = false;
            PropertyIsRequired(model, x => x.ChildBool);

            model.ChildBool = true;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildBool);
        }

        [TestMethod]
        public void Test_IsRequired_NotEqualTo_DateTime()
        {
            var model = new NotEqualsToModel();
            model.DependentDate = new DateTime(2014, 3, 18);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2034, 3, 18);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_NotEqualTo_Int()
        {
            var model = new NotEqualsToModel();
            model.DependentInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = -42;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.NotEqualToAny

        [TestMethod]
        public void Test_IsRequired_NotEqualToAny_Int()
        {
            var model = new NotEqualToAnyModel();

            model.DependentInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = 2;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = 3;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 237234;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.GreaterThan

        [TestMethod]
        public void Test_IsRequired_GreaterThan_DateTime()
        {
            var model = new GreaterThanModel();
            model.DependentDate = new DateTime(2014, 3, 18);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            // Nulls are not considered greater than.
            model.DependentDate = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2034, 3, 19);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_GreaterThan_Int()
        {
            var model = new GreaterThanModel();
            model.DependentInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered greater than.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentInt = 43;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.GreaterThanOrEqualTo

        [TestMethod]
        public void Test_IsRequired_GreaterThanOrEqualTo_DateTime()
        {
            var model = new GreaterThanOrEqualToModel();
            model.DependentDate = new DateTime(2014, 3, 17);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            // Nulls are not considered greater than.
            model.DependentDate = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2034, 3, 18);
            PropertyIsRequired(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2034, 3, 19);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_GreaterThanOrEqualTo_Int()
        {
            var model = new GreaterThanOrEqualToModel();
            model.DependentInt = 41;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered greater than.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentInt = 42;
            PropertyIsRequired(model, x => x.ChildInt);

            model.DependentInt = 43;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.LessThan

        [TestMethod]
        public void Test_IsRequired_LessThan_DateTime()
        {
            var model = new LessThanModel();
            model.DependentDate = new DateTime(2014, 3, 18);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            // Nulls are not considered less than.
            model.DependentDate = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2014, 3, 17);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_LessThan_Int()
        {
            var model = new LessThanModel();
            model.DependentInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered greater than.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentInt = 41;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.LessThanOrEqualTo

        [TestMethod]
        public void Test_IsRequired_LessThanOrEqualTo_DateTime()
        {
            var model = new LessThanOrEqualToModel();
            model.DependentDate = new DateTime(2014, 3, 19);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            // Nulls are not considered greater than.
            model.DependentDate = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2014, 3, 18);
            PropertyIsRequired(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2014, 3, 17);
            PropertyIsRequired(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_IsRequired_LessThanOrEqualTo_Int()
        {
            var model = new LessThanOrEqualToModel();
            model.DependentInt = 43;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            // Nulls are not considered greater than.
            model.DependentInt = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentInt = 42;
            PropertyIsRequired(model, x => x.ChildInt);

            model.DependentInt = 41;
            PropertyIsRequired(model, x => x.ChildInt);

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.Contains

        [TestMethod]
        public void TestIsRequiredContainsInt()
        {
            var model = new ContainsModel();

            model.DependentInts = new[] {13, 14};
            ValidationAssert.ModelStateIsValid(model, x => x.ContainsInt);

            model.DependentInts = new[] {12, 13, 14};
            PropertyIsRequired(model, x => x.ContainsInt);
        }

        #endregion

        #endregion

        #region Other tests

        [TestMethod]
        public void TestTargetValueIsAlwaysDynamicallyRetrieved()
        {
            // This is to test that the attribute is always re-retrieving the
            // dynamic value. Attributes aren't always re-created and we don't
            // want them to return static values, even though those values are
            // almost always static in production. In tests they are not.
            var model = new DynamicTargetValueModel();
            model.DependentStringProp = "neat";
            model.StringProp = null;
            DynamicTargetValueModel.GetMyValueValue = "neat";

            PropertyIsRequired(model, x => x.StringProp);

            model.DependentStringProp = "something else";
            ValidationAssert.ModelStateIsValid(model, x => x.StringProp);

            // At this point we should be able to prove that a dynamic value was used.
            DynamicTargetValueModel.GetMyValueValue = "something else";
            PropertyIsRequired(model, x => x.StringProp);
        }

        [TestMethod]
        public void
            TestIsValidThrowsExceptionIfThereIsATypeMismatchBetweenTheDependentValueTypeAndTheSuppliedTargetValueAndATypeConversionIsNotSpecified()
        {
            var model = new TargetValueDependentValueMismatchModel();
            // NOTE: The Child RequiredWhen value is (short)2.
            model.Dependent = (int)2;
            model.Child = 43;

            MyAssert.Throws<InvalidOperationException>(
                () => ValidationAssert.ModelStateIsValid(model, x => x.Dependent));

            var otherModel = new TargetValueWithConvertedValueType();
            otherModel.Dependent = 2;
            PropertyIsRequired(otherModel, x => x.Child);
        }

        #endregion

        #region Models

        private enum SomeEnum
        {
            EqualsOne = 1,
            EqualsTwo = 2,
            EqualsThree = 3
        }

        private class DynamicTargetValueModel
        {
            public static string GetMyValueValue { get; set; }

            public string DependentStringProp { get; set; }

            [RequiredWhen("DependentStringProp", "GetMyValue", typeof(DynamicTargetValueModel),
                ErrorMessage = "The StringProp field is required.")]
            public string StringProp { get; set; }

            public static string GetMyValue(IContainer container)
            {
                return GetMyValueValue;
            }
        }

        private class BetweenModel
        {
            [RequiredWhen("DependentDecimal", ComparisonType.Between, (double)4.5, (double)24.84,
                convertToDecimal: true)]
            public decimal? ChildDecimal { get; set; }

            public decimal? DependentDecimal { get; set; }

            [RequiredWhen("DependentDouble", ComparisonType.Between, (double)4.5, (double)24.84)]
            public double? ChildDouble { get; set; }

            public double? DependentDouble { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.Between, 4, 24)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class NotBetweenModel
        {
            [RequiredWhen("DependentDecimal", ComparisonType.NotBetween, (double)4.5, (double)24.84,
                convertToDecimal: true)]
            public decimal? ChildDecimal { get; set; }

            public decimal? DependentDecimal { get; set; }

            [RequiredWhen("DependentDouble", ComparisonType.NotBetween, (double)4.5, (double)24.84)]
            public double? ChildDouble { get; set; }

            public double? DependentDouble { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.NotBetween, 4, 24)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class EqualsToModel
        {
            [RequiredWhen("DependentString", ComparisonType.EqualTo, "some value")]
            public string ChildString { get; set; }

            public string DependentString { get; set; }

            [RequiredWhen("DependentString", ComparisonType.EqualTo, null)]
            public string ChildStringNull { get; set; }

            [RequiredWhen("DependentBool", ComparisonType.EqualTo, true)]
            public bool? ChildBool { get; set; }

            public bool? DependentBool { get; set; }

            [RequiredWhen("DependentDate", ComparisonType.EqualTo, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [RequiredWhen("DependentDate", ComparisonType.EqualTo, null)]
            public DateTime? ChildDateNull { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.EqualTo, 42)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.EqualTo, null)]
            public int? ChildIntNull { get; set; }
        }

        private class EqualsToAnyModel
        {
            [RequiredWhen("DependentInt", ComparisonType.EqualToAny, new[] {1, 2})]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class ContainsModel
        {
            [RequiredWhen("DependentInts", ComparisonType.Contains, 12)]
            public int? ContainsInt { get; set; }

            public int[] DependentInts { get; set; }
        }

        private class NotEqualsToModel
        {
            [RequiredWhen("DependentString", ComparisonType.NotEqualTo, "some value")]
            public string ChildString { get; set; }

            public string DependentString { get; set; }

            [RequiredWhen("DependentString", ComparisonType.NotEqualTo, null)]
            public string ChildStringNull { get; set; }

            [RequiredWhen("DependentBool", ComparisonType.NotEqualTo, true)]
            public bool? ChildBool { get; set; }

            public bool DependentBool { get; set; }

            [RequiredWhen("DependentNullabool", ComparisonType.NotEqualTo, true)]
            public bool? ChildNullabool { get; set; }

            public bool? DependentNullabool { get; set; }

            [RequiredWhen("DependentDate", ComparisonType.NotEqualTo, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime DependentDate { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.NotEqualTo, 42)]
            public int? ChildInt { get; set; }

            public int DependentInt { get; set; }
        }

        private class NotEqualToAnyModel
        {
            [RequiredWhen("DependentInt", ComparisonType.NotEqualToAny, new[] {1, 2})]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class GreaterThanModel
        {
            [RequiredWhen("DependentDate", ComparisonType.GreaterThan, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.GreaterThan, 42)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class GreaterThanOrEqualToModel
        {
            [RequiredWhen("DependentDate", ComparisonType.GreaterThanOrEqualTo, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.GreaterThanOrEqualTo, 42)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class LessThanModel
        {
            [RequiredWhen("DependentDate", ComparisonType.LessThan, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.LessThan, 42)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class LessThanOrEqualToModel
        {
            [RequiredWhen("DependentDate", ComparisonType.LessThanOrEqualTo, 2014, 3, 18)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [RequiredWhen("DependentInt", ComparisonType.LessThanOrEqualTo, 42)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class TargetValueDependentValueMismatchModel
        {
            // This is for testing that the target value supplied has the
            // same type as the dependent property value. Equality can't be
            // done if the types are different(ex: (int)1 != (short)1, at least with IComparable.Equals).
            [RequiredWhen("Dependent", ComparisonType.EqualTo, (short)2)]
            public int? Child { get; set; }

            public int? Dependent { get; set; }
        }

        private class TargetValueWithConvertedValueType
        {
            // This is for testing that the target value supplied has the
            // same type as the dependent property value. Equality can't be
            // done if the types are different(ex: (int)1 != (short)1, at least with IComparable.Equals).
            [RequiredWhen("Dependent", ComparisonType.EqualTo, (short)2, typeToConvertTargetValueTo: typeof(int))]
            public int? Child { get; set; }

            public int? Dependent { get; set; }
        }

        #endregion

        #region Test Classes

        private class TestRequiredWhenAttribute : RequiredWhenAttribute
        {
            private IContainer _container;

            public object TargetValue => GetThreadSafeValue(_container).TargetValue;

            public TestRequiredWhenAttribute(string dependentProperty, object targetValue) : base(dependentProperty,
                targetValue) { }

            public TestRequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, double minValue,
                double maxValue) : base(dependentProperty, comparisonType, minValue, maxValue) { }

            public TestRequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, int minValue,
                int maxValue) : base(dependentProperty, comparisonType, minValue, maxValue) { }

            public TestRequiredWhenAttribute(string dependentProperty, int year, int month, int day) : base(
                dependentProperty, year, month, day) { }

            public TestRequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType, int year,
                int month, int day) : base(dependentProperty, comparisonType, year, month, day) { }

            public TestRequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType,
                object targetValue) : base(dependentProperty, comparisonType, targetValue) { }

            public TestRequiredWhenAttribute(string dependentProperty, string dynamicValueCallbackMethod,
                Type dynamicValueCallbackType) : base(dependentProperty, dynamicValueCallbackMethod,
                dynamicValueCallbackType) { }

            public TestRequiredWhenAttribute(string dependentProperty, ComparisonType comparisonType,
                string dynamicValueCallbackMethod, Type dynamicValueCallbackType) : base(dependentProperty,
                comparisonType, dynamicValueCallbackMethod, dynamicValueCallbackType) { }

            public void SetContainer(IContainer container)
            {
                _container = container;
            }
        }

        #endregion
    }
}
