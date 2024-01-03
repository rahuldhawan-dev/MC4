using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FireDistrictTest
    {
        #region ToString()

        [TestMethod]
        public void Test_ToString_ReturnsDistrictName()
        {
            var target = new FireDistrict {
                DistrictName = "I'm a name"
            };
            Assert.AreEqual(target.DistrictName, target.ToString());
        }

        #endregion

        #region CanBeDeleted

        [TestMethod]
        public void Test_CanBeDeleted_ReturnsTrue_WhenNoLinkedObjects()
        {
            Assert.IsTrue(new FireDistrict().CanBeDeleted);
        }

        [TestMethod]
        public void Test_CanBeDeleted_ReturnsFalse_WhenLinkedHydrants()
        {
            var target = new FireDistrict {
                Hydrants = new HashSet<Hydrant> { new Hydrant() }
            };

            Assert.IsFalse(target.CanBeDeleted);
        }

        [TestMethod]
        public void Test_CanBeDeleted_ReturnsFalse_WhenLinkedTowns()
        {
            var target = new FireDistrict {
                TownFireDistricts = new HashSet<FireDistrictTown> { new FireDistrictTown() }
            };

            Assert.IsFalse(target.CanBeDeleted);
        }

        #endregion
    }
}
