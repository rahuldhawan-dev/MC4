using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class CutoffSawQuestionnaireTest
    {
        [TestMethod]
        public void TestWorkOrderLocationReturnsLocationInformationFromWorkOrder()
        {
            var target = new CutoffSawQuestionnaire();

            Assert.IsTrue(String.IsNullOrWhiteSpace(target.WorkOrderLocation));

            var town = new Town {ShortName = "Anytown", State = new State {Abbreviation = "NJ"}};
            var street = new Street {FullStName = "Main Street"};
            var workOrder = new WorkOrder {StreetNumber = "123", Street = street, Town = town, ZipCode = "12345"};
            target.WorkOrder = workOrder;

            Assert.AreEqual(
                String.Format(CutoffSawQuestionnaire.ADDRESS_FORMAT, workOrder.StreetAddress, workOrder.TownAddress),
                target.WorkOrderLocation);
        }
    }
}
