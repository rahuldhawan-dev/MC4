using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class EnvironmentalPermitTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestEnvironmentalPermitExpirationNotification()
        {
            var now = DateTime.Now;

            var operatingCenters = new List<OperatingCenter> {
                new OperatingCenter {
                    OperatingCenterCode = "NJ7"
                },
                new OperatingCenter {
                    OperatingCenterCode = "NJ4"
                },
            };
            var permitType = new EnvironmentalPermitType {
                Description = "foo"
            };
            var publicWaterSupply = new PublicWaterSupply {
                Identifier = "123123"
            };
            var facilities = new List<Facility> {
                new Facility {
                    Id = 1,
                    OperatingCenter = operatingCenters.First(),
                    FacilityName = "foo"
                },
                new Facility {
                    Id = 2,
                    OperatingCenter = operatingCenters.Last(),
                    FacilityName = "bar"
                }
            };

            var model = new EnvironmentalPermit {
                Id = 42,
                PermitNumber = "12345",
                PermitExpirationDate = now,
                PermitRenewalDate = now,
                Description = "This is a permit to see the things.",
                OperatingCenters = operatingCenters,
                EnvironmentalPermitType = permitType,
                PublicWaterSupply = publicWaterSupply,
                Facilities = facilities,
                RecordUrl = "http://recordUrl"
            };

            var template = RenderTemplate(
                "MapCall.Common.Resources.NotificationTemplates.Environmental.PermitTypesExpiration.EnvironmentalPermitExpiration.cshtml", 
                model);

            MyAssert.StringsAreEqual(
                $@"Permit Number: <a href=""http://recordUrl"">{model.PermitNumber}</a><br/>
Expiration Date: {model.PermitExpirationDate} <br/>
Renewal Date: {model.PermitRenewalDate} <br/>
Description: {model.Description} <br/>
Operating Centers: {operatingCenters.First()}, {operatingCenters.Last()} <br/>
Permit Type: {permitType} <br/>
PWSID: {publicWaterSupply} <br/>
Facilities: {facilities.First()}, {facilities.Last()}", template);
        }
    }
}
