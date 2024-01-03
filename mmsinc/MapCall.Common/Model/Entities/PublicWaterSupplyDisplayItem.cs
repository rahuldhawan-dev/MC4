using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyDisplayItem : DisplayItem<PublicWaterSupply>
    {
        public string Identifier { get; set; }
        public string OperatingArea { get; set; }
        public string System { get; set; }

        /// bug 1720 - requests that this is flipped around.
        /// 1720 originally wanted this as OperatingArea - Identifier - System
        /// WO0000000195491 wants this as Identifier - OperatingArea - System 
        public override string Display => $"{Identifier} - {OperatingArea} - {System}".Replace("  ", " ");
    }
}
