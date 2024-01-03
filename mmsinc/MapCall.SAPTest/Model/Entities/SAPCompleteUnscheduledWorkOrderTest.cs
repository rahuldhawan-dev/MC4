using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass()]
    public class SapCompleteUnscheduledWorkOrderTest
    {
        private ProductionWorkOrder productionWorkOrder;

        [TestInitialize]
        public void TestInitialize() { }

        #region Public Methods

        public ProductionWorkOrder GetTestProductionWorkOrderComplete()
        {
            productionWorkOrder = new ProductionWorkOrder();
            productionWorkOrder.PlanningPlant = new PlanningPlant {Code = "P212"};
            productionWorkOrder.SAPNotificationNumber = 15623143;
            productionWorkOrder.SAPWorkOrder = "100622897";
            productionWorkOrder.FunctionalLocation = "NJLK-HO-ADDIS";
            productionWorkOrder.RequestedBy = new Employee {FirstName = "", LastName = "", MiddleName = ""};
            productionWorkOrder.Priority = new ProductionWorkOrderPriority {Description = "Emergency", Id = 1};
            productionWorkOrder.ProductionWorkDescription = new ProductionWorkDescription {
                BreakdownIndicator = true, Description = "Replace", Id = 1,
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Description = "BRE", Code = "BRE"}
            };

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

            var EmployeeAssignment = new HashSet<EmployeeAssignment> {
                new EmployeeAssignment
                    {AssignedOn = DateTime.Now, DateStarted = DateTime.Now, DateEnded = DateTime.Now},
                new EmployeeAssignment {AssignedOn = DateTime.Now, DateStarted = DateTime.Now}
            };
            productionWorkOrder.EmployeeAssignments = EmployeeAssignment;

            var productionWorkOrderMaterialUsed = new HashSet<ProductionWorkOrderMaterialUsed> {
                new ProductionWorkOrderMaterialUsed {
                    Material = new Material {PartNumber = "ABC"}, Quantity = 1,
                    StockLocation = new StockLocation {Description = ""}
                },
                new ProductionWorkOrderMaterialUsed {
                    Material = new Material {PartNumber = "ABC"}, Quantity = 1,
                    StockLocation = new StockLocation {Description = ""}
                },
            };
            productionWorkOrder.ProductionWorkOrderMaterialUsed = productionWorkOrderMaterialUsed;

            return productionWorkOrder;
        }

        #endregion
    }
}
