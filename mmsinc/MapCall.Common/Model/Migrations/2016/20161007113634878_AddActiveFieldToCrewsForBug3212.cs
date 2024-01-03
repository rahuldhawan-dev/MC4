using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161007113634878), Tags("Production")]
    public class AddActiveFieldToCrewsForBug3212 : Migration
    {
        public override void Up()
        {
            Alter.Table("Crews")
                 .AddColumn("Active")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("Active").FromTable("Crews");
        }
    }
}
