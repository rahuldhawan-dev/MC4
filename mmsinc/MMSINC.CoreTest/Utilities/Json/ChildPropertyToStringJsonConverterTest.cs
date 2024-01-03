using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Newtonsoft.Json;

namespace MMSINC.CoreTest.Utilities.Json
{
    [TestClass]
    public class ChildPropertyToStringJsonConverterTest
    {
        [TestMethod]
        public void TestJsonDisplaysChildPropertyForEntity()
        {
            var planningPlant = new PlanningPlant {Code = "D219"};

            var target = JsonConvert.SerializeObject(new TestJsonConverter {PlanningPlant = planningPlant});

            Assert.AreEqual("{\"PlanningPlant\":\"D219\"}", target);
        }

        [TestMethod]
        public void TestBadPropertyThrowsException()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(() => JsonConvert.SerializeObject(
                new TestJsonCoverterWithBadProperty
                    {PlanningPlant = new PlanningPlant()}));
        }

        private class TestJsonConverter
        {
            [JsonConverter(typeof(MMSINC.Utilities.Json.ChildPropertyToStringJsonConverter), "Code")]
            public virtual PlanningPlant PlanningPlant { get; set; }
        }

        private class TestJsonCoverterWithBadProperty
        {
            [JsonConverter(typeof(MMSINC.Utilities.Json.ChildPropertyToStringJsonConverter), "BadProperty")]
            public virtual PlanningPlant PlanningPlant { get; set; }
        }
    }
}
