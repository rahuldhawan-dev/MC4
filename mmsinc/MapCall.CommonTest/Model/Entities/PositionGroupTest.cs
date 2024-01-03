using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class PositionGroupTest
    {
        [TestMethod]
        public void TestToStringReturnsFormattedStrin()
        {
            var group = "group";
            var positionDescription = "position description";
            var businessUnit = "business unit";
            var businessUnitDescription = "business unit description";
            var sapCompanyCode = new SAPCompanyCode {Description = "SAPCompanyCode"};
            var sapPositionGroupKey = "sap position group key";
            var target = new PositionGroup {
                Group = group,
                PositionDescription = positionDescription,
                BusinessUnit = businessUnit,
                BusinessUnitDescription = businessUnitDescription,
                SAPCompanyCode = sapCompanyCode,
                SAPPositionGroupKey = sapPositionGroupKey
            };

            Assert.AreEqual(
                String.Format(PositionGroup.TO_STRING_FORMAT, group, positionDescription, businessUnit,
                    businessUnitDescription, sapCompanyCode.Description, sapPositionGroupKey), target.ToString());
        }
    }
}
