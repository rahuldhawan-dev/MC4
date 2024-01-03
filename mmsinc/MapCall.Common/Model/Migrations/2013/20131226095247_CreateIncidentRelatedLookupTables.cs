using System.Collections.Generic;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131226095247), Tags("Production")]
    public class CreateIncidentRelatedLookupTables : Migration
    {
        public const int MAX_DESCRIPTION_LENGTH = 50;

        // Needs a higher length because there's a record with a 54 length.
        public const int MAX_GENERAL_LIABILITY_CODE_DESCRIPTION = 55;

        private void CreateTable(string tableName, int descLength = MAX_DESCRIPTION_LENGTH)
        {
            Create.Table(tableName)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(descLength).NotNullable().Unique();
        }

        public override void Up()
        {
            CreateTable("IncidentClassifications");
            CreateTable("GeneralLiabilityCodes", MAX_GENERAL_LIABILITY_CODE_DESCRIPTION);
            CreateTable("IncidentTypes");
            CreateTable("MotorVehicleCodes");

            Execute.Sql(@"INSERT INTO [IncidentClassifications] ([Description])
SELECT [LookupValue] as [Description] FROM [Lookup] WHERE [LookupType] = 'IncidentClassification'

INSERT INTO [GeneralLiabilityCodes] ([Description])
SELECT [LookupValue] as [Description] FROM [Lookup] WHERE [LookupType] = 'GeneralLiabilityCode'

INSERT INTO [IncidentTypes] ([Description])
SELECT [LookupValue] as [Description] FROM [Lookup] WHERE [LookupType] = 'IncidentType'

INSERT INTO [MotorVehicleCodes] ([Description])
SELECT [LookupValue] as [Description] FROM [Lookup] WHERE [LookupType] = 'MotorVehicleCode'
");
        }

        public override void Down()
        {
            Delete.Table("IncidentClassifications");
            Delete.Table("GeneralLiabilityCodes");
            Delete.Table("IncidentTypes");
            Delete.Table("MotorVehicleCodes");
        }
    }
}
