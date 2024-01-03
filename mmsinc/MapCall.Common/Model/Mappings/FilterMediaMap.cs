using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using ColumnNames = MapCall.Common.Model.Migrations.CreateTablesForBug1510.ColumnNames.FilterMedia;
using StringLengths = MapCall.Common.Model.Migrations.CreateTablesForBug1510.StringLengths.FilterMedia;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaMap : ClassMap<FilterMedia>
    {
        public const string TABLE_NAME = CreateTablesForBug1510.TableNames.FILTER_MEDIA;

        public FilterMediaMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Column(ColumnNames.ID);
            References(x => x.FilterType).Column(ColumnNames.FILTER_TYPE_ID);
            References(x => x.LevelControlMethod).Column(ColumnNames.LEVEL_CONTROL_METHOD_ID).Nullable();
            References(x => x.Equipment).Column(ColumnNames.EQUIPMENT_ID);
            References(x => x.WashType).Column(ColumnNames.WASH_TYPE_ID).Nullable();
            References(x => x.MediaType).Column(ColumnNames.MEDIA_TYPE_ID);
            References(x => x.Location).Column(ColumnNames.LOCATION_ID);

            Map(x => x.FilterNumber).Not.Nullable();
            Map(x => x.EquipmentIdentifier).Nullable().Length(StringLengths.EQUIPMENT_IDENTIFIER);
            Map(x => x.YearInService).Precision(10);
            Map(x => x.EstimatedMediaLifecycle).Precision(10);
            Map(x => x.CapacityMGD).Precision(24);
            Map(x => x.Coefficient).Length(StringLengths.COEFFICIENT);
            Map(x => x.FilterDimensions).Length(StringLengths.FILTER_DIMENSIONS);
            Map(x => x.MediaArea).Precision(10);
            Map(x => x.MediaVolume).Precision(10);
            Map(x => x.GravelSupportMedia);
            Map(x => x.MonthlyMediaExpense).Precision(19).Scale(4);
            Map(x => x.AnnualInspectionCosts).Precision(19).Scale(4);
            Map(x => x.AnnualAnalysisCosts).Precision(19).Scale(4);
            Map(x => x.AnnualCompanyLaborCosts).Precision(19).Scale(4);
            Map(x => x.EquipmentCriticalRating).Precision(10);
            Map(x => x.YearLastPainted).Precision(10);
            Map(x => x.ServedByStandbyPower);
            Map(x => x.TurbidimeterModel).Length(StringLengths.TURBIDIMETER_MODEL);
            Map(x => x.Notes).Length(StringLengths.NOTES);
            Map(x => x.ProductCode).Length(StringLengths.PRODUCT_CODE);
            Map(x => x.AnthraciteDepth).Precision(10);
            Map(x => x.GACDepth).Precision(10);
            Map(x => x.SandDepth).Precision(10);
            Map(x => x.GravelDepth).Precision(10);
            Map(x => x.LastTimeChanged);
            Map(x => x.LastTimeCleaned);
            Map(x => x.AirScouring);
            Map(x => x.Recycling);
            Map(x => x.Comment).Length(StringLengths.COMMENT);

            HasMany(x => x.FilterMediaDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.FilterMediaNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
