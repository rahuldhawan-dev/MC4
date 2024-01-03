using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class TargetInvocationExceptionExtensionsTest
    {
        [TestMethod]
        public void TestUnwindUnravelsAndReturnsFirstNonTIE()
        {
            var expected = new Exception();
            var actual =
                new TargetInvocationException(new TargetInvocationException(new TargetInvocationException(expected)))
                   .Unwind();

            Assert.IsNotNull(actual);
            Assert.AreSame(expected, actual);
        }
    }
}
