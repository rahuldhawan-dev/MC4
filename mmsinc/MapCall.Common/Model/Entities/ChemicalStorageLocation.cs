using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalStorageLocation : IEntityWithChangeTracking<User>
    {
        #region Constants

        public struct DisplayNames
        {
            public const string STORAGE_LOCATION_DESCRIPTION = "Description";
        }

        public struct StringLengths
        {
            public const int STORAGE_LOCATION_NUMBER = 10,
                             STORAGE_LOCATION_DESCRIPTION = 25;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual PlanningPlant PlanningPlant { get; set; }
        public virtual ChemicalWarehouseNumber ChemicalWarehouseNumber { get; set; }
        public virtual string StorageLocationNumber { get; set; }
        public virtual bool IsActive { get; set; }

        [View(DisplayNames.STORAGE_LOCATION_DESCRIPTION)]
        public virtual string StorageLocationDescription { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        #endregion
    }
}
