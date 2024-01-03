using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140311110919), Tags("Production")]
    public class AddCreatedByToJobSiteExcavationsTable : Migration
    {
        public const int MAX_CREATEDBY = 50;

        public override void Up()
        {
            Alter.Table("JobSiteExcavations")
                 .AddColumn("CreatedBy")
                 .AsString(MAX_CREATEDBY)
                 .NotNullable()
                 .WithDefaultValue(string.Empty);
        }

        public override void Down()
        {
            Delete.Column("CreatedBy").FromTable("JobSiteExcavations");
        }
    }
}
