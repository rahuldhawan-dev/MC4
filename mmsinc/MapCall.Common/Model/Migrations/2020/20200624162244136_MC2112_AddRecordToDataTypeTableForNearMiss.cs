using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200624162244136), Tags("Production")]
    public class MC2112_AddRecordToDataTypeTableForNearMiss : Migration
    {
        public override void Up()
        {
            this.AddDataType("NearMisses");
        }

        public override void Down()
        {
            this.RemoveDataType("NearMisses");
        }
    }
}
