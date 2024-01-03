using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class EquipmentTest : BaseNotificationTest
    {
        #region Pending Retirement

        [TestMethod]
        public void TestPendingRetirementNotification()
        {
            var opCenter = new OperatingCenter { OperatingCenterCode = "NJ7" };
            var department = new Department { Description = "The Departedment" };
            var facility = new Facility {
                Id = 1,
                OperatingCenter = opCenter,
                FacilityName = "foo",
                Department = department
            };
            var dateInstalled = new DateTime(2021, 06, 06, 06, 06, 06);
            var type = new EquipmentType { Description = "fooFoo" };
            var purpose = new EquipmentPurpose {
                Description = "foo",
                EquipmentType = type
            };
            var equipmentManufacturer = new EquipmentManufacturer { Description = "SAP eq maker" };
            var abcIndicator = new ABCIndicator { Description = "HIGH" };
            var model = new Equipment {
                RecordUrl = "https://234231",
                Id = 12321,
                Facility = facility,
                WBSNumber = "12345",
                SerialNumber = "12309876",
                Description = "keyboard with port and starboard attachments",
                EquipmentManufacturer = equipmentManufacturer,
                EquipmentPurpose = purpose,
                CriticalRating = 99,
                Number = 999,
                DateInstalled = dateInstalled,
                SAPEquipmentId = 9999,
                PSMTCPA = false,
                SafetyNotes = "do not type Scary Gary three times fast while looking into a mirror",
                MaintenanceNotes = "wash 10x daily",
                OperationNotes = "always use caps lock",
                ABCIndicator = abcIndicator
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.PendingRetirement.cshtml";
            var template = RenderTemplate(streamPath, model);

            MyAssert.StringsAreEqual(
                $@"<a href=""https://234231"">Equipment Pending Retirement</a> <br />

Id: 12321 <br />
    
    Department: The Departedment <br />
    Facility: foo - NJ7-1 <br />
    
Identifier: NJ7-1--12321 <br />
Description: keyboard with port and starboard attachments <br />
WBS Number: 12345 <br/>
    
        Type:  - 0 : - foo -  -  <br />
        SAP Type:  - fooFoo <br />
    
Equipment Manufacturer: SAP eq maker <br />
Model:  <br />
SAP Equipment Id: 9999 <br />
ABC Indicator: HIGH <br />
Serial Number: 12309876 <br />

Number: 999 <br />
Critical Rating: 99 <br />
Date Installed: 6/6/2021 6:06:06 AM <br />
Prerequisites:  <br />
Process Safety Management: False <br />
Company Requirement: False <br />
Environmental / Water Quality Regulatory: False <br />
OSHA Requirement: False <br />
Other Requirement: False <br />

Safety Notes: do not type Scary Gary three times fast while looking into a mirror <br />
Maintenance Notes: wash 10x daily <br />
Operation Notes: always use caps lock
", template);
        }

        [TestMethod]
        public void TestPendingRetirementNotificationMostlyNull()
        {
            var opCenter = new OperatingCenter { OperatingCenterCode = "NJ7" };
            var department = new Department { Description = "The Departedment" };
            var facility = new Facility {
                    Id = 1,
                    OperatingCenter = opCenter,
                    FacilityName = "foo",
                    Department = department
            };
            var type = new EquipmentType { Description = "fooFoo" };
            var purpose = new EquipmentPurpose {
                Description = "foo",
                EquipmentType = type
            };
            var equipmentManufacturer = new EquipmentManufacturer { Description = "SAP eq maker" };
            var model = new Equipment {
                RecordUrl = "https://234231",
                Id = 12321,
                Facility = facility,
                WBSNumber = "12345",
                SerialNumber = "12309876",
                Description = "keyboard with port and starboard attachments",
                EquipmentManufacturer = equipmentManufacturer,
                EquipmentPurpose = purpose,
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.PendingRetirement.cshtml";
            var template = RenderTemplate(streamPath, model);

            MyAssert.StringsAreEqual(
                $@"<a href=""https://234231"">Equipment Pending Retirement</a> <br />

Id: 12321 <br />
    
    Department: The Departedment <br />
    Facility: foo - NJ7-1 <br />
    
Identifier: NJ7-1--12321 <br />
Description: keyboard with port and starboard attachments <br />
WBS Number: 12345 <br/>
    
        Type:  - 0 : - foo -  -  <br />
        SAP Type:  - fooFoo <br />
    
Equipment Manufacturer: SAP eq maker <br />
Model:  <br />
SAP Equipment Id:  <br />
ABC Indicator:  <br />
Serial Number: 12309876 <br />

Number:  <br />
Critical Rating:  <br />
Date Installed:  <br />
Prerequisites:  <br />
Process Safety Management: False <br />
Company Requirement: False <br />
Environmental / Water Quality Regulatory: False <br />
OSHA Requirement: False <br />
Other Requirement: False <br />

Safety Notes:  <br />
Maintenance Notes:  <br />
Operation Notes: 
", template);
        }

        #endregion

        #region Retired

        [TestMethod]
        public void TestRetiredNotification()
        {
            var opCenter = new OperatingCenter { OperatingCenterCode = "NJ7" };
            var department = new Department { Description = "The Departedment" };
            var facility = new Facility {
                Id = 1,
                OperatingCenter = opCenter,
                FacilityName = "foo",
                Department = department
            };
            var dateInstalled = new DateTime(2021, 06, 06, 06, 06, 06);
            var type = new EquipmentType { Description = "fooFoo" };
            var purpose = new EquipmentPurpose {
                Description = "foo",
                EquipmentType = type
            };
            var equipmentManufacturer = new EquipmentManufacturer { Description = "SAP eq maker" };
            var abcIndicator = new ABCIndicator { Description = "HIGH" };
            var model = new Equipment {
                RecordUrl = "https://234231",
                Id = 12321,
                Facility = facility,
                WBSNumber = "12345",
                SerialNumber = "12309876",
                Description = "keyboard with port and starboard attachments",
                EquipmentManufacturer = equipmentManufacturer,
                EquipmentPurpose = purpose,
                CriticalRating = 99,
                Number = 999,
                DateInstalled = dateInstalled,
                SAPEquipmentId = 9999,
                PSMTCPA = false,
                SafetyNotes = "do not type Scary Gary three times fast while looking into a mirror",
                MaintenanceNotes = "wash 10x daily",
                OperationNotes = "always use caps lock",
                ABCIndicator = abcIndicator
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.Retired.cshtml";
            var template = RenderTemplate(streamPath, model);

            MyAssert.StringsAreEqual(
                $@"<a href=""https://234231"">Equipment Retired</a> <br />

Id: 12321 <br />
    
    Department: The Departedment <br />
    Facility: foo - NJ7-1 <br />
    
Identifier: NJ7-1--12321 <br />
Description: keyboard with port and starboard attachments <br />
WBS Number: 12345 <br/>
    
        Type:  - 0 : - foo -  -  <br />
        SAP Type:  - fooFoo <br />
    
Equipment Manufacturer: SAP eq maker <br />
Model:  <br />
SAP Equipment Id: 9999 <br />
ABC Indicator: HIGH <br />
Serial Number: 12309876 <br />

Number: 999 <br />
Critical Rating: 99 <br />
Date Installed: 6/6/2021 6:06:06 AM <br />
Prerequisites:  <br />
PSMTCPA: False <br />

Safety Notes: do not type Scary Gary three times fast while looking into a mirror <br />
Maintenance Notes: wash 10x daily <br />
Operation Notes: always use caps lock
", template);
        }

        [TestMethod]
        public void TestRetiredNotificationMostlyNull()
        {
            var opCenter = new OperatingCenter { OperatingCenterCode = "NJ7" };
            var department = new Department { Description = "The Departedment" };
            var facility = new Facility {
                Id = 1,
                OperatingCenter = opCenter,
                FacilityName = "foo",
                Department = department
            };
            var type = new EquipmentType { Description = "fooFoo" };
            var purpose = new EquipmentPurpose {
                Description = "foo",
                EquipmentType = type
            };
            var equipmentManufacturer = new EquipmentManufacturer { Description = "SAP eq maker" };
            var model = new Equipment {
                RecordUrl = "https://234231",
                Id = 12321,
                Facility = facility,
                WBSNumber = "12345",
                SerialNumber = "12309876",
                Description = "keyboard with port and starboard attachments",
                EquipmentManufacturer = equipmentManufacturer,
                EquipmentPurpose = purpose
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.Equipment.Retired.cshtml";
            var template = RenderTemplate(streamPath, model);

            MyAssert.StringsAreEqual(
                $@"<a href=""https://234231"">Equipment Retired</a> <br />

Id: 12321 <br />
    
    Department: The Departedment <br />
    Facility: foo - NJ7-1 <br />
    
Identifier: NJ7-1--12321 <br />
Description: keyboard with port and starboard attachments <br />
WBS Number: 12345 <br/>
    
        Type:  - 0 : - foo -  -  <br />
        SAP Type:  - fooFoo <br />
    
Equipment Manufacturer: SAP eq maker <br />
Model:  <br />
SAP Equipment Id:  <br />
ABC Indicator:  <br />
Serial Number: 12309876 <br />

Number:  <br />
Critical Rating:  <br />
Date Installed:  <br />
Prerequisites:  <br />
PSMTCPA:  <br />

Safety Notes:  <br />
Maintenance Notes:  <br />
Operation Notes: 
", template);
        }

        #endregion
    }
}
