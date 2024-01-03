using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SearchAliasAttributeTest
    {
        [TestMethod]
        public void TestConstructorSetsExpectedParameters()
        {
            var alias = "Rystromicus";
            var associationPath = "association path";
            var property = "Prop.Id";

            var target = new SearchAliasAttribute(associationPath, alias, property);

            Assert.AreEqual(alias, target.Alias);
            Assert.AreEqual(associationPath, target.AssociationPath);
            Assert.AreEqual(property, target.Property);
        }
    }
}
