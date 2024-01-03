using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SewerMainCleaningTest
    {
        [TestMethod]
        public void TestSendSAPReturnsCorrectlyForVariousScenariosWithOpening1()
        {
            var target = new SewerMainCleaning();
            target.Opening1 = new SewerOpening {OperatingCenter = new OperatingCenter {SAPEnabled = true}};

            Assert.IsTrue(target.SendToSAP);

            target.SAPNotificationNumber = "123";
            Assert.IsFalse(target.SendToSAP);

            target.SAPNotificationNumber = null;
            target.Opening1.OperatingCenter.IsContractedOperations = true;
            Assert.IsFalse(target.SendToSAP);

            target.Opening1.OperatingCenter.IsContractedOperations = false;
            target.Opening1.OperatingCenter.SAPEnabled = false;

            Assert.IsFalse(target.SendToSAP);
        }

        [TestMethod]
        public void TestSendSAPReturnsCorrectlyForVariousScenariosWithOpening2()
        {
            var target = new SewerMainCleaning();
            target.Opening2 = new SewerOpening {OperatingCenter = new OperatingCenter {SAPEnabled = true}};

            Assert.IsTrue(target.SendToSAP);

            target.SAPNotificationNumber = "123";
            Assert.IsFalse(target.SendToSAP);

            target.SAPNotificationNumber = null;
            target.Opening2.OperatingCenter.IsContractedOperations = true;
            Assert.IsFalse(target.SendToSAP);

            target.Opening2.OperatingCenter.IsContractedOperations = false;
            target.Opening2.OperatingCenter.SAPEnabled = false;

            Assert.IsFalse(target.SendToSAP);
        }
    }

    [TestClass]
    public class HydrantInspectionTest
    {
        [TestMethod]
        public void TestSendSAPReturnsCorrectlyForVariousScenarios()
        {
            var target = new HydrantInspection
                {Hydrant = new Hydrant {OperatingCenter = new OperatingCenter {SAPEnabled = true}}};

            Assert.IsTrue(target.SendToSAP);

            target.SAPNotificationNumber = "123";
            Assert.IsFalse(target.SendToSAP);

            target.SAPNotificationNumber = null;
            target.Hydrant.OperatingCenter.IsContractedOperations = true;
            Assert.IsFalse(target.SendToSAP);
        }
    }

    [TestClass]
    public class ValveInspectionTest
    {
        [TestMethod]
        public void TestSendSAPReturnsCorrectlyForVariousScenarios()
        {
            var target = new ValveInspection
                {Valve = new Valve {OperatingCenter = new OperatingCenter {SAPEnabled = true}}};

            Assert.IsTrue(target.SendToSAP);

            target.SAPNotificationNumber = "123";
            Assert.IsFalse(target.SendToSAP);

            target.SAPNotificationNumber = null;
            target.Valve.OperatingCenter.IsContractedOperations = true;
            Assert.IsFalse(target.SendToSAP);
        }
    }
}
