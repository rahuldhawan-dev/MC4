using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class DefaultValueConverterTest
    {
        #region Fields

        private DefaultValueConverter _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new DefaultValueConverter();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToPrimaryReturnsTheSameValuePassedIn()
        {
            var expected = new object();
            Assert.AreSame(expected, _target.ToPrimary(expected));
        }

        [TestMethod]
        public void TestToSecondaryReturnsTheSameValuePassedIn()
        {
            var expected = new object();
            Assert.AreSame(expected, _target.ToSecondary(expected));
        }

        #endregion
    }
}
