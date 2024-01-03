using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystemBasinDisplayItem : DisplayItem<WasteWaterSystemBasin>
    {
        #region Properties

        public string BasinName { get; set; }

        #endregion

        #region Public Methods

        public override string Display => BasinName;

        #endregion
    }
}
