using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170707141315572), Tags("Production")]
    public class AddMarkoutEditableToOperatingCentersForBug3847 : Migration
    {
        public override void Up()
        {
            Alter.Table("OPeratingCenters").AddColumn("MarkoutsEditable")
                 .AsBoolean().NotNullable().WithDefaultValue(false);
            Execute.Sql(
                "UPDATE OPERATINGCenters set MarkoutsEditable = 1 WHERE isNull(StateId, 0) <> 1");
        }

        public override void Down()
        {
            Delete.Column("MarkoutsEditable").FromTable("OperatingCenters");
        }
    }
}
