using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Status

        [TestMethod]
        public void TestStatusWorksAsExpected()
        {
            var orderTypeDoesNotRequireSupervisorApproval =
                new OrderType {Id = OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11};
            var orderTypeThatRequiresSupervisorApproval = new OrderType {Id = OrderType.Indices.CORRECTIVE_ACTION_20};
            var model = new ProductionWorkOrder {
                ProductionWorkDescription = new ProductionWorkDescription {
                    OrderType = orderTypeDoesNotRequireSupervisorApproval
                }
            };

            model.DateCancelled = DateTime.Now;
            model.DateCompleted = DateTime.Now;

            // Test cancellation also has priority over DateCompleted
            Assert.AreEqual(WorkOrderStatus.Cancelled, model.Status);

            // Test completion when not approved or cancelled
            model.DateCancelled = null;
            Assert.AreEqual(WorkOrderStatus.Completed, model.Status);

            // Test requires supervisor approval when completed but not yet approved
            model.ProductionWorkDescription.OrderType = orderTypeThatRequiresSupervisorApproval;
            Assert.AreEqual(WorkOrderStatus.RequiresSupervisorApproval, model.Status);

            // Test other when there are no current assignments and not completed
            model.DateCompleted = null;
            model.CurrentAssignments.Clear();
            Assert.AreEqual(WorkOrderStatus.Other, model.Status);

            var ass = new EmployeeAssignment();
            model.CurrentAssignments.Add(ass);

            // Test scheduled previously if any assignments are dated for a day prior than today
            ass.AssignedFor = DateTime.Today.AddDays(-1);
            Assert.AreEqual(WorkOrderStatus.ScheduledPreviously, model.Status);

            // Test scheduled currently if an assignment is dated for today or for the future
            ass.AssignedFor = DateTime.Today;
            Assert.AreEqual(WorkOrderStatus.ScheduledCurrently, model.Status);
        }

        #endregion

        #region Supervisor Approval

        private ProductionWorkOrder InitializePWOForSupervisorApproval()
        {
            var orderType = new OrderType {Id = OrderType.Indices.CORRECTIVE_ACTION_20};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            return new ProductionWorkOrder {ProductionWorkDescription = productionWorkDescription};
        }

        [TestMethod]
        public void TestCanBeSupervisorApprovedReturnsFalseIfNotCompleted()
        {
            var pwo = InitializePWOForSupervisorApproval();
            pwo.DateCompleted = null;
            pwo.ApprovedOn = null;
            pwo.DateCancelled = null;

            Assert.IsFalse(pwo.CanBeSupervisorApproved);
        }

        [TestMethod]
        public void TestCanBeSupervisorApprovedReturnsFalseIfApprovedIEAlreadyApproved()
        {
            var pwo = InitializePWOForSupervisorApproval();
            pwo.DateCompleted = null;
            pwo.ApprovedOn = DateTime.Now;
            pwo.DateCancelled = null;

            Assert.IsFalse(pwo.CanBeSupervisorApproved);
        }

        [TestMethod]
        public void TestCanBeSupervisorApprovedReturnsFalseIfCancelled()
        {
            var pwo = InitializePWOForSupervisorApproval();
            pwo.DateCompleted = null;
            pwo.ApprovedOn = null;
            pwo.DateCancelled = DateTime.Now;

            Assert.IsFalse(pwo.CanBeSupervisorApproved);
        }

        [TestMethod]
        public void TestCanBeSupervisorApprovedOnlyReturnsTrueForCorrectiveOperationalAndCapitalOrders()
        {
            var pwo = InitializePWOForSupervisorApproval();
            pwo.DateCompleted = DateTime.Now;
            //Overrider the Cutoff Date for Operational PWO cutoff date
            ProductionWorkOrder.CUTOFF_DATE_FOR_SUPERVISOR_APPROVAL_FOR_OPERATIONAL_WORK_ORDERS = new DateTime(2023, 11, 1);

            pwo.ProductionWorkDescription.OrderType.Id = OrderType.Indices.OPERATIONAL_ACTIVITY_10;
            Assert.IsTrue(pwo.CanBeSupervisorApproved);

            //Set back the Cutoff Date for Operational PWO cutoff date since it is a static field to avoid unintended effects
            ProductionWorkOrder.CUTOFF_DATE_FOR_SUPERVISOR_APPROVAL_FOR_OPERATIONAL_WORK_ORDERS = new DateTime(2024, 1, 1);

            pwo.ProductionWorkDescription.OrderType.Id = OrderType.Indices.CORRECTIVE_ACTION_20;
            Assert.IsTrue(pwo.CanBeSupervisorApproved);

            pwo.ProductionWorkDescription.OrderType.Id = OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11;
            Assert.IsFalse(pwo.CanBeSupervisorApproved);

            pwo.ProductionWorkDescription.OrderType.Id = OrderType.Indices.RP_CAPITAL_40;
            Assert.IsTrue(pwo.CanBeSupervisorApproved);

            pwo.ProductionWorkDescription.OrderType.Id = OrderType.Indices.ROUTINE_13;
            Assert.IsFalse(pwo.CanBeSupervisorApproved);

            // Future proofing this test in case we add more indices.
            foreach (var id in Enumerable.Range(5, 100))
            {
                pwo.ProductionWorkDescription.OrderType.Id = id;
                Assert.IsFalse(pwo.CanBeSupervisorApproved,
                    "If this is true, a new Id has been added that has not been tested.");
            }
        }

        #endregion

        #region MaterialApproval

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsFalseIfMaterialAlreadyApproved()
        {
            var pwo = new ProductionWorkOrder {MaterialsApprovedOn = DateTime.Now};

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsFalseIfCancelled()
        {
            var pwo = new ProductionWorkOrder {DateCancelled = DateTime.Now};

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsFalseIfNotCompletedOrApproved()
        {
            var pwo = new ProductionWorkOrder { };

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        #region 0010 - Operational Activity

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsTrueIfOperationalActivity10AndApprovedAndCompletedAndHasMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.OPERATIONAL_ACTIVITY_10};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };
            pwo.ProductionWorkOrderMaterialUsed.Add(new ProductionWorkOrderMaterialUsed
                {NonStockDescription = "something", Material = null});

            Assert.IsTrue(pwo.CanBeMaterialApproved);
        }

        [TestMethod]
        public void
            TestCanBeMaterialApprovedReturnsFalseIfOperationalActivity10AndApprovedAndCompletedButHasNoMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.OPERATIONAL_ACTIVITY_10};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        #endregion

        #region 0020 - Corrective Action

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsTrueIfCorrectiveAction20AndApprovedAndCompletedAndHasMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.CORRECTIVE_ACTION_20};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };
            pwo.ProductionWorkOrderMaterialUsed.Add(new ProductionWorkOrderMaterialUsed
                {NonStockDescription = "something", Material = null});

            Assert.IsTrue(pwo.CanBeMaterialApproved);
        }

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsFalseIfCorrectiveAction20AndApprovedAndCompletedButHasNoMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.CORRECTIVE_ACTION_20};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        #endregion

        #region 0040 - RP Capital

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsTrueIfRPCapital40AndApprovedAndCompletedAndHasMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.RP_CAPITAL_40};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };
            pwo.ProductionWorkOrderMaterialUsed.Add(new ProductionWorkOrderMaterialUsed
                {NonStockDescription = "something", Material = null});

            Assert.IsTrue(pwo.CanBeMaterialApproved);
        }

        [TestMethod]
        public void TestCanBeMaterialApprovedReturnsFalseIfRPCapital40AndApprovedAndCompletedButHasNoMaterials()
        {
            var orderType = new OrderType {Id = OrderType.Indices.RP_CAPITAL_40};
            var productionWorkDescription = new ProductionWorkDescription {OrderType = orderType};
            var pwo = new ProductionWorkOrder {
                ApprovedOn = DateTime.Now,
                DateCompleted = DateTime.Now,
                ProductionWorkDescription = productionWorkDescription
            };

            Assert.IsFalse(pwo.CanBeMaterialApproved);
        }

        #endregion

        #endregion

        #region CanBeCancelled

        [TestMethod]
        public void TestCannotBeCancelledIfCompleted()
        {
            var target = new ProductionWorkOrder {DateCompleted = DateTime.Now};

            Assert.IsFalse(target.CanBeCancelled);
        }

        #endregion

        #region CanBeCompleted

        [TestMethod]
        public void TestCanBeCompletedReturnsTrueWhenCanBeCompleted()
        {
            var target = new ProductionWorkOrder();
            target.EmployeeAssignments.Add(
                new EmployeeAssignment {DateStarted = DateTime.Now, DateEnded = DateTime.Now});

            Assert.IsTrue(target.CanBeCompleted);
        }

        [TestMethod]
        public void TestCanBeCompletedReturnsFalseWhenAlreadyComplete()
        {
            var target = new ProductionWorkOrder {DateCompleted = DateTime.Now};
            target.EmployeeAssignments.Add(new EmployeeAssignment());

            Assert.IsFalse(target.CanBeCompleted);
        }

        [TestMethod]
        public void TestCanBeCompletedReturnsFalseWhenCancelled()
        {
            var target = new ProductionWorkOrder {DateCancelled = DateTime.Now};
            target.EmployeeAssignments.Add(new EmployeeAssignment());

            Assert.IsFalse(target.CanBeCompleted);
        }

        [TestMethod]
        public void TestCanBeCompletedReturnsFalseWithOpenAssignment()
        {
            var target = new ProductionWorkOrder();
            target.EmployeeAssignments.Add(new EmployeeAssignment {DateStarted = DateTime.Now});

            Assert.IsFalse(target.CanBeCompleted);
        }

        #endregion

        #region LinkedToEnvironmentalPermit

        [TestMethod]
        public void TestLinkedToEnvironmentalPermitReturnsTrueWhenMainEquipmentLinkedToEnvironmentalPermit()
        {
            var equipment = GetEntityFactory<Equipment>().Create();
            var permit = GetEntityFactory<EnvironmentalPermit>().Create(new {Equipment = new[] {equipment}});
            equipment.EnvironmentalPermits.Add(permit);
            var target = GetEntityFactory<ProductionWorkOrder>().Create(new {Equipment = equipment});
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = target,
                IsParent = true,
                Equipment = equipment
            });
            target.Equipments.Add(pwoe);

            Assert.IsTrue(target.LinkedToEnvironmentalPermit);
        }

        [TestMethod]
        public void TestLinkedToEnvironmentalPermitReturnsTrueWhenChildEquipmentLinkedToEnvironmentalPermit()
        {
            var equipment = GetEntityFactory<Equipment>().Create();
            var permit = GetEntityFactory<EnvironmentalPermit>().Create(new {Equipment = new[] {equipment}});
            equipment.EnvironmentalPermits.Add(permit);
            var target = GetEntityFactory<ProductionWorkOrder>().Create();
            target.Equipments.Add(
                new ProductionWorkOrderEquipment {ProductionWorkOrder = target, Equipment = equipment});

            Assert.IsTrue(target.LinkedToEnvironmentalPermit);
        }

        [TestMethod]
        public void TestLinkedToEnviromentalPermitReturnsFalseWhenNotLinkedToEquipmentOrEquipments()
        {
            var target = GetEntityFactory<ProductionWorkOrder>().Create(new {Equipment = (Equipment)null});

            Assert.IsFalse(target.LinkedToEnvironmentalPermit);
        }

        #endregion

        #region OrderType

        [TestMethod]
        public void TestOrderTypeReturnsCorrectly()
        {
            var correctOrderType = new OrderType {
                Id = OrderType.Indices.CORRECTIVE_ACTION_20
            };

            // Setup the order type in the Model since Model.OrderType has no setter

            var model = new ProductionWorkOrder {
                ProductionWorkDescription = new ProductionWorkDescription {
                    OrderType = correctOrderType
                }
            };

            // Should return true 
            Assert.AreEqual(OrderType.Indices.CORRECTIVE_ACTION_20, model.OrderType.Id);
        }

        #endregion

        #region CurrentlyAssignedEmployee

        [TestMethod]
        public void TestThatCurrentlyAssignedEmployeeIsReturningLastElementInList()
        {
            var employeeAssignmentList = GetEntityFactory<EmployeeAssignment>().CreateList(2);
            var employeeList = new HashSet<EmployeeAssignment>(employeeAssignmentList);

            var model = new ProductionWorkOrder {
                CurrentAssignments = employeeList
            };

            var expected = employeeAssignmentList.LastOrDefault().AssignedTo.ToString();

            Assert.AreEqual(expected, model.CurrentlyAssignedEmployee);

            expected = employeeAssignmentList.FirstOrDefault().AssignedTo.ToString();

            Assert.AreNotEqual(expected, model.CurrentlyAssignedEmployee);
        }

        #endregion

        #region ProductionWorkOrderRequiresSupervisorApproval

        [TestMethod]
        public void TestRequiresSupervisorApprovalReturnsTrueWhenOrderTypeIsCorrectiveActionOrderRPCapital()
        {
            var orderTypes = GetEntityFactory<OrderType>().CreateList(5);

            foreach (var orderType in orderTypes)
            {
                var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = orderType});
                var order = GetEntityFactory<ProductionWorkOrder>().Create(new
                    {ProductionWorkDescription = workDescription, DateCompleted = DateTime.Now});

                Session.Evict(order);
                order = Session.Load<ProductionWorkOrder>(order.Id);

                if (orderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20 ||
                    orderType.Id == OrderType.Indices.RP_CAPITAL_40)
                {
                    Assert.IsTrue(order.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
                }
            }
        }

        [TestMethod]
        public void TestRequiresSupervisorApprovalReturnsFalseWhenOrderTypeIsNotCorrectiveActionOrderRPCapital()
        {
            var orderTypes = GetEntityFactory<OrderType>().CreateList(5);

            foreach (var orderType in orderTypes)
            {
                var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = orderType});
                var order = GetEntityFactory<ProductionWorkOrder>().Create(new
                    {ProductionWorkDescription = workDescription, DateCompleted = DateTime.Now});

                Session.Evict(order);
                order = Session.Load<ProductionWorkOrder>(order.Id);

                if (orderType.Id != OrderType.Indices.CORRECTIVE_ACTION_20 &&
                    orderType.Id != OrderType.Indices.RP_CAPITAL_40)
                {
                    Assert.IsFalse(order.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
                }
            }
        }

        [TestMethod]
        public void TestRequiresSupervisorApprovalReturnsFalseWhenNotCompleted()
        {
            var orderTypes = GetEntityFactory<OrderType>().CreateList(5);
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = orderTypes[1]});
            var order = GetEntityFactory<ProductionWorkOrder>()
               .Create(new {ProductionWorkDescription = workDescription});
            Session.Evict(order);
            order = Session.Load<ProductionWorkOrder>(order.Id);

            Assert.IsFalse(order.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
        }

        [TestMethod]
        public void TestRequiresSupervisorApprovalReturnsFalseWhenApproved()
        {
            var orderTypes = GetEntityFactory<OrderType>().CreateList(5);
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = orderTypes[1]});
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new
                {ProductionWorkDescription = workDescription, DateCompleted = DateTime.Now, ApprovedOn = DateTime.Now});
            Session.Evict(order);
            order = Session.Load<ProductionWorkOrder>(order.Id);

            Assert.IsFalse(order.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
        }

        [TestMethod]
        public void TestRequiresSupervisorApprovalReturnsFalseWhenCancelled()
        {
            var orderTypes = GetEntityFactory<OrderType>().CreateList(5);
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = orderTypes[1]});
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                ProductionWorkDescription = workDescription, DateCompleted = DateTime.Now, DateCancelled = DateTime.Now
            });
            Session.Evict(order);
            order = Session.Load<ProductionWorkOrder>(order.Id);

            Assert.IsFalse(order.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
        }

        [TestMethod]
        public void TestToString()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create();

            Assert.AreEqual(order.Id.ToString(), order.ToString());
        }

        #endregion

        #region LockoutFormRequired

        //ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT && !x.SkipRequirement);

        [TestMethod]
        public void TestLockFormRequiredReturnsTrueWhenLockoutFormRequired()
        {
            var target = new ProductionWorkOrder();
            target.ProductionWorkOrderProductionPrerequisites.Add(new ProductionWorkOrderProductionPrerequisite {
                ProductionPrerequisite = new ProductionPrerequisite
                    {Id = ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT}
            });

            Assert.IsTrue(target.LockoutFormRequired);
        }

        [TestMethod]
        public void TestLockFormRequiredReturnsFalseWhenLockoutFormNotRequired()
        {
            var target = new ProductionWorkOrder();

            Assert.IsFalse(target.LockoutFormRequired);
        }

        [TestMethod]
        public void TestLockFormRequiredReturnsFalseWhenLockoutFormRequiredWithSkipRequirement()
        {
            var target = new ProductionWorkOrder();
            target.ProductionWorkOrderProductionPrerequisites.Add(new ProductionWorkOrderProductionPrerequisite {
                ProductionPrerequisite = new ProductionPrerequisite
                    {Id = ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT},
                SkipRequirement = true
            });

            Assert.IsFalse(target.LockoutFormRequired);
        }

        #endregion

        #region IsEligibleForRedTagPermit

        [TestMethod]
        public void TestIsEligibleForRedTagPermitIsTrueWhenRedTagPermitPrerequisitesExist()
        {
            var target = new ProductionWorkOrder();

            target.ProductionWorkOrderProductionPrerequisites.Add(
                new ProductionWorkOrderProductionPrerequisite {
                    ProductionPrerequisite = new ProductionPrerequisite {
                        Id = ProductionPrerequisite.Indices.RED_TAG_PERMIT
                    }
                });

            Assert.IsTrue(target.IsEligibleForRedTagPermit);
        }

        [TestMethod]
        public void TestIsEligibleForRedTagPermitIsFalseWhenRedTagPermitPrerequisitesDoNotExist()
        {
            var target = new ProductionWorkOrder();

            Assert.IsFalse(target.IsEligibleForRedTagPermit);
        }

        #endregion

        #region ConfinedSpaceFormRequired

        [TestMethod]
        public void TestConfinedSpaceFormRequiredReturnsTrueWhenConfinedSpaceFormRequired()
        {
            var target = new ProductionWorkOrder();
            target.ProductionWorkOrderProductionPrerequisites.Add(new ProductionWorkOrderProductionPrerequisite {
                ProductionPrerequisite = new ProductionPrerequisite
                    {Id = ProductionPrerequisite.Indices.IS_CONFINED_SPACE}
            });

            Assert.IsTrue(target.ConfinedSpaceFormRequired);
        }

        [TestMethod]
        public void TestConfinedSpaceFormRequiredReturnsFalseWhenConfinedSpaceFormNotRequired()
        {
            var target = new ProductionWorkOrder();

            Assert.IsFalse(target.ConfinedSpaceFormRequired);
        }

        [TestMethod]
        public void TestConfinedSpaceFormRequireReturnsFalseWhenConfinedSpaceFormRequireWithSkipRequirement()
        {
            var target = new ProductionWorkOrder();
            target.ProductionWorkOrderProductionPrerequisites.Add(new ProductionWorkOrderProductionPrerequisite {
                ProductionPrerequisite = new ProductionPrerequisite
                    {Id = ProductionPrerequisite.Indices.IS_CONFINED_SPACE},
                SkipRequirement = true
            });

            Assert.IsFalse(target.ConfinedSpaceFormRequired);
        }

        #endregion

        #region MaintenancePlan

        [TestMethod]
        public void TestMaintenancePlanIdReturnsCorrectly()
        {
            var maintenancePlanId = 123456789012;

            // Setup maintenance plan Id in the Model since Model.MaintenancePlanId has no setter
            var model = new ProductionWorkOrder {SAPMaintenancePlanId = maintenancePlanId};

            // Should return true 
            Assert.AreEqual(maintenancePlanId, model.SAPMaintenancePlanId);
        }

        #endregion
        
        #region ActualCompletionHours

        [TestMethod]
        public void TestActualCompletionHoursWorksAsExpected()
        {
            var workorder = new ProductionWorkOrder();
            Assert.AreEqual(0.0M, workorder.ActualCompletionHours);

            workorder.EmployeeAssignments.Add(new EmployeeAssignment { HoursWorked = 1.11M });
            workorder.EmployeeAssignments.Add(new EmployeeAssignment { HoursWorked = 2.22M });
            workorder.EmployeeAssignments.Add(new EmployeeAssignment { HoursWorked = 3.33M });
            Assert.AreEqual(6.66M, workorder.ActualCompletionHours);
        }

        #endregion

        #region EmployeesAssigned

        [TestMethod]
        public void TestEmployeesAssignedReturnsCommaDelimitedListIfMoreThanOne()
        {
            var employeeA = new Employee { FirstName = "Rick", LastName = "Sanchez" };
            var employeeB = new Employee { FirstName = "Morty", LastName = "Smith" };

            var employeeAssignmentA = new EmployeeAssignment { AssignedTo = employeeA };
            var employeeAssignmentB = new EmployeeAssignment { AssignedTo = employeeB };

            var model = new ProductionWorkOrder();

            model.EmployeeAssignments.Add(employeeAssignmentA);
            model.EmployeeAssignments.Add(employeeAssignmentB);

            var expectedResult = employeeA.FullName + ", " + employeeB.FullName;

            // Should return true 
            Assert.AreEqual(expectedResult, model.EmployeesAssigned);
        }

        #endregion

        #region RequiresTankInspection

        [TestMethod]
        public void TestRequiresTankInspectionPropertyWithNoInspection()
        {
            //Create a Maintenance Plan with Task Group T10
            var taskGroup = GetFactory<TaskGroupFactory>().Create(new {TaskGroupId = TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID });
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create(new{ TaskGroup = taskGroup});
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(new { Abbreviation = EquipmentType.ComparisonValue.POTABLE_WATER_TANK});
            //Create a Produciton WO with no tank inspections and set the production WO Priority to Routine off schedule
            ProductionWorkOrderPriority productionWorkOrderPriority = GetFactory<EmergencyProductionWorkOrderPriorityFactory>().Create();
            productionWorkOrderPriority.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED;
            var productionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = productionWorkOrderPriority, EquipmentType = equipmentType, MaintenancePlan = maintenancePlan});
            Assert.AreEqual(true, productionWorkOrder.RequiresTankInspection);
        }

        [TestMethod]
        public void TestRequiresTankInspectionPropertyWithInspection()
        {
            //Create a Maintenance Plan with Task Group T10
            var taskGroup = GetFactory<TaskGroupFactory>().Create(new { TaskGroupId = TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID });
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create(new { TaskGroup = taskGroup });
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(new { Abbreviation = EquipmentType.ComparisonValue.POTABLE_WATER_TANK });
            //Create a Produciton WO with tank inspections and set the production WO Priority to 
            ProductionWorkOrderPriority productionWorkOrderPriority = GetFactory<EmergencyProductionWorkOrderPriorityFactory>().Create();
            productionWorkOrderPriority.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED;
            var productionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = productionWorkOrderPriority, EquipmentType = equipmentType, MaintenancePlan = maintenancePlan });
            productionWorkOrder.TankInspections.Add(GetFactory<TankInspectionFactory>().Create());
            Assert.AreEqual(true, productionWorkOrder.RequiresTankInspection);
        }

        [TestMethod]
        public void TestRequiresTankInspectionPropertyWithNoInspectionsWithRoutinePriority()
        {
            //Create a Maintenance Plan with Task Group T10
            var taskGroup = GetFactory<TaskGroupFactory>().Create(new { TaskGroupId = TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID });
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create(new { TaskGroup = taskGroup });
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(new { Abbreviation = EquipmentType.ComparisonValue.POTABLE_WATER_TANK });
            //Create a Produciton WO with tank inspections and set the production WO Priority to Routine
            ProductionWorkOrderPriority productionWorkOrderPriority = GetFactory<EmergencyProductionWorkOrderPriorityFactory>().Create();
            productionWorkOrderPriority.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE;
            var productionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = productionWorkOrderPriority, EquipmentType = equipmentType, MaintenancePlan = maintenancePlan });
            Assert.AreEqual(true, productionWorkOrder.RequiresTankInspection);
        }

        [TestMethod]
        public void TestRequiresTankInspectionPropertyWithNonTankEquipment()
        {
            //Create a Maintenance Plan with Task Group T10
            var taskGroup = GetFactory<TaskGroupFactory>().Create(new { TaskGroupId = TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID });
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create(new { TaskGroup = taskGroup });
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create(new { Abbreviation = EquipmentType.ComparisonValue.POTABLE_WATER_TANK });
            //Create a Produciton WO with tank inspections and set the production WO Priority to Routine
            ProductionWorkOrderPriority productionWorkOrderPriority = GetFactory<EmergencyProductionWorkOrderPriorityFactory>().Create();
            productionWorkOrderPriority.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE;
            var productionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = productionWorkOrderPriority, EquipmentType = equipmentType, MaintenancePlan = maintenancePlan });
            productionWorkOrder.TankInspections.Add(GetFactory<TankInspectionFactory>().Create());
            Assert.AreEqual(true, productionWorkOrder.RequiresTankInspection);
        }

        #endregion
    }
}
