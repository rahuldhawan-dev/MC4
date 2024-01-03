using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160128142845528), Tags("Production")]
    public class AddFieldsForBug2752 : Migration
    {
        private const string FACILITIES = "tblFacilities";

        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilitySizes", "Small", "Medium", "Large");
            this.CreateLookupTableWithValues("FacilityTransformerWiringTypes", "Wye", "Delta");

            Alter.Table(FACILITIES)
                 .AddColumn("Priority").AsDecimal(18, 2).Nullable()
                 .AddColumn("FacilitySizeId").AsInt32().Nullable()
                 .ForeignKey("FK_Facilities_FacilitySizes_FacilitySizeId", "FacilitySizes", "Id")
                 .AddColumn("UtilityAccountNumber").AsString(50).Nullable()
                 .AddColumn("PrimaryVoltageKV").AsDecimal(18, 2).Nullable()
                 .AddColumn("TransformerResistancePercentage").AsDecimal(18, 2).Nullable()
                 .AddColumn("TransformerReactancePercentage").AsDecimal(18, 2).Nullable()
                 .AddColumn("FacilityTransformerWiringTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_Facilities_FacilityTransformerWiringTypes_FacilityTransformerWiringTypeId",
                      "FacilityTransformerWiringTypes", "Id")
                 .AddColumn("PrimaryFuseSize").AsDecimal(18, 2).Nullable()
                 .AddColumn("PrimaryFuseType").AsString(50).Nullable()
                 .AddColumn("PrimaryFuseManufacturer").AsString(100).Nullable()
                 .AddColumn("LineToLineFaultAmps").AsDecimal(18, 2).Nullable()
                 .AddColumn("LineToLineNeutralFaultAmps").AsDecimal(18, 2).Nullable()
                 .AddColumn("ArcFlashNotes").AsText().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Facilities_FacilityTransformerWiringTypes_FacilityTransformerWiringTypeId")
                  .OnTable(FACILITIES);
            Delete.ForeignKey("FK_Facilities_FacilitySizes_FacilitySizeId").OnTable(FACILITIES);

            Delete.Column("Priority").FromTable(FACILITIES);
            Delete.Column("FacilitySizeId").FromTable(FACILITIES);
            Delete.Column("UtilityAccountNumber").FromTable(FACILITIES);
            Delete.Column("PrimaryVoltageKV").FromTable(FACILITIES);
            Delete.Column("TransformerResistancePercentage").FromTable(FACILITIES);
            Delete.Column("TransformerReactancePercentage").FromTable(FACILITIES);
            Delete.Column("FacilityTransformerWiringTypeId").FromTable(FACILITIES);
            Delete.Column("PrimaryFuseSize").FromTable(FACILITIES);
            Delete.Column("PrimaryFuseType").FromTable(FACILITIES);
            Delete.Column("PrimaryFuseManufacturer").FromTable(FACILITIES);
            Delete.Column("LineToLineFaultAmps").FromTable(FACILITIES);
            Delete.Column("LineToLineNeutralFaultAmps").FromTable(FACILITIES);
            Delete.Column("ArcFlashNotes").FromTable(FACILITIES);

            Delete.Table("FacilitySizes");
            Delete.Table("FacilityTransformerWiringTypes");
        }
    }
}
