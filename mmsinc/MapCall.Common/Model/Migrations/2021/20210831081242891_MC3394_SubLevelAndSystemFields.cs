using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210831081242891), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3394_SubLevelAndSystemFields : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("LicenseSubLevel")
                  .OnTable("OperatorLicenses")
                  .AsAnsiString(100)
                  .Nullable();
        }
    }
}

