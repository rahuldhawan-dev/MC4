using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using IAlterTableAddColumnOrAlterColumnOrSchemaSyntaxExtensions =
    MMSINC.ClassExtensions.FluentMigratorExtensions.IAlterTableAddColumnOrAlterColumnOrSchemaSyntaxExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418786), Tags("Production")]
    public class NormalizeShortCycleWorkOrderCompletionStringFieldsToEntityLookupsForMC1803 : Migration
    {
        public override void Up()
        {
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "AdditionalWorkNeeded");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "Purpose");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "ServiceFound");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "ServiceLeft");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "OperatedPointOfControl");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "HeatType");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "MeterPositionLocation");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "MeterDirectionalLocation");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "ReadingDeviceDirectionalLocation");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "ReadingDeviceSupplementalLocation");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "ReadingDevicePositionalLocation");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "Register1ReasonCode");
            this.NullEmptyStringValues("ShortCycleWorkOrderCompletions", "Register2ReasonCode");

            PopulateSAPValues("MeterDirectionalLocation", "MeterDirections");
            PopulateSAPValues("MeterSupplementalLocation", "MeterSupplementalLocations");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "AdditionalWorkNeeded",
                "ShortCycleWorkOrderCompletionAdditionalWorkNeeded", 40, newColumnName: "AdditionalWorkNeededId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "Purpose",
                "ShortCycleWorkOrderCompletionPurposes", 40, newColumnName: "PurposeId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "OperatedPointOfControl",
                "ShortCycleWorkOrderPointsOfControl", 4, newColumnName: "OperatedPointOfControlId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "HeatType",
                "ShortCycleWorkOrderCompletionHeatTypes", 30, newColumnName: "HeatTypeId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "ActionFlag",
                "ShortCycleWorkOrderCompletionActionFlags", 2, newColumnName: "ActionFlagId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "ActivityReason",
                "ShortCycleWorkOrderCompletionActivityReasons", 4, newColumnName: "ActivityReasonId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "Register1ReasonCode",
                "ShortCycleWorkOrderCompletionRegisterReasonCodes", 30, newColumnName: "Register1ReasonCodeId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletions", "FSRInteraction",
                "ShortCycleWorkOrderCompletionFSRInteractions", 30, newColumnName: "FSRInteractionId");

            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "ServiceFound",
                "ServiceInstallationPositions", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "ServiceLeft",
                "ServiceInstallationPositions", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "MeterPositionLocation",
                "SmallMeterLocations", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "MeterDirectionalLocation",
                "MeterDirections", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "MeterSupplementalLocation",
                "MeterSupplementalLocations", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "ReadingDeviceDirectionalLocation",
                "MeterDirections", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "ReadingDeviceSupplementalLocation",
                "MeterSupplementalLocations", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "ReadingDevicePositionalLocation",
                "SmallMeterLocations", foreignValue: "SAPCode");
            this.NormalizeToExistingTable("ShortCycleWorkOrderCompletions", "Register2ReasonCode",
                "ShortCycleWorkOrderCompletionRegisterReasonCodes");
        }

        private void PopulateSAPValues(string columnName, string tableName)
        {
            Execute.Sql(
                $"INSERT INTO {tableName} (Description, SAPCode) SELECT DISTINCT c.{columnName}, c.{columnName} FROM ShortCycleWorkOrderCompletions c WHERE c.{columnName} IS NOT NULL AND NOT EXISTS (SELECT 1 FROM {tableName} WHERE SAPCode = c.{columnName});");
        }

        public override void Down()
        {
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "ServiceFound",
                "ServiceInstallationPositions", 4, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "ServiceLeft",
                "ServiceInstallationPositions", 4, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "MeterPositionLocation",
                "SmallMeterLocations", 2, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "MeterDirectionalLocation",
                "MeterDirections", 30, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWOrkOrderCompletions", "MeterSupplementalLocation",
                "MeterSupplementalLocations", 2, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "ReadingDeviceDirectionalLocation",
                "MeterDirections", 30, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "ReadingDeviceSupplementalLocation",
                "MeterSupplementalLocations", 30, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "ReadingDevicePositionalLocation",
                "SmallMeterLocations", 30, foreignValue: "SAPCode");
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderCompletions", "Register2ReasonCode",
                "ShortCycleWorkOrderCompletionRegisterReasonCodes", 30);

            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "AdditionalWorkNeeded",
                "ShortCycleWorkOrderCompletionAdditionalWorkNeeded", 40, newColumnName: "AdditionalWorkNeededId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "Purpose",
                "ShortCycleWorkOrderCompletionPurposes", 40, newColumnName: "PurposeId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "OperatedPointOfControl",
                "ShortCycleWorkOrderPointsOfControl", 4, newColumnName: "OperatedPointOfControlId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "HeatType",
                "ShortCycleWorkOrderCompletionHeatTypes", 30, newColumnName: "HeatTypeId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "ActionFlag",
                "ShortCycleWorkOrderCompletionActionFlags", 2, newColumnName: "ActionFlagId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "ActivityReason",
                "ShortCycleWorkOrderCompletionActivityReasons", 4, newColumnName: "ActivityReasonId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "Register1ReasonCode",
                "ShortCycleWorkOrderCompletionRegisterReasonCodes", 30, newColumnName: "Register1ReasonCodeId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletions", "FSRInteraction",
                "ShortCycleWorkOrderCompletionFSRInteractions", 30, newColumnName: "FSRInteractionId");
        }
    }
}
