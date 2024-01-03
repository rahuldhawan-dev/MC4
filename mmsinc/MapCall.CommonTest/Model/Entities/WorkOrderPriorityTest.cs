﻿using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WorkOrderPriorityTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var description = "this is the description";
            var target = new WorkOrderPriority {Description = description};

            Assert.AreEqual(description, target.ToString());
        }
    }
}
