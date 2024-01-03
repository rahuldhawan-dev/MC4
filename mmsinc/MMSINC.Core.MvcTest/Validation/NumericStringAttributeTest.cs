using MMSINC.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class NumericStringAttributeTest
    {
        #region Private Members

        private NumericStringAttribute _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new NumericStringAttribute();
        }

        #endregion

        [TestMethod]
        public void TestIsValidReturnsTrueIfStringIsNumerical()
        {
            Assert.IsTrue(_target.IsValid("0123456789"));
            Assert.IsFalse(_target.IsValid("a1j2nme3zz"));
        }

        [TestMethod]
        public void TestIsValidReturnsTrueForNullAndEmptyStrings()
        {
            // Validators always return true for null/empty strings. 
            // use Required when they shouldn't be null/empty.
            Assert.IsTrue(_target.IsValid(string.Empty));
            Assert.IsTrue(_target.IsValid(null));
        }
    }
}
