using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPCreateUnscheduledWorkOrderTest
    {
        private ProductionWorkOrder productionWorkOrder;

        [TestInitialize]
        public void TestInitialize() { }

        #region Public Methods

        public ProductionWorkOrder GetTestProductionWorkOrderCreate()
        {
            productionWorkOrder = new ProductionWorkOrder();
            productionWorkOrder.PlanningPlant = new PlanningPlant {Code = "P212"};
            productionWorkOrder.ProductionWorkDescription = new ProductionWorkDescription {
                OrderType = new OrderType {SAPCode = "0040"},
                BreakdownIndicator = true,
                Description = "Replace",
                Id = 1,
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Description = "BRE", Code = "BRE"}
            };
            productionWorkOrder.Priority = new ProductionWorkOrderPriority {Description = "Emergency", Id = 1};
            productionWorkOrder.FunctionalLocation = "NJLK-HO-ADDIS";
            productionWorkOrder.DateReceived = DateTime.Now;
            productionWorkOrder.WBSElement = "1523-PL1";
            productionWorkOrder.RequestedBy = new Employee {FirstName = "", LastName = "", MiddleName = ""};

            ProductionPrerequisite[] permit = new ProductionPrerequisite[3] {
                new ProductionPrerequisite {Description = "Air Permit"},
                new ProductionPrerequisite {Description = "Job Safety Checklist"},
                new ProductionPrerequisite {Description = "Has Lockout Requirement"}
            };

            EquipmentModel[] equipmentModels = new EquipmentModel[1] {
                new EquipmentModel {
                    Description = "abc"
                }
            };

            var employeeAssignment = new HashSet<EmployeeAssignment> {
                new EmployeeAssignment {
                    Employees = new[] {new Employee {EmployeeId = "18502261"}}, AssignedOn = DateTime.Now,
                    DateStarted = DateTime.Now
                },
                new EmployeeAssignment {
                    Employees = new[] {new Employee {EmployeeId = "18502261"}}, AssignedOn = DateTime.Now,
                    DateStarted = DateTime.Now
                }
            };
            productionWorkOrder.EmployeeAssignments = employeeAssignment;

            return productionWorkOrder;
        }

        #endregion
    }
}
