using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160812133407883), Tags("Production")]
    public class AddFieldsForBug3110 : Migration
    {
        public struct TableNames
        {
            public const string SAMPLE_SITES = "tblWQSample_sites";
        }

        public override void Up()
        {
            Alter.Table(TableNames.SAMPLE_SITES)
                 .AddColumn("CustomerParticipationConfirmed").AsBoolean().Nullable()
                 .AddColumn("CustomerName").AsAnsiString(255).Nullable()
                 .AddColumn("CustomerHomePhone").AsAnsiString(20).Nullable()
                 .AddColumn("CustomerAltPhone").AsAnsiString(20).Nullable()
                 .AddColumn("CustomerEmail").AsAnsiString(255).Nullable()
                 .AddForeignKeyColumn("CustomerPlumbingMaterialId", "ServiceMaterials", "ServiceMaterialID")
                 .AddColumn("FieldVerifiedServiceMaterial").AsBoolean().Nullable()
                 .AddColumn("FieldVerifiedServiceCustomerSideMaterial").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CustomerParticipationConfirmed").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("CustomerName").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("CustomerHomePhone").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("CustomerAltPhone").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("CustomerEmail").FromTable(TableNames.SAMPLE_SITES);
            Delete.ForeignKeyColumn(TableNames.SAMPLE_SITES, "CustomerPlumbingMaterialId", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.Column("FieldVerifiedServiceMaterial").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("FieldVerifiedServiceCustomerSideMaterial").FromTable(TableNames.SAMPLE_SITES);
        }
    }
}
