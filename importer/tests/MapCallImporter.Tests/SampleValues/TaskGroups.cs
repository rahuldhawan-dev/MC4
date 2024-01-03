using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MapCallImporter.SampleValues
{
    public static class TaskGroups
    {
        public const string TASKGROUP_ID = "CH01",
                            NAME = "PM HYPO GEN CELL CONNECT & CABLE CLEAN";

        public static string GetInsertQuery()
        {
            return $"INSERT INTO TaskGroups (Id, TaskGroupId, TaskGroupName, TaskDetails, TaskDetailsSummary, TaskGroupCategoryId, MaintenancePlanTaskTypeId) VALUES (1, '{TASKGROUP_ID}', '{NAME}', NULL, NULL, NULL, NULL);";
        }
    }
}
