using MMSINC.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for VisibilityChangeEventArgs
    /// </summary>
    [TestClass]
    public class VisibilityChangeEventArgsTest
    {
        #region Constants

        private const bool DEFAULT_CONSTRUCTOR_ARGUMENT = true;

        #endregion

        #region Private Members

        private VisibilityChangeEventArgs _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void VisibilityChangeEventArgsTestInitialize()
        {
            _target = new VisibilityChangeEventArgs(DEFAULT_CONSTRUCTOR_ARGUMENT);
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsNewVisibilityToValueOfArgument()
        {
            _target = new VisibilityChangeEventArgs(DEFAULT_CONSTRUCTOR_ARGUMENT);
            Assert.IsTrue(_target.NewVisibility);

            _target = new VisibilityChangeEventArgs(!DEFAULT_CONSTRUCTOR_ARGUMENT);
            Assert.IsFalse(_target.NewVisibility);
        }

        [TestMethod]
        public void TestConstructorSetsOldVisibilityToInverseValueOfArgument()
        {
            _target = new VisibilityChangeEventArgs(DEFAULT_CONSTRUCTOR_ARGUMENT);
            Assert.IsFalse(_target.OldVisibility);

            _target = new VisibilityChangeEventArgs(!DEFAULT_CONSTRUCTOR_ARGUMENT);
            Assert.IsTrue(_target.OldVisibility);
        }
    }
}
