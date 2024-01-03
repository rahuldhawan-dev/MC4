using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for OperatingCenterTest
    /// </summary>
    [TestClass]
    public class OperatingCenterTest
    {
        [TestMethod]
        public void TestFullDescriptionPropertyReturnsOpCntrAndOpCntrName()
        {
            var target = new OperatingCenter {
                OpCntr = "FOO",
                OpCntrName = "FOO TOWNSHIP"
            };

            Assert.AreEqual(
                string.Format("{0} - {1}", target.OpCntr, target.OpCntrName),
                target.FullDescription);
        }

        [TestMethod]
        public void TestContractorsReturnsOnlyContractorsForOperatingCenterWithContractorsAccess()
        {
            var target = new OperatingCenter {
                OpCntr = "Foo",
                OpCntrName = "Foodom Township"
            };
            var contractor = new Contractor {
                ContractorsAccess = true
            };
            var contractor2 = new Contractor
            {
                ContractorsAccess = true
            };

            var conOpCntr = new ContractorOperatingCenter {
                OperatingCenter = target,
                Contractor = contractor
            };

            Assert.AreEqual(1, target.Contractors.Count);
            Assert.AreEqual(contractor, target.Contractors[0]);
        }

        [TestMethod]
        public void TestPermitUserNameReturnsPermitOMUserNameIfSet()
        {
            var expected = "foo@bar.com";
            var oc = new OperatingCenter {
                PermitsOMUserName = expected
            };

            Assert.AreEqual(expected, oc.PermitsUserName);
        }

        [TestMethod]
        public void TestPermitUserNameReturnsPermitCapitalUserNameIfPermitOMUserNameNotSet()
        {
            var expected = "foo@bar.com";
            var oc = new OperatingCenter {
                PermitsCapitalUserName = expected
            };

            Assert.AreEqual(expected, oc.PermitsUserName);
        }

        [TestMethod]
        public void TestDataCollectionMapUrlReturnsIfSet()
        {
            var expected = "https://arcgis-collector://";
            var oc = new OperatingCenter {
                DataCollectionMapUrl = expected
            };

            Assert.AreEqual(expected, oc.DataCollectionMapUrl);
        }
    }
}
