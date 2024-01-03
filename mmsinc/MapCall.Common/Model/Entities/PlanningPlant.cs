using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PlanningPlant : EntityLookup
    {
        #region Consts

        public const int CODE_LENGTH = 4;

        #endregion

        #region Private Members

        private PlanningPlantDisplayItem _display;

        #endregion

        #region Properties

        public virtual string Code { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion

        #region Exposed Methods

        public virtual string Display => (_display ?? (_display = new PlanningPlantDisplayItem {
            Code = Code,
            OperatingCenter = OperatingCenter?.OperatingCenterCode,
            Description = Description
        })).Display;

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class PlanningPlantDisplayItem : DisplayItem<PlanningPlant>
    {
        public string Code { get; set; }

        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        public string Description { get; set; }

        public override string Display => $"{Code} - {OperatingCenter} - {Description}";
    }
}
