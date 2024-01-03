using System;
using System.Linq;
using MMSINC.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class StringLengthNotRequiredAttributeTest
    {
        #region Fields

        private static readonly AttributeUsageAttribute _attrUsage =
            (AttributeUsageAttribute)
            typeof(StringLengthNotRequiredAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                                                    .Single();

        #endregion

        [TestMethod]
        public void TestAttributeCanBeAppliedToProperties()
        {
            Assert.IsTrue(_attrUsage.ValidOn.HasFlag(AttributeTargets.Property));
        }

        [TestMethod]
        public void TestAttributeCanBeAppliedToClasses()
        {
            Assert.IsTrue(_attrUsage.ValidOn.HasFlag(AttributeTargets.Class));
        }
    }
}
