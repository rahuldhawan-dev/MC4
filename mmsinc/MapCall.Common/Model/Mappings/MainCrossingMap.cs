using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainCrossingMap : ClassMap<MainCrossing>
    {
        #region Constructors

        public MainCrossingMap()
        {
            Id(x => x.Id, "MainCrossingID");

            Map(x => x.IsCompanyOwned);
            Map(x => x.LengthOfSegment);
            Map(x => x.IsCriticalAsset);
            Map(x => x.MaximumDailyFlow);
            Map(x => x.Comments);
            Map(x => x.InspectionFrequency)
               .Not.Nullable();
            Map(x => x.RailwayCrossingId).Length(MainCrossing.StringLengths.RAILWAY_CROSSING_ID).Nullable();
            Map(x => x.EmergencyPhoneNumber).Length(MainCrossing.StringLengths.EMERGENCY_PHONE_NUMBER).Nullable();
            Map(x => x.IsolationValves).Nullable();
            Map(x => x.DateRetired).Nullable();

            References(x => x.AssetCategory).Not.Nullable();
            References(x => x.RailwayOwnerType).Nullable();
            References(x => x.OperatingCenter)
               .Not.Nullable();
            References(x => x.Town)
               .Nullable();
            References(x => x.ClosestCrossStreet, "ClosestStreetID")
               .Nullable();
            References(x => x.Street).Nullable();
            References(x => x.Coordinate);
            References(x => x.BodyOfWater);
            References(x => x.CrossingCategory)
               .Not.Nullable();
            References(x => x.PressureZone);
            References(x => x.CustomerRange);
            References(x => x.PWSID, "PWSID");
            References(x => x.MainMaterial);
            References(x => x.MainDiameter);
            References(x => x.SupportStructure);
            References(x => x.CrossingType);
            References(x => x.ConstructionType);
            References(x => x.InspectionFrequencyUnit).Not.Nullable();
            References(x => x.MainCrossingStatus).Not.Nullable();
            References(x => x.InspectionType).Nullable();
            References(x => x.MainInCasing).Nullable();
            References(x => x.ConsequenceOfFailure, "MainCrossingConsequenceOfFailureTypeId").Not.Nullable();
            References(x => x.TypicalOperatingPressureRange, "TypicalOperatingPressureRangeId")
               .Nullable();
            References(x => x.PressureSurgePotentialType, "PressureSurgePotentialTypeId")
               .Nullable();

            HasMany(x => x.MainCrossingDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.MainCrossingNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.Inspections)
               .KeyColumn("MainCrossingId")
               .Inverse()
               .Cascade.AllDeleteOrphan();
            HasMany(x => x.WorkOrders).KeyColumn("MainCrossingId");
            HasManyToMany(x => x.Valves)
               .Table("MainCrossingsValves")
               .ParentKeyColumn("MainCrossingId")
               .ChildKeyColumn("ValveId")
               .Cascade
               .None(); // This needs to be None unless you want maincrossings to be deleted when a valve gets deleted.
            HasManyToMany(x => x.ImpactTo)
               .Table("MainCrossingsImpactToTypes")
               .ParentKeyColumn("MainCrossingId")
               .ChildKeyColumn("MainCrossingImpactToTypeId")
               .Cascade.None();

            /*
             * So what's this formula supposed to do?
             * An inspection frequency of a year means that an inspection just has to happen anytime during a year, not within
             * the timespan of a year. So having an inspection on 12/31/2013 and an inspection on 1/1/2014 is perfectly valid
             * for saying that an inspection occurred for 2013 and 2014 and that another one doesn't need to occur until 12/31/2015.
             * 
             * Same thing goes for month/week/day
             * 
             * If the MainCrossing has no inspections at all, then it's assumed that one is required.
             * 
             * The Sqlite version is basically a load and is of no use to production code. God forbid sqlite included
             * some actual date arithmetic functions and not just string formatting.
             */
            Map(x => x.RequiresInspection)
                // Sql Server
               .DbSpecificFormula(@"(CASE 
                        WHEN MainCrossingStatusId <> 1 
                            THEN 0  
                        WHEN NOT EXISTS (select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId)
                            THEN 1
						WHEN 'Year' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (DateDiff(""YYYY"", (select top 1 mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc), getdate()) >= InspectionFrequency) 
									THEN 1
									ELSE 0
								END
						WHEN 'Month' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (DateDiff(""mm"", (select top 1 mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc), getdate()) >= InspectionFrequency) 
									THEN 1
									ELSE 0
								END
						WHEN 'Week' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (DateDiff(""WW"", (select top 1 mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc), getdate()) >= InspectionFrequency) 
									THEN 1
									ELSE 0
								END
						WHEN 'Day' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (DateDiff(""D"", (select top 1 mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc), getdate()) >= InspectionFrequency) 
									THEN 1
									ELSE 0
								END
                        ELSE 1 
						END)",
                    // SQLite
                    @"(CASE 
                        WHEN MainCrossingStatusId <> 1 
                            THEN 0  
                        WHEN NOT EXISTS (select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId)
                            THEN 1
						WHEN 'Year' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (strftime('%Y', 'now') - strftime('%Y', (select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)) >= InspectionFrequency) 
									THEN 1
									ELSE 0
								END
						WHEN 'Month' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (((strftime('%Y', 'now') * 12 + strftime('%m', 'now')) - ((strftime('%Y', (select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)) * 12) + strftime('%m', (select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)))) >= (InspectionFrequency)) 
									THEN 1
									ELSE 0
								END
						WHEN 'Week' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (julianday('now','localtime') - julianday((select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)) >= (InspectionFrequency * 7)) 
									THEN 1
									ELSE 0
								END
						WHEN 'Day' = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = InspectionFrequencyUnitId) 
							THEN 
								CASE
									WHEN (julianday('now','localtime') - julianday((select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)) >= (InspectionFrequency)) 
									THEN 1
									ELSE 0
								END
                        ELSE 0
						END)");

            // Annoyingly, this formula can't be referenced in the messy formula above.
            Map(x => x.LastInspectedOn)
               .DbSpecificFormula(
                    "(select top 1 mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc)",
                    "(select mci.InspectedOn from MainCrossingInspections mci where mci.MainCrossingId = MainCrossingId order by mci.InspectedOn desc limit 1)");
            Map(x => x.HasWorkOrder)
               .DbSpecificFormula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM WorkOrders wo WHERE wo.MainCrossingId = MainCrossingId) THEN 1 ELSE 0 END)");
        }

        #endregion
    }
}
