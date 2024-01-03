using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ProductionWorkOrderTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestWorkOrderCreatedNotification()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription", OrderType = new OrderType {Description = "test", Id = 1}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},
                OrderNotes = "This is a Order Note, I like order notes"
            };
            workOrder.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = workOrder,
                Equipment = new Equipment { Description = "TestDescription" },
                IsParent = true
            });
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderCreated.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Created Notification</h2>

<a>1</a><br />

Operating Center:  - TestOC <br />

Planning Plant:  -  - TestPlant <br />

Facility: TheFacility - -0 <br />

Work Order Description: TestDescription <br />

    Corrective Problem Code: TestCode <br />

    Equipment Description: TestDescription <br />

Notes: This is a Order Note, I like order notes <br />

";

            Assert.AreEqual(expected, template);
        }

        #region ProductionWorkOrderAssigned

        [TestMethod]
        public void TestProductionWorkOrderAssignedNotification()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "Frish Water"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType {Description = "test", Id = OrderType.Indices.CORRECTIVE_ACTION_20}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},
                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            workOrder.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = workOrder,
                Equipment = new Equipment { Description = "TestEquipmentDescription" },
                IsParent = true
            });
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.website.com/"
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderAssigned.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Employee Assigned Notification</h2>

<a href=""http://www.website.com/"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Description: TestDescription<br />
    Equipment Description: TestEquipmentDescription<br /> 
    
        <br />
        Employee: Jason Mendoza<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Michael TheDemon<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Chidi Anagonye<br />
        Assigned For: 9/25/1990<br />
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void
            TestProductionWorkOrderAssignedNotificationDoesNotIncludeEquipmentDescriptionIfOrderTypeIsPlantMaintenance11()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "Frish Water"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType
                        {Description = "test", Id = OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.website.com/"
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderAssigned.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Employee Assigned Notification</h2>

<a href=""http://www.website.com/"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Description: TestDescription<br />
    
        <br />
        Employee: Jason Mendoza<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Michael TheDemon<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Chidi Anagonye<br />
        Assigned For: 9/25/1990<br />
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void
            TestProductionWorkOrderAssignedNotificationDoesNotIncludeEquipmentDescriptionIfOrderTypeIsRoutine13()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter { OperatingCenterName = "Fresh Water" },
                PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                Facility = new Facility { FacilityName = "TheFacility" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType { Description = "test", Id = OrderType.Indices.ROUTINE_13 }
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },

                OrderNotes = "This is an order note",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Jerry",
                            LastName = "Adrich"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Mike",
                            LastName = "Dropp"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Will",
                            LastName = "Kohlbach"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.website.com/"
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderAssigned.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Employee Assigned Notification</h2>

<a href=""http://www.website.com/"">1</a><br />
Operating Center:  - Fresh Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Description: TestDescription<br />
    
        <br />
        Employee: Jerry Adrich<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Mike Dropp<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Will Kohlbach<br />
        Assigned For: 9/25/1990<br />
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void
            TestProductionWorkOrderAssignedNotificationDoesNotIncludeEquipmentDescriptionIfOrderTypeIsNotPlantMaintenance11AndEquipmentIsNull()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "Frish Water"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType {Description = "test", Id = OrderType.Indices.CORRECTIVE_ACTION_20}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.website.com/"
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderAssigned.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Employee Assigned Notification</h2>

<a href=""http://www.website.com/"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Description: TestDescription<br />
    
        <br />
        Employee: Jason Mendoza<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Michael TheDemon<br />
        Assigned For: 9/25/1990<br />
    
    
        <br />
        Employee: Chidi Anagonye<br />
        Assigned For: 9/25/1990<br />
    
";

            Assert.AreEqual(expected, template);
        }

        #endregion

        #region SupervisorApprovalRequired

        [TestMethod]
        public void TestSupervisorApprovalRequiredNotificationIncludesEquipmentDescriptionWhenOrderTypeIsNotPlantMaintenance11()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter { OperatingCenterName = "Frish Water" },
                PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                Facility = new Facility { FacilityName = "TheFacility" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType { Description = "test", Id = OrderType.Indices.CORRECTIVE_ACTION_20 }
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                Equipments = { new ProductionWorkOrderEquipment { Id = 1, Equipment = new Equipment { Description = "TestEquipmentDescription" }, IsParent = true } },

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = { 
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.internet.com"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.SupervisorApprovalRequired.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Supervisor Approval Required</h2>

<a href=""http://www.internet.com"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Number: 1<br />
Work Order Description: TestDescription<br />
    Equipment Description: TestEquipmentDescription<br />
Notes: This is a Order Note, I like order notes
    <br />
        Employee: Jason Mendoza<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Michael TheDemon<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Chidi Anagonye<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSupervisorApprovalRequiredNotificationDoesNotIncludeEquipmentDescriptionWhenOrderTypeIsPlantMaintenance11()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter { OperatingCenterName = "Frish Water" },
                PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                Facility = new Facility { FacilityName = "TheFacility" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType { Description = "test", Id = OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11 }
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                Equipments = { new ProductionWorkOrderEquipment { Id = 1, Equipment = new Equipment { Description = "TestEquipmentDescription" }, IsParent = true } },

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.internet.com"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.SupervisorApprovalRequired.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Supervisor Approval Required</h2>

<a href=""http://www.internet.com"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Number: 1<br />
Work Order Description: TestDescription<br />
Notes: This is a Order Note, I like order notes
    <br />
        Employee: Jason Mendoza<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Michael TheDemon<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Chidi Anagonye<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSupervisorApprovalRequiredNotificationDoesNotIncludeEquipmentDescriptionWhenOrderTypeIsRoutine13()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter { OperatingCenterName = "Frish Water" },
                PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                Facility = new Facility { FacilityName = "TheFacility" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType { Description = "test", Id = OrderType.Indices.ROUTINE_13 }
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },
                Equipments = { new ProductionWorkOrderEquipment { Id = 1, Equipment = new Equipment { Description = "TestEquipmentDescription" }, IsParent = true } },

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.internet.com"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.SupervisorApprovalRequired.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Supervisor Approval Required</h2>

<a href=""http://www.internet.com"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Number: 1<br />
Work Order Description: TestDescription<br />
Notes: This is a Order Note, I like order notes
    <br />
        Employee: Jason Mendoza<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Michael TheDemon<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Chidi Anagonye<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSupervisorApprovalRequiredNotificationDoesNotIncludeEquipmentDescriptionWhenOrderTypeIsNotPlantMaintenance11AndEquipmentIsNull()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter { OperatingCenterName = "Frish Water" },
                PlanningPlant = new PlanningPlant { Id = 1, Description = "TestPlant" },
                Facility = new Facility { FacilityName = "TheFacility" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType { Description = "test", Id = OrderType.Indices.CORRECTIVE_ACTION_20 }
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode { Code = "TestCode" },

                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = { 
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990,9,25),
                        DateStarted = new DateTime(2019,11,1),
                        DateEnded = new DateTime(2019,11,1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                }
            };
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder,
                RecordUrl = "http://www.internet.com"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.SupervisorApprovalRequired.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Supervisor Approval Required</h2>

<a href=""http://www.internet.com"">1</a><br />
Operating Center:  - Frish Water<br />
Planning Plant:  -  - TestPlant<br />
Facility: TheFacility - -0<br />
Work Order Number: 1<br />
Work Order Description: TestDescription<br />
Notes: This is a Order Note, I like order notes
    <br />
        Employee: Jason Mendoza<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Michael TheDemon<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
    <br />
        Employee: Chidi Anagonye<br />
        Date Started: 11/1/2019 12:00:00 AM<br />
        Date Ended: 11/1/2019 12:00:00 AM
    
";

            Assert.AreEqual(expected, template);
        }

        #endregion

        [TestMethod]
        public void TestWorkOrderCompletedNotification()
        {
            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription",
                    OrderType = new OrderType {Description = "test", Id = 1}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},
                OrderNotes = "This is a Order Note, I like order notes",
                EmployeeAssignments = {
                    new EmployeeAssignment {
                        Id = 1,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Jason",
                            LastName = "Mendoza"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 2,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Michael",
                            LastName = "TheDemon"
                        }
                    },
                    new EmployeeAssignment {
                        Id = 3,
                        AssignedFor = new DateTime(1990, 9, 25),
                        DateStarted = new DateTime(2019, 11, 1),
                        DateEnded = new DateTime(2019, 11, 1),
                        AssignedTo = new Employee {
                            FirstName = "Chidi",
                            LastName = "Anagonye"
                        }
                    }
                },

                CompletedBy = new User {UserName = "Me"},
                DateCompleted = new DateTime(2019, 11, 1),
                FailureCode = new ProductionWorkOrderFailureCode {Description = "It failed"},
                ActionCode = new ProductionWorkOrderActionCode {Description = "we took all the action"}
            };
            workOrder.Equipments.Add(new ProductionWorkOrderEquipment {
                ProductionWorkOrder = workOrder,
                Equipment = new Equipment { Description = "TestDescription" },
                IsParent = true
            });
            var model = new ProductionWorkOrderNotification {
                ProductionWorkOrder = workOrder
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.ProductionWorkOrderCompleted.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Work Order Completed Notification</h2>

<a>1</a><br />

Operating Center:  - TestOC <br />
Planning Plant:  -  - TestPlant <br />
Facility: TheFacility - -0 <br />
Work Order Description: TestDescription <br />
Corrective Problem Code: TestCode<br />

     Equipment Description: --0 <br /> 
<br/>
Notes: This is a Order Note, I like order notes <br />
Assignment Details: <br />
    
        Employee: Jason Mendoza <br/>
        Assigned For: 9/25/1990 <br/>
        Date Started: 11/1/2019 12:00:00 AM <br/>
        Date Ended: 11/1/2019 12:00:00 AM <br/>
    
    
        Employee: Michael TheDemon <br/>
        Assigned For: 9/25/1990 <br/>
        Date Started: 11/1/2019 12:00:00 AM <br/>
        Date Ended: 11/1/2019 12:00:00 AM <br/>
    
    
        Employee: Chidi Anagonye <br/>
        Assigned For: 9/25/1990 <br/>
        Date Started: 11/1/2019 12:00:00 AM <br/>
        Date Ended: 11/1/2019 12:00:00 AM <br/>
    

Completion Details: <br />

    Completed By: Me <br />
    Completed on: 11/1/2019 12:00:00 AM <br />
    Failure Code: It failed <br />
    Action Code: we took all the action <br />";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestRedTagPermitOutOfServiceNotification()
        {
            var employee = new Employee();

            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility { FacilityName = "TheFacility", Street = new Street { Name = "Oak Road" }, Town = new Town { ShortName = "Fair Haven", State = new State { Abbreviation = "NJ" } }, StreetNumber = "12", ZipCode = "07704" },
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription", OrderType = new OrderType {Description = "test", Id = 1}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},
                Equipments = new HashSet<ProductionWorkOrderEquipment>() {
                    new ProductionWorkOrderEquipment { 
                        Equipment = new Equipment() {
                            Description = "Equipment"
                        },
                        IsParent = true
                    }
                },
                OrderNotes = "This is a Order Note, I like order notes"
            };

            var redTagPermit = new RedTagPermit {
                ProductionWorkOrder = workOrder,
                CreatedAt = new DateTime(2021, 4, 26),
                PersonResponsible = employee,
                AuthorizedBy = employee,
                AreaProtected = "The area that's on fire currently",
                EquipmentImpairedOn = new DateTime(2021, 4, 22),
                Equipment = new Equipment(),
                EmergencyOrganizationNotified = true,
                AdditionalInformationForProtectionType = "Additional information about the type of protection"
            };

            var model = new RedTagPermitNotification {
                ProductionWorkOrderRecordUrl = "http://Thisisatestofathing",
                RecordUrl = "http://Thisisanothertestforthething",
                RedTagPermit = redTagPermit
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.RedTagPermitOutOfService.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"<h2>Red Tag Permit - Out Of Service</h2>

Red Tag Permit: <a href=""http://Thisisanothertestforthething"">0</a><br />

Created On: 4/26/2021 12:00:00 AM <br />

Production Work Order: <a href=""http://Thisisatestofathing"">1</a> <br />

Operating Center:  - TestOC <br />

Facility: TheFacility - -0 <br />

Facility Address: 12  Oak Road  Fair Haven, NJ 07704  <br />

IOC Contact Number: 1-866-801-1123 <br />

Facility Insurance Id:  <br />

Equipment Description:  <br />

Name Of Responsible Person:  <br />

Type Of Protection:  <br />

    Type of Protection Description: Additional information about the type of protection<br />

Area Protected: The area that&#39;s on fire currently <br />

Reason For Impairment:  <br />

Number Of Turns To Close: 0 <br />

Authorized By:  <br />

Fire Protection Equipment Operator:  <br />

Date Equipment Impaired: 4/22/2021 12:00:00 AM <br />

List Of Precautions taken: 
<ul>
    <li>Emergency Organization Notified: True</li>
    <li>Public Fire Department Notified: </li>
    <li>Hazardous Operations Stopped: </li>
    <li>Hot Work Prohibited: </li>
    <li>Smoking Prohibited: </li>
    <li>Continuous Work Authorized: </li>
    <li>Ongoing Patrol Of Area: </li>
    <li>Hydrant Connected To Sprinkler: </li>
    <li>Pipe Plugs On Hand: </li>
    <li>Has Other Precaution: </li>
    <li>Other Precaution Description: </li>
</ul>";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestRedTagPermitInServiceNotification()
        {
            var employee = new Employee();

            var workOrder = new ProductionWorkOrder {
                Id = 1,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC"},
                PlanningPlant = new PlanningPlant {Id = 1, Description = "TestPlant"},
                Facility = new Facility {FacilityName = "TheFacility", InsuranceId = "003836.76-01", Street = new Street{Name = "Oak Road"}, Town = new Town{ShortName= "Fair Haven", State = new State{ Abbreviation = "NJ"} }, StreetNumber = "12", ZipCode = "07704"},
                ProductionWorkDescription = new ProductionWorkDescription {
                    Description = "TestDescription", OrderType = new OrderType {Description = "test", Id = 1}
                },
                CorrectiveOrderProblemCode = new CorrectiveOrderProblemCode {Code = "TestCode"},
                Equipments = new HashSet<ProductionWorkOrderEquipment>() {
                    new ProductionWorkOrderEquipment {
                        Equipment = new Equipment() {
                            Description = "TestDescription"
                        },
                        IsParent = true
                    }
                },
                OrderNotes = "This is a Order Note, I like order notes"
            };

            var redTagPermit = new RedTagPermit {
                ProductionWorkOrder = workOrder,
                CreatedAt = new DateTime(2021, 4, 26),
                PersonResponsible = employee,
                AuthorizedBy = employee,
                AreaProtected = "The area that's on fire currently",
                EquipmentImpairedOn = new DateTime(2021, 4, 22),
                EquipmentRestoredOn = new DateTime(2021, 4, 25),
                Equipment = new Equipment(),
                EmergencyOrganizationNotified = true,
                AdditionalInformationForProtectionType = "Additional information about the type of protection"
            };

            var model = new RedTagPermitNotification {
                ProductionWorkOrderRecordUrl = "http://Thisisatestofathing",
                RecordUrl = "http://Thisisanothertestforthething",
                RedTagPermit = redTagPermit
            };

            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Production.WorkManagement.RedTagPermitInService.cshtml";

            var template = RenderTemplate(streamPath, model);

            var expected = @"<h2>Red Tag Permit - In Service</h2>

Red Tag Permit: <a href=""http://Thisisanothertestforthething"">0</a><br />

Created On: 4/26/2021 12:00:00 AM <br />

Production Work Order: <a href=""http://Thisisatestofathing"">1</a> <br />

Operating Center:  - TestOC <br />

Facility: TheFacility - -0 <br />

Facility Address: 12  Oak Road  Fair Haven, NJ 07704  <br />

IOC Contact Number: 1-866-801-1123 <br />

Facility Insurance Id: 003836.76-01 <br />

Equipment Description:  <br />

Name Of Responsible Person:  <br />

Type Of Protection:  <br />

    Type of Protection Description: Additional information about the type of protection <br />

Area Protected: The area that&#39;s on fire currently <br />

Reason For Impairment:  <br />

Number Of Turns To Close: 0 <br />

Authorized By:  <br />

Fire Protection Equipment Operator:  <br />

Date Equipment Impaired: 4/22/2021 12:00:00 AM <br />

Date Equipment Restored: 4/25/2021 12:00:00 AM <br />

List Of Precautions taken: 
<ul>
    <li>Emergency Organization Notified: True</li>
    <li>Public Fire Department Notified: </li>
    <li>Hazardous Operations Stopped: </li>
    <li>Hot Work Prohibited: </li>
    <li>Smoking Prohibited: </li>
    <li>Continuous Work Authorized: </li>
    <li>Ongoing Patrol Of Area: </li>
    <li>Hydrant Connected To Sprinkler: </li>
    <li>Pipe Plugs On Hand: </li>
    <li>Has Other Precaution: </li>
    <li>Other Precaution Description: </li>
</ul>";

            Assert.AreEqual(expected, template);
        }
    }
}
