using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Validation;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class ClientCallbackAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorSetsCallbackMethodPropertyFromParam()
        {
            var target = new ClientCallbackAttribute("mop");
            Assert.AreEqual("mop", target.CallbackMethod);
        }

        [TestMethod]
        public void TestIsValidAlwaysReturnsSuccess()
        {
            // Creating a new target to ensure no setup has been done to it
            // if it ever gets expanded upon.
            var target = new ClientCallbackAttribute("aegjage");
            Assert.AreSame(ValidationResult.Success,
                target.GetValidationResult(null, new ValidationContext(new object(), null, null)));
            Assert.IsTrue(target.IsValid(null));
        }

        private ModelClientValidationRule GetResultRule()
        {
            var target = new ClientCallbackAttribute("some method");
            target.ErrorMessage = "I am an error.";
            return target.GetClientValidationRules(null, null).Single();
        }

        [TestMethod]
        public void TestGetClientValidationRulesReturnsExpectedRuleWithErrorMessageSet()
        {
            var rule = GetResultRule();
            Assert.AreEqual("I am an error.", rule.ErrorMessage);
        }

        [TestMethod]
        public void TestGetClientValidationRuleReturnsRuleWithValidationTypeSetToClientCallbackInAllLowerCase()
        {
            var rule = GetResultRule();
            Assert.AreEqual("clientcallback", rule.ValidationType);
        }

        [TestMethod]
        public void TestGetClientValidationRuleReturnsRuleWithMethodParameterSetToCallbackMethod()
        {
            var rule = GetResultRule();
            Assert.AreEqual("some method", rule.ValidationParameters["method"]);
        }

        #endregion
    }
}
