using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallImporter.SampleValues
{
    public static class ProductionWorkDescriptions
    {
        public static string GetInsertQuery()
        {
            return
                "INSERT INTO ProductionWorkDescriptions (Id, Description, EquipmentTypeId, PlantMaintenanceActivityTypeId, OrderTypeId, ProductionSkillSetId, BreakdownIndicator, MaintenancePlanTaskTypeId, TaskGroupId) VALUES (1, 'REPAIR', 121, 1, 3, 6, 1, NULL, NULL);" +
                "INSERT INTO ProductionWorkDescriptions (Description, BreakdownIndicator, OrderTypeId) VALUES ('Maintenance Plan', 0, 5);";
        }
    }
}
