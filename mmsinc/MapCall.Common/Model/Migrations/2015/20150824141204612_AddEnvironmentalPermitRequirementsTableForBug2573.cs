using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150824141204612), Tags("Production")]
    public class AddEnvironmentalPermitRequirementsTableForBug2573 : Migration
    {
        public struct TableNames
        {
            public const string REQUIREMENT_TYPES = "EnvironmentalPermitRequirementTypes",
                                VALUE_UNITS = "EnvironmentalPermitRequirementValueUnits",
                                VALUE_DEFINITIONS = "EnvironmentalPermitRequirementValueDefinitions",
                                TRACKING_FREQUENCIES = "EnvironmentalPermitRequirementTrackingFrequencies",
                                REPORTING_FREQUENCIES = "EnvironmentalPermitRequirementReportingFrequencies",
                                REQUIREMENTS = "EnvironmentalPermitRequirements";
        }

        public const string TABLE_NAME = "EnvironmentalPermitRequirements";

        public override void Up()
        {
            Create.Table(TableNames.REQUIREMENT_TYPES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(50);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Allocation Limit')", TableNames.REQUIREMENT_TYPES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Water Quality Constituent Limit')",
                TableNames.REQUIREMENT_TYPES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Hours of Operation')", TableNames.REQUIREMENT_TYPES);

            Create.Table(TableNames.VALUE_UNITS)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(10);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('GPM')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('GPD')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('MGD')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('MGM')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('MGY')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('MG/L')", TableNames.VALUE_UNITS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Feet ASL')", TableNames.VALUE_UNITS);

            Create.Table(TableNames.VALUE_DEFINITIONS)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Discrete')", TableNames.VALUE_DEFINITIONS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Average')", TableNames.VALUE_DEFINITIONS);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Total')", TableNames.VALUE_DEFINITIONS);

            Create.Table(TableNames.TRACKING_FREQUENCIES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Exceedance')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('15 min')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Hourly')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Daily')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Weekly')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Bi-Weekly')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Monthly')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Quarterly')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Bi-Annually')", TableNames.TRACKING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Annually')", TableNames.TRACKING_FREQUENCIES);

            Create.Table(TableNames.REPORTING_FREQUENCIES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('On Exception')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Daily')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Weekly')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Monthly')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Quarterly')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Bi-Annually')", TableNames.REPORTING_FREQUENCIES);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Annually')", TableNames.REPORTING_FREQUENCIES);

            Create.Table(TableNames.REQUIREMENTS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PermitId", "EnvironmentalPermits", "EnvironmentalPermitId", false)
                  .WithForeignKeyColumn("RequirementTypeId", TableNames.REQUIREMENT_TYPES, nullable: false)
                  .WithColumn("Requirement").AsString(50).NotNullable()
                  .WithForeignKeyColumn("ValueUnitId", TableNames.VALUE_UNITS, nullable: false)
                  .WithForeignKeyColumn("ValueDefinitionId", TableNames.VALUE_DEFINITIONS, nullable: false)
                  .WithForeignKeyColumn("TrackingFrequencyId", TableNames.TRACKING_FREQUENCIES, nullable: false)
                  .WithForeignKeyColumn("ReportingFrequencyId", TableNames.REPORTING_FREQUENCIES, nullable: false)
                  .WithColumn("ReportingFrequencyDetails").AsCustom("text").Nullable()
                  .WithForeignKeyColumn("ProcessOwnerId", "tblEmployee", "tblEmployeeId")
                  .WithForeignKeyColumn("ReportingOwnerId", "tblEmployee", "tblEmployeeId", false)
                  .WithColumn("ReportDataStorageLocation").AsString(50).Nullable()
                  .WithColumn("ReportCreationInstructions").AsString(50).Nullable()
                  .WithColumn("ReportSendTo").AsCustom("text").Nullable()
                  .WithColumn("ReportSendDetails").AsString(50).Nullable()
                  .WithColumn("Notes").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.REQUIREMENTS);
            Delete.Table(TableNames.REQUIREMENT_TYPES);
            Delete.Table(TableNames.VALUE_UNITS);
            Delete.Table(TableNames.VALUE_DEFINITIONS);
            Delete.Table(TableNames.TRACKING_FREQUENCIES);
            Delete.Table(TableNames.REPORTING_FREQUENCIES);
        }
    }
}
