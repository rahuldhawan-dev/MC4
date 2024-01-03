using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Validation;
using Moq;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class CompareToAttributeTest
    {
        #region Fields

        private CompareToAttribute _target;
        private ModelMetadata _modelMetadata;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new CompareToAttribute("OtherIntProperty", ComparisonType.GreaterThanOrEqualTo, TypeCode.Int32);

            //  var controllerContext = new ControllerContext();
            var provider = new Mock<ModelMetadataProvider>();
            var container = new Metadata.TestModel();
            Func<String> func = () => container.Foo;
            var containerType = typeof(Metadata.TestModel);
            _modelMetadata = new ModelMetadata(provider.Object, containerType, func, typeof(ViewModel), "IntProperty");
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsComparisonType()
        {
            var expected = ComparisonType.LessThanOrEqualTo;
            var target = new CompareToAttribute("OtherProp", expected, TypeCode.Int32);
            Assert.AreEqual(expected, target.ComparisonType);
        }

        [TestMethod]
        public void TestConstructorSetsConvertedValueType()
        {
            var expected = TypeCode.Int32; // Can't use NotDefault here cause of the NotImplementedExceptions.
            var target = new CompareToAttribute("OtherProp", default(ComparisonType), expected);
            Assert.AreEqual(expected, target.ConvertedValueType);
        }

        [TestMethod]
        public void TestConstructorThrowsForNullOrEmptyOrWhitespaceOtherPropertyArgument()
        {
            MyAssert.Throws<ArgumentNullException>(() =>
                new CompareToAttribute(null, default(ComparisonType), default(TypeCode)));
            MyAssert.Throws<ArgumentNullException>(() =>
                new CompareToAttribute(string.Empty, default(ComparisonType), default(TypeCode)));
            MyAssert.Throws<ArgumentNullException>(() =>
                new CompareToAttribute("    ", default(ComparisonType), default(TypeCode)));
        }

        private void AssertTypeCodeNotImplemented(TypeCode typeCode)
        {
            MyAssert.Throws<ArgumentNullException>(
                () => new CompareToAttribute(null, default(ComparisonType), typeCode));
        }

        [TestMethod]
        public void TestConstructorThrowsNotImplementedExceptionIfTypeCodeIsString()
        {
            AssertTypeCodeNotImplemented(TypeCode.String);
        }

        [TestMethod]
        public void TestConstructorThrowsNotImplementedExceptionIfTypeCodeIsObject()
        {
            AssertTypeCodeNotImplemented(TypeCode.Object);
        }

        [TestMethod]
        public void TestConstructorThrowsNotImplementedExceptionIfTypeCodeIsBoolean()
        {
            AssertTypeCodeNotImplemented(TypeCode.Boolean);
        }

        [TestMethod]
        public void TestConstructorThrowsNotImplementedExceptionIfTypeCodeIsDBNull()
        {
            AssertTypeCodeNotImplemented(TypeCode.DBNull);
        }

        [TestMethod]
        public void TestConstructorThrowsNotImplementedExceptionIfTypeCodeIsEmpty()
        {
            AssertTypeCodeNotImplemented(TypeCode.Empty);
        }

        #endregion

        #region GetClientValidationRules

        private ModelClientValidationRule GetClientValidationRule()
        {
            // We can pass null in for the controllerContext at the moment since it's
            // not used by our implementation.
            return _target.GetClientValidationRules(_modelMetadata, null).Single();
        }

        private void AssertValidationParameterSet(string paramName, string expectedValue)
        {
            Assert.AreEqual(expectedValue, GetClientValidationRule().ValidationParameters[paramName]);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsRuleWithValidationTypeSetToCompareTo()
        {
            Assert.AreEqual("compareto", GetClientValidationRule().ValidationType);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsErrorMessageSetByUser()
        {
            var expected = "Imma errorin and stuff";
            _target.ErrorMessage = expected;
            Assert.AreEqual(expected, GetClientValidationRule().ErrorMessage);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsDefaultErrorMessageIfNotSetByUser()
        {
            Assert.AreEqual("IntProperty must be greater than or equal to OtherIntProperty.",
                GetClientValidationRule().ErrorMessage);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsParameterForOtherProperty()
        {
            AssertValidationParameterSet("other", "OtherIntProperty");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsParameterForComparison()
        {
            AssertValidationParameterSet("comparison", "greaterthanorequalto");
        }

        private void AssertTypeParameter(TypeCode typeCode, string expectedValue)
        {
            _target = new CompareToAttribute("OtherIntProperty", ComparisonType.EqualTo, typeCode);
            AssertValidationParameterSet("type", expectedValue);
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsInt16()
        {
            AssertTypeParameter(TypeCode.Int16, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsInt32()
        {
            AssertTypeParameter(TypeCode.Int32, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsInt64()
        {
            AssertTypeParameter(TypeCode.Int64, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsUInt16()
        {
            AssertTypeParameter(TypeCode.UInt16, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsUInt32()
        {
            AssertTypeParameter(TypeCode.UInt32, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsUInt64()
        {
            AssertTypeParameter(TypeCode.UInt64, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsByte()
        {
            AssertTypeParameter(TypeCode.Byte, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithIntegerIfTypeCodeIsSByte()
        {
            AssertTypeParameter(TypeCode.SByte, "integer");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithFloatIfTypeCodeIsDouble()
        {
            AssertTypeParameter(TypeCode.Double, "float");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithFloatIfTypeCodeIsDecimal()
        {
            AssertTypeParameter(TypeCode.Decimal, "float");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithFloatIfTypeCodeIsSingleAndReadyToMingle()
        {
            AssertTypeParameter(TypeCode.Single, "float");
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsTypeParameterWithDateIfTypeCodeIsDateTime()
        {
            AssertTypeParameter(TypeCode.DateTime, "date");
        }

        #region ComparisonType.EqualsTo

        [TestMethod]
        public void Test_EqualsTo_String()
        {
            var model = new EqualsToModel();
            model.ChildString = null;
            model.DependentString = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);

            model.DependentString = "some value";
            ValidationAssert.ModelStateHasError(model, x => x.ChildString,
                "ChildString must be equal to DependentString.");

            model.ChildString = "some value";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);
        }

        [TestMethod]
        public void Test_EqualsTo_String_EmptyStringAndNullsAreEqual()
        {
            var model = new EqualsToModel();
            model.ChildString = null;
            model.DependentString = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);

            model.ChildString = string.Empty;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);
        }

        [TestMethod]
        public void Test_EqualsTo_DateTime()
        {
            var model = new EqualsToModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = new DateTime(2014, 3, 18);
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate, "ChildDate must be equal to DependentDate.");
        }

        [TestMethod]
        public void Test_EqualsTo_Int()
        {
            var model = new EqualsToModel();
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = 42;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be equal to DependentInt.");
        }

        #endregion

        #region ComparisonType.NotEqualTo

        [TestMethod]
        public void Test_NotEqualTo_String()
        {
            var model = new NotEqualsToModel();
            model.DependentString = "some value";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);

            model.ChildString = "some value";
            ValidationAssert.ModelStateHasError(model, x => x.ChildString,
                "ChildString must be not equal to DependentString.");

            model.ChildString = "some other value";
            ValidationAssert.ModelStateIsValid(model, x => x.ChildString);
        }

        [TestMethod]
        public void Test_NotEqualTo_DateTime()
        {
            var model = new NotEqualsToModel();
            model.DependentDate = new DateTime(2014, 3, 18);
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.ChildDate = model.DependentDate;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be not equal to DependentDate.");

            model.ChildDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);
        }

        [TestMethod]
        public void Test_NotEqualTo_Int()
        {
            var model = new NotEqualsToModel();
            model.DependentInt = 42;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.ChildInt = model.DependentInt;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be not equal to DependentInt.");

            model.ChildInt = 1;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);
        }

        #endregion

        #region ComparisonType.GreaterThan

        [TestMethod]
        public void Test_GreaterThan_DateTime()
        {
            var model = new GreaterThanModel();
            model.ChildDate = DateTime.Today.AddDays(1);
            model.DependentDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = model.ChildDate;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be greater than DependentDate.");

            model.DependentDate = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be greater than DependentDate.");
        }

        [TestMethod]
        public void Test_GreaterThan_Int()
        {
            var model = new GreaterThanModel();
            model.ChildInt = 12;
            model.DependentInt = 11;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = model.ChildInt;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be greater than DependentInt.");

            model.DependentInt = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be greater than DependentInt.");
        }

        #endregion

        #region ComparisonType.GreaterThanOrEqualTo

        [TestMethod]
        public void Test_GreaterThanOrEqualTo_DateTime()
        {
            var model = new GreaterThanOrEqualToModel();
            model.ChildDate = DateTime.Today.AddDays(1);
            model.DependentDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = model.ChildDate;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be greater than or equal to DependentDate.");
        }

        [TestMethod]
        public void Test_GreaterThanOrEqualTo_Int()
        {
            var model = new GreaterThanOrEqualToModel();
            model.ChildInt = 12;
            model.DependentInt = 11;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = model.ChildInt;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt,
                "ChildInt must be greater than or equal to DependentInt.");
        }

        #endregion

        #region ComparisonType.LessThan

        [TestMethod]
        public void Test_LessThan_DateTime()
        {
            var model = new LessThanModel();
            model.ChildDate = DateTime.Today.AddDays(-1);
            model.DependentDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = model.ChildDate;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate, "ChildDate must be less than DependentDate.");

            model.DependentDate = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate, "ChildDate must be less than DependentDate.");
        }

        [TestMethod]
        public void Test_LessThan_Int()
        {
            var model = new LessThanModel();
            model.ChildInt = 10;
            model.DependentInt = 11;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = model.ChildInt;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be less than DependentInt.");

            model.DependentInt = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt, "ChildInt must be less than DependentInt.");
        }

        #endregion

        #region ComparisonType.LessThanOrEqualTo

        [TestMethod]
        public void Test_LessThanOrEqualTo_DateTime()
        {
            var model = new LessThanOrEqualToModel();
            model.ChildDate = DateTime.Today.AddDays(-1);
            model.DependentDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = model.ChildDate;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.DependentDate = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be less than or equal to DependentDate.");
        }

        [TestMethod]
        public void Test_LessThanOrEqualTo_Int()
        {
            var model = new LessThanOrEqualToModel();
            model.ChildInt = 10;
            model.DependentInt = 11;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = model.ChildInt;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildInt);

            model.DependentInt = null;
            ValidationAssert.ModelStateHasError(model, x => x.ChildInt,
                "ChildInt must be less than or equal to DependentInt.");
        }

        #endregion

        [TestMethod]
        public void TestValidationIsSuccessfulIfIgnoreNullValuesIsTrueAndTheValueIsNull()
        {
            var model = new IgnoreNullsModel();
            model.DependentDate = DateTime.Today;
            model.ChildDate = null;
            ValidationAssert.ModelStateIsValid(model, x => x.ChildDate);

            model.ChildDate = DateTime.Today.AddDays(4);
            ValidationAssert.ModelStateHasError(model, x => x.ChildDate,
                "ChildDate must be less than or equal to DependentDate.");
        }

        #endregion

        #endregion

        #region Helper classes

        private class ViewModel
        {
            public int IntProperty { get; set; }
            public int OtherIntProperty { get; set; }
        }

        private class EqualsToModel
        {
            [CompareTo("DependentString", ComparisonType.EqualTo, TypeCode.String)]
            public string ChildString { get; set; }

            public string DependentString { get; set; }

            [CompareTo("DependentDate", ComparisonType.EqualTo, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.EqualTo, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class NotEqualsToModel
        {
            [CompareTo("DependentString", ComparisonType.NotEqualTo, TypeCode.String)]
            public string ChildString { get; set; }

            public string DependentString { get; set; }

            [CompareTo("DependentDate", ComparisonType.NotEqualTo, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.NotEqualTo, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int DependentInt { get; set; }
        }

        private class GreaterThanModel
        {
            [CompareTo("DependentDate", ComparisonType.GreaterThan, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.GreaterThan, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class GreaterThanOrEqualToModel
        {
            [CompareTo("DependentDate", ComparisonType.GreaterThanOrEqualTo, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.GreaterThanOrEqualTo, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class LessThanModel
        {
            [CompareTo("DependentDate", ComparisonType.LessThan, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.LessThan, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class LessThanOrEqualToModel
        {
            [CompareTo("DependentDate", ComparisonType.LessThanOrEqualTo, TypeCode.DateTime)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }

            [CompareTo("DependentInt", ComparisonType.LessThanOrEqualTo, TypeCode.Int32)]
            public int? ChildInt { get; set; }

            public int? DependentInt { get; set; }
        }

        private class IgnoreNullsModel
        {
            [CompareTo("DependentDate", ComparisonType.LessThanOrEqualTo, TypeCode.DateTime, IgnoreNullValues = true)]
            public DateTime? ChildDate { get; set; }

            public DateTime? DependentDate { get; set; }
        }

        #endregion
    }
}
