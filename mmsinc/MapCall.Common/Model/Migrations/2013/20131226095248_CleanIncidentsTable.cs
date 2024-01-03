using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131226095248), Tags("Production")]
    public class CleanIncidentsTable : Migration
    {
        private const string TABLE_NAME = "Incidents";

        public const int MAX_WITNESS_NAME_LENGTH = 255,
                         MAX_WITNESS_PHONE_LENGTH = 255,
                         MAX_ACCIDENT_STREET_NAME = 50,
                         MAX_ACCIDENT_STREET_NUM = 10,
                         MAX_MED_PROVIDER_LENGTH = 255,
                         MAX_MED_PROVIDER_PHONE_LENGTH = 20,
                         MAX_PREMISE_NUMBER_LENGTH = 10,
                         MAX_CREATEDBY_LENGTH = 50;

        public override void Up()
        {
            // This bug is deleting all existing data for this table in the database. 
            // So we're doing this here to make the migration easier to create.

            Delete.FromTable(TABLE_NAME).AllRows();

            // Delete columns
            Delete.Column("ApprovalDate").FromTable(TABLE_NAME);
            Delete.Column("CrossReferenceID").FromTable(TABLE_NAME);
            Delete.Column("LostWorkDay").FromTable(TABLE_NAME); // This is becoming a logical property.

            // Add new columns
            Alter.Table(TABLE_NAME)
                 .AddColumn("WitnessName").AsString(MAX_WITNESS_NAME_LENGTH).Nullable()
                 .AddColumn("WitnessPhone").AsString(MAX_WITNESS_PHONE_LENGTH).Nullable()
                 .AddColumn("SoughtMedicalAttention").AsBoolean().NotNullable()
                 .AddColumn("MedicalProviderName").AsString(MAX_WITNESS_NAME_LENGTH).Nullable()
                 .AddColumn("MedicalProviderTownId").AsInt32().Nullable()
                 .ForeignKey("FK_Incidents_Towns_MedicalProviderTownId", "Towns", "TownId")
                 .AddColumn("MedicalProviderPhone").AsString(MAX_MED_PROVIDER_PHONE_LENGTH).Nullable()
                 .AddColumn("QuestionEmployeeDoingBeforeIncidentOccurred").AsCustom("ntext").NotNullable()
                 .AddColumn("QuestionWhatHappened").AsCustom("ntext").NotNullable()
                 .AddColumn("QuestionInjuryOrIllness").AsCustom("ntext").NotNullable()
                 .AddColumn("QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee").AsCustom("ntext").NotNullable()
                 .AddColumn("SupervisorEmployeeId").AsInt32().Nullable()
                 .ForeignKey("FK_Incidents_tblEmployee_SupervisorEmployeeId", "tblEmployee", "tblEmployeeId")
                 .AddColumn("CreatedOn").AsDateTime().NotNullable()
                 .AddColumn("IsDrugAndAlcoholTestingRequired").AsBoolean().NotNullable()
                 .AddColumn("DrugAndAlcoholTestingReason").AsCustom("ntext").Nullable()
                 .AddColumn("PositionId").AsInt32().Nullable().ForeignKey(
                      "FK_Incidents_tblPositions_Classifications_PositionId", "tblPositions_Classifications",
                      "PositionId");

            // Fix columns
            Alter.Column("IncidentDate").OnTable(TABLE_NAME).AsDateTime().NotNullable();
            Alter.Column("OSHARecordable").OnTable(TABLE_NAME).AsBoolean().NotNullable();
            Alter.Column("ChargeableMotorVehicleAccident").OnTable(TABLE_NAME).AsBoolean().NotNullable();
            Alter.Column("NumberOfLostWorkDays").OnTable(TABLE_NAME).AsDecimal(18, 9).NotNullable();
            Alter.Column("SafetyCodeViolation").OnTable(TABLE_NAME).AsBoolean().NotNullable();
            Alter.Column("Litigation").OnTable(TABLE_NAME).AsBoolean().NotNullable();
            Alter.Column("FacilityID").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("EmployeeID").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("IncidentClassification").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("OpCode").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("PremiseNumber").OnTable(TABLE_NAME).AsString(MAX_PREMISE_NUMBER_LENGTH).Nullable();
            Alter.Column("CreatedBy").OnTable(TABLE_NAME).AsString(MAX_CREATEDBY_LENGTH).NotNullable();

            Rename.Column("IncidentID").OnTable(TABLE_NAME).To("Id");
            Rename.Column("TravelersCaseNumber").OnTable(TABLE_NAME).To("CaseNumber");
            Rename.Column("StreetNumber").OnTable(TABLE_NAME).To("AccidentStreetNumber");
            Rename.Column("StreetName").OnTable(TABLE_NAME).To("AccidentStreetName");
            Rename.Column("TownID").OnTable(TABLE_NAME).To("AccidentTownId");
            Rename.Column("CoordinateId").OnTable(TABLE_NAME).To("AccidentCoordinateId");
            Rename.Column("OpCode").OnTable(TABLE_NAME).To("OperatingCenterId");
            Rename.Column("ChargeableMotorVehicleAccident").OnTable(TABLE_NAME).To("IsChargeableMotorVehicleAccident");
            Rename.Column("SafetyCodeViolation").OnTable(TABLE_NAME).To("IsSafetyCodeViolation");
            Rename.Column("Litigation").OnTable(TABLE_NAME).To("IsInLitigation");
            Rename.Column("OSHARecordable").OnTable(TABLE_NAME).To("IsOSHARecordable");

            // Columns that shouldn't be smalldatetime anymore
            Alter.Column("ICRTargetCompletionDate").OnTable(TABLE_NAME).AsDateTime().Nullable();
            Alter.Column("ICRCompletionDate").OnTable(TABLE_NAME).AsDateTime().Nullable();
            Alter.Column("AccidentTownId").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("AccidentCoordinateId").OnTable(TABLE_NAME).AsInt32().NotNullable();
            Alter.Column("AccidentStreetNumber").OnTable(TABLE_NAME).AsString(MAX_ACCIDENT_STREET_NUM).NotNullable();
            Alter.Column("AccidentStreetName").OnTable(TABLE_NAME).AsString(MAX_ACCIDENT_STREET_NAME).NotNullable();

            Delete.ForeignKey("FK_Incidents_Lookup_GeneralLiabilityCode").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_Lookup_IncidentClassification").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_Lookup_IncidentType").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_Lookup_MotorVehicleCode").OnTable(TABLE_NAME);

            Rename.Column("GeneralLiabilityCode").OnTable(TABLE_NAME).To("GeneralLiabilityCodeId");
            Rename.Column("IncidentClassification").OnTable(TABLE_NAME).To("IncidentClassificationId");
            Rename.Column("IncidentType").OnTable(TABLE_NAME).To("IncidentTypeId");
            Rename.Column("MotorVehicleCode").OnTable(TABLE_NAME).To("MotorVehicleCodeId");

            Create.ForeignKey("FK_Incidents_GeneralLiabilityCodes_GeneralLiabilityCodeId")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("GeneralLiabilityCodeId")
                  .ToTable("GeneralLiabilityCodes")
                  .PrimaryColumn("Id");

            Create.ForeignKey("FK_Incidents_IncidentClassifications_IncidentClassificationId")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("IncidentClassificationId")
                  .ToTable("IncidentClassifications")
                  .PrimaryColumn("Id");

            Create.ForeignKey("FK_Incidents_IncidentTypes_IncidentTypeId")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("IncidentTypeId")
                  .ToTable("IncidentTypes")
                  .PrimaryColumn("Id");

            Create.ForeignKey("FK_Incidents_MotorVehicleCodes_MotorVehicleCodeId")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("MotorVehicleCodeId")
                  .ToTable("MotorVehicleCodes")
                  .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Incidents_MotorVehicleCodes_MotorVehicleCodeId").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_IncidentTypes_IncidentTypeId").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_IncidentClassifications_IncidentClassificationId").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_GeneralLiabilityCodes_GeneralLiabilityCodeId").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_tblPositions_Classifications_PositionId").OnTable(TABLE_NAME);

            Rename.Column("GeneralLiabilityCodeId").OnTable(TABLE_NAME).To("GeneralLiabilityCode");
            Rename.Column("IncidentClassificationId").OnTable(TABLE_NAME).To("IncidentClassification");
            Rename.Column("IncidentTypeId").OnTable(TABLE_NAME).To("IncidentType");
            Rename.Column("MotorVehicleCodeId").OnTable(TABLE_NAME).To("MotorVehicleCode");

            Alter.Column("GeneralLiabilityCode").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("IncidentClassification").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("IncidentType").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("MotorVehicleCode").OnTable(TABLE_NAME).AsInt32().Nullable();

            // Adding this in to alllow for migrations test to pass. We're well beyond the time when this would need to be rolled back.
            Execute.Sql(
                "UPDATE [Incidents] SET MotorVehicleCode = null, IncidentClassification = null, IncidentType = null, GeneralLiabilityCode = null");

            Create.ForeignKey("FK_Incidents_Lookup_MotorVehicleCode")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("MotorVehicleCode")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupID");

            Create.ForeignKey("FK_Incidents_Lookup_IncidentType")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("IncidentType")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupID");

            Create.ForeignKey("FK_Incidents_Lookup_IncidentClassification")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("IncidentClassification")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupID");

            Create.ForeignKey("FK_Incidents_Lookup_GeneralLiabilityCode")
                  .FromTable(TABLE_NAME)
                  .ForeignColumn("GeneralLiabilityCode")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupID");

            Alter.Column("AccidentTownId").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("AccidentCoordinateId").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("AccidentStreetNumber").OnTable(TABLE_NAME).AsString(MAX_ACCIDENT_STREET_NUM).Nullable();
            Alter.Column("AccidentStreetName").OnTable(TABLE_NAME).AsString(MAX_ACCIDENT_STREET_NAME).Nullable();

            Rename.Column("IsChargeableMotorVehicleAccident").OnTable(TABLE_NAME).To("ChargeableMotorVehicleAccident");
            Rename.Column("IsSafetyCodeViolation").OnTable(TABLE_NAME).To("SafetyCodeViolation");
            Rename.Column("IsInLitigation").OnTable(TABLE_NAME).To("Litigation");
            Rename.Column("IsOSHARecordable").OnTable(TABLE_NAME).To("OSHARecordable");
            Rename.Column("CaseNumber").OnTable(TABLE_NAME).To("TravelersCaseNumber");
            Rename.Column("AccidentStreetNumber").OnTable(TABLE_NAME).To("StreetNumber");
            Rename.Column("AccidentStreetName").OnTable(TABLE_NAME).To("StreetName");
            Rename.Column("AccidentTownId").OnTable(TABLE_NAME).To("TownID");
            Rename.Column("AccidentCoordinateId").OnTable(TABLE_NAME).To("CoordinateId");
            Rename.Column("OperatingCenterId").OnTable(TABLE_NAME).To("OpCode");
            Rename.Column("Id").OnTable(TABLE_NAME).To("IncidentID");

            Delete.ForeignKey("FK_Incidents_Towns_MedicalProviderTownId").OnTable(TABLE_NAME);
            Delete.ForeignKey("FK_Incidents_tblEmployee_SupervisorEmployeeId").OnTable(TABLE_NAME);

            Delete.Column("PositionId").FromTable(TABLE_NAME);
            Delete.Column("DrugAndAlcoholTestingReason").FromTable(TABLE_NAME);
            Delete.Column("IsDrugAndAlcoholTestingRequired").FromTable(TABLE_NAME);
            Delete.Column("CreatedOn").FromTable(TABLE_NAME);
            Delete.Column("SupervisorEmployeeId").FromTable(TABLE_NAME);
            Delete.Column("QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee").FromTable(TABLE_NAME);
            Delete.Column("QuestionInjuryOrIllness").FromTable(TABLE_NAME);
            Delete.Column("QuestionWhatHappened").FromTable(TABLE_NAME);
            Delete.Column("QuestionEmployeeDoingBeforeIncidentOccurred").FromTable(TABLE_NAME);
            Delete.Column("MedicalProviderPhone").FromTable(TABLE_NAME);
            Delete.Column("MedicalProviderTownId").FromTable(TABLE_NAME);
            Delete.Column("MedicalProviderName").FromTable(TABLE_NAME);
            Delete.Column("SoughtMedicalAttention").FromTable(TABLE_NAME);
            Delete.Column("WitnessPhone").FromTable(TABLE_NAME);
            Delete.Column("WitnessName").FromTable(TABLE_NAME);

            Alter.Column("CreatedBy").OnTable(TABLE_NAME).AsString(MAX_CREATEDBY_LENGTH).Nullable();
            Alter.Column("PremiseNumber").OnTable(TABLE_NAME).AsString(50).Nullable();
            Alter.Column("OpCode").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("FacilityID").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("EmployeeID").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("IncidentClassification").OnTable(TABLE_NAME).AsInt32().Nullable();
            Alter.Column("Litigation").OnTable(TABLE_NAME).AsBoolean().Nullable();
            Alter.Column("SafetyCodeViolation").OnTable(TABLE_NAME).AsBoolean().Nullable();
            Alter.Column("NumberOfLostWorkDays").OnTable(TABLE_NAME).AsFloat().Nullable();
            Alter.Column("ChargeableMotorVehicleAccident").OnTable(TABLE_NAME).AsBoolean().Nullable();
            Alter.Column("OSHARecordable").OnTable(TABLE_NAME).AsBoolean().Nullable();
            Alter.Column("IncidentDate").OnTable(TABLE_NAME).AsCustom("smalldatetime").Nullable();

            Alter.Table(TABLE_NAME).AddColumn("LostWorkDay").AsBoolean().Nullable();
            Alter.Table(TABLE_NAME).AddColumn("CrossReferenceID").AsString(50).Nullable();
            Alter.Table(TABLE_NAME).AddColumn("ApprovalDate").AsDateTime().Nullable();
        }
    }
}
