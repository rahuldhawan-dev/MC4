using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobSiteCheckListMap : ClassMap<JobSiteCheckList>
    {
        public JobSiteCheckListMap()
        {
            Id(x => x.Id);

            Map(x => x.Address)
               .Not.Nullable();
            Map(x => x.AllEmployeesWearingAppropriatePersonalProtectionEquipment)
               .Nullable();
            Map(x => x.SpotterAssigned)
               .Nullable();
            Map(x => x.IsManufacturerDataOnSiteForShoringOrShieldingEquipment)
               .Nullable();
            Map(x => x.IsTheExcavationGuardedFromAccidentalEntry)
               .Nullable();
            Map(x => x.AreThereAnyVisualSignsOfPotentialSoilCollapse)
               .Nullable();
            Map(x => x.IsTheExcavationSubjectToVibration)
               .Nullable();
            Map(x => x.AllMaterialsSetBackFromEdgeOfTrenches)
               .Nullable();
            Map(x => x.AllStructuresSupportedOrProtected)
               .Nullable();
            Map(x => x.AreExposedUtilitiesProtected)
               .Nullable();
            Map(x => x.AtmosphericCarbonMonoxideLevel)
               .Nullable();
            Map(x => x.AtmosphericLowerExplosiveLimit)
               .Nullable();
            Map(x => x.AtmosphericOxygenLevel)
               .Nullable();
            Map(x => x.CheckListDate)
               .Not.Nullable();
            Map(x => x.CompliesWithStandards)
               .Nullable();
            Map(x => x.CreatedBy)
               .Not.Nullable()
               .Length(JobSiteCheckList.StringLengths.CREATED_BY);
            Map(x => x.CreatedAt)
               .Not.Nullable();
            Map(x => x.HasAtmosphereBeenTested)
               .Nullable();
            Map(x => x.HasBarricadesForTrafficControl)
               .Not.Nullable();
            Map(x => x.HasConesForTrafficControl)
               .Not.Nullable();
            Map(x => x.HasExcavationOverFourFeetDeep)
               .Not.Nullable();
            Map(x => x.HasExcavationFiveFeetOrDeeper)
               .Not.Nullable();
            Map(x => x.HasFlagPersonForTrafficControl)
               .Not.Nullable();
            Map(x => x.HasPoliceForTrafficControl)
               .Not.Nullable();
            Map(x => x.HasSignsForTrafficControl)
               .Not.Nullable();
            Map(x => x.IsALadderInPlace)
               .Nullable();
            Map(x => x.IsEmergencyMarkoutRequest)
               .Nullable();
            Map(x => x.IsLadderOnSlope)
               .Nullable();
            Map(x => x.IsMarkoutValidForSite)
               .Nullable();
            Map(x => x.IsPressurizedRisksRestrainedFieldRequired)
               .Not.Nullable();
            Map(x => x.IsShoringSystemUsed)
               .Nullable();
            Map(x => x.IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical)
               .Nullable();
            Map(x => x.LadderExtendsAboveGrade)
               .Nullable();
            Map(x => x.MarkedElectric)
               .Nullable();
            Map(x => x.MarkedFuelGas)
               .Nullable();
            Map(x => x.MarkedOther)
               .Nullable();
            Map(x => x.MarkedSanitarySewer)
               .Nullable();
            Map(x => x.MarkedTelephone)
               .Nullable();
            Map(x => x.MarkedWater)
               .Nullable();
            Map(x => x.MarkoutNumber)
               .Nullable()
               .Length(JobSiteCheckList.StringLengths.MARKOUT_NUMBER);
            Map(x => x.ShoringSystemInstalledTwoFeetFromBottomOfTrench)
               .Nullable();
            Map(x => x.ShoringSystemSidesExtendAboveBaseOfSlope)
               .Nullable();
            Map(x => x.SupervisorSignOffDate)
               .Nullable();
            Map(x => x.WaterControlSystemsInUse)
               .Nullable();
            Map(x => x.SAPWorkOrderId)
               .Nullable()
               .Length(JobSiteCheckList.StringLengths.WORK_ORDER_ID);
            Map(x => x.HasExcavation).Nullable();
            Map(x => x.SafetyBriefDateTime).Nullable();
            Map(x => x.AnyPotentialWeatherHazards).Nullable();
            Map(x => x.HadConversationAboutWeatherHazard).Nullable();
            Map(x => x.AnyTimeOfDayConstraints).Nullable();
            Map(x => x.AnyTrafficHazards).Nullable();
            Map(x => x.InvolveConfinedSpace).Nullable();
            Map(x => x.AnyPotentialOverheadHazards).Nullable();
            Map(x => x.AnyUndergroundHazards).Nullable();
            Map(x => x.AreThereElectricalHazards).Nullable();
            Map(x => x.WorkingWithACPipe).Nullable();
            Map(x => x.HaveYouInspectedSlings).Nullable();
            Map(x => x.CrewMembersTrainedInACPipe).Nullable();
            Map(x => x.HaveEquipmentToDoJobSafely).Nullable();
            Map(x => x.HaveEquipmentToDoJobSafelyNotes).Nullable().Length(JobSiteCheckList.StringLengths.NOTES);
            Map(x => x.ReviewedErgonomicHazards).Nullable();
            Map(x => x.ReviewedErgonomicHazardsNotes).Nullable();
            Map(x => x.ReviewedLocationOfSafetyEquipment).Nullable();
            Map(x => x.OtherHazardsIdentified).Nullable();
            Map(x => x.OtherHazardNotes).Nullable().Length(JobSiteCheckList.StringLengths.NOTES);
            Map(x => x.HadDiscussionAboutHazardsAndPrecautions).Nullable();
            Map(x => x.HadDiscussionAboutHazardsAndPrecautionsNotes).Nullable()
                                                                    .Length(JobSiteCheckList.StringLengths.NOTES);
            Map(x => x.CrewMembersRemindedOfStopWorkAuthority).Nullable();
            Map(x => x.HeadProtection).Nullable();
            Map(x => x.HandProtection).Nullable();
            Map(x => x.ElectricalProtection).Nullable();
            Map(x => x.FootProtection).Nullable();
            Map(x => x.EyeProtection).Nullable();
            Map(x => x.FaceShield).Nullable();
            Map(x => x.SafetyGarment).Nullable();
            Map(x => x.HearingProtection).Nullable();
            Map(x => x.RespiratoryProtection).Nullable();
            Map(x => x.PPEOther).Nullable();
            Map(x => x.PPEOtherNotes).Nullable().Length(JobSiteCheckList.StringLengths.NOTES);
            Map(x => x.HadConversationAboutWeatherHazardNotes).Nullable().Length(JobSiteCheckList.StringLengths.NOTES);

            References(x => x.CompetentEmployee)
               .Not.Nullable();
            References(x => x.Coordinate)
               .Not.Nullable();
            References(x => x.MapCallWorkOrder)
               .Nullable();
            References(x => x.NoRestraintReason, "NoRestraintReasonTypeId")
               .Nullable();
            References(x => x.OperatingCenter)
               .Not.Nullable();
            References(x => x.PressurizedRiskRestrainedType)
               .Nullable();

            // Required field, but db is full of nulls.
            References(x => x.RestraintMethod, "RestraintMethodTypeId")
               .Nullable();
            References(x => x.SupervisorSignOffEmployee)
               .Nullable();
            References(x => x.SoilConditionsWithinExcavationType)
               .Nullable();
            References(x => x.SoilCompositionExcavationType)
               .Nullable();
            HasManyToMany(x => x.ProtectionTypes)
               .Table("JobSiteCheckListsJobSiteExcavationProtectionTypes")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteExcavationProtectionTypeId")
               .Cascade.All();

            HasMany(x => x.Comments).KeyColumn("JobSiteCheckListId")
                                    .Inverse()
                                    .Cascade.AllDeleteOrphan();
            HasMany(x => x.CrewMembers).KeyColumn("JobSiteCheckListId")
                                       .Inverse()
                                       .Cascade.AllDeleteOrphan();
            HasMany(x => x.Excavations).KeyColumn("JobSiteCheckListId")
                                       .Inverse()
                                       .Cascade.AllDeleteOrphan();
            HasMany(x => x.JobSiteCheckListNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.JobSiteCheckListDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.SafetyBriefWeatherHazardTypes)
               .Table("JobSiteCheckListSafetyBriefWeatherHazardAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefWeatherHazardTypeId");

            HasManyToMany(x => x.SafetyBriefTimeOfDayConstraintTypes)
               .Table("JobSiteCheckListSafetyBriefTimeOfDayConstraintAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefTimeOfDayConstraintTypeId");

            HasManyToMany(x => x.SafetyBriefTrafficHazardTypes)
               .Table("JobSiteCheckListSafetyBriefTrafficHazardAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefTrafficHazardTypeId");

            HasManyToMany(x => x.SafetyBriefOverheadHazardTypes)
               .Table("JobSiteCheckListSafetyBriefOverheadHazardAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefOverheadHazardTypeId");

            HasManyToMany(x => x.SafetyBriefUndergroundHazardTypes)
               .Table("JobSiteCheckListSafetyBriefUndergroundHazardAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefUndergroundHazardTypeId");

            HasManyToMany(x => x.SafetyBriefElectricalHazardTypes)
               .Table("JobSiteCheckListSafetyBriefElectricalHazardAnswers")
               .ParentKeyColumn("JobSiteCheckListId")
               .ChildKeyColumn("JobSiteCheckListSafetyBriefElectricalHazardTypeId");
        }
    }
}
