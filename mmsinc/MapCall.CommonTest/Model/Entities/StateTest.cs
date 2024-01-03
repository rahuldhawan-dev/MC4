using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void TestToStringReturnsAbbreviation()
        {
            var state = new State {Abbreviation = "QQQ"};

            Assert.AreEqual(state.Abbreviation, state.ToString());
        }
    }
}
