using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200918121747729), Tags("Production")]
    public class MC2621UpdateLengthOfPositionDescription : Migration
    {
        public override void Up()
        {
            Alter.Column("PositionDescription").OnTable("PositionGroups").AsString(100);
        }

        public override void Down()
        {
            //noop
        }
    }
}
