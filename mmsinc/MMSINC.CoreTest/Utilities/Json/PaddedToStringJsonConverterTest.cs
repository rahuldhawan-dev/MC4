using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.Json;
using Newtonsoft.Json;

namespace MMSINC.CoreTest.Utilities.Json
{
    [TestClass]
    public class PaddedToStringJsonConverterTest
    {
        [TestMethod]
        public void TestJsonPadsTheAppropriateNumberCharacters()
        {
            var foo = new TestJsonPadding {PlannerGroup2 = 1, PlannerGroup4 = 10, PlannerGroup10 = 817};

            var target = JsonConvert.SerializeObject(foo);

            Assert.AreEqual("{\"PlannerGroup2\":\"01\",\"PlannerGroup4\":\"0010\",\"PlannerGroup10\":\"0000000817\"}",
                target);
        }

        private class TestJsonPadding
        {
            [JsonConverter(typeof(MMSINC.Utilities.Json.PaddedToStringJsonConverter), 2)]
            public virtual int? PlannerGroup2 { get; set; }

            [JsonConverter(typeof(MMSINC.Utilities.Json.PaddedToStringJsonConverter), 4)]
            public virtual int? PlannerGroup4 { get; set; }

            [JsonConverter(typeof(MMSINC.Utilities.Json.PaddedToStringJsonConverter), 10)]
            public virtual int? PlannerGroup10 { get; set; }
        }
    }
}
