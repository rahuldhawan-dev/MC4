using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class EnumModelBinderTest
    {
        #region Test enum

        private enum TestIntEnum : int
        {
            Default = 0,
            Other = 1
        }

        #endregion

        #region Fields

        private EnumModelBinder _target;
        private ModelBindingContext _bindingContext;
        private ControllerContext _controllerContext;
        private Mock<IValueProvider> _valueProvider;
        private const string MODEL_NAME = "ModelMcModelson";

        #endregion

        #region Init/cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _valueProvider = new Mock<IValueProvider>();

            _bindingContext = new ModelBindingContext {
                ModelName = MODEL_NAME,
                ValueProvider = _valueProvider.Object
            };
            _controllerContext = new ControllerContext();

            _target = new EnumModelBinder(typeof(TestIntEnum));
        }

        private ValueProviderResult CreateValueProviderResult(object rawValue, string attemptedValue)
        {
            return new ValueProviderResult(rawValue, attemptedValue, System.Globalization.CultureInfo.CurrentCulture);
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsExceptionIfGenericArgIsNotEnum()
        {
            MyAssert.Throws<InvalidOperationException>(() => new EnumModelBinder(typeof(string)));
        }

        #endregion

        #region BindModel

        [TestMethod]
        public void TestBindModelReturnsNullIfValueProviderReturnsNullProviderValue()
        {
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns((ValueProviderResult)null);
            Assert.IsNull(_target.BindModel(_controllerContext, _bindingContext));
        }

        [TestMethod]
        public void TestBindModelSetsModelStateValueToProviderValue()
        {
            var vpResult = CreateValueProviderResult(1, "1");
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            _target.BindModel(_controllerContext, _bindingContext);
            Assert.AreSame(vpResult, _bindingContext.ModelState[MODEL_NAME].Value);
        }

        private void TestNullAttemptedValue(string value)
        {
            // Explicitly setting null here because we're not testing RawValue.
            var vpResult = CreateValueProviderResult(null, value);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            Assert.IsNull(_target.BindModel(_controllerContext, _bindingContext));
        }

        [TestMethod]
        public void TestBindModelReturnsNullIfProvidedValueAttemptedValueIsNull()
        {
            TestNullAttemptedValue(null);
        }

        [TestMethod]
        public void TestBindModelReturnsNullIfProvidedValueAttemptedValueIsEmpty()
        {
            TestNullAttemptedValue(string.Empty);
        }

        [TestMethod]
        public void TestBindModelReturnsNullIfProvidedValueAttemptedValueIsWhitespace()
        {
            TestNullAttemptedValue("    ");
        }

        private void TestAttemptedValueIsCorrect(TestIntEnum expected, string attempted)
        {
            // Explicitly setting null here because we're not testing RawValue.
            var vpResult = CreateValueProviderResult(null, attempted);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            var result = _target.BindModel(_controllerContext, _bindingContext);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestBindModelReturnsEnumValueWhenAttemptedValueIsStringVersionOfEnumValue()
        {
            TestAttemptedValueIsCorrect(TestIntEnum.Default, "0");
            TestAttemptedValueIsCorrect(TestIntEnum.Other, "1");
        }

        [TestMethod]
        public void TestBindModelReturnsEnumValueWhenAttemptedValueIsEnumName()
        {
            TestAttemptedValueIsCorrect(TestIntEnum.Default, "Default");
            TestAttemptedValueIsCorrect(TestIntEnum.Other, "Other");
        }

        [TestMethod]
        public void TestBindModelReturnsEnumValueWhenAttemptedValueIsEnumNameButWithDifferentCase()
        {
            TestAttemptedValueIsCorrect(TestIntEnum.Default, "deFauLt");
            TestAttemptedValueIsCorrect(TestIntEnum.Other, "OTHER");
        }

        private void TestBindBadValue(string badValue)
        {
            var vpResult = CreateValueProviderResult(null, badValue);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            Assert.IsNull(_target.BindModel(_controllerContext, _bindingContext));
        }

        [TestMethod]
        public void TestBindModelShouldReturnNullWhenNameDoesntExistInEnum()
        {
            TestBindBadValue("burgers");
        }

        [TestMethod]
        public void TestBindModelReturnsNullWhenNumericValueIsNotDefined()
        {
            Assert.IsFalse(Enum.IsDefined(typeof(TestIntEnum), 325235));
            TestBindBadValue("325235");
        }

        private void TestBindModelAddsModelError(string badValue)
        {
            TestBindBadValue(badValue);
            Assert.IsTrue(_bindingContext.ModelState.ContainsKey(MODEL_NAME));
            var modelStateError = _bindingContext.ModelState[MODEL_NAME].Errors.First();
            var errMessage = string.Format(EnumModelBinder.ERROR_FORMAT, typeof(TestIntEnum).FullName, badValue);
            Assert.AreEqual(errMessage, modelStateError.Exception.Message);
        }

        [TestMethod]
        public void TestBindModelAddsModelErrorWhenNameDoesntExist()
        {
            TestBindModelAddsModelError("burger");
        }

        [TestMethod]
        public void TestBindModelAddsModelErrorWhenValueIsNotDefined()
        {
            TestBindModelAddsModelError("325235");
        }

        #endregion

        #endregion
    }
}
