using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalStorageMap : ClassMap<ChemicalStorage>
    {
        public const string TABLE_NAME = "ChemicalStorage";

        public ChemicalStorageMap()
        {
            Table(TABLE_NAME);
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Chemical).Not.Nullable();
            References(x => x.Coordinate).Nullable();

            // NOTE: This is nullable in the db for some reason. All records
            // have a value and it's a required field.
            References(x => x.WarehouseNumber).Nullable();
            References(x => x.Facility).Nullable();

            Map(x => x.MaxStorageQuantityGallons).Nullable();
            Map(x => x.MinStorageQuantityGallons).Nullable();
            Map(x => x.MaxStorageQuantityPounds).Nullable();
            Map(x => x.MinStorageQuantityPounds).Nullable();
            Map(x => x.ReorderLevelNonPeakProductionGallons).Nullable();
            Map(x => x.ReorderLevelPeakProductionGallons).Nullable();
            Map(x => x.ReorderLevelNonPeakProductionPounds).Nullable();
            Map(x => x.ReorderLevelPeakProductionPounds).Nullable();
            Map(x => x.TypicalOrderQuantityGallons).Nullable();
            Map(x => x.TypicalOrderQuantityPounds).Nullable();
            Map(x => x.Crtk).Column("CRTK").Nullable();
            Map(x => x.DeliveryInstructions).Nullable();
            Map(x => x.Location).Length(ChemicalStorage.StringLengths.LOCATION).Nullable();
            Map(x => x.ContainerType).Length(ChemicalStorage.StringLengths.CONTAINER_TYPE).Nullable();
            Map(x => x.MaximumDailyInventory).Length(ChemicalStorage.StringLengths.MAXIMUM_DAILY_INVENTORY).Nullable();
            Map(x => x.AverageDailyInventory).Length(ChemicalStorage.StringLengths.AVERAGE_DAILY_INVENTORY).Nullable();
            Map(x => x.DaysOnSite).Nullable();
            Map(x => x.StoragePressure).Length(ChemicalStorage.StringLengths.STORAGE_PRESSURE).Nullable();
            Map(x => x.StorageTemperature).Length(ChemicalStorage.StringLengths.STORAGE_TEMPERATURE).Nullable();
            Map(x => x.IsActive).Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
