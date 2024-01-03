using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class IAssetTest
    {
        [TestMethod]
        public void TestIdentifierForValve()
        {
            var valve = new Valve {ValveNumber = "123"};

            Assert.AreEqual(valve.ValveNumber, valve.Identifier);
        }

        [TestMethod]
        public void TestIdentifierForHydrant()
        {
            var hydrant = new Hydrant {HydrantNumber = "123"};

            Assert.AreEqual(hydrant.HydrantNumber, hydrant.Identifier);
        }

        [TestMethod]
        public void TestIdentifierForService()
        {
            var service = new Service {PremiseNumber = "123", ServiceNumber = 456};

            Assert.AreEqual($"p#:{service.PremiseNumber}, s#:{service.ServiceNumber}", service.Identifier);
        }

        [TestMethod]
        public void TestIdentifierForSewerLateral()
        {
            Assert.Inconclusive("test not yet written");
        }

        [TestMethod]
        public void TestIdentifierForSewerMayne()
        {
            Assert.Inconclusive("test not yet written");
        }

        [TestMethod]
        public void TestIdentifierForSewerOpening()
        {
            var opening = new SewerOpening {OpeningNumber = "123"};

            Assert.AreEqual(opening.OpeningNumber, opening.Identifier);
        }

        [TestMethod]
        public void TestIdentifierForStormCatch()
        {
            var stormCatch = new StormWaterAsset {AssetNumber = "123"};

            Assert.AreEqual(stormCatch.AssetNumber, stormCatch.Identifier);
        }

        // NOT NEEDED: equipment already has an 'Identifier' field
        // public void TestIdentifierForEquipment()

        [TestMethod]
        public void TestIdentifierForMainCrossing()
        {
            var crossing = new MainCrossing {Id = 123};

            Assert.AreEqual($"CR{crossing.Id}", crossing.Identifier);
        }
    }
}
