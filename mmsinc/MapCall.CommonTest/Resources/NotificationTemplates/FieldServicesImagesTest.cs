using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesImagesTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestAsbuiltImageCoordinateChangedNotification()
        {
            var model = new AsBuiltImage {
                Coordinate = new Coordinate {Latitude = 12, Longitude = 32},
                TaskNumber = "d",
                Town = new Town {
                    ShortName = "g",
                    County = new County {
                        Name = "f",
                        State = new State {
                            Abbreviation = "e"
                        }
                    }
                },
                TownSection = new TownSection {Name = "h"},
                ProjectName = "o",
                StreetPrefix = "i",
                Street = "j",
                StreetSuffix = "k",
                CrossStreetPrefix = "l",
                CrossStreet = "m",
                CrossStreetSuffix = "n",
                CoordinatesModifiedBy = "some guy",
                PhysicalInService = new DateTime(2020, 4, 7, 1, 0, 0),
                FileName = "As Built Image Coordinate Changed.pdf"
            };
            model.SetPropertyValueByName("Id", 42);
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.FieldServices.Images.AsBuiltImageCoordinateChanged.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>As Built Image Coordinate Changed</h2>

AsBuiltImageId: 42 <br />
Coordinates: 12, 32 <br />
Changed By User: some guy <br />
WBS #: d <br />
State: e <br />
County: f <br />
Town: g <br />
Town Section: h <br />
Project Name: o <br />
Street Prefix: i <br />
Street: j <br />
Street Suffix: k <br />
Cross Street Prefix: l <br />
Cross Street: m <br />
Cross Street Suffix: n
<br />
Physical In Service: 04/07/2020<br />
File Name: As Built Image Coordinate Changed.pdf", template);
        }

        [TestMethod]
        public void TestAsbuiltImageCoordinateChangedNotificationWithPhysicalInServiceNull()
        {
            var model = new AsBuiltImage {
                Coordinate = new Coordinate {Latitude = 12, Longitude = 32},
                TaskNumber = "d",
                Town = new Town {
                    ShortName = "g",
                    County = new County {
                        Name = "f",
                        State = new State {
                            Abbreviation = "e"
                        }
                    }
                },
                TownSection = new TownSection{Name = "h"},
                ProjectName = "o",
                StreetPrefix = "i",
                Street = "j",
                StreetSuffix = "k",
                CrossStreetPrefix = "l",
                CrossStreet = "m",
                CrossStreetSuffix = "n",
                CoordinatesModifiedBy = "some guy",
                PhysicalInService = null,
                FileName = "As Built Image Coordinate Changed.pdf"
            };
            model.SetPropertyValueByName("Id", 42);
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.FieldServices.Images.AsBuiltImageCoordinateChanged.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>As Built Image Coordinate Changed</h2>

AsBuiltImageId: 42 <br />
Coordinates: 12, 32 <br />
Changed By User: some guy <br />
WBS #: d <br />
State: e <br />
County: f <br />
Town: g <br />
Town Section: h <br />
Project Name: o <br />
Street Prefix: i <br />
Street: j <br />
Street Suffix: k <br />
Cross Street Prefix: l <br />
Cross Street: m <br />
Cross Street Suffix: n
<br />
File Name: As Built Image Coordinate Changed.pdf", template);
        }
    }
}
