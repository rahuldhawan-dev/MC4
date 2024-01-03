using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalStorage : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int LOCATION = 60,
                             CONTAINER_TYPE = 30,
                             MAXIMUM_DAILY_INVENTORY = 30,
                             AVERAGE_DAILY_INVENTORY = 30,
                             STORAGE_PRESSURE = 30,
                             STORAGE_TEMPERATURE = 30;
        }

        #endregion

        public virtual string TableName => ChemicalStorageMap.TABLE_NAME;

        public virtual int Id { get; set; }
        public virtual Chemical Chemical { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual ChemicalWarehouseNumber WarehouseNumber { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual string PartNumber { get; set; }
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

        [View("Location(s)")]
        public virtual string Location { get; set; }

        public virtual string ContainerType { get; set; }
        public virtual string MaximumDailyInventory { get; set; }
        public virtual string AverageDailyInventory { get; set; }
        public virtual int? DaysOnSite { get; set; }
        public virtual string StoragePressure { get; set; }
        public virtual string StorageTemperature { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual IList<Document<ChemicalStorage>> Documents { get; set; }
        public virtual IList<Note<ChemicalStorage>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public ChemicalStorage()
        {
            Notes = new List<Note<ChemicalStorage>>();
            Documents = new List<Document<ChemicalStorage>>();
        }

        public override string ToString()
        {
            return new ChemicalStorageDisplayItem {
                Chemical = Chemical?.Name, ChemicalSymbol = Chemical?.ChemicalSymbol,
                WarehouseNumber = WarehouseNumber?.WarehouseNumber
            }.Display;
        }

        public virtual object GetChemicalStorageJson()
        {
            return new {
                Id,
                Chemical = Chemical.Name,
                WarehouseNumber.WarehouseNumber,
                Facility = Facility.FacilityName,
                Chemical.SdsHyperlink,
                Chemical.PartNumber,
                IsActive,
                State = Facility.Town?.State?.Abbreviation,
                MaxStorageQuantityGallons,
                MinStorageQuantityGallons,
                MaxStorageQuantityPounds,
                MinStorageQuantityPounds
            };
        }
    }

    public class ChemicalStorageDisplayItem : DisplayItem<ChemicalStorage>
    {
        [SelectDynamic("Name")]
        public string Chemical { get; set; }

        [SelectDynamic("ChemicalSymbol", Field = "Chemical")]
        public string ChemicalSymbol { get; set; }

        [SelectDynamic("WarehouseNumber")]
        public string WarehouseNumber { get; set; }

        public override string Display => string.IsNullOrWhiteSpace(ChemicalSymbol)
            ? $"{Chemical} - {WarehouseNumber}"
            : $"{ChemicalSymbol} - {Chemical} - {WarehouseNumber}";
    }
}
