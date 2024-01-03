using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyPressureZones
{
    public class PublicWaterSupplyPressureZoneViewModel : ViewModel<PublicWaterSupplyPressureZone>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public virtual int? PublicWaterSupply { get; set; }

        [Required, StringLength(PublicWaterSupplyPressureZone.StringLengths.HYDRAULIC_MODEL_NAME)]
        public virtual string HydraulicModelName { get; set; }

        [Required, 
         Range(PublicWaterSupplyPressureZone.PressureRangeValues.Min.LOWER, PublicWaterSupplyPressureZone.PressureRangeValues.Min.UPPER),
         CompareTo(nameof(HydraulicGradientMax), ComparisonType.LessThan, TypeCode.Int32)]
        public virtual int? HydraulicGradientMin { get; set; }

        [Required, 
         Range(PublicWaterSupplyPressureZone.PressureRangeValues.Max.LOWER, PublicWaterSupplyPressureZone.PressureRangeValues.Max.UPPER),
         CompareTo(nameof(HydraulicGradientMin), ComparisonType.GreaterThan, TypeCode.Int32)]
        public virtual int? HydraulicGradientMax { get; set; }

        [Range(PublicWaterSupplyPressureZone.PressureRangeValues.Min.LOWER, PublicWaterSupplyPressureZone.PressureRangeValues.Min.UPPER),
         CompareTo(nameof(PressureMax), ComparisonType.LessThan, TypeCode.Int32, IgnoreNullValues = true)]
        public virtual int? PressureMin { get; set; }

        [Range(PublicWaterSupplyPressureZone.PressureRangeValues.Max.LOWER, PublicWaterSupplyPressureZone.PressureRangeValues.Max.UPPER),
         CompareTo(nameof(PressureMin), ComparisonType.GreaterThan, TypeCode.Int32, IgnoreNullValues = true)]
        public virtual int? PressureMax { get; set; }

        [StringLength(PublicWaterSupplyPressureZone.StringLengths.COMMON_NAME)]
        public virtual string CommonName { get; set; }

        #endregion

        #region Constructors

        public PublicWaterSupplyPressureZoneViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
