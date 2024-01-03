using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160906100711994), Tags("Production")]
    public class Bug3154 : Migration
    {
        public override void Up()
        {
            Create.Column("AcknowledgedByContractor").OnTable("Restorations").AsBoolean().NotNullable()
                  .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("AcknowledgedByContractor").FromTable("Restorations");
        }
    }
}
