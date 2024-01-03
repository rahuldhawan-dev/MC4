using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyDisplayItemForNearMiss : DisplayItem<PublicWaterSupply>
    {
        #region Properties

        public string Identifier { get; set; }
        public string System { get; set; }
        public override string Display => $"{Identifier} - {System}".Replace(" ", " ");

        #endregion
    }
}
