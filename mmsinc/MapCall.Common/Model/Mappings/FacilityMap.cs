using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityMap : ClassMap<Facility>
    {
        #region Constants

        public const string TABLE_NAME = "tblFacilities";

        #endregion

        #region Constructors

        public FacilityMap()
        {
            Table(TABLE_NAME);

            LazyLoad();

            Id(x => x.Id, "RecordID");
            References(x => x.AWSecurityTier, "FacilityAWSecurityTierId").Nullable();
            References(x => x.CompanySubsidiary).Nullable();
            References(x => x.Condition).Nullable();
            References(x => x.ConsequenceOfFailure).Nullable();
            References(x => x.Coordinate).Cascade.All();
            References(x => x.Department).Not.Nullable();
            References(x => x.ElectricalProvider).Nullable();
            References(x => x.FacilityOwner, "Facility_Ownership");
            References(x => x.FacilityStatus, "Status");
            References(x => x.FEMAFloodRating, "FEMA_Flood_Rating");
            References(x => x.LikelihoodOfFailure).Nullable();
            References(x => x.NearestCrossStreet, "CrossStreetID");
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PlanningPlant).Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            References(x => x.PublicWaterSupplyPressureZone).Nullable();
            References(x => x.ParentFacility).Nullable();
            References(x => x.Performance).Nullable();
            References(x => x.StrategyTier).Nullable();
            References(x => x.Street);
            References(x => x.Town);
            References(x => x.TownSection).Nullable();
            References(x => x.WasteWaterSystem);
            References(x => x.WasteWaterSystemBasin).Nullable();
            References(x => x.Process).Nullable().Fetch.Join();
            References(x => x.SystemDeliveryType).Nullable();
            References(x => x.MaintenanceRiskOfFailure, "MaintenanceRiskOfFailureId").Nullable();
            References(x => x.CreatedBy).Nullable();
            References(x => x.InsuranceScoreQuartile, "InsuranceScoreQuartile").Nullable();
            
            Map(x => x.Administration);
            Map(x => x.ArcFlashStudyRequired).Nullable();
            Map(x => x.BoosterStation);
            Map(x => x.CellularAntenna);
            Map(x => x.ChemicalFeed);
            Map(x => x.HasConfinedSpaceRequirement).Nullable();
            Map(x => x.ConsequenceOfFailureFactor).Nullable();
            Map(x => x.CriticalRating);
            Map(x => x.Dam);
            Map(x => x.CreatedAt);
            Map(x => x.FacilityReliableCapacityMGD);
            Map(x => x.EnvironmentalRegulatorIDNumber).Length(Facility.StringLengths.ENVIRONMENTAL_REGULATOR_ID_NUMBER);
            Map(x => x.FacilityRatedCapacityMGD);
            Map(x => x.FacilityAuxPowerCapacityMGD);
            Map(x => x.FacilityOperatingCapacityMGD);
            Map(x => x.UsedInProductionCapacityCalculation);
            Map(x => x.DPCC);
            Map(x => x.DistributivePumping, "Distributive_Pumping");
            Map(x => x.ElectricalAccountNumber).Nullable().Length(Facility.StringLengths.ELECTRICAL_ACCOUNT_NUMBER);
            Map(x => x.ElevatedStorage, "Elevated_Storage");
            Map(x => x.EmergencyPower);
            Map(x => x.EntityNotes, "Notes").Length(Facility.StringLengths.NOTES);
            Map(x => x.FacilityContactInfo, "Facility_Contact_Info").Length(Facility.StringLengths.FACILITY_CONTACT_INFO);
            Map(x => x.FacilityInspectionFrequency, "Facility_Inspection_Frequency").Length(Facility.StringLengths.FACILITY_INSPECTION_FREQUENCY);
            Map(x => x.FacilityLoopGrouping, "Facility_Loop_Grouping").Length(Facility.StringLengths.FACILITY_LOOP_GROUPING);
            Map(x => x.FacilityLoopGroupingSub, "Facility_Loop_Grouping_Sub").Length(Facility.StringLengths.FACILITY_LOOP_GROUPING_SUB);
            Map(x => x.FacilityLoopSequence, "Facility_Loop_Sequence");
            Map(x => x.FacilityName).Length(Facility.StringLengths.FACILITY_NAME);
            Map(x => x.FieldOperations);
            Map(x => x.Filtration);
            Map(x => x.FunctionalLocation, "FunctionalLocationId")
               .Nullable().Length(Facility.StringLengths.FUNCTIONAL_LOCATION);
            Map(x => x.GroundStorage, "Ground_Storage");
            Map(x => x.GroundWaterSupply, "Ground_Water_Supply");
            Map(x => x.Interconnection);
            Map(x => x.FacilityTotalCapacityMGD);
            Map(x => x.DesignationTreatmentPlant).Length(Facility.StringLengths.DESIGNATION_TREATMENT_PLANT);
            Map(x => x.DesignationPumpStation).Length(Facility.StringLengths.DESIGNATION_PUMP_STATION);
            Map(x => x.OnSiteAnalyticalInstruments, "On_Site_Analytical_Instruments");
            Map(x => x.Operations).Length(Facility.StringLengths.OPERATIONS);
            Map(x => x.PointOfEntry, "Point_Of_Entry");
            Map(x => x.PressureReducing);
            Map(x => x.PropertyOnly);
            Map(x => x.PSM).Not.Nullable();
            Map(x => x.RegionalPlanningArea).Length(Facility.StringLengths.REGIONAL_PLANNING_AREA);
            Map(x => x.Reservoir);
            Map(x => x.ResidualsGeneration, "Residuals_Generation");
          
            Map(x => x.RMP).Not.Nullable();
            Map(x => x.RMPNumber).Nullable();
            Map(x => x.SCADAIntrusionAlarm, "SCADA_Intrusion_Alarm");
            Map(x => x.SecurityCategory, "Security_Category").Length(Facility.StringLengths.SECURITY_CATEGORY);
            Map(x => x.SecurityGrouping, "Security_Grouping").Length(Facility.StringLengths.SECURITY_GROUPING);
            Map(x => x.SecurityInspectionFrequency, "Security_Inspection_Frequency")
               .Length(Facility.StringLengths.SECURITY_INSPECTION_FREQUENCY);
            Map(x => x.SecurityLoopSequence, "Security_Loop_Sequence")
               .Length(Facility.StringLengths.SECURITY_LOOP_SEQUENCE);
            Map(x => x.SewerLiftStation);
            Map(x => x.WasteWaterTreatmentFacility);
            Map(x => x.SICNumber);
            Map(x => x.SpoilsStaging, "Spoils_Staging");
            Map(x => x.SurfaceWaterSupply, "Surface_Water_Supply");
            Map(x => x.System).Length(Facility.StringLengths.SYSTEM);
            Map(x => x.SampleStation);
            Map(x => x.StreetNumber).Length(Facility.StringLengths.STREET_NUMBER);
            Map(x => x.TReport, "T_Report");
            Map(x => x.WaterShed).Length(Facility.StringLengths.WATER_SHED);
            Map(x => x.WaterTreatmentFacility);
            Map(x => x.WeightedRiskOfFailureScore).Nullable();
            Map(x => x.YearInService).Length(Facility.StringLengths.YEAR_IN_SERVICE);
            Map(x => x.ZipCode).Length(Facility.StringLengths.ZIP_CODE);
            Map(x => x.SWMStation, "SwmStation").Not.Nullable();
            Map(x => x.WellProd).Not.Nullable();
            Map(x => x.WellMonitoring).Not.Nullable();
            Map(x => x.ClearWell).Not.Nullable();
            Map(x => x.RawWaterIntake).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.Radionuclides).Not.Nullable();
            Map(x => x.CommunityRightToKnow).Not.Nullable();
            Map(x => x.IsInVamp).Nullable();
            Map(x => x.VampUrl).Length(Facility.StringLengths.VAMP_URL).Nullable();
            Map(x => x.RiskBasedCompletedDate).Nullable(); 
            Map(x => x.CriticalFacilityIdentified).Nullable(); 
            Map(x => x.AssessmentCompletedOn).Nullable();
            Map(x => x.BasicGroundWaterSupply).Not.Nullable();
            Map(x => x.RawWaterPumpStation).Not.Nullable();
            Map(x => x.WaterStress).Not.Nullable();
            Map(x => x.IgnitionEnterprisePortal).Not.Nullable();
            Map(x => x.ArcFlashLabelRequired).Not.Nullable();
            Map(x => x.InsuranceId).Nullable().Length(Facility.StringLengths.INSURANCE_ID);
            Map(x => x.InsuranceScore).Nullable();
            Map(x => x.InsuranceVisitDate).Nullable();

            //HasMany
            HasMany(x => x.FacilityDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.FacilityNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.Equipment).KeyColumn("FacilityID").LazyLoad().Cascade.All();
            HasMany(x => x.ChildFacilities).KeyColumn("ParentFacilityId").LazyLoad();
            HasMany(x => x.Interconnections).KeyColumn("FacilityId").LazyLoad();
            HasMany(x => x.KwhCosts).KeyColumn("FacilityId").LazyLoad();
            HasMany(x => x.FacilityProcesses).KeyColumn("FacilityId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.FacilityAreas).KeyColumn("FacilityId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ArcFlashStudies).KeyColumn("FacilityId").LazyLoad();
            HasMany(x => x.FacilitySystemDeliveryEntryTypes).KeyColumn("FacilityId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.CommunityRightToKnows).KeyColumn("FacilityId").LazyLoad();
            HasMany(x => x.SystemDeliveryEntries).KeyColumn("FacilityId").LazyLoad().ReadOnly();
            HasMany(x => x.MaintenancePlans).KeyColumn("FacilityId").LazyLoad();
            // THIS IS DONE THROUGH A CTE/VIEW
            HasMany(x => x.MostRecentArcFlashStudies).KeyColumn("FacilityId").LazyLoad().Cascade.None().Inverse();
            //Formula
            Map(x => x.HasSensorAttached)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM Equipment E JOIN TFProd_Equipment_Sensor tes ON tes.EquipmentID = E.EquipmentID WHERE E.FacilityID = RecordId ) THEN 1 ELSE 0 END)");
        }

        #endregion
    }
}
