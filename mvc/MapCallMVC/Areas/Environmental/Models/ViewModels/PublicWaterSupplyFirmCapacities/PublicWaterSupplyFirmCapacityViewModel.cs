using System;
using System.ComponentModel.DataAnnotations;
using StructureMap;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities
{
    public class PublicWaterSupplyFirmCapacityViewModel : ViewModel<PublicWaterSupplyFirmCapacity>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public virtual int? PublicWaterSupply { get; set; }

        [Required]
        public virtual float? CurrentSystemPeakDailyDemandMGD { get; set; }

        [Required]
        public virtual DateTime? CurrentSystemPeakDailyDemandYearMonth { get; set; }

        public virtual float? TotalSystemSourceCapacityMGD { get; set; }

        [Required,
         Range(PublicWaterSupplyFirmCapacity.CapacityMultiplierRange.MIN_VALUE,
             PublicWaterSupplyFirmCapacity.CapacityMultiplierRange.MAX_VALUE)]
        public virtual decimal? FirmCapacityMultiplier { get; set; }

        public virtual decimal? TotalCapacityFacilitySumMGD { get; set; }

        public virtual decimal? FirmSystemSourceCapacityMGD => TotalCapacityFacilitySumMGD * FirmCapacityMultiplier;

        #endregion

        #region Constructors

        public PublicWaterSupplyFirmCapacityViewModel(IContainer container) : base(container) { }

        #endregion
    }
}