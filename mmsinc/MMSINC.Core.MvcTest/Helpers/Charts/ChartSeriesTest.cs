using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Helpers.Charts
{
    [TestClass]
    public class ChartSeriesTest
    {
        #region Tests

        [TestMethod]
        public void TestAddingDuplicateKeysThrowsException()
        {
            var target = new ChartSeries<string, int>();
            target.Add("blah", 3);
            MyAssert.Throws<ArgumentException>(() => target.Add("blah", 4));
        }

        #endregion
    }
}
