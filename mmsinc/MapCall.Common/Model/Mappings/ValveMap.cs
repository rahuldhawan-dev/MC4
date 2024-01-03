using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveMap : ClassMap<Valve>
    {
        /*
           | Small Zone | Large Zone | Year  |
           |          1 |          5 | 2011  |
           |          2 |          6 | 2012  |
           |          3 |          5 | 2013  |
           |          4 |          6 | 2014  | Repeat
           |		  7 |		     | Annual|
        */

        #region Constants

        public struct Sql
        {
            /// <summary>
            /// Valves are currently due inspection based on their zone. Valve Zone 7 is due once a year. 
            /// Valve Zones 5 and 6 are for large valves, they are due every 2 years. 
            /// If their valve zone is due for the year and they do not have an inspection for said year, they are due
            /// This will tell you which large valve zone is due for the current year 
            ///     SQL: (ABS(2011-YEAR(GETDATE()))%2)+5
            ///     C#:  (((Math.Abs(2011) - DateTime.Now.Year)%2) + 5)
            /// Valves Zones 1,2,3,4 are for small valves, they are due every 4 years.
            /// If their valve zone is due for the year and they do not have an inspection for said year, they are due
            /// This will tell you which small valve zone is due for the current year 
            ///     SQL: ((ABS(2011-YEAR(GETDATE()))%4)+1))
            ///     C#:  (((Math.Abs(2011) - DateTime.Now.Year)%4) + 1)
            /// 2011 is the seed year, that's when this change took place. 
            /// See ValveMapTest for some examples
            /// </summary>
            public static readonly string
                // This SQL is reused in reports. It has been split up to allow for that
                // The already inspected case is a separate constant here because 
                // it is not needed in other places this is used.
                // {0} -- year were are checking if the inspection is required
                // {1} -- additional case 
                //          - e.g. when not already inspected or 
                //          -      when not installed for year
                //
                // for an explanation of the requires inspection method check the ValveMapTests comments
                INSPECTABLE_PARTIAL_FORMAT_STRING =
                    // NOT ACTIVE
                    $@"
    					WHEN (AssetStatusId not in({string.Join(",", AssetStatus.ACTIVE_STATUSES)}))
                        THEN 0" +
                    // NOT PUBLIC
                    @"
					WHEN ((ValveBillingId <> 3 and ValveBillingId is not null) AND (InspectionFrequency is null or InspectionFrequencyUnitId is null))
                        THEN 0" +
                    // IT IS A BLOW OFF WITH FLUSHING
                    @"
                    WHEN ValveControlsId = 3
                        THEN 0" +
                    // BPU VALVE
                    @"
                    WHEN BPUKPI = 1 
                        THEN 0" +
                    // VALVE IS LESS THAN 2 inches AND IS A CONTROLS A BLOW OFF
                    @"
                    WHEN ({0} < 2.0
                            AND
                            ValveControlsId = 2
                          )  
                        THEN 0",
                REQUIRES_INSPECTION_FORMAT_STRING = @"(CASE
                    " + INSPECTABLE_PARTIAL_FORMAT_STRING +
                                                    // ZONE IS NOT REQUIRED FOR INSPECTION AND NO INSPECTION FREQUENCY IS SET
                                                    @"
                    WHEN ({3} = 0 AND ValveZoneId in (1,2,3,4) AND ValveZoneId <> ((ABS(2011-{1})%4)+1))
                        THEN 0" +
                                                    // ZONE IS NOT REQUIRED FOR INSPECTION AND NO INSPECTION FREQUENCY IS SET
                                                    @"
                    WHEN ({3} = 0 AND ValveZoneId in (5,6) AND ValveZoneId <> ((ABS(2011-{1})%2)+5))
                        THEN 0" +
                                                    // IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS
                                                    @"
                    WHEN ({3} = 1 AND InspectionFrequency is not null AND NullIf(InspectionFrequency,0) is not null AND InspectionFrequencyUnitId = 4 AND (ValveZoneId % nullif(InspectionFrequency, 0)) <> ((ABS(2011-{1})%nullif(InspectionFrequency, 0))))
                        THEN 0
                    {2}
                    ELSE 1 END)",
                REQUIRES_INSPECTION_FORMAT_STRING_SQLITE = @"(CASE
                    " + INSPECTABLE_PARTIAL_FORMAT_STRING +
                                                           // ZONE IS NOT REQUIRED FOR INSPECTION AND NO INSPECTION FREQUENCY IS SET
                                                           @"
                    WHEN ({3} = 0 AND ValveZoneId in (1,2,3,4) AND ValveZoneId <> ((ABS(2011-{1})%4)+1))
                        THEN 0" +
                                                           // ZONE IS NOT REQUIRED FOR INSPECTION AND NO INSPECTION FREQUENCY IS SET
                                                           @"
                    WHEN ({3} = 0 AND ValveZoneId in (5,6) AND ValveZoneId <> ((ABS(2011-{1})%2)+5))
                        THEN 0" +
                                                           // IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS
                                                           @"
                    WHEN ({3} = 1 AND InspectionFrequency is not null AND NullIf(InspectionFrequency,0) is not null AND InspectionFrequencyUnitId = 4 AND (ValveZoneId % nullif(InspectionFrequency, 0)) <> ((ABS(2011-{1})%nullif(InspectionFrequency, 0))))
                        THEN 0
                    {2}
                    ELSE 1 END)",
                REQUIRES_INSPECTION_INSPECTED =
                    // ALREADY INSPECTED FOR THE YEAR
                    @"
                    WHEN (EXISTS (SELECT 1 FROM ValveInspections vi WHERE vi.ValveId = Id and YEAR(vi.DateInspected) = YEAR(getDate()) AND vi.Inspected = 1))
                        THEN 0",
                INSPECTABLE_FORMAT_STRING = @"(CASE " + INSPECTABLE_PARTIAL_FORMAT_STRING + " ELSE 1 END) = 1";

            public static readonly string REQUIRES_BLOWOFF_INSPECTION = $@"
                    (CASE
                        WHEN (ValveControlsId is null OR 3 <> ValveControlsId)
                            THEN 0
                        WHEN AssetStatusId not in ({string.Join(",", AssetStatus.ACTIVE_STATUSES)})
                            THEN 0  
                        WHEN 'PUBLIC' <> (select UPPER(vb.Description) from ValveBillings vb where vb.Id = ValveBillingId)
                            THEN 0  
                        WHEN NOT EXISTS (select v.DateInspected from BlowOffInspections v where v.ValveId = Id)
                            THEN 1
						WHEN (DateDiff(""YYYY"", (select top 1 b.DateInspected from BlowOffInspections b where b.ValveId = Id order by b.DateInspected desc), getdate()) < 1) 
						    THEN 0
                        ELSE 1 
					END)
                    ",
                                          REQUIRES_BLOWOFF_INSPECTION_SQLITE = $@"
                    (CASE
                        WHEN (ValveControlsId is null OR 'BLOW OFF WITH FLUSHING' <> (select UPPER(vc.Description) from ValveControls vc where vc.Id = ValveControlsId))
                            THEN 0
                        WHEN AssetStatusId not in ({string.Join(",", AssetStatus.ACTIVE_STATUSES)})
                            THEN 0  
                        WHEN 'PUBLIC' <> (select UPPER(vb.Description) from ValveBillings vb where vb.Id = ValveBillingId)
                            THEN 0  
                        WHEN NOT EXISTS (select v.DateInspected from BlowOffInspections v where v.ValveId = Id)
                            THEN 1
						WHEN (strftime('%Y', 'now') - strftime('%Y', (select b.DateInspected from BlowOffInspections b where b.ValveId = Id order by b.DateInspected desc limit 1)) < 1) 
						    THEN 0
                        ELSE 1 
					END)
                    ";

            public const string LAST_INSPECTION =
                                    @"(SELECT max(vi.DateInspected) from ValveInspections vi where vi.ValveId = Id and vi.Inspected = 1 and (vi.MinimumRequiredTurns is null OR isNull(vi.Turns, 0) >= vi.MinimumRequiredTurns))",
                                LAST_NON_INSPECTION =
                                    @"(SELECT max(vi.DateInspected) from ValveInspections vi where vi.ValveId = Id and (vi.Inspected = 0 OR (vi.MinimumRequiredTurns is not null AND vi.Turns < vi.MinimumRequiredTurns)))",
                                LAST_BLOWOFF_INSPECTION =
                                    @"(SELECT max(boi.DateInspected) from BlowOffInspections boi where boi.ValveId = Id and boi.WorkOrderRequest1 is null and boi.WorkOrderRequest2 is null and boi.WorkOrderRequest3 is null)",
                                LAST_BLOWOFF_NON_INSPECTION =
                                    @"(SELECT max(boi.DateInspected) from BlowOffInspections boi where boi.ValveId = Id and boi.WorkOrderRequest1 is not null)",
                                IS_LARGE_VALVE = @"(
                    CASE WHEN ((SELECT vs.Size from ValveSizes vs WHERE vs.Id = ValveSizeId) >= 12) THEN 1
                    ELSE 0 END)",
                                IN_NORMAL_POSITION =
                                    "(CASE WHEN (select top 1 i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id) order by i.DateInspected Desc) = NormalPositionId THEN 1 WHEN NormalPositionId IS NULL OR (select TOP 1 i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id)) IS NULL THEN NULL ELSE 0 END)",
                                IN_NORMAL_POSITION_SQLITE =
                                    "(CASE WHEN (select i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id) order by i.DateInspected Desc limit 1) = NormalPositionId THEN 1 WHEN NormalPositionId IS NULL OR (select i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id) LIMIT 1) IS NULL THEN NULL ELSE 0 END)",
                                POSITION_LEFT =
                                    "(select top 1 i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id) order by i.DateInspected Desc)",
                                POSITION_LEFT_SQLITE =
                                    "(select i.PositionLeftId from ValveInspections i where i.Inspected = 1 and i.ValveId = Id and i.DateInspected = (select max(m.DateInspected) from ValveInspections m where m.Inspected = 1 and m.ValveId = i.ValveId and m.ValveId = Id) order by i.DateInspected Desc LIMIT 1)";
        }

        #endregion

        #region Constructors

        public ValveMap()
        {
            Id(x => x.Id);

            DynamicUpdate();

            References(x => x.OperatingCenter).Not.Nullable();
            //TODO: TownId?
            References(x => x.Town).Column("Town").Not.Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.ValveBilling).Not.Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.InspectionFrequencyUnit).Column("InspectionFrequencyUnitId").Nullable();
            References(x => x.NormalPosition).Nullable();
            References(x => x.OpenDirection).Column("OpensId").Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.MainType).Nullable();
            References(x => x.ValveControls).Nullable();
            References(x => x.ValveMake).Nullable();
            References(x => x.ValveType).Nullable();
            References(x => x.ValveSize).Nullable();
            References(x => x.Gradient).Nullable();
            References(x => x.Status).Column("AssetStatusId").Not.Nullable();
            References(x => x.Initiator).Nullable();
            References(x => x.Coordinate).Nullable().Cascade.All();
            References(x => x.ValveZone).Nullable();
            References(x => x.WaterSystem).Nullable();
            References(x => x.FunctionalLocation).Nullable();
            References(x => x.UpdatedBy).Nullable();

            Map(x => x.DepthFeet).Nullable();
            Map(x => x.DepthInches).Nullable();
            Map(x => x.BPUKPI).Not.Nullable();
            Map(x => x.ControlsCrossing).Not.Nullable();
            Map(x => x.Critical).Not.Nullable();
            Map(x => x.CriticalNotes).Nullable().Length(Valve.StringLengths.CRITICAL_NOTES);
            Map(x => x.DateRetired).Nullable();
            Map(x => x.DateTested).Nullable();
            Map(x => x.Elevation).Nullable();
            Map(x => x.InspectionFrequency).Nullable();
            Map(x => x.LegacyId).Length(Valve.StringLengths.LEGACY_ID).Nullable();
            Map(x => x.MapPage).Nullable().Length(Valve.StringLengths.MAP_PAGE);
            Map(x => x.ObjectID).Nullable();
            Map(x => x.Route).Nullable();
            Map(x => x.Stop).Nullable();
            Map(x => x.SketchNumber).Nullable().Length(Valve.StringLengths.SKETCH_NUMBER);
            Map(x => x.StreetNumber).Nullable().Length(Valve.StringLengths.STREET_NUMBER);
            Map(x => x.Traffic).Not.Nullable();
            Map(x => x.Turns).Nullable();
            Map(x => x.ValveLocation).Nullable().Length(Valve.StringLengths.VALVE_LOCATION);
            Map(x => x.ValveNumber).Nullable().Length(Valve.StringLengths.VALVE_NUMBER);
            Map(x => x.ValveSuffix).Not.Nullable();
            Map(x => x.WorkOrderNumber).Nullable().Length(Valve.StringLengths.WORK_ORDER_NUMBER);
            Map(x => x.CreatedAt).Not.Nullable();
            // Reference?
            //Map(x => x.ImageActionID);
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.SAPEquipmentId).Nullable();
            Map(x => x.CanHaveBlowOffInspections).Not.Nullable()
                                                 .Formula("(CASE WHEN (ValveControlsId = " +
                                                          ValveControl.Indices.BLOW_OFF_WITH_FLUSHING +
                                                          ") THEN 1 ELSE 0 END)");
            Map(x => x.GISUID)
               .Length(Valve.StringLengths.GISUID)
               .Nullable();
            Map(x => x.SAPErrorCode).Nullable();

            Map(x => x.HasImages).Not.Nullable()
                                 .Formula(
                                      "(CASE WHEN EXISTS (select 1 from ValveImages where ValveImages.ValveId = Id) THEN 1 ELSE 0 END)");

            HasMany(x => x.WorkOrders);
            HasMany(x => x.ValveImages);
            HasMany(x => x.ValveInspections);
            HasMany(x => x.BlowOffInspections).Cascade.AllDeleteOrphan();
            HasMany(x => x.LateralHydrants).KeyColumn("LateralValveId");
            HasManyToMany(x => x.MainCrossings)
               .Table("MainCrossingsValves")
               .ParentKeyColumn("ValveId")
               .ChildKeyColumn("MainCrossingId")
               .Cascade
               .None(); // This needs to be None unless you want valves to be deleted when a main crossing gets deleted.

            HasMany(x => x.ValveNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ValveDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();

            Map(x => x.RequiresInspection)
               .Formula("(SELECT due.RequiresInspection FROM ValvesDueInspection due WHERE due.Id = Id)");
            Map(x => x.RequiresBlowOffInspection).DbSpecificFormula(Sql.REQUIRES_BLOWOFF_INSPECTION,
                Sql.REQUIRES_BLOWOFF_INSPECTION_SQLITE);
            Map(x => x.LastInspectionDate).DbSpecificFormula(Sql.LAST_INSPECTION, Sql.LAST_INSPECTION.ToSqlite());
            Map(x => x.LastNonInspectionDate)
               .DbSpecificFormula(Sql.LAST_NON_INSPECTION, Sql.LAST_NON_INSPECTION.ToSqlite());
            Map(x => x.LastBlowOffInspectionDate)
               .DbSpecificFormula(Sql.LAST_BLOWOFF_INSPECTION, Sql.LAST_BLOWOFF_INSPECTION.ToSqlite());
            Map(x => x.LastBlowOffNonInspectionDate).DbSpecificFormula(Sql.LAST_BLOWOFF_NON_INSPECTION,
                Sql.LAST_BLOWOFF_NON_INSPECTION.ToSqlite());
            Map(x => x.IsLargeValve).DbSpecificFormula(Sql.IS_LARGE_VALVE, Sql.IS_LARGE_VALVE.ToSqlite());
            Map(x => x.InNormalPosition).DbSpecificFormula(Sql.IN_NORMAL_POSITION, Sql.IN_NORMAL_POSITION_SQLITE);
            References(x => x.PositionLeft).DbSpecificFormula(Sql.POSITION_LEFT, Sql.POSITION_LEFT_SQLITE);
            Map(x => x.HasOpenWorkOrder)
               .DbSpecificFormula(
                    "(CASE WHEN (SELECT Count(1) FROM WorkOrders WO where WO.ValveID = Id AND WO.DateCompleted IS NULL AND WO.CancelledAt IS NULL) > 0 THEN 1 ELSE 0 END)")
               .LazyLoad();

            References(x => x.MostRecentOpenWorkOrderWorkDescription).Nullable()
                                                                     .DbSpecificFormula(
                                                                          @"(select top 1 wo.WorkDescriptionID from WorkOrders wo where wo.ValveID = Id and wo.DateCompleted is null and wo.CancelledAt is null order by wo.WorkOrderID desc)",
                                                                          @"(select wo.WorkDescriptionID from WorkOrders wo where wo.ValveID = Id and wo.DateCompleted is null and wo.CancelledAt is null order by wo.WorkOrderID desc limit 1)");
        }

        #endregion
    }
}
