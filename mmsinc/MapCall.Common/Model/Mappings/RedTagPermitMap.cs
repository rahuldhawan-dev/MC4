using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RedTagPermitMap : ClassMap<RedTagPermit> 
    {
        #region Constructors

        public RedTagPermitMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.Equipment).Not.Nullable();
            References(x => x.PersonResponsible).Not.Nullable();
            References(x => x.AuthorizedBy).Not.Nullable();
            References(x => x.ProtectionType).Not.Nullable();

            Map(x => x.NumberOfTurnsToClose).Not.Nullable();
            Map(x => x.EquipmentImpairedOn).Not.Nullable();
            Map(x => x.EquipmentRestoredOn).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.EmergencyOrganizationNotified).Nullable();
            Map(x => x.PublicFireDepartmentNotified).Nullable();
            Map(x => x.HazardousOperationsStopped).Nullable();
            Map(x => x.HotWorkProhibited).Nullable();
            Map(x => x.SmokingProhibited).Nullable();
            Map(x => x.ContinuousWorkAuthorized).Nullable();
            Map(x => x.OngoingPatrolOfArea).Nullable();
            Map(x => x.HydrantConnectedToSprinkler).Nullable();
            Map(x => x.PipePlugsOnHand).Nullable();
            Map(x => x.FireHoseLaidOut).Nullable();
            Map(x => x.HasOtherPrecaution).Nullable();

            Map(x => x.OtherPrecautionDescription)
               .Length(RedTagPermit.StringLengths.OTHER_PRECAUTION_DESCRIPTION)
               .Nullable();

            Map(x => x.AdditionalInformationForProtectionType)
               .Length(RedTagPermit.StringLengths.ADDITIONAL_INFORMATION_FOR_PROTECTION_TYPE)
               .Nullable();

            Map(x => x.AreaProtected)
               .Length(RedTagPermit.StringLengths.AREA_PROTECTED)
               .Not.Nullable();

            Map(x => x.ReasonForImpairment)
               .Length(RedTagPermit.StringLengths.REASON_FOR_IMPAIRMENT)
               .Not.Nullable();

            Map(x => x.FireProtectionEquipmentOperator)
               .Length(RedTagPermit.StringLengths.FIRE_PROTECTION_EQUIPMENT_OPERATOR)
               .Not.Nullable();

            Map(x => x.EquipmentRestoredOnChangeReason)
               .Length(RedTagPermit.StringLengths.EQUIPMENT_RESTORED_ON_CHANGE_REASON)
               .Nullable();
        }

        #endregion
    }
}
