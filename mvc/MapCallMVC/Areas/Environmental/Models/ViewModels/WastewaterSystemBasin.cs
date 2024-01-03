using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    // No need for separate Create/Edit models.
    public class WasteWaterSystemBasinViewModel : ViewModel<WasteWaterSystemBasin>
    {
        #region Properties

        [Required]
        [StringLength(WasteWaterSystemBasin.StringLengths.MAX_NAME_LENGTH)]
        public string BasinName { get; set; }
        [Range(0, 999.999)]
        public decimal? FirmCapacity { get; set; }
        [Range(0, 999.999)]
        public decimal? FirmCapacityUnderStandbyPower { get; set; }
        public DateTime? FirmCapacityDateUpdated { get; set; }
        [DropDown]
        [EntityMustExist(typeof(WasteWaterSystem))]
        [EntityMap]
        [Required]
        public int? WasteWaterSystem { get; set; }

        #endregion

        #region Constructor

        public WasteWaterSystemBasinViewModel(IContainer container) : base(container) { }

        #endregion
    }
    public class CreateWasteWaterSystemBasin : WasteWaterSystemBasinViewModel
    {
        #region Constructors

        public CreateWasteWaterSystemBasin(IContainer container) : base(container) { }

        #endregion
    }

    public class EditWasteWaterSystemBasin : WasteWaterSystemBasinViewModel
    {
        #region Constructors

        public EditWasteWaterSystemBasin(IContainer container) : base(container) { }

        #endregion
    }

    // This model's needed so index can have sortable columns.
    public class SearchWasteWaterSystemBasin : SearchSet<WasteWaterSystemBasin>
    {
        [View(WasteWaterSystemBasin.DisplayNames.WASTEWATER_SYSTEM_BASIN_ID)]
        public int? Id { get; set; }
        public string BasinName { get; set; }
        [DropDown]
        [EntityMustExist(typeof(WasteWaterSystem))]
        [EntityMap, View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }
    }
}