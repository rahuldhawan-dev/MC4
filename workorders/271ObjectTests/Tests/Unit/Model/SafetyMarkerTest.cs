using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SafetyMarkerTestTest
    /// </summary>
    [TestClass]
    public class SafetyMarkerTest
    {
        [TestMethod]
        public void TestAreRetrievedProperty()
        {
            var target = new SafetyMarker();

            Assert.IsFalse(target.AreRetrieved,
                "SafetyMarkers should not be marked retrieved until retrieval date has been set");

            target.MarkersRetrievedOn = DateTime.Now;

            Assert.IsTrue(target.AreRetrieved,
                "SafetyMarkers should be marked retrieved once the retrieval date has been set");
        }
    }
}
