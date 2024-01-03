using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EquipmentPurposeTest
    {
        public const string TO_STRING_FORMAT = "{0} - {1} : - {2} - {3} - {4}";

        [TestMethod]
        public void TestToStringReturnsFormattedString()
        {
            var subcat = new EquipmentSubCategory {Description = "subcat"};
            var cat = new EquipmentCategory {Description = "cat"};
            var target = new EquipmentPurpose
                {EquipmentSubCategory = subcat, EquipmentCategory = cat, Description = "bah", Abbreviation = "WOPR"};
            var expected = String.Format(TO_STRING_FORMAT,
                target.Abbreviation,
                target.Id,
                target.Description,
                cat.Description,
                subcat.Description);

            Assert.AreEqual(expected, target.ToString());
        }
    }
}
