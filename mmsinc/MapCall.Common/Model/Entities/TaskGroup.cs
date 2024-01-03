using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TaskGroup : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int TASK_GROUP_ID = 8,
                             TASK_GROUP_NAME = 50,
                             TASK_DETAILS_SUMMARY = 250;
        }

        public struct Display
        {
            public const string EQUIPMENT_PURPOSE = "Equipment Purposes",
                                EQUIPMENT_DETAIL_TYPE = "Equipment Details",
                                ESTIMATED_HRS_WORK = "Estimated Hrs Work",
                                COST_OF_CONTRACTOR = "Contractor Cost";
        }

        public struct TaskGroupIds
        {
            public const string OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID = "T10";
        }
        
        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string TaskGroupId { get; set; }
        public virtual string TaskGroupName { get; set; }
        public virtual TaskGroupCategory TaskGroupCategory { get; set; }
        public virtual MaintenancePlanTaskType MaintenancePlanTaskType { get; set; }
        public virtual string TaskDetails { get; set; }
        public virtual string TaskDetailsSummary { get; set; }

        public virtual IList<EquipmentType> EquipmentTypes { get; set; } = new List<EquipmentType>();
        public virtual IList<EquipmentLifespan> EquipmentLifespans { get; set; } = new List<EquipmentLifespan>();
        public virtual IList<EquipmentPurpose> EquipmentPurposes { get; set; } = new List<EquipmentPurpose>();

        #region Logical properties

        public virtual string Caption => $"{TaskGroupId} - {TaskGroupName}"; 

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString() => new TaskGroupDisplayItem {
            Caption = Caption
        }.Display;

        #endregion
    }
}