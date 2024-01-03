using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Models
{
    [TestClass]
    public class TaskOrderGeneratorFormTest
    {
        [TestMethod]
        public void TestTotalOfAllEstimatedCostsReturnsTheTotalOfAllTotals()
        {
            var totalContractorLaborCost = 555;
            var totalOtherCost = 16;
            var target = new TaskOrderGeneratorForm(new Container()) {
                TotalContractorLaborCost = totalContractorLaborCost,
                TotalOtherCost = totalOtherCost
            };

            Assert.AreEqual(totalContractorLaborCost + totalOtherCost, target.TotalOfAllEstimatedCosts);
        }
    }
}
