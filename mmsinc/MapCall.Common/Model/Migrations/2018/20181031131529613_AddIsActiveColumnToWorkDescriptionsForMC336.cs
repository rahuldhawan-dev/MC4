using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181031131529613), Tags("Production")]
    public class AddIsActiveColumnToWorkDescriptionsForMC336 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkDescriptions").AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
            Update.Table("WorkDescriptions").Set(new {IsActive = false})
                  .Where(new {Description = "SERVICE LINE REPAIR CAPITAL"});
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("WorkDescriptions");
        }
    }
}
