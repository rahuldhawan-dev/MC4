using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentMap : ClassMap<Incident>
    {
        #region Constructors

        public IncidentMap()
        {
            Id(x => x.Id);

            Map(x => x.AccidentStreetName)
               .Not.Nullable()
               .Length(Incident.StringLengths.MAX_ACCIDENT_STREET_NAME);
            Map(x => x.AccidentStreetNumber)
               .Not.Nullable()
               .Length(Incident.StringLengths.MAX_ACCIDENT_STREET_NUMBER);
            Map(x => x.AnyImmediateCorrectiveActionsApplied)
               .Nullable()
               .Length(int.MaxValue);
            Map(x => x.IncidentSummary)
               .Nullable()
               .Length(Incident.StringLengths.INCIDENT_SUMMARY);
            Map(x => x.AthleticActivitiesInLastTwelveMonths)
               .Nullable();
            Map(x => x.CaseNumber)
               .Nullable()
               .Length(Incident.StringLengths.MAX_CASE_NUMBER);
            Map(x => x.CaseManager)
               .Nullable()
               .Length(Incident.StringLengths.MAX_CASE_MANAGER);
            Map(x => x.Claimant)
               .Nullable()
               .Length(Incident.StringLengths.MAX_CLAIMANT);
            Map(x => x.ContractorCompany)
               .Nullable()
               .Length(Incident.StringLengths.CONTRACTOR_COMPANY);
            Map(x => x.ContractorName)
               .Nullable()
               .Length(Incident.StringLengths.CONTRACTOR_NAME);
            Map(x => x.CreatedAt)
               .Not.Nullable();
            Map(x => x.CreatedBy)
               .Not.Nullable()
               .Length(Incident.StringLengths.MAX_CREATEDBY);
            Map(x => x.DateInvestigationWillBeCompleted)
               .Nullable();
            Map(x => x.DrugAndAlcoholTestingNotes)
               .Nullable();
            Map(x => x.FiveWhysCompleted).Not.Nullable();
            Map(x => x.IncidentCommitteeReportCompletionDate, "ICRCompletionDate")
               .Nullable();
            Map(x => x.IncidentCommitteeReportTargetCompletionDate, "ICRTargetCompletionDate")
               .Nullable();
            Map(x => x.IncidentCommitteeReportResults, "ICRResults")
               .Nullable();
            Map(x => x.IncidentDate).Not.Nullable();
            Map(x => x.IncidentReportedDate).Not.Nullable();
            Map(x => x.IsChargeableMotorVehicleAccident)
               .Not.Nullable();
            Map(x => x.IsInLitigation)
               .Not.Nullable();
            Map(x => x.IsOvertime)
               .Nullable(); // Nullable because existing records won't have this value set.
            Map(x => x.IsSafetyCodeViolation)
               .Not.Nullable();
            Map(x => x.UpdatedAt)
               .Not.Nullable();
            Map(x => x.MarkoutNumber)
               .Nullable()
               .Length(Incident.StringLengths.MAX_MARKOUT_NUMBER);
            Map(x => x.MedicalProviderName)
               .Nullable()
               .Length(Incident.StringLengths.MAX_MED_PROVIDER_NAME);
            Map(x => x.MedicalProviderPhone)
               .Nullable()
               .Length(Incident.StringLengths.MAX_MED_PROVIDER_PHONE);
            Map(x => x.NatureOfPriorInjury)
               .Nullable();
            Map(x => x.NumberOfHoursOvertimeInPastWeek).Nullable();
            Map(x => x.IsOSHARecordable)
               .Not.Nullable();
            Map(x => x.OtherEmployers)
               .Nullable();
            Map(x => x.PoliceReportFiled)
               .Not.Nullable();
            Map(x => x.PremiseNumber)
               .Nullable()
               .Length(Incident.StringLengths.MAX_PREMISE_NUMBER);
            Map(x => x.PriorInjuryDate)
               .Nullable();
            Map(x => x.PriorInjuryMedicalProvider)
               .Nullable();
            Map(x => x.QuestionEmployeeDoingBeforeIncidentOccurred)
               .Not.Nullable();
            Map(x => x.QuestionWhatHappened)
               .Not.Nullable();
            Map(x => x.QuestionInjuryOrIllness)
               .Not.Nullable();
            Map(x => x.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee)
               .Not.Nullable();
            Map(x => x.QuestionHaveHadSimilarInjuryBefore)
               .Not.Nullable();
            Map(x => x.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths)
               .Not.Nullable();
            Map(x => x.QuestionHaveJobOutsideOfAmericanWater)
               .Not.Nullable();
            Map(x => x.SoughtMedicalAttention)
               .Not.Nullable();
            Map(x => x.WitnessName)
               .Nullable()
               .Length(Incident.StringLengths.MAX_WITNESS_NAME);
            Map(x => x.WitnessPhone)
               .Nullable()
               .Length(Incident.StringLengths.MAX_WITNESS_PHONE);
            Map(x => x.WorkOrderId)
               .Nullable()
               .Length(Incident.StringLengths.MAX_WORKORDER_ID);
            Map(x => x.TravelersReport).Nullable();
            Map(x => x.RecommendedMedicalProvider)
               .Length(Incident.StringLengths.RECOMMENDED_MEDICAL_PROVIDER)
               .Nullable();
            Map(x => x.NonMedicalTreatmentRecommendation)
               .Length(Incident.StringLengths.NON_MEDICAL_TREATMENT_RECOMMENDATION)
               .Nullable();
            Map(x => x.EmployeeAcceptedRecommendationByNurse).Nullable();
            Map(x => x.NursePhone)
               .Nullable()
               .Length(Incident.StringLengths.NURSE_PHONE);
            Map(x => x.ReasonEmployeeDidNotAcceptRecommendationByNurse).Nullable();
            Map(x => x.Why1)
               .Nullable()
               .Length(Incident.StringLengths.FIVE_WHYS);
            Map(x => x.Why2)
               .Nullable()
               .Length(Incident.StringLengths.FIVE_WHYS);
            Map(x => x.Why3)
               .Nullable()
               .Length(Incident.StringLengths.FIVE_WHYS);
            Map(x => x.Why4)
               .Nullable()
               .Length(Incident.StringLengths.FIVE_WHYS);
            Map(x => x.Why5)
               .Nullable()
               .Length(Incident.StringLengths.FIVE_WHYS);
            Map(x => x.DateSubmitted)
               .Nullable();
            Map(x => x.ClaimsCarrierId)
               .Nullable()
               .Length(Incident.StringLengths.MAX_CLAIMS_CARRIER_ID);

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ActionItems)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.EmployeeAvailabilities).KeyColumn("IncidentId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.IncidentInvestigations).KeyColumn("IncidentId").Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.BodyParts).Table("IncidentsBodyParts").ParentKeyColumn("IncidentId").ChildKeyColumn("BodyPartId");
            HasMany(x => x.EmployeeAccountabilityActions).KeyColumn("IncidentId").Inverse().Cascade.None();

            References(x => x.EmployeeSpokeWithNurse, "EmployeeSpeakToNurseId").Not.Nullable();
            References(x => x.IncidentNurseRecommendationType).Nullable();
            References(x => x.AccidentTown).Not.Nullable();
            References(x => x.AccidentCoordinate).Not.Nullable();
            References(x => x.DrugAndAlcoholTestingDecision, "IncidentDrugAndAlcoholTestingDecisionId").Not.Nullable();
            References(x => x.DrugAndAlcoholTestingResult, "IncidentDrugAndAlcoholTestingResultId").Nullable();
            References(x => x.Employee).Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.GeneralLiabilityCode).Nullable();
            References(x => x.IncidentClassification).Not.Nullable();
            References(x => x.IncidentShift).Not.Nullable();
            References(x => x.IncidentType).Nullable();
            References(x => x.MedicalProviderTown).Nullable();
            References(x => x.MotorVehicleCode).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Position).Nullable();
            References(x => x.Supervisor, "SupervisorEmployeeId").Nullable();
            References(x => x.PersonnelArea, "PersonnelAreaId").Nullable();
            References(x => x.Vehicle).Nullable();
            References(x => x.Department).Nullable();
            References(x => x.IncidentStatus).Not.Nullable();
            References(x => x.AtRiskBehaviorSection).Nullable();
            References(x => x.AtRiskBehaviorSubSection).Nullable();
            References(x => x.EventExposureType).Nullable();
            References(x => x.EmployeeType).Not.Nullable();
            References(x => x.ContractorObservedBy).Nullable();
            References(x => x.WorkersCompensationClaimStatus).Nullable();
            References(x => x.MapCallWorkOrder).Nullable();
            References(x => x.SeriousInjuryOrFatalityType).Nullable();
        }

        #endregion
    }
}
