using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230127084000641), Tags("Production")]
    public class MC5150_AddRiskCharacteristicsLastUpdatedFields : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("RiskCharacteristicsLastUpdatedById")
                  .OnTable("Equipment")
                  .AsInt32()
                  .Nullable()
                  .ForeignKey("tblPermissions", "RecId");

            Create.Column("RiskCharacteristicsLastUpdatedOn")
                  .OnTable("Equipment")
                  .AsDateTime()
                  .Nullable();
        }
    }
}

