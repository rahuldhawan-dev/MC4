using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsIncidentsTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Operations.Incidents.{0}.cshtml";

        #region Tests

        #region HS Incident

        [TestMethod]
        public void TestHSIncidentNotification()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                Employee = new Employee { EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q" },
                EmployeeType = new EmployeeType { Id = 1, Description = "Employee"},
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision {Description = "A Test"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncident", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: OJ - Simpson <br />

    Employee Name: Mr. Q Jones <br />
    Employee ID: 123456 <br />
  
Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        [TestMethod]
        public void TestHSIncidentNotificationWhenParametersAreNull()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                Employee = new Employee { EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q" },
                EmployeeType = new EmployeeType { Id = 1, Description = "Employee" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision {Description = "A Test"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncident", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed:  <br />
Operating Center: OJ - Simpson <br />

    Employee Name: Mr. Q Jones <br />
    Employee ID: 123456 <br />
  
Incident Classification: Classification of incidents <br />
Incident Type:  <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        [TestMethod]
        public void TestHSIncidentForContractorNotification()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                ContractorCompany = "Flip Liquid",
                ContractorName = "Flippy Zipply",
                EmployeeType = new EmployeeType { Id = 2, Description = "Contractor" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncident", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: OJ - Simpson <br />

    Contractor Name: Flippy Zipply <br />
    Contractor Company: Flip Liquid <br />

Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        #endregion

        #region HS Incident OSHA Recordable

        [TestMethod]
        public void TestHSIncidentOSHARecordableNotification()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                Employee = new Employee { EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q" },
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.EMPLOYEE, Description = "Employee" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision {Description = "A Test"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentOSHARecordable", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

<strong>Is OSHA Recordable: Yes</strong>
<br />
a : 1/1/2014 1:01:01 AM
<br />
Incident ID: 42
<br />
Incident Date: 1/1/2024 1:01:01 AM
<br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM
<br />
Operating Center: OJ - Simpson
<br />
        
            Employee Name: Mr. Q Jones
            <br />
            Employee ID: 123456
            <br />
        
Incident Classification: Classification of incidents
<br />
Incident Type: Type of incident
<br />
Any Immediate Corrective Actions Applied: 
<br />
Sought Medical Attention: No", template);
        }

        [TestMethod]
        public void TestHSIncidentOSHARecordableForContractorNotification()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                ContractorCompany = "Flip Liquid",
                ContractorName = "Flippy Zipply",
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.CONTRACTOR, Description = "Contractor" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentOSHARecordable", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

<strong>Is OSHA Recordable: Yes</strong>
<br />
a : 1/1/2014 1:01:01 AM
<br />
Incident ID: 42
<br />
Incident Date: 1/1/2024 1:01:01 AM
<br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM
<br />
Operating Center: OJ - Simpson
<br />
    
        Contractor Name: Flippy Zipply
        <br />
        Contractor Company: Flip Liquid
        <br />
    
Incident Classification: Classification of incidents
<br />
Incident Type: Type of incident
<br />
Any Immediate Corrective Actions Applied: 
<br />
Sought Medical Attention: No", template);
        }

        [TestMethod]
        public void TestHSIncidentOSHARecordableNotificationWhenParametersAreNull()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "OJ", OperatingCenterName = "Simpson" },
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.EMPLOYEE, Description = "Employee" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision {Description = "A Test"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentOSHARecordable", model);

            Assert.AreEqual(@"<h2>Incident Added</h2>

<strong>Is OSHA Recordable: Yes</strong>
<br />
a : 1/1/2014 1:01:01 AM
<br />
Incident ID: 42
<br />
Incident Date: 1/1/2024 1:01:01 AM
<br />
Date Investigation Will Be Completed: 
<br />
Operating Center: OJ - Simpson
<br />
Incident Classification: Classification of incidents
<br />
Incident Type: 
<br />
Any Immediate Corrective Actions Applied: 
<br />
Sought Medical Attention: No", template);
        }

        #endregion

        #region HS Incident Classification Change

        [TestMethod]
        public void TestHSIncidentClassificationNotification()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Weird" },
                Employee = new Employee { EmployeeId = "123456", LastName = "Pie", FirstName = "Hotdog", MiddleName = "Qt" },
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.EMPLOYEE, Description = "Employee" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentClassificationUpdated", model);

            Assert.AreEqual(@"<h2>HS Incident Classification Updated</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: NJ - Weird <br />
        
            Employee Name: Hotdog Qt Pie <br/>
            Employee ID: 123456 <br/>
        
Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        [TestMethod]
        public void TestHSIncidentClassificationNotificationForContractor()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Weird" },
                Employee = new Employee { EmployeeId = "123456", LastName = "Pie", FirstName = "Hotdog", MiddleName = "Qt" },
                ContractorName = "Hotdog Q Pie",
                ContractorCompany = "Pie Co",
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.CONTRACTOR, Description = "Contractor" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentClassificationUpdated", model);

            Assert.AreEqual(@"<h2>HS Incident Classification Updated</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: NJ - Weird <br />

    Contractor Name: Hotdog Q Pie <br />
    Contractor Company: Pie Co <br />

Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        [TestMethod]
        public void TestHSIncidentClassificationNotificationNullEmployeeShouldBeImpossible()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Weird" },
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.EMPLOYEE, Description = "Employee" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentClassificationUpdated", model);

            Assert.AreEqual(@"<h2>HS Incident Classification Updated</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: NJ - Weird <br />
Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }
        
        [TestMethod]
        public void TestHSIncidentClassificationNotificationNullContractorShouldBeImpossible()
        {
            var model = new Incident {
                CreatedBy = "a",
                CreatedAt = new DateTime(2014, 1, 1, 1, 1, 1),
                IncidentDate = new DateTime(2024, 1, 1, 1, 1, 1),
                DateInvestigationWillBeCompleted = new DateTime(2043, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Weird" },
                EmployeeType = new EmployeeType { Id = EmployeeType.Indices.CONTRACTOR, Description = "Contractor" },
                IncidentClassification = new IncidentClassification { Description = "Classification of incidents" },
                IncidentType = new IncidentType { Description = "Type of incident" },
                IsOSHARecordable = true,
                SoughtMedicalAttention = false,
                DrugAndAlcoholTestingDecision = new IncidentDrugAndAlcoholTestingDecision { Description = "A Test" }
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "HSIncidentClassificationUpdated", model);

            Assert.AreEqual(@"<h2>HS Incident Classification Updated</h2>

a : 1/1/2014 1:01:01 AM <br />
Incident ID: 42 <br />
Incident Date: 1/1/2024 1:01:01 AM <br />
Date Investigation Will Be Completed: 1/1/2043 1:01:01 AM <br />
Operating Center: NJ - Weird <br />

    Contractor Name:  <br />
    Contractor Company:  <br />

Incident Classification: Classification of incidents <br />
Incident Type: Type of incident <br />
Is OSHA Recordable: Yes <br />
Any Immediate Corrective Actions Applied: <br />
Sought Medical Attention: No ", template);
        }

        #endregion

        #endregion
    }
}
