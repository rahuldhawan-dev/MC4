using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ProductionFacilitiesTest : BaseNotificationTest
    {
        private const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Production.Facilities.{0}.cshtml";

        private const string EQUIPMENT_NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.{0}.cshtml";

        private const string INTERCONNECTIONS_NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Production.Interconnections.{0}.cshtml";

        private ProductionPrerequisite[] permit = new ProductionPrerequisite[3] {
            new ProductionPrerequisite {Description = "Air Permit"},
            new ProductionPrerequisite {Description = "Job Safety Checklist"},
            new ProductionPrerequisite {Description = "Has Lockout Requirement"}
        };

        [TestMethod]
        public void TestInterconnectionContractEndsNotification()
        {
            var model = new Interconnection {
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                RecordUrl = "http://recordUrl"
            };
            var template = RenderTemplate(INTERCONNECTIONS_NOTIFICATION_PATH_FORMAT, "InterconnectionContractEnds", model);

            Assert.AreEqual(@"Interconnection ID: <a href=""http://recordUrl"">0</a><br />


Operating Center: QQ1 - OpCenter Name <br />
Facility: This is the facility name <br />


Contract Start Date:  <br />
Contract End Date:  <br />", template);

            // Also test that this renders if the Facility is null for some reason.
            model.Facility = null;

            template = RenderTemplate(INTERCONNECTIONS_NOTIFICATION_PATH_FORMAT, "InterconnectionContractEnds", model);

            Assert.AreEqual(@"Interconnection ID: <a href=""http://recordUrl"">0</a><br />


Contract Start Date:  <br />
Contract End Date:  <br />", template);
        }

        [TestMethod]
        public void TestProductionFacilitiesEquipmentInServiceNotification()
        {
            const string templateName = "EquipmentInService";
            var model = new Common.Model.Entities.Equipment {
                Id = 2,
                Number = 123,
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                //  Identifier = "123456",
                Description = "PAHP-Truck Took Kit Hydraulic Power Unit",
                WBSNumber = "R24-62P1.19-P-0002",
                SerialNumber = "9999999034"
            };

            const string strFacilityDepartment =
                "    \r\n    Department:  <br />\r\n    Facility: This is the facility name - QQ1-0 <br />\r\n    \r\n";
            const string strProductionPrerequisites =
                "Prerequisites: Air Permit; Job Safety Checklist; Has Lockout Requirement <br />";
            var template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            const string strIdentifier = "Identifier: QQ1-0--2 <br />\r\n";
            var expected = "<a>Equipment Placed In Service</a> <br />\r\n\r\nId: 2 <br />\r\n" +
                           strFacilityDepartment +
                           strIdentifier +
                           "Description: " + model.Description + " <br />\r\n" +
                           "WBS Number: " + model.WBSNumber + " <br />\r\n" +
                           "Equipment Manufacturer:  <br />\r\n" +
                           "Model:  <br />\r\nSAP Equipment Id:  <br />\r\n" +
                           "ABC Indicator:  <br />\r\n" +
                           "Serial Number: " + model.SerialNumber + " <br />\r\n\r\n" +
                           "Number: " + model.Number + " <br />\r\n" +
                           "Critical Rating:  <br />\r\n" +
                           "Date Installed:  <br />\r\n" +
                           "Prerequisites:  <br />\r\n" +
                           "Process Safety Management: False <br />\r\n" +
                           "Company Requirement: False <br />\r\n" +
                           "Environmental / Water Quality Regulatory: False <br />\r\n" +
                           "OSHA Requirement: False <br />\r\n" +
                           "Other Requirement: False <br />\r\n" +
                           "\r\nSafety Notes:  <br />\r\n" +
                           "Maintenance Notes:  <br />\r\n" +
                           "Operation Notes: \r\n";

            Assert.AreEqual(expected, template);
            //Test for Production Prerequisites is not null
            model.ProductionPrerequisites = permit;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            Assert.AreEqual(expected.Replace("Prerequisites:  <br />", strProductionPrerequisites), template);

            //Test in case Facility is null
            model.Facility = null;
            model.ProductionPrerequisites = null;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            expected = expected.Replace(strIdentifier, "Identifier: --2 <br />\r\n");
            Assert.AreEqual(expected.Replace(strFacilityDepartment, string.Empty), template);
        }

        [TestMethod]
        public void TestProductionFacilitiesEquipmentRecordCreatedNotification()
        {
            const string templateName = "EquipmentRecordCreated";
            var model = new Common.Model.Entities.Equipment {
                Id = 2,
                Number = 123,
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                //    Identifier = "123456",
                Description = "PAHP-Truck Took Kit Hydraulic Power Unit",
                WBSNumber = "R24-62P1.19-P-0002",
                SerialNumber = "9999999034",
                IsReplacement = false,
                CreatedBy = new User {UserName = "userid", FullName = "Captain Morgan"}
            };

            const string strProductionPrerequisites =
                "Prerequisites: Air Permit; Job Safety Checklist; Has Lockout Requirement <br />";
            var strWbsNumber = "    \r\n    " + "WBS Number: " + model.WBSNumber + " <br />\r\n    \r\n";
            var strFacilityDepartment =
                "    \r\n    Department:  <br />\r\n    Facility: This is the facility name - QQ1-0 <br />\r\n    \r\n";
            const string strIdentifier = "Identifier: QQ1-0--2 <br />\r\n";
            var template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            var expected = "<a>New Equipment Record Added</a> <br />\r\n\r\nId: 2 <br />\r\n" +
                           "Created By: " + model.CreatedBy + "\r\n" +
                           strFacilityDepartment +
                           strIdentifier +
                           "Description: " + model.Description + " <br />\r\n" +
                           "WBS Number: " + model.WBSNumber + " <br />\r\n" +
                           "Status:  <br />\r\n" +
                           "Equipment Manufacturer:  <br />\r\n" +
                           "Model:  <br />\r\nSAP Equipment Id:  <br />\r\n" +
                           "Is Replacement: False\r\n" +
                           "SAP Equipment Replaced: " + "\r\n" +
                           "ABC Indicator:  <br />\r\n" +
                           "Serial Number: " + model.SerialNumber + " <br />\r\n\r\n" +
                           "Number: " + model.Number + " <br />\r\n" +
                           "Critical Rating:  <br />\r\n" +
                           "Date Installed:  <br />\r\n" +
                           "Prerequisites:  <br />\r\n" +
                           "Process Safety Management: False <br />\r\n" +
                           "Company Requirement: False <br />\r\n" +
                           "Environmental / Water Quality Regulatory: False <br />\r\n" +
                           "OSHA Requirement: False <br />\r\n" +
                           "Other Requirement: False <br />\r\n" +
                           "\r\nSafety Notes:  <br />\r\n" +
                           "Maintenance Notes:  <br />\r\n" +
                           "Operation Notes: \r\n";

            Assert.AreEqual(expected, template);
            //Test for Production Prerequisites is not null
            model.ProductionPrerequisites = permit;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            Assert.AreEqual(expected.Replace("Prerequisites:  <br />", strProductionPrerequisites), template);
            //Test in case Facility is null
            model.Facility = null;
            model.ProductionPrerequisites = null;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            expected = expected.Replace(strIdentifier, "Identifier: --2 <br />\r\n");
            Assert.AreEqual(expected.Replace(strFacilityDepartment, string.Empty), template);
        }

        [TestMethod]
        public void TestProductionFacilitiesFieldInstalledNotification()
        {
            const string templateName = "FieldInstalled";
            var model = new Common.Model.Entities.Equipment {
                Id = 2,
                Number = 123,
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                Description = "PAHP-Truck Took Kit Hydraulic Power Unit",
                SerialNumber = "9999999034",
                IsReplacement = false
            };

            const string strProductionPrerequisites =
                "Prerequisites: Air Permit; Job Safety Checklist; Has Lockout Requirement <br />";
            const string strIdentifier = "Identifier: QQ1-0--2 <br />\r\n";
            var template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            string expected =
                "<a>Equipment Field Installed</a> <br />\r\n\r\nId: 2 <br />\r\n    \r\n    Department:  <br />\r\n    Facility: This is the facility name - QQ1-0 <br />\r\n    \r\n" +
                strIdentifier +
                "Description: PAHP-Truck Took Kit Hydraulic Power Unit <br />\r\nEquipment Manufacturer:  <br />\r\nModel:  <br />\r\n" +
                "SAP Equipment Id:  <br />\r\nABC Indicator:  <br />\r\nSerial Number: 9999999034 <br />\r\n\r\nNumber: 123 <br />\r\nCritical Rating:  <br />\r\nDate Installed:  <br />\r\n" +
                "Prerequisites:  <br />\r\nProcess Safety Management: False <br />\r\nCompany Requirement: False <br />\r\nEnvironmental / Water Quality Regulatory: False <br />" +
                "\r\nOSHA Requirement: False <br />\r\nOther Requirement: False <br />\r\n\r\nSafety Notes:  <br />\r\nMaintenance Notes:  <br />\r\nOperation Notes: \r\n";
            Assert.AreEqual(expected, template);
            //Test for Production Prerequisites is not null
            model.ProductionPrerequisites = permit;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            Assert.AreEqual(expected.Replace("Prerequisites:  <br />", strProductionPrerequisites), template);
        }

        [TestMethod]
        public void TestProductionFacilitiesEquipmentPendingRetirementNotification()
        {
            const string templateName = "PendingRetirement";
            var model = new Common.Model.Entities.Equipment {
                Id = 2,
                Number = 123,
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                Description = "PAHP-Truck Took Kit Hydraulic Power Unit",
                SerialNumber = "9999999034",
                IsReplacement = false
            };
            const string strProductionPrerequisites =
                "Prerequisites: Air Permit; Job Safety Checklist; Has Lockout Requirement <br />";
            const string strIdentifier = "Identifier: QQ1-0--2 <br />\r\n";
            var template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            string expected =
                "<a>Equipment Pending Retirement</a> <br />\r\n\r\nId: 2 <br />\r\n    \r\n    Department:  <br />\r\n    Facility: This is the facility name - QQ1-0 <br />\r\n    \r\n" +
                strIdentifier +
                "Description: PAHP-Truck Took Kit Hydraulic Power Unit <br />\r\nWBS Number:  <br/>\r\nEquipment Manufacturer:  <br />\r\nModel:  <br />\r\n" +
                "SAP Equipment Id:  <br />\r\nABC Indicator:  <br />\r\nSerial Number: 9999999034 <br />\r\n\r\nNumber: 123 <br />\r\nCritical Rating:  <br />\r\nDate Installed:  <br />\r\n" +
                "Prerequisites:  <br />\r\nProcess Safety Management: False <br />\r\nCompany Requirement: False <br />\r\nEnvironmental / Water Quality Regulatory: False <br />\r\n" +
                "OSHA Requirement: False <br />\r\nOther Requirement: False <br />\r\n\r\nSafety Notes:  <br />\r\nMaintenance Notes:  <br />\r\nOperation Notes: \r\n";
            Assert.AreEqual(expected, template);
            //Test for Production Prerequisites is not null
            model.ProductionPrerequisites = permit;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            Assert.AreEqual(expected.Replace("Prerequisites:  <br />", strProductionPrerequisites), template);
        }

        [TestMethod]
        public void TestProductionFacilitiesEquipmentRetiredNotification()
        {
            const string templateName = "Retired";
            var model = new Common.Model.Entities.Equipment {
                Id = 2,
                Number = 123,
                Facility = new Facility {
                    FacilityName = "This is the facility name",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "QQ1", OperatingCenterName = "OpCenter Name"}
                },
                Description = "PAHP-Truck Took Kit Hydraulic Power Unit",
                SerialNumber = "9999999034",
                IsReplacement = false
            };
            const string strProductionPrerequisites =
                "Prerequisites: Air Permit; Job Safety Checklist; Has Lockout Requirement <br />";
            const string strIdentifier = "Identifier: QQ1-0--2 <br />\r\n";
            var template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            string expected =
                "<a>Equipment Retired</a> <br />\r\n\r\nId: 2 <br />\r\n    \r\n    Department:  <br />\r\n    Facility: This is the facility name - QQ1-0 <br />\r\n    \r\n" +
                strIdentifier +
                "Description: PAHP-Truck Took Kit Hydraulic Power Unit <br />\r\nWBS Number:  <br/>\r\nEquipment Manufacturer:  <br />\r\nModel:  <br />\r\n" +
                "SAP Equipment Id:  <br />\r\nABC Indicator:  <br />\r\nSerial Number: 9999999034 <br />\r\n\r\nNumber: 123 <br />\r\nCritical Rating:  <br />\r\nDate Installed:  <br />\r\n" +
                "Prerequisites:  <br />\r\nPSMTCPA:  <br />\r\n\r\nSafety Notes:  <br />\r\nMaintenance Notes:  <br />\r\nOperation Notes: \r\n";
            Assert.AreEqual(expected, template);
            //Test for Production Prerequisites is not null
            model.ProductionPrerequisites = permit;
            template = RenderTemplate(EQUIPMENT_NOTIFICATION_PATH_FORMAT, templateName, model);
            Assert.AreEqual(expected.Replace("Prerequisites:  <br />", strProductionPrerequisites), template);
        }

        [TestMethod]
        public void TestHumanResourcesFacilityArcFlashStudyExpirationNotification()
        {
            const string templateName = "ArcFlashStudyExpiresIn1Year";

            var emailDate = DateTime.Now.ToShortDateString();

            var model = new ArcFlashStudy {
                Id = 1,
                RecordUrl = "http://recordUrl"
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, templateName, model);
            string expected =
                $"<a href=\"http://recordUrl\">1</a> <br />\r\n\r\n<text>\r\n    The ArcFlash Study 1 will expire 1 year from {emailDate} please start your new study.\r\n\r\n</text>";

            Assert.AreEqual(expected, template);
        }
    }
}
