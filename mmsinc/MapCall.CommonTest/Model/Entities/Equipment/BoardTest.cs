using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestToStringReturnsBoardName()
        {
            var target = new Board();
            target.Name = "Name";
            Assert.AreEqual("Name", target.ToString());
        }

        [TestMethod]
        public void TestToStringTrimsWhiteSpaceOffBoardNameValue()
        {
            var target = new Board();
            target.Name = "Name          ";
            Assert.AreEqual("Name", target.ToString());
        }
    }
}
