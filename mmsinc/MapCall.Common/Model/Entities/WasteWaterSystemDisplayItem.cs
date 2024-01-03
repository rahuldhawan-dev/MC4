using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystemDisplayItem : DisplayItem<WasteWaterSystem>
    {
        #region Properties

        public OperatingCenter OperatingCenter { get; set; }
        public override int Id { get; set; }
        public string WasteWaterSystemName { get; set; }

        public override string Display =>
            $"{OperatingCenter?.OperatingCenterCode}WW{Id.ToString("0000")} - {WasteWaterSystemName}";

        #endregion
    }
}
