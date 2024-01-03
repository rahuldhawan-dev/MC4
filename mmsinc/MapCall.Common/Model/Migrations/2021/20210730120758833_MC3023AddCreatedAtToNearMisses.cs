using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210730120758833), Tags("Production")]
    public class MC3023AddCreatedAtToNearMisses : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AddColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue("2021-01-01 00:00:00");
        }

        public override void Down()
        {
            Delete.Column("CreatedAt").FromTable("NearMisses");
        }
    }
}
