using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class Int32ArrayModelBinderTest
    {
        #region Fields

        private Int32ArrayModelBinder _target;
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

            _target = new Int32ArrayModelBinder();
        }

        private ValueProviderResult CreateValueProviderResult(object rawValue, string attemptedValue)
        {
            return new ValueProviderResult(rawValue, attemptedValue, System.Globalization.CultureInfo.CurrentCulture);
        }

        #endregion

        #region Tests

        #region BindModel

        private int[] GetResult(object expectedRawValue, string expectedAttemptedValue)
        {
            var vpResult = CreateValueProviderResult(expectedRawValue, expectedAttemptedValue);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            return (int[])_target.BindModel(_controllerContext, _bindingContext);
        }

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

        [TestMethod]
        public void TestBindModelReturnsRawValueIfRawValueIsIntArray()
        {
            var expected = new int[] {1, 2, 3};
            var result = GetResult(expected,
                "doesn't matter what this is because it should be completely ignored by the model binder");
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void TestBindModelSplitsCommaSeparatedValuesIntoAnIntArray()
        {
            var result = GetResult(null, "1,2,    3");
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
        }

        [TestMethod]
        public void TestBindModelThrowsExceptionWhenItCanNotParseInteger()
        {
            // ArgumentException's error message gets a line break added to it for some reason.
            MyAssert.ThrowsWithMessage<ArgumentException>(() => GetResult(null, "A"), @"Value 'A' is not a valid Int32.
Parameter name: ModelMcModelson");
        }

        #endregion

        #endregion
    }
}
