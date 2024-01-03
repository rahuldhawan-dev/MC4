using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170307133402545), Tags("Production")]
    public class AddImportedFieldToWQComplaintsForBug3119 : Migration
    {
        public override void Up()
        {
            Alter.Table("WaterQualityComplaints")
                 .AddColumn("Imported")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Imported").FromTable("WaterQualityComplaints");
        }
    }
}
