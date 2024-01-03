using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class InterconnectionTest
    {
        [TestMethod]
        public void TestLatitudeReturnsCoordinateLatitudeLongitudeIfNotNull()
        {
            var coordinate = new Coordinate {Latitude = 43, Longitude = -74};
            var interconnection = new Interconnection {Coordinate = coordinate};

            Assert.AreEqual(coordinate.Latitude, interconnection.Latitude);
            Assert.AreEqual(coordinate.Longitude, interconnection.Longitude);
        }

        [TestMethod]
        public void TestLatitudeReturnsNullIfCoordinateNull()
        {
            var interconnection = new Facility();

            Assert.IsNull(interconnection.Latitude);
            Assert.IsNull(interconnection.Longitude);
        }
    }
}
