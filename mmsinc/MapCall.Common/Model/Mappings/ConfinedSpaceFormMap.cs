using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormMap : ClassMap<ConfinedSpaceForm>
    {
        public const string TABLE_NAME = "ConfinedSpaceForms";

        public ConfinedSpaceFormMap()
        {
            Id(x => x.Id);

            Map(x => x.BeginEntryAuthorizedAt).Nullable();
            Map(x => x.BumpTestConfirmedAt).Nullable();
            Map(x => x.CanBeControlledByVentilationAlone).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.EmergencyResponseAgency).Nullable()
                                               .Length(ConfinedSpaceForm.StringLengths.EMERGENCY_RESPONSE_AGENCY);
            Map(x => x.EmergencyResponseContact).Nullable()
                                                .Length(ConfinedSpaceForm.StringLengths.EMERGENCY_RESPONSE_CONTACT);
            Map(x => x.GeneralDateTime).Not.Nullable();
            Map(x => x.HasAccessSafetyEquipment).Nullable();
            Map(x => x.HasContractRescueService).Nullable();
            Map(x => x.HasEyeSafetyEquipment).Nullable();
            Map(x => x.HasFallSafetyEquipment).Nullable();
            Map(x => x.HasFootSafetyEquipment).Nullable();
            Map(x => x.HasGFCISafetyEquipment).Nullable();
            Map(x => x.HasHandSafetyEquipment).Nullable();
            Map(x => x.HasHeadSafetyEquipment).Nullable();
            Map(x => x.HasLightingSafetyEquipment).Nullable();
            Map(x => x.HasOtherSafetyEquipment).Nullable();
            Map(x => x.HasOtherSafetyEquipmentNotes).Nullable()
                                                    .Length(ConfinedSpaceForm.StringLengths
                                                                             .HAS_OTHER_SAFETY_EQUIPMENT_NOTES);
            Map(x => x.HasRespiratorySafetyEquipment).Nullable();
            Map(x => x.HasRetrievalSystem).Nullable();
            Map(x => x.HasVentilationSafetyEquipment).Nullable();
            Map(x => x.HasWarningSafetyEquipment).Nullable();
            Map(x => x.HazardSignedAt).Nullable();
            Map(x => x.IsFireWatchRequired).Nullable();
            Map(x => x.IsHotWorkPermitRequired).Nullable();
            Map(x => x.ShortCycleWorkOrderNumber).Nullable();
            Map(x => x.LocationAndDescriptionOfConfinedSpace)
               .Length(ConfinedSpaceForm.StringLengths.LOCATION_AND_DESCRIPTION).Not.Nullable();
            Map(x => x.MethodOfCommunicationOtherNotes)
               .Length(ConfinedSpaceForm.StringLengths.METHOD_OF_COMMUNICATION_OTHER_NOTES).Nullable();
            Map(x => x.PermitBeginsAt).Nullable();
            Map(x => x.PermitCancelledAt).Nullable();
            Map(x => x.PermitEndsAt).Nullable();
            Map(x => x.PurposeOfEntry).Length(ConfinedSpaceForm.StringLengths.PURPOSE_OF_ENTRY).Not.Nullable();
            Map(x => x.ReclassificationSignedAt).Nullable();
            Map(x => x.PermitCancellationNote).Length(ConfinedSpaceForm.StringLengths.PERMIT_CANCELLATION_NOTE)
                                              .Nullable();

            References(x => x.BeginEntryAuthorizedBy, "BeginEntryAuthorizedByEmployeeId").Nullable();
            References(x => x.BumpTestConfirmedBy, "BumpTestConfirmedByEmployeeId").Nullable();
            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();
            References(x => x.GasMonitor).Nullable();
            References(x => x.HazardSignedBy, "HazardSignedByEmployeeId").Nullable();
            References(x => x.MethodOfCommunication, "ConfinedSpaceFormMethodOfCommunicationId").Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PermitCancelledBy, "PermitCancelledByEmployeeId").Nullable();
            References(x => x.ProductionWorkOrder).Nullable();
            References(x => x.ReclassificationSignedBy, "ReclassificationSignedByEmployeeId").Nullable();
            References(x => x.WorkOrder).Nullable();

            HasMany(x => x.AtmosphericTests).KeyColumn("ConfinedSpaceFormId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Hazards).KeyColumn("ConfinedSpaceFormId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Entrants).KeyColumn("ConfinedSpaceFormId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
