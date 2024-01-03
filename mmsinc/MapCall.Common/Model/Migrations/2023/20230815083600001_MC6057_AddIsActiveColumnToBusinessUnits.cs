using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230815083600001), Tags("Production")]
    public class MC6057_AddIsActiveColumnToBusinessUnits : Migration
    {
        public override void Up()
        {
            Alter.Table("BusinessUnits").AddColumn("IsActive").AsBoolean().SetExistingRowsTo(true);          
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("BusinessUnits");
        }
    }
}

