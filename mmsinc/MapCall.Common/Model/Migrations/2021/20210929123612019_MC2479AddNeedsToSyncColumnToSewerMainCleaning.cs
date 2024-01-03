using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210929123612019), Tags("Production")]
    public class MC2479AddNeedsToSyncColumnToSewerMainCleaning : Migration
    {
        public override void Up()
        {
            Create.Column("LastSyncedAt").OnTable("SewerMainCleanings").AsDateTime().Nullable();
            Create.Column("NeedsToSync").OnTable("SewerMainCleanings").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("LastSyncedAt").FromTable("SewerMainCleanings");
            Delete.Column("NeedsToSync").FromTable("SewerMainCleanings");
        }
    }
}
