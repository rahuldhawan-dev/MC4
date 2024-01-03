using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class StringListModelBinderTest
    {
        #region Fields

        private StringListModelBinder _target;
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

            _target = new StringListModelBinder();
        }

        private ValueProviderResult CreateValueProviderResult(object rawValue, string attemptedValue)
        {
            return new ValueProviderResult(rawValue, attemptedValue, System.Globalization.CultureInfo.CurrentCulture);
        }

        #endregion

        #region Tests

        #region BindModel

        [TestMethod]
        public void TestBindModelReturnsNullIfValueProviderReturnsNullProviderValue()
        {
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns((ValueProviderResult)null);
            Assert.IsNull(_target.BindModel(_controllerContext, _bindingContext));
        }

        [TestMethod]
        public void TestBindModelReturnsNullIfValueProviderReturnsEmptyStringForAttemptedValue()
        {
            var result = CreateValueProviderResult(new[] {string.Empty}, string.Empty);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(result);
            Assert.IsNull(_target.BindModel(_controllerContext, _bindingContext));
        }

        [TestMethod]
        public void TestBindModelReturnsListOfStringsForValuesThatAreCommaSeparatedAndThoseResultsAreInOrderToo()
        {
            var attemptedValue = "What,Okay,Cool";
            var vpResult = CreateValueProviderResult(new[] {attemptedValue}, attemptedValue);
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            var result = (List<string>)_target.BindModel(_controllerContext, _bindingContext);

            Assert.AreEqual("What", result[0]);
            Assert.AreEqual("Okay", result[1]);
            Assert.AreEqual("Cool", result[2]);
        }

        [TestMethod]
        public void TestBindModelSetsModelStateValueToProviderValue()
        {
            var vpResult = CreateValueProviderResult(1, "1");
            _valueProvider.Setup(x => x.GetValue(MODEL_NAME)).Returns(vpResult);
            _target.BindModel(_controllerContext, _bindingContext);
            Assert.AreSame(vpResult, _bindingContext.ModelState[MODEL_NAME].Value);
        }

        #endregion

        #endregion
    }
}
