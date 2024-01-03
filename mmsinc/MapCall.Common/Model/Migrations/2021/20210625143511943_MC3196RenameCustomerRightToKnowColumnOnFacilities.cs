using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210625143511943), Tags("Production")]
    public class MC3196RenameCustomerRightToKnowColumnOnFacilities : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Column("CustomerRightToKnow").OnTable("tblFacilities").To("CommunityRightToKnow");
        }
    }
}

