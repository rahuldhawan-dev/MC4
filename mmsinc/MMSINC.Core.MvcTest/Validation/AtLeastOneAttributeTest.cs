using System;
using System.Collections.Generic;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class AtLeastOneAttributeTest
    {
        #region Private Members

        private AtLeastOneAttribute _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new AtLeastOneAttribute();
        }

        #endregion

        [TestMethod]
        public void TestIsValidThrowsArgumentExceptionWhenAppliedToNonIEnumerableProperty()
        {
            MyAssert.Throws<ArgumentException>(() => _target.IsValid(1));
            MyAssert.Throws<ArgumentException>(() => _target.IsValid('c'));
            MyAssert.DoesNotThrow(() => _target.IsValid(new[] {1, 2, 3}));
            MyAssert.DoesNotThrow(() => _target.IsValid("foo"));
        }

        [TestMethod]
        public void TestIsValidThrowsArgumentExceptionIfArgumentIsNull()
        {
            MyAssert.Throws<ArgumentException>(() => _target.IsValid(null));
        }

        [TestMethod]
        public void TestIsValidReturnsFalseIfPropertyHasNoElements()
        {
            Assert.IsFalse(_target.IsValid(new List<object>()));
        }

        [TestMethod]
        public void TestIsValidReturnsTrueIfPropertyHasElements()
        {
            Assert.IsTrue(_target.IsValid(new[] {1, 2, 3}));
        }
    }
}
