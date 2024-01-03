using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210707221011656), Tags("Production")]
    public class MC2717AddingPendingRetirementToFacilityStatus : Migration
    {
        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT [dbo].[FacilityStatuses] ON");
            Insert.IntoTable("FacilityStatuses").Row(new { FacilityStatusID = 162, Description = "Pending Retirement" });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[FacilityStatuses] OFF");
        }

        public override void Down()
        {
            Delete.FromTable("FacilityStatuses").Row(new { FacilityStatusID = 162 });
        }
    }
}

