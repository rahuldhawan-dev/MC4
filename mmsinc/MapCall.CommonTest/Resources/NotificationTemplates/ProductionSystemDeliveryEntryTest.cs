using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ProductionSystemDeliveryEntryTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestSystemDeliveryEntryValidated()
        {
            var operatingCenter = new OperatingCenter {
                OperatingCenterName = "The best operating center"
            };

            var facility = new Facility {
                FacilityName = "The best facility",
                OperatingCenter = operatingCenter
            };

            var employee = new Employee {
                FirstName = "Michael",
                LastName = "The Demon"
            };

            var systemdeliveryentry = new SystemDeliveryEntry {
                Id = 1,
                EnteredBy = employee
            };

            var entryDate = new DateTime(2021, 3, 1);
            for (var i = 0; i <= 6; i++)
            {
                systemdeliveryentry.FacilityEntries.Add(new SystemDeliveryFacilityEntry {
                    SystemDeliveryEntry = systemdeliveryentry,
                    Facility = facility,
                    EntryDate = entryDate.AddDays(i),
                    EntryValue = 3.14m
                });
            }
            
            systemdeliveryentry.Facilities.Add(facility);
            systemdeliveryentry.OperatingCenters.Add(operatingCenter);

            var model = new SystemDeliveryEntryNotification {
                RecordUrl = "www.test.com",
                Entity = systemdeliveryentry,
                OperatingCenter = operatingCenter
            };
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.SystemDeliveryApprover.SystemDeliveryEntryValidation.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"

<h2>System Delivery Entry has been completed</h2>

<a href=""www.test.com"">1</a><br />

Operating Center:  - The best operating center <br />

Facilities: The best facility <br />

Entry Dates: 3/1/2021,3/2/2021,3/3/2021,3/4/2021,3/5/2021,3/6/2021,3/7/2021 <br />

Entered By: Michael The Demon <br />";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSystemDeliveryEntryAdjustmentNotification()
        {
            var operatingCenter = new OperatingCenter {
                OperatingCenterName = "The best operating center"
            };

            var facility = new Facility {
                FacilityName = "The best facility",
                OperatingCenter = operatingCenter
            };

            var employee = new Employee {
                FirstName = "Michael",
                LastName = "The Demon"
            };

            var systemDeliveryEntry = new SystemDeliveryEntry {
                Id = 1,
                EnteredBy = employee
            };

            var entryDate = new DateTime(2021, 3, 1);
            for (var i = 0; i <= 6; i++)
            {
                systemDeliveryEntry.FacilityEntries.Add(new SystemDeliveryFacilityEntry {
                    SystemDeliveryEntry = systemDeliveryEntry,
                    Facility = facility,
                    EntryDate = entryDate.AddDays(i),
                    EntryValue = 3.14m
                });
            }
            
            systemDeliveryEntry.Facilities.Add(facility);
            systemDeliveryEntry.OperatingCenters.Add(operatingCenter);

            var model = new SystemDeliveryFacilityEntryAdjustment {
                SystemDeliveryFacilityEntry = systemDeliveryEntry.FacilityEntries.First(),
                SystemDeliveryEntry = systemDeliveryEntry,
                EnteredBy = employee,
                OriginalEntryValue = systemDeliveryEntry.FacilityEntries.First().EntryValue,
                AdjustedDate = systemDeliveryEntry.FacilityEntries.First().EntryDate,
                AdjustedEntryValue = 1.5m,
                DateTimeEntered = new DateTime(2021, 4, 2)
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.SystemDeliveryEntry.SystemDeliveryEntryAdjustment.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>System Delivery Entry has been adjusted</h2>

<a href=""https://mapcall.awapps.com/Modules/mvc/production/systemdeliveryentry/show/1"">1</a><br />

Date: 3/1/2021 12:00:00 AM <br />

Original Entry Value: 3.14 <br />

Adjusted Value: 1.5 <br />

Operating Center:  - The best operating center <br />

Facility: The best facility - -0 <br />

Entered By: Michael The Demon <br />";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSystemDeliveryEntryDueNotification()
        {
            var operatingCenter = new OperatingCenter {
                OperatingCenterName = "The best operating center"
            };

            var facility = new Facility {
                FacilityName = "The best facility",
                OperatingCenter = operatingCenter
            };

            var model = new SystemDeliveryEntryDueNotification {
                Facility = facility,
                OperatingCenter = operatingCenter
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Production.SystemDeliveryEntry.SystemDeliveryEntryDue.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>System Delivery Entry is due for the following</h2>

Operating Center:  - The best operating center <br />

Facility: The best facility - -0 <br />";

            Assert.AreEqual(expected, template);
        }
    }
}
