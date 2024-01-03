using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyPressureZones
{
    public class SearchPublicWaterSupplyPressureZoneViewModel : SearchSet<PublicWaterSupplyPressureZone>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }

        [StringLength(PublicWaterSupplyPressureZone.StringLengths.HYDRAULIC_MODEL_NAME)]
        public string HydraulicModelName { get; set; }

        [StringLength(PublicWaterSupplyPressureZone.StringLengths.COMMON_NAME)]
        public string CommonName { get; set; }

        #endregion
    }
}
