using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass]
    public class SAPEntityTest
    {
        [TestMethod]
        public void TestSAPPriorityReturnsCorrectIdForSAP()
        {
            var priorities = new Dictionary<string, string> {
                {"Emergency", "2"},
                {"High Priority", "3"},
                {"High", "3"},
                {"Routine", "4"},
                {"Medium", "6"},
                {"Low", "7"},
            };

            foreach (var priority in priorities)
            {
                var sapValue = new SAP.Model.Entities.SAPEntity().SAPPriority(priority.Key);
                Assert.AreEqual(priority.Value, sapValue,
                    $"{priority.Key} should have returned {priority.Value}, instead returned {sapValue}");
            }
        }

        [TestMethod]
        public void TestPurposeCodeReturnsCorrectPurposeCode()
        {
            var purposes = new Dictionary<string, string> {
                {"Customer", "I01"},
                {"Equip Reliability", "I02"},
                {"Safety", "I03"},
                {"Compliance", "I04"},
                {"Regulatory", "I05"},
                {"Seasonal", "I06"},
                {"Leak Detection", "I07"},
                {"Revenue 150-500", "I08"},
                {"Revenue 500-1000", "I09"},
                {"Revenue >1000", "I10"},
                {"Damaged/Billable", "I11"},
                {"Estimates", "I12"},
                {"Water Quality", "I13"},
                {"Asset Record Control", "I14"},
                {"Demolition", "I15"},
                {"Locate", "I16"},
                {"Clean Out", "I17"},
                {"Construction Project", "DV02"}
            };

            foreach (var purpose in purposes)
            {
                var entity = new SAP.Model.Entities.SAPEntity();
                Assert.AreEqual(purpose.Value , entity.GetPurposeCode(purpose.Key));
            }
        }
    }
}
