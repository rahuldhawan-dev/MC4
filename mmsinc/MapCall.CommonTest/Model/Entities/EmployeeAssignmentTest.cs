using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EmployeeAssignmentTest : MapCallMvcInMemoryDatabaseTestBase<EmployeeAssignment>
    {
        #region Fields

        private ProductionWorkOrder _order;
        private IList<ProductionPrerequisite> _prereqs;
        private ProductionWorkOrderProductionPrerequisite _prodPrereq;
        private Employee _employee;
        private EmployeeAssignment _assignment;
        private ProductionPreJobSafetyBrief _brief;
        private IList<OrderType> _orderTypes;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFieldsForEmployeeAssignmentThatCanBeStarted();
        }

        private void InitializeFieldsForEmployeeAssignmentThatCanBeStarted()
        {
            _orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            _employee = GetEntityFactory<Employee>().Create();
            _order = GetEntityFactory<ProductionWorkOrder>().Create();
            _prereqs = GetFactory<ProductionPrerequisiteFactory>().CreateAll();
            _prodPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                ProductionWorkOrder =_order,
                ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT),
                SatisfiedOn = DateTime.Now,
                SkipRequirement = true
            });
            _order.ProductionWorkOrderProductionPrerequisites.Add(_prodPrereq);
            _assignment = GetEntityFactory<EmployeeAssignment>().Create(new { ProductionWorkOrder = _order, AssignedTo = _employee });
            _brief = GetEntityFactory<ProductionPreJobSafetyBrief>().Create(new { ProductionWorkOrder = _order });
            _brief.Workers.Add(new ProductionPreJobSafetyBriefWorker {Employee = _employee});
        }

        #endregion

        [TestMethod]
        public void TestCanBeStartedReturnsFalseWhenPreJobSafetyBriefIsNotCompletedAtAllAndOrderTypeIsOperatingActivityOrCorrective()
        {
            var operationalActivityWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = _orderTypes[0]});
            var correctiveWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = _orderTypes[2]});

            _order.ProductionWorkDescription = operationalActivityWorkDescription;
            _order.ProductionPreJobSafetyBriefs.Clear();
            Assert.IsFalse(_assignment.CanBeStarted);

            _order.ProductionWorkDescription = correctiveWorkDescription;
            _order.ProductionPreJobSafetyBriefs.Clear();
            Assert.IsFalse(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenPreJobSafetyBriefIsNotCompletedAtAllAndOrderTypeIsNotOperationalActivityOrCorrective()
        {
            var plantMaintenanceWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = _orderTypes[1]});

            _order.ProductionWorkDescription = plantMaintenanceWorkDescription;
            _order.ProductionPreJobSafetyBriefs.Clear();

            Assert.IsTrue(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseWhenPreJobSafetyBriefIsNotCompletedForTheAssignedEmployee()
        {
            var correctiveWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {OrderType = _orderTypes[2]});
            _order.ProductionWorkDescription = correctiveWorkDescription;
            // Change the existing one to some other employee that isn't the same as the assigned one.
            _brief.Workers.Single().Employee = GetEntityFactory<Employee>().Create();
            Assert.IsFalse(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenPreJobSafetyBriefIsCompletedForAssignedEmployee()
        {
            _brief.Workers.Clear();
            var brief = new ProductionPreJobSafetyBrief();
            brief.Workers.Add(new ProductionPreJobSafetyBriefWorker {
                Employee = _assignment.AssignedTo
            });
            _order.ProductionPreJobSafetyBriefs.Add(brief);

            Assert.IsTrue(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseWhenProductionPrerequisiteLockoutFormAndNoLockoutFormExists()
        {
            _order.LockoutForms.Clear();
            _prodPrereq.SkipRequirement = false;
            Assert.IsFalse(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenProductionPrerequisiteLockoutFormAndNoLockoutFormExistsButSkipRequirementIsTrue()
        {
            _order.LockoutForms.Clear();
            _prodPrereq.SkipRequirement = true;
            Assert.IsTrue(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenProductionPrerequisiteLockoutFormAndALockoutFormExists()
        {
            var lockoutForm = GetEntityFactory<LockoutForm>().Create(new { ProductionWorkOrder = _order });
            _order.LockoutForms.Add(lockoutForm);
            Assert.IsTrue(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsFalseWhenProductionPrerequisiteConfinedSpaceFormRequiredAndNoConfinedSpaceFormsExist()
        {
            _order.ConfinedSpaceForms.Clear();
            _prodPrereq.SkipRequirement = false;
            _prodPrereq.ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE);
            Assert.IsFalse(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenProductionPrerequisiteConfinedSpaceFormsAndNoConfinedSpaceFormExistsButSkipRequirementIsTrue()
        {
            _order.ConfinedSpaceForms.Clear();
            _prodPrereq.SkipRequirement = true;
            _prodPrereq.ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE);
            Assert.IsTrue(_assignment.CanBeStarted);
        }
        
        [TestMethod]
        public void TestCanBeStartedReturnsFalseWhenProductionPrerequisiteConfinedSpaceFormRequiredAndConfinedSpaceFormIsNotCompleted()
        {
            var csf = GetFactory<ConfinedSpaceFormFactory>().Create(new { ProductionWorkOrder = _order });
            _order.ConfinedSpaceForms.Add(csf);
            _prodPrereq.SkipRequirement = false;
            _prodPrereq.ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE);
            Assert.IsFalse(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestCanBeStartedReturnsTrueWhenProductionPrerequisiteConfinedSpaceFormRequiredAndConfinedSpaceFormIsCompleted()
        {
            var csf = GetFactory<CompletedConfinedSpaceFormFactory>().Create(new { ProductionWorkOrder = _order });
            _order.ConfinedSpaceForms.Add(csf);
            var test = GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create(new { ConfinedSpaceForm = csf });
            _prodPrereq.SkipRequirement = false;
            _prodPrereq.ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE);
            Assert.IsTrue(_assignment.CanBeStarted);
        }

        /*
         * A production work order with a red tag permit prerequisite may or may not require the user to actually create red tag permits
         *  1. Do we have any prerequisites of type red tag permit and are not 'skip requirement'?
         *  2. Yes, we have a red tag prerequisite, so - does this work order need red tag permit authorizations?
         *  3. Yes, this work order needs authorization, so - does this work order not have any red tag permits filled out yet
         *  4. Yes, the employee cannot begin work
         */
        [TestMethod]
        public void TestCanBeStartedGivenARedTagPermitProductionPrerequisiteExists()
        {
            _order.ProductionWorkOrderProductionPrerequisites.Clear();
            _prodPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                ProductionWorkOrder = _order,
                ProductionPrerequisite = _prereqs.FirstOrDefault(x => x.Id == ProductionPrerequisite.Indices.RED_TAG_PERMIT),
                SkipRequirement = false
            });
            _order.ProductionWorkOrderProductionPrerequisites.Add(_prodPrereq);

            // Test case 1.
            Assert.IsFalse(_assignment.CanBeStarted);

            // Test case 2.
            _order.NeedsRedTagPermitAuthorization = false;
            Assert.IsTrue(_assignment.CanBeStarted);

            _order.NeedsRedTagPermitAuthorization = true;
            Assert.IsFalse(_assignment.CanBeStarted);

            // Test case 3.
            _order.RedTagPermit = GetEntityFactory<RedTagPermit>().Create();
            Assert.IsTrue(_assignment.CanBeStarted);
        }

        [TestMethod]
        public void TestRequiredTankInspectionNotCompletedReturnsFalseWhenTankInspectionNotRequired()
        {
            Assert.IsFalse(_assignment.RequiredTankInspectionNotCompleted);
        }

        [TestMethod]
        public void TestRequiredTankInspectionNotCompletedReturnsFalseWhenTankInspectionRequiredAndTankInspectionCompleted()
        {
            var priority = GetFactory<RoutineProductionWorkOrderPriorityFactory>().Create();
            var equipmentTypeTank = GetFactory<EquipmentTypeTankFactory>().Create();
            var tankInspection = GetEntityFactory<TankInspection>().Create();
            _order.TankInspections.Add(tankInspection);
            _order.Priority = priority;
            _order.EquipmentType = equipmentTypeTank;

            Assert.IsFalse(_assignment.RequiredTankInspectionNotCompleted);
        }

        [TestMethod]
        public void TestRequiredTankInspectionNotCompletedReturnsTrueWhenTankInspectionRequiredAndTankInspectionNotCompleted()
        {
            var priority = GetFactory<RoutineProductionWorkOrderPriorityFactory>().Create();
            
            // Only Potable Water Tanks (TNK-WPOT) require inspections
            var equipmentTypeTank = GetFactory<EquipmentTypeWaterTankFactory>().Create();
            var taskGroup = GetFactory<TaskGroupFactory>().Create(new { TaskGroupId = TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID });
            var maintenancPlan = GetFactory<MaintenancePlanFactory>().Create(new { TaskGroup = taskGroup});
            _order.Priority = priority;
            _order.EquipmentType = equipmentTypeTank;
            _order.MaintenancePlan = maintenancPlan;

            Assert.IsTrue(_assignment.RequiredTankInspectionNotCompleted);
        }
    }
}
