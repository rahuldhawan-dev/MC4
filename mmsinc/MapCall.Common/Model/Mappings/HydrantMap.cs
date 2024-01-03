using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantMap : ClassMap<Hydrant>
    {
        #region Constructors

        public HydrantMap()
        {
            Id(x => x.Id);

            DynamicUpdate();

            References(x => x.Town).Column("Town").Not.Nullable();
            References(x => x.Street).Nullable();

            References(x => x.Facility).Nullable();
            References(x => x.LateralValve).Nullable();
            References(x => x.FireDistrict).Nullable();
            References(x => x.HydrantTagStatus).Nullable();
            References(x => x.HydrantManufacturer).Column("ManufacturerId").Nullable();
            References(x => x.HydrantModel).Nullable();
            References(x => x.Initiator).Nullable();
            References(x => x.Status).Column("AssetStatusId").Not.Nullable();
            References(x => x.LateralSize).Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.OpenDirection).Column("OpensDirectionId").Nullable();
            References(x => x.Gradient).Nullable();
            References(x => x.HydrantSize).Nullable();
            References(x => x.InspectionFrequencyUnit).Nullable();
            References(x => x.PaintingFrequencyUnit).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.HydrantMainSize).Nullable();
            References(x => x.HydrantThreadType).Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.HydrantBilling)
               .Not.Nullable();
            References(x => x.Coordinate).Nullable().Cascade.All();
            References(x => x.FunctionalLocation).Nullable();
            References(x => x.WaterSystem).Nullable();
            References(x => x.MainType).Nullable();
            References(x => x.UpdatedBy).Nullable();
            References(x => x.HydrantType).Nullable();
            References(x => x.HydrantOutletConfiguration).Nullable();

            Map(x => x.IsNonBPUKPI).Not.Nullable();
            Map(x => x.Critical).Not.Nullable();
            Map(x => x.CriticalNotes).Nullable().Length(Hydrant.StringLengths.CRITICAL_NOTES);
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.DateRetired).Nullable();
            Map(x => x.DateTested).Nullable();
            Map(x => x.IsDeadEndMain).Not.Nullable();
            Map(x => x.Elevation).Nullable();
            Map(x => x.HydrantNumber).Not.Nullable().Length(Hydrant.StringLengths.HYDRANT_NUMBER);
            Map(x => x.HydrantSuffix).Not.Nullable();
            Map(x => x.LegacyId).Length(Hydrant.StringLengths.LEGACY_ID).Nullable();
            Map(x => x.InspectionFrequency).Nullable();
            Map(x => x.PaintingFrequency).Nullable();
            Map(x => x.Location).Nullable().Length(Hydrant.StringLengths.LOCATION);
            Map(x => x.MapPage).Nullable().Length(Hydrant.StringLengths.MAP_PAGE);
            Map(x => x.OutOfService)
               .Formula(
                    @"(CASE WHEN EXISTS (select null from HydrantsOutOfService hoos where hoos.HydrantId = Id and hoos.BackInServiceDate is null) THEN 1 ELSE 0 END)");
            Map(x => x.InspectionsPerYear)
               .Formula($@"(CASE 
                    WHEN InspectionFrequencyUnitId = {RecurringFrequencyUnit.Indices.YEAR}
                        THEN 1*InspectionFrequency
                    WHEN InspectionFrequencyUnitId = {RecurringFrequencyUnit.Indices.MONTH}
                        THEN 12*InspectionFrequency
                    WHEN InspectionFrequencyUnitId = {RecurringFrequencyUnit.Indices.WEEK}
                        THEN 52*InspectionFrequency
                    WHEN InspectionFrequencyUnitId = {RecurringFrequencyUnit.Indices.DAY}
                        THEN 365*InspectionFrequency
                    ELSE
                        1
                    END)");

            Map(x => x.HasWorkOrder)
               .DbSpecificFormula(
                    "(CASE WHEN (SELECT Count(1) FROM WorkOrders WO where WO.HydrantID = Id) > 0 THEN 1 ELSE 0 END)")
               .LazyLoad();
            Map(x => x.HasOpenWorkOrder)
               .DbSpecificFormula(
                    "(CASE WHEN (SELECT Count(1) FROM WorkOrders WO where WO.HydrantID = Id AND WO.DateCompleted IS NULL AND WO.CancelledAt IS NULL) > 0 THEN 1 ELSE 0 END)")
               .LazyLoad();
            References(x => x.MostRecentOpenWorkOrderWorkDescription).Nullable()
                                                                     .DbSpecificFormula(
                                                                          @"(select top 1 wo.WorkDescriptionID from WorkOrders wo where wo.HydrantId = Id and wo.DateCompleted is null and wo.CancelledAt is null order by wo.WorkOrderID desc)",
                                                                          @"(select wo.WorkDescriptionID from WorkOrders wo where wo.HydrantId = Id and wo.DateCompleted is null and wo.CancelledAt is null order by wo.WorkOrderID desc limit 1)");

            Map(x => x.Route).Nullable();
            Map(x => x.Stop).Nullable();
            Map(x => x.StreetNumber).Nullable().Length(Hydrant.StringLengths.STREET_NUMBER);
            Map(x => x.ValveLocation).Nullable().Length(Hydrant.StringLengths.VALVE_LOCATION);
            Map(x => x.WorkOrderNumber).Nullable().Length(Hydrant.StringLengths.WORKORDER);
            Map(x => x.YearManufactured).Nullable();
            Map(x => x.ClowTagged).Not.Nullable();
            Map(x => x.ObjectID).Nullable();
            //Map(x => x.PremiseNumber).Nullable().Length(Hydrant.StringLengths.PREMISE_NUMBER);
            Map(x => x.BillingDate).Nullable();
            Map(x => x.BranchLengthFeet).Nullable();
            Map(x => x.BranchLengthInches).Nullable();
            Map(x => x.DepthBuryFeet).Nullable();
            Map(x => x.DepthBuryInches).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.SAPEquipmentId).Nullable();
            Map(x => x.LastNonInspectionDate).Nullable()
                                             .Formula(
                                                  "(select max(hi.DateInspected) from HydrantInspections hi where hi.HydrantId = Id and hi.WorkOrderRequest1 is not null)");

            Map(x => x.GISUID)
               .Length(Hydrant.StringLengths.GISUID)
               .Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            // Hanging out with us for a while.
            Map(x => x.FLRouteNumber);
            Map(x => x.FLRouteSequence);
            Map(x => x.Zone).Nullable();
            Map(x => x.PaintingZone).Nullable();

            HasOne(x => x.HydrantDuePainting);
            HasOne(x => x.HydrantDueInspection);

            HasMany(x => x.WorkOrders).KeyColumn("HydrantID").Where("CancelledAt IS NULL");
            HasMany(x => x.HydrantInspections).KeyColumn("HydrantId").Cascade.AllDeleteOrphan();
            HasMany(x => x.Paintings).KeyColumn("HydrantId").Cascade.AllDeleteOrphan();
            HasMany(x => x.HydrantNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.HydrantDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.OutOfServiceRecords).KeyColumn("HydrantId").Cascade.All();
        }

        #endregion
    }
}
