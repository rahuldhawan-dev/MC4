using MMSINC.Data;
using MMSINC.Metadata;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CountEquipmentMaintenancePlansByMaintenancePlan : IEntity
    {
        #region Constants

        public struct DisplayNames
        {
            public const string ASSET_COUNT = "# of Assets";
        }

        #endregion

        public virtual int Id { get; set; }

        [View(DisplayName = DisplayNames.ASSET_COUNT)]
        public virtual int? AssetCount { get; set; }
    }
}
