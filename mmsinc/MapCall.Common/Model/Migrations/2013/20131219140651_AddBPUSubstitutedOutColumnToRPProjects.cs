using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131219140651), Tags("Production")]
    public class AddBPUSubstitutedOutColumnToRPProjects : Migration
    {
        public override void Up()
        {
            Alter.Table("RPProjects")
                 .AddColumn("BPUSubstitutedOut")
                 .AsBoolean()
                 .WithDefaultValue(false)
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.Column("BPUSubstitutedOut").FromTable("RPProjects");
        }
    }
}
