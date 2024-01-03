using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200529121636061), Tags("Production")]
    public class MC2203PersonnelAreaIdNowUnique : Migration
    {
        public override void Up()
        {
            Alter.Column("PersonnelAreaId").OnTable("PersonnelAreas").AsInt32().Unique().NotNullable();
        }

        public override void Down() { }
    }
}
