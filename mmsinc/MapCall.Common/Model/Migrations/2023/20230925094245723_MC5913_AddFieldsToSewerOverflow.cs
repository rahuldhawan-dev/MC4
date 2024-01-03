using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230925094245723), Tags("Production")]
    public class MC5913_AddFieldsToSewerOverflow : Migration
    {
        private const string TABLE_NAME = "SewerOverflows";

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn("SewageRecoveredGallons").AsInt32().Nullable()
                 .AddForeignKeyColumn("DischargeWeatherRelatedTypeId", "DischargeWeatherRelatedTypes")
                 .AddForeignKeyColumn("SewerOverflowDischargeLocationId", "SewerOverflowDischargeLocations")
                 .AddForeignKeyColumn("SewerOverflowTypeId", "SewerOverflowTypes")
                 .AddForeignKeyColumn("SewerOverflowCauseId", "SewerOverflowCauses")
                 .AddColumn("DischargeLocationOther").AsAnsiString(250).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DischargeLocationOther").FromTable(TABLE_NAME);
            Delete.ForeignKeyColumn(TABLE_NAME, "SewerOverflowCauseId", "SewerOverflowCauses");
            Delete.ForeignKeyColumn(TABLE_NAME, "SewerOverflowTypeId", "SewerOverflowTypes");
            Delete.ForeignKeyColumn(TABLE_NAME, "SewerOverflowDischargeLocationId", "SewerOverflowDischargeLocations");
            Delete.ForeignKeyColumn(TABLE_NAME, "DischargeWeatherRelatedTypeId", "DischargeWeatherRelatedTypes");
            Delete.Column("SewageRecoveredGallons").FromTable(TABLE_NAME);
        }
    }
}

