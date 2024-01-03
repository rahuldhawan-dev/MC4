using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetReliability : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int NOTE_LENGTH = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Employee Employee { get; set; }
        [View("Technology Used")]
        public virtual AssetReliabilityTechnologyUsedType AssetReliabilityTechnologyUsedType { get; set; }
        public virtual DateTime DateTimeEntered { get; set; }
        [View("Repair Cost If Not Allowed To Fail")]
        public virtual int RepairCostNotAllowedToFail { get; set; }
        [View("Repair Cost If Allowed To Fail")]
        public virtual int RepairCostAllowedToFail { get; set; }
        public virtual int CostAvoidance { get; set; }
        public virtual string TechnologyUsedNote { get; set; }
        public virtual string CostAvoidanceNote { get; set; }

        #endregion
    }
}
