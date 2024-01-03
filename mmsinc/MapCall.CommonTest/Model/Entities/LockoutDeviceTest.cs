using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class LockoutDeviceTest
    {
        [TestMethod]
        public void TestToStringReturnsColorAndSerialAlongWithDescription()
        {
            var color = new LockoutDeviceColor {Description = "Blue"};
            var device = new LockoutDevice
                {LockoutDeviceColor = color, SerialNumber = "112233", Description = "Device 101"};

            Assert.AreEqual(
                String.Format(LockoutDevice.FORMAT_STRING, color.Description, device.SerialNumber, device.Description),
                device.ToString());
        }
    }
}
