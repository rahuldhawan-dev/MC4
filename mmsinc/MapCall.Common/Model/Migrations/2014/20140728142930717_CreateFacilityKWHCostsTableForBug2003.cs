using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140728142930717), Tags("Production")]
    public class CreateFacilityKWHCostsTableForBug2003 : Migration
    {
        public const string TABLE_NAME = "FacilityKWHCosts";

        public struct ColumnNames
        {
            public const string FACILITY_ID = "FacilityId",
                                COST_PER_KWH = "CostPerKWH",
                                START_DATE = "StartDate",
                                END_DATE = "EndDate";
        }

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FACILITY_ID).AsInt32()
                  .ForeignKey(String.Format("FK_{0}_tblFacilities_{1}", TABLE_NAME, ColumnNames.FACILITY_ID),
                       "tblFacilities", "RecordId").NotNullable()
                  .WithColumn(ColumnNames.COST_PER_KWH).AsCurrency().NotNullable()
                  .WithColumn(ColumnNames.START_DATE).AsDateTime().NotNullable()
                  .WithColumn(ColumnNames.END_DATE).AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}
