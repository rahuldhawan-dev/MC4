using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class RequiresConfirmationAttributeTest
    {
        [TestMethod]
        public void TestIsValidReturnsFalse()
        {
            var fieldName = "foo";
            var expected = String.Format("The field foo is invalid.", fieldName);
            var context = new ValidationContext(new object(), null, null) {DisplayName = fieldName};
            var target = new RequiresConfirmationAttribute();

            var result = target.GetValidationResult(false, context);

            Assert.AreEqual(expected, result.ErrorMessage);
        }

        [TestMethod]
        public void TestIsValidReturnsTrueWhenValid()
        {
            var target = new RequiresConfirmationAttribute();

            var result = target.IsValid(true);

            Assert.IsTrue(result);
        }
    }
}
