using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionPreJobSafetyBriefMap : ClassMap<ProductionPreJobSafetyBrief>
    {
        public ProductionPreJobSafetyBriefMap()
        {
            Id(x => x.Id).Not.Nullable();

            Map(x => x.AnyPotentialWeatherHazards).Not.Nullable();
            Map(x => x.AnyTimeOfDayConstraints).Not.Nullable();
            Map(x => x.AnyTrafficHazards).Not.Nullable();
            Map(x => x.AnyPotentialOverheadHazards).Not.Nullable();
            Map(x => x.AnyUndergroundHazards).Not.Nullable();
            Map(x => x.AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel).Not.Nullable();
            Map(x => x.AreThereElectricalOrOtherEnergyHazards).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CrewMembersRemindedOfStopWorkAuthority).Not.Nullable();
            Map(x => x.CrewMembersRemindedOfStopWorkAuthorityNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.DescriptionOfWork).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.DESCRIPTION_OF_WORK);
            Map(x => x.DoesJobInvolveUseOfChemicals).Not.Nullable();
            Map(x => x.ElectricalProtection).Not.Nullable();
            Map(x => x.EyeProtection).Not.Nullable();
            Map(x => x.FaceShield).Not.Nullable();
            Map(x => x.FallProtection).Not.Nullable();
            Map(x => x.FootProtection).Not.Nullable();
            Map(x => x.HadConversationAboutWeatherHazard).Not.Nullable();
            Map(x => x.HadConversationAboutWeatherHazardNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.HandProtection).Not.Nullable();
            Map(x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection).Not.Nullable();
            Map(x => x.HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.HasStretchAndFlexBeenPerformed).Not.Nullable();
            Map(x => x.HasStretchAndFlexBeenPerformedNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.HaveEquipmentToDoJobSafely).Not.Nullable();
            Map(x => x.HaveEquipmentToDoJobSafelyNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.HeadProtection).Not.Nullable();
            Map(x => x.HearingProtection).Not.Nullable();
            Map(x => x.IsSafetyDataSheetAvailableForEachChemical).Nullable();
            Map(x => x.IsSafetyDataSheetAvailableForEachChemicalNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.InvolveConfinedSpace).Not.Nullable();
            Map(x => x.OtherHazardsIdentified).Not.Nullable();
            Map(x => x.OtherHazardNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.PPEOther).Not.Nullable();
            Map(x => x.PPEOtherNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.RespiratoryProtection).Not.Nullable();
            Map(x => x.ReviewedErgonomicHazards).Not.Nullable();
            Map(x => x.ReviewedErgonomicHazardsNotes).Nullable();
            Map(x => x.ReviewedLocationOfSafetyEquipment).Not.Nullable();
            Map(x => x.ReviewedLocationOfSafetyEquipmentNotes).Nullable().Length(ProductionPreJobSafetyBrief.StringLengths.NOTES);
            Map(x => x.SafetyBriefDateTime).Not.Nullable();
            Map(x => x.SafetyGarment).Not.Nullable();
            Map(x => x.TypeOfFallPreventionProtectionSystemBeingUsed).Length(ProductionPreJobSafetyBrief.StringLengths.NOTES).Nullable();

            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();
            References(x => x.ProductionWorkOrder).Nullable();
            References(x => x.OperatingCenter).Nullable();
            References(x => x.Facility).Nullable();
            
            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Workers).KeyColumn("ProductionPreJobSafetyBriefId").Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.SafetyBriefWeatherHazardTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefWeatherHazardTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefWeatherHazardTypeId");

            HasManyToMany(x => x.SafetyBriefTimeOfDayConstraintTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefTimeOfDayConstraintTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefTimeOfDayConstraintTypeId");

            HasManyToMany(x => x.SafetyBriefTrafficHazardTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefTrafficHazardTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefTrafficHazardTypeId");

            HasManyToMany(x => x.SafetyBriefOverheadHazardTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefOverheadHazardTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefOverheadHazardTypeId");

            HasManyToMany(x => x.SafetyBriefUndergroundHazardTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefUndergroundHazardTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefUndergroundHazardTypeId");

            HasManyToMany(x => x.SafetyBriefElectricalHazardTypes)
               .Table("ProductionPreJobSafetyBriefsProductionPreJobSafetyBriefElectricalHazardTypes")
               .ParentKeyColumn("ProductionPreJobSafetyBriefId")
               .ChildKeyColumn("ProductionPreJobSafetyBriefElectricalHazardTypeId");
        }
    }
}
