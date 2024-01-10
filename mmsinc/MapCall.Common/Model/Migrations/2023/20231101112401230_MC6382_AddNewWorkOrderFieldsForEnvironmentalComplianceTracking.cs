using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231101112401230), Tags("Production")]
    public class MC6382_AddNewWorkOrderFieldsForEnvironmentalComplianceTracking : Migration
    {
        public override void Up()
        {
            Create
               .Column("IsThisAMultiTenantFacility")
               .OnTable("WorkOrders")
               .AsBoolean()
               .Nullable();
            Create
               .Column("NumberOfPitcherFiltersDelivered")
               .OnTable("WorkOrders")
               .AsInt16()
               .Nullable();
            Create
               .Column("DescribeWhichUnits")
               .OnTable("WorkOrders")
               .AsString(256)
               .Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsThisAMultiTenantFacility").FromTable("WorkOrders");
            Delete.Column("NumberOfPitcherFiltersDelivered").FromTable("WorkOrders");
            Delete.Column("DescribeWhichUnits").FromTable("WorkOrders");
        }
    }
}

