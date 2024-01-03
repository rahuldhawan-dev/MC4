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
    [TestClass]
    public class SAPCompleteWorkOrderTest
    {
        private WorkOrder workOrder;

        [TestInitialize]
        public void TestInitialize() { }

        #region Public Methods

        public WorkOrder GetTestCompleteWorkOrder()
        {
            workOrder = new WorkOrder();
            workOrder.AssetType = new AssetType {Description = "MAIN"};
            workOrder.Notes = "test";
            workOrder.SAPWorkOrderNumber = 90417125;
            workOrder.DateCompleted = DateTime.Now;
            workOrder.RequestingEmployee = new User {FullName = "Apurva", UserName = "a"};
            workOrder.LostWater = 1;
            workOrder.WorkDescription = new WorkDescription {Description = "WATER MAIN BREAK REPAIR"};
            workOrder.ApprovedOn = DateTime.Now;
            MainBreak[] mainBreak = new MainBreak[1] {
                new MainBreak {
                    MainFailureType = new MainFailureType {Description = ""},
                    MainBreakSoilCondition = new MainBreakSoilCondition {Description = ""},
                    CustomersAffected = 0,
                    ShutdownTime = 0,
                    ChlorineResidual = 0,
                    ServiceSize = new ServiceSize {Size = 0},
                    ReplacedWith = new MainBreakMaterial {Description = ""},
                    Depth = 0,
                    FootageReplaced = 0,
                    MainBreakDisinfectionMethod = new MainBreakDisinfectionMethod {Description = "Chlorination"},
                    MainBreakFlushMethod = new MainBreakFlushMethod {Description = "Hydrant"}
                }
            };
            workOrder.MainBreaks = mainBreak;

            CrewAssignment[] CrewAssignment = new CrewAssignment[3] {
                new CrewAssignment {
                    EmployeesOnJob = 2,
                    AssignedOn = Convert.ToDateTime("01/12/2017 11:52:02 AM"),
                    //DateStarted = Convert.ToDateTime("01/13/2017 14:27:00"),
                    //DateEnded = Convert.ToDateTime("01/14/2017 14:32:00")
                },

                new CrewAssignment {
                    EmployeesOnJob = 3,
                    AssignedOn = Convert.ToDateTime("01/13/2017 10:52:02 AM"),
                    DateStarted = Convert.ToDateTime("01/16/2017 11:52:02 AM"),
                    DateEnded = Convert.ToDateTime("01/17/2017 02:52:02 AM")
                },
                new CrewAssignment {
                    EmployeesOnJob = 3,
                    AssignedOn = Convert.ToDateTime("01/14/2017 11:52:02 AM"),
                    DateStarted = Convert.ToDateTime("01/18/2017 11:52:02 AM"),
                    DateEnded = Convert.ToDateTime("01/19/2017 02:52:02 AM")
                },
            };
            workOrder.CrewAssignments = CrewAssignment;
            workOrder.BusinessUnit = "123";
            return workOrder;
        }

        public WorkOrder GetTestCompleteWorkOrderForNULL()
        {
            workOrder = new WorkOrder();
            workOrder.AssetType = new AssetType {Description = "VALVE"};
            workOrder.Notes = "test";
            workOrder.SAPWorkOrderNumber = 90365156;
            workOrder.DateCompleted = DateTime.Now;
            workOrder.RequestingEmployee = new User {FullName = "Apurva", UserName = "a"};
            workOrder.LostWater = 1;
            workOrder.WorkDescription = new WorkDescription {
                Description = "apurva",
                PlantMaintenanceActivityType = new PlantMaintenanceActivityType {Description = "MLS"}
            };
            workOrder.ApprovedOn = DateTime.Now;
            MainBreak[] mainBreak = new MainBreak[1] {
                new MainBreak {
                    MainFailureType = new MainFailureType {Description = ""},
                    MainBreakSoilCondition = new MainBreakSoilCondition {Description = ""},
                    CustomersAffected = 0,
                    ShutdownTime = 0,
                    ChlorineResidual = 0,
                    ServiceSize = new ServiceSize {Size = 0},
                    ReplacedWith = new MainBreakMaterial {Description = ""},
                    Depth = 0,
                    FootageReplaced = 0
                }
            };
            workOrder.MainBreaks = mainBreak;

            CrewAssignment[] CrewAssignment = new CrewAssignment[3] {
                new CrewAssignment {
                    AssignedOn = Convert.ToDateTime("01/27/2017 3:30 PM"),
                },

                new CrewAssignment {
                    EmployeesOnJob = 1,
                    AssignedOn = Convert.ToDateTime("01/30/2017 10:27 AM"),
                    DateStarted = Convert.ToDateTime("01/30/2017 10:28:00 AM"),
                    DateEnded = Convert.ToDateTime("01/30/2017 4:41:00 PM")
                },
                new CrewAssignment {
                    EmployeesOnJob = 5,
                    AssignedOn = Convert.ToDateTime("01/30/2017 5:31 PM"),
                    DateStarted = Convert.ToDateTime("01/30/2017 5:31:00 PM"),
                    DateEnded = Convert.ToDateTime("01/30/2017 8:53:00 PM")
                },
            };
            workOrder.CrewAssignments = CrewAssignment;

            return workOrder;
        }

        public WorkOrder GetTestGoodsIssue()
        {
            workOrder = new WorkOrder();
            workOrder.AssetType = new AssetType {Description = "VALVE"};
            //workOrder.MaterialsUsed = new MaterialUsed { StockLocation = new StockLocation { OperatingCenter = new OperatingCenter { de } } } 
            workOrder.SAPWorkOrderNumber = 90000614;
            workOrder.DateCompleted = DateTime.Now;
            workOrder.MaterialPostingDate = DateTime.Now;

            MaterialUsed[] MaterialsUsed = new MaterialUsed[3] {
                new MaterialUsed {
                    Material = new Material {PartNumber = "1405687", Description = "ABC"}, Quantity = 1,
                    StockLocation = new StockLocation {SAPStockLocation = "TYK"}
                },
                new MaterialUsed {
                    Material = new Material {PartNumber = "1405688", Description = "XYZ"}, Quantity = 2,
                    StockLocation = new StockLocation {SAPStockLocation = "LKG"}
                },
                new MaterialUsed {
                    Material = new Material {PartNumber = null, Description = "XYZ"}, Quantity = 2,
                    StockLocation = new StockLocation {SAPStockLocation = "LKG"}
                }
            };
            workOrder.MaterialsUsed = MaterialsUsed;

            return workOrder;
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestMapToWorkOrderMapsProperties()
        {
            var costCenter = "123456";
            var entity = GetTestCompleteWorkOrder();
            var model = new SAPCompleteWorkOrder(entity) { CostCenter = costCenter};
            model.MapToWorkOrder(entity);
            
            Assert.AreEqual(costCenter, entity.BusinessUnit);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotNullOutCostCenter()
        {
            var order = GetTestCompleteWorkOrder();
            order.BusinessUnit = "12321";
            var model = new SAPCompleteWorkOrder(order) { CostCenter = ""};

            model.MapToWorkOrder(order);

            Assert.AreEqual("12321", order.BusinessUnit);
        }

        #endregion
    }
}
