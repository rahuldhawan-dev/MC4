using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.Common.Model.ViewModels
{
    public class ScheduledMaintenancePlan
    {
        #region Properties

        public int MaintenancePlanId { get; set; }
        public DateTime Start { get; set; }
        public int ProductionWorkOrderFrequencyId { get; set; }
        public int OperatingCenterId { get; set; }
        public int PlanningPlantId { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public string LocalTaskDescription { get; set; }
        public ProductionWorkDescription WorkDescription { get; set; }
        public Facility Facility { get; set; }
        public IList<Equipment> Equipment { get; set; }
        public IList<ScheduledAssignment> Assignments { get; set; }
        
        #endregion

        #region Constructors

        public ScheduledMaintenancePlan() { }

        public ScheduledMaintenancePlan(MaintenancePlan from)
        {
            MaintenancePlanId = from.Id;
            Start = from.Start;
            ProductionWorkOrderFrequencyId = from.ProductionWorkOrderFrequency.Id;
            OperatingCenterId = from.OperatingCenter.Id;
            PlanningPlantId = from.PlanningPlant.Id;
            EquipmentType = from.EquipmentTypes.FirstOrDefault();
            WorkDescription = from.WorkDescription;
            Facility = from.Facility;
            Equipment = from.Equipment;
            LocalTaskDescription = from.LocalTaskDescription;
            Assignments = from.ScheduledAssignments.ToList();
        }

        #endregion
    }
}
