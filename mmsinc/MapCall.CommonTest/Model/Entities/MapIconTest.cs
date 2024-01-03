using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MapIconTest
    {
        [TestMethod]
        public void TestUrlReturnsPathToIconFile()
        {
            var icon = new MapIcon {
                FileName = "foo bar"
            };

            Assert.AreEqual(String.Format(MapIcon.URL_FORMAT, icon.FileName), icon.Url);
        }
    }
}
