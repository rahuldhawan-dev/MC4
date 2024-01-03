using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210614162646170), Tags("Production")]
    public class MC2717AddingPendingToFacilityStatus : Migration
    {
        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT [dbo].[FacilityStatuses] ON");
            Insert.IntoTable("FacilityStatuses").Row(new { FacilityStatusID = 161, Description = "Pending" });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[FacilityStatuses] OFF");
        }

        public override void Down()
        {
            Delete.FromTable("FacilityStatuses").Row(new { FacilityStatusID = 161});
        }
    }
}

