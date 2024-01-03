using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ProjectTest
    {
        [TestMethod]
        public void TestToStringReturnsProjectName()
        {
            var target = new Project();
            target.Name = "Some Name";
            Assert.AreEqual("Some Name", target.ToString());
        }

        [TestMethod]
        public void TestToStringTrimsNameValue()
        {
            var target = new Project();
            target.Name = "Name          ";
            Assert.AreEqual("Name", target.ToString());
        }
    }
}
