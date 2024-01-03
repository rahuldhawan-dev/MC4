using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140818152205782), Tags("Production")]
    public class CreatePositionGroupsTable : Migration
    {
        public const int MAX_GROUP_LENGTH = 10,
                         MAX_POSITION_DESCRIPTION = 50,
                         MAX_BUSINESS_UNIT = 6,
                         MAX_BUSINESS_UNIT_DESCRIPTION = 20;

        public override void Up()
        {
            Create.Table("PositionGroups")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("GroupCode").AsString(MAX_GROUP_LENGTH).NotNullable()
                  .WithColumn("PositionDescription").AsString(MAX_POSITION_DESCRIPTION).NotNullable()
                  .WithColumn("BusinessUnit").AsString(MAX_BUSINESS_UNIT).NotNullable()
                  .WithColumn("BusinessUnitDescription").AsString(MAX_BUSINESS_UNIT_DESCRIPTION).NotNullable()
                  .WithColumn("StateId").AsInt32().Nullable()
                  .ForeignKey("FK_PositionGroups_States_StateId", "States", "StateId")
                  .WithColumn("SAPCompanyCodeId").AsInt32().NotNullable()
                  .ForeignKey("FK_PositionGroups_SAPCompanyCodes_SAPCompanyCodeId", "SAPCompanyCodes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PositionGroups_SAPCompanyCodes_SAPCompanyCodeId").OnTable("PositionGroups");
            Delete.ForeignKey("FK_PositionGroups_States_StateId").OnTable("PositionGroups");
            Delete.Table("PositionGroups");
        }
    }
}
