using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalStorageViewModel : ViewModel<ChemicalStorage>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Chemical))]
        [Required]
        public virtual int? Chemical { get; set; }

        [Coordinate, EntityMap, EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap(MapDirections.None),
         EntityMustExist(typeof(OperatingCenter))]
        [Required]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("Environmental", "ChemicalWarehouseNumber", "ByOperatingCenter", DependsOn = "OperatingCenter"),
         EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        [Required]
        public virtual int? WarehouseNumber { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap,
         EntityMustExist(typeof(Facility))]
        [Required]
        public virtual int? Facility { get; set; }

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        [Required]
        public virtual int? State { get; set; }

        public virtual float? MaxStorageQuantityGallons { get; set; }        
        public virtual float? MinStorageQuantityGallons { get; set; }
        public virtual float? MaxStorageQuantityPounds { get; set; }
        public virtual float? MinStorageQuantityPounds { get; set; }
        public virtual float? ReorderLevelNonPeakProductionGallons { get; set; }
        public virtual float? ReorderLevelPeakProductionGallons { get; set; }
        public virtual float? ReorderLevelNonPeakProductionPounds { get; set; }
        public virtual float? ReorderLevelPeakProductionPounds { get; set; }
        public virtual float? TypicalOrderQuantityGallons { get; set; }
        public virtual float? TypicalOrderQuantityPounds { get; set; }
        public virtual bool? Crtk { get; set; }
        public virtual string DeliveryInstructions { get; set; }

        [StringLength(ChemicalStorage.StringLengths.LOCATION)]
        public virtual string Location { get; set; }

        [StringLength(ChemicalStorage.StringLengths.CONTAINER_TYPE)]
        public virtual string ContainerType { get; set; }

        [StringLength(ChemicalStorage.StringLengths.MAXIMUM_DAILY_INVENTORY)]
        public virtual string MaximumDailyInventory { get; set; }

        [StringLength(ChemicalStorage.StringLengths.AVERAGE_DAILY_INVENTORY)]
        public virtual string AverageDailyInventory { get; set; }

        public virtual int? DaysOnSite { get; set; }

        [StringLength(ChemicalStorage.StringLengths.STORAGE_PRESSURE)]
        public virtual string StoragePressure { get; set; }

        [StringLength(ChemicalStorage.StringLengths.STORAGE_TEMPERATURE)]
        public virtual string StorageTemperature { get; set; }

        public virtual bool IsActive { get; set; }

        #endregion

        #region Constructors

        public ChemicalStorageViewModel(IContainer container) : base(container) { }

        #endregion

        public override void Map(ChemicalStorage entity)
        {
            base.Map(entity);

            // TODO: This is unnecessary. ChemicalStorage.WarehouseNumber is not nullable, nor is its OPC, nor is its State.
            State = entity.WarehouseNumber?.OperatingCenter?.State.Id ?? entity.Facility?.OperatingCenter?.State?.Id;
            OperatingCenter = entity.WarehouseNumber?.OperatingCenter?.Id ?? entity.Facility?.OperatingCenter?.Id;
        }

        public override ChemicalStorage MapToEntity(ChemicalStorage entity)
        {
            base.MapToEntity(entity);

            if (entity.Chemical != null && entity.Facility != null)
            {
                // TODO: Setting properties on different entities only works by accident.
                // This should not be done here.
                entity.Facility.ChemicalFeed = true;
            }

            return entity;
        }
    }

    public class CreateChemicalStorage : ChemicalStorageViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State"), EntityMap(MapDirections.None),
         EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter;
            set => base.OperatingCenter = value;
        }

        #endregion

        #region Constructors

        public CreateChemicalStorage(IContainer container) : base(container) { }

        #endregion
    }

    public class EditChemicalStorage : ChemicalStorageViewModel
    {
        #region Constructors

        public EditChemicalStorage(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchChemicalStorage : SearchSet<ChemicalStorage>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        public int? WarehouseNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Chemical))]
        [SearchAlias("Chemical", "Id", Required = true)]
        public int? Chemical { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("e.OperatingCenter", "s", "State.Id")]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State)), EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("Facility", "e", "OperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap,
         EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [View("CRTK")]
        public bool? Crtk { get; set; }

        [DropDown(), EntityMap, EntityMustExist(typeof(StateOfMatter))]
        [SearchAlias("Chemical.ChemicalStates", "Id")]
        public int? ChemicalForm { get; set; }

        public bool? IsActive { get; set; }

        #endregion
    }
}
