using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140827152222818), Tags("Production")]
    public class AddPositionGroupToEmployeesTableBug2024 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee")
                 .AddColumn("PositionGroupId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_tblEmployee_PositionGroups_PositionGroupId", "PositionGroups", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblEmployee_PositionGroups_PositionGroupId").OnTable("tblEmployee");
            Delete.Column("PositionGroupId").FromTable("tblEmployee");
        }
    }
}
