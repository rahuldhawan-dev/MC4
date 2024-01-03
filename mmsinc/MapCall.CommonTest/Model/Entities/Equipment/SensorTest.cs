using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SensorTest
    {
        [TestMethod]
        public void TestToStringReturnsProjectSiteBoardSensorNameAndSensorDescriptionAndLocation()
        {
            var target = new Sensor();
            target.Name = "SensorName";
            target.Description = "SensorDescription";
            target.Location = "SensorLocation";
            target.Board = new Board {
                Name = "BoardName",
                Site = new Site {
                    Name = "SiteName",
                    Project = new Project {
                        Name = "ProjectName"
                    }
                }
            };

            Assert.AreEqual("ProjectName - SiteName - BoardName - SensorName - SensorDescription - SensorLocation",
                target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsAllTheVariousValuesWithTrimmedWhiteSpace()
        {
            var target = new Sensor();
            target.Name = "SensorName         ";
            target.Description = "SensorDescription               ";
            target.Location = "SensorLocation      ";
            target.Board = new Board {
                Name = "BoardName           ",
                Site = new Site {
                    Name = "SiteName          ",
                    Project = new Project {
                        Name = "ProjectName                "
                    }
                }
            };

            Assert.AreEqual("ProjectName - SiteName - BoardName - SensorName - SensorDescription - SensorLocation",
                target.ToString());
        }
    }
}
