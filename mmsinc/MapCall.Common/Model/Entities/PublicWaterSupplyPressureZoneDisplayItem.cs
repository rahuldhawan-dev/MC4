using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyPressureZoneDisplayItem : DisplayItem<PublicWaterSupplyPressureZone>
    {
        #region Properties

        public string HydraulicModelName { get; set; }

        #endregion

        #region Public Methods

        public override string Display => HydraulicModelName;

        #endregion
    }
}
