using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180920105026750), Tags("Production")]
    public class MC279_LIMSIntegration : Migration
    {
        public override void Up()
        {
            Create.Table("LIMSStatuses")
                  .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            this.EnableIdentityInsert("LIMSStatuses");

            Insert.IntoTable("LIMSStatuses").Row(new {Id = 1, Description = "Not Ready"});
            Insert.IntoTable("LIMSStatuses").Row(new {Id = 2, Description = "Ready to Send"});
            Insert.IntoTable("LIMSStatuses").Row(new {Id = 3, Description = "Sent Successfully"});
            Insert.IntoTable("LIMSStatuses").Row(new {Id = 4, Description = "Send Failed"});

            this.DisableIdentityInsert("LIMSStatuses");

            Create.Column("LIMSStatusId").OnTable("BacterialWaterSamples")
                  .AsInt32().NotNullable().WithDefaultValue(1)
                  .ForeignKey("FK_BacterialWaterSamples_LIMSStatusId_LIMSStatuses", "LIMSStatuses", "Id");

            Create.Column("LIMSResponse").OnTable("BacterialWaterSamples")
                  .AsCustom("ntext").Nullable();

            Create.Column("SubmittedToLIMSAt").OnTable("BacterialWaterSamples")
                  .AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_BacterialWaterSamples_LIMSStatusId_LIMSStatuses").OnTable("BacterialWaterSamples");
            Delete.Column("SubmittedToLIMSAt").FromTable("BacterialWaterSamples");
            Delete.Column("LIMSResponse").FromTable("BacterialWaterSamples");
            Delete.Column("LIMSStatusId").FromTable("BacterialWaterSamples");
            Delete.Table("LIMSStatuses");
        }
    }
}
