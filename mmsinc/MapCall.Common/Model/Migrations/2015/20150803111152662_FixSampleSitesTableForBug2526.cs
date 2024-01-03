using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150803111152662), Tags("Production")]
    public class FixSampleSitesTableForBug2526 : Migration
    {
        /* IN CASE THIS IS NEEDED AGAIN:
        select
            s.SampleSiteID
            ,s.Town as [Site Town]
            ,lt.Town as [Linked Town]
            ,nt.Town as [New Town]
        from
            tblWQSample_Sites s
        inner join
            Towns lt
        on
            lt.TownId = s.TownId
        inner join
            Towns nt
        on
            nt.State = s.State and nt.Town like s.Town + '%'
        where
            s.Town is not null
        and
            s.TownId <> nt.TownId
        and
            s.Town <> lt.Town
        and
            lt.Town <> nt.Town
        union all
        select
            s.SampleSiteID
            ,null as [Site Town]
            ,t.Town as [Linked Town]
            ,null as [New Town]
        from
            tblWQSample_Sites s
        inner join
            Towns t
        on
            t.TownId = s.TownId
        where
            s.Town is null
        union all
        select
            SampleSiteID
            ,null as [Site Town]
            ,null as [Linked Town]
            ,null as [New Town]
        from
            tblWQSample_Sites
        where
            town is null and townid is null
         */

        public override void Up()
        {
            Execute.Sql("update tblWQSample_Sites set Town = ltrim(rtrim(Town));");
            Execute.Sql(@"update
	tblWQSample_Sites
set
	TownId = t.TownId
from
	Towns t
where
	(tblWQSample_Sites.State = t.State or tblWQSample_Sites.State IS NULL or t.State IS NULL)
and
	tblWQSample_Sites.Town = t.Town");
            Execute.Sql(@"UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 69
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 70
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 71
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 72
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 73
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 74
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 75
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 78
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 79
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 80
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 81
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 95
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 96
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 97
UPDATE tblWQSample_Sites SET TownId = 260 WHERE SampleSiteId = 617
UPDATE tblWQSample_Sites SET TownId = 269 WHERE SampleSiteId = 618
UPDATE tblWQSample_Sites SET TownId = 109 WHERE SampleSiteId = 619
UPDATE tblWQSample_Sites SET TownId = 265 WHERE SampleSiteId = 620
UPDATE tblWQSample_Sites SET TownId = 248 WHERE SampleSiteId = 621
UPDATE tblWQSample_Sites SET TownId = 262 WHERE SampleSiteId = 622
UPDATE tblWQSample_Sites SET TownId = 230 WHERE SampleSiteId = 623
UPDATE tblWQSample_Sites SET TownId = 226 WHERE SampleSiteId = 624
UPDATE tblWQSample_Sites SET TownId = 243 WHERE SampleSiteId = 625
UPDATE tblWQSample_Sites SET TownId = 232 WHERE SampleSiteId = 626
UPDATE tblWQSample_Sites SET TownId = 210 WHERE SampleSiteId = 627
UPDATE tblWQSample_Sites SET TownId = 270 WHERE SampleSiteId = 628
UPDATE tblWQSample_Sites SET TownId = 241 WHERE SampleSiteId = 629
UPDATE tblWQSample_Sites SET TownId = 255 WHERE SampleSiteId = 630
UPDATE tblWQSample_Sites SET TownId = 259 WHERE SampleSiteId = 631
UPDATE tblWQSample_Sites SET TownId = 264 WHERE SampleSiteId = 633
UPDATE tblWQSample_Sites SET TownId = 255 WHERE SampleSiteId = 634
UPDATE tblWQSample_Sites SET TownId = 254 WHERE SampleSiteId = 635
UPDATE tblWQSample_Sites SET TownId = 261 WHERE SampleSiteId = 636
UPDATE tblWQSample_Sites SET TownId = 114 WHERE SampleSiteId = 637
UPDATE tblWQSample_Sites SET TownId = 270 WHERE SampleSiteId = 641
UPDATE tblWQSample_Sites SET TownId = 241 WHERE SampleSiteId = 646
UPDATE tblWQSample_Sites SET TownId = 245 WHERE SampleSiteId = 647
UPDATE tblWQSample_Sites SET TownId = 240 WHERE SampleSiteId = 648
UPDATE tblWQSample_Sites SET TownId = 270 WHERE SampleSiteId = 650
UPDATE tblWQSample_Sites SET TownId = 227 WHERE SampleSiteId = 651
UPDATE tblWQSample_Sites SET TownId = 266 WHERE SampleSiteId = 653
UPDATE tblWQSample_Sites SET TownId = 232 WHERE SampleSiteId = 654
UPDATE tblWQSample_Sites SET TownId = 235 WHERE SampleSiteId = 655
UPDATE tblWQSample_Sites SET TownId = 212 WHERE SampleSiteId = 657
UPDATE tblWQSample_Sites SET TownId = 244 WHERE SampleSiteId = 658
UPDATE tblWQSample_Sites SET TownId = 247 WHERE SampleSiteId = 660
UPDATE tblWQSample_Sites SET TownId = 239 WHERE SampleSiteId = 661
UPDATE tblWQSample_Sites SET TownId = 115 WHERE SampleSiteId = 662
UPDATE tblWQSample_Sites SET TownId = 235 WHERE SampleSiteId = 674
UPDATE tblWQSample_Sites SET TownId = 134 WHERE SampleSiteId = 739
UPDATE tblWQSample_Sites SET TownId = 134 WHERE SampleSiteId = 740
UPDATE tblWQSample_Sites SET TownId = 134 WHERE SampleSiteId = 741
UPDATE tblWQSample_Sites SET TownId = 134 WHERE SampleSiteId = 742
UPDATE tblWQSample_Sites SET TownId = 310 WHERE SampleSiteId = 932
UPDATE tblWQSample_Sites SET TownId = 199 WHERE SampleSiteId = 973
UPDATE tblWQSample_Sites SET TownId = 199 WHERE SampleSiteId = 974
UPDATE tblWQSample_Sites SET TownId = 200 WHERE SampleSiteId = 975
UPDATE tblWQSample_Sites SET TownId = 200 WHERE SampleSiteId = 976
UPDATE tblWQSample_Sites SET TownId = 202 WHERE SampleSiteId = 977
UPDATE tblWQSample_Sites SET TownId = 198 WHERE SampleSiteId = 978
UPDATE tblWQSample_Sites SET TownId = 198 WHERE SampleSiteId = 979
UPDATE tblWQSample_Sites SET TownId = 202 WHERE SampleSiteId = 980
UPDATE tblWQSample_Sites SET TownId = 205 WHERE SampleSiteId = 981
UPDATE tblWQSample_Sites SET TownId = 202 WHERE SampleSiteId = 982
UPDATE tblWQSample_Sites SET TownId = 202 WHERE SampleSiteId = 983
UPDATE tblWQSample_Sites SET TownId = 202 WHERE SampleSiteId = 984
UPDATE tblWQSample_Sites SET TownId = 205 WHERE SampleSiteId = 985
UPDATE tblWQSample_Sites SET TownId = 189 WHERE SampleSiteId = 1070
UPDATE tblWQSample_Sites SET TownId = 193 WHERE SampleSiteId = 1119");

            this.ExtractLookupTableLookup("tblWQSample_Sites", "Availability", "SampleSiteAvailability", 50,
                "Availability", deleteLookupValues: false, lookupIsTableSpecific: false);
            this.ExtractLookupTableLookup("tblWQSample_Sites", "Site_Status_Id", "SampleSiteStatuses", 50,
                "site_status_id", lookupIsTableSpecific: false);
            Rename.Column("Site_Status_Id").OnTable("tblWQSample_Sites").To("SiteStatusId");

            Execute.Sql(
                "UPDATE tblWQSample_Sites SET OpCode = oc.OperatingCenterId FROM OperatingCenters oc WHERE oc.OperatingCenterCode = tblWQSample_Sites.OpCode");
            Rename.Column("OpCode").OnTable("tblWQSample_sites").To("OperatingCenterId");
            Alter.Column("OperatingCenterId").OnTable("tblWQSample_Sites")
                 .AsForeignKey("OperatingCenterId", "OperatingCenters", "OperatingCenterId");

            Execute.Sql("UPDATE tblWQSample_Sites SET County = NULL where County = 'Passiac'");
            Execute.Sql("UPDATE tblWQSample_Sites SET County = REPLACE(County, ' County', '')");

            Execute.Sql(
                "UPDATE tblWQSample_Sites SET County = c.CountyId FROM Counties c WHERE c.Name = tblWQSample_Sites.County");
            Rename.Column("County").OnTable("tblWQSample_Sites").To("CountyId");
            Alter.Column("CountyId").OnTable("tblWQSample_Sites")
                 .AsForeignKey("CountyId", "Counties", "CountyId");

            Execute.Sql(
                "UPDATE tblWQSample_Sites SET State = c.StateId FROM States c WHERE c.Abbreviation = tblWQSample_Sites.State");
            Rename.Column("State").OnTable("tblWQSample_Sites").To("StateId");
            Alter.Column("StateId").OnTable("tblWQSample_Sites")
                 .AsForeignKey("StateId", "States", "StateId");

            Create.Column("OutOfServiceArea").OnTable("tblWQSample_Sites").AsBoolean().NotNullable()
                  .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("OutOfServiceArea").FromTable("tblWQSample_Sites");

            this.ReplaceLookupTableLookup("tblWQSample_Sites", "Availability", "SampleSiteAvailability", 50,
                "Availability", lookupIsTableSpecific: false);
            Rename.Column("SiteStatusId").OnTable("tblWQSample_Sites").To("Site_Status_Id");
            this.ReplaceLookupTableLookup("tblWQSample_Sites", "Site_Status_Id", "SampleSiteStatuses", 50,
                "site_status_id", lookupIsTableSpecific: false);

            Delete.ForeignKey("FK_tblWQSample_Sites_OperatingCenters_OperatingCenterId").OnTable("tblWQSample_Sites");
            Alter.Column("OperatingCenterId").OnTable("tblWQSample_Sites").AsString(4).Nullable();
            Rename.Column("OperatingCenterId").OnTable("tblWQSample_Sites").To("OpCode");
            Execute.Sql(
                "UPDATE tblWQSample_Sites SET OpCode = oc.OperatingCenterCode FROM OperatingCenters oc WHERE oc.OperatingCenterId = tblWQSample_Sites.OpCode");

            Delete.ForeignKey("FK_tblWQSample_Sites_Counties_CountyId").OnTable("tblWQSample_Sites");
            Alter.Column("CountyId").OnTable("tblWQSample_Sites").AsString(35).Nullable();
            Rename.Column("CountyId").OnTable("tblWQSample_Sites").To("County");
            Execute.Sql(
                "UPDATE tblWQSample_Sites SET County = c.Name FROM Counties c WHERE c.CountyId = tblWQSample_Sites.County");

            Delete.ForeignKey("FK_tblWQSample_Sites_States_StateId").OnTable("tblWQSample_Sites");
            Alter.Column("StateId").OnTable("tblWQSample_Sites").AsString(4).Nullable();
            Rename.Column("StateId").OnTable("tblWQSample_Sites").To("State");
            Execute.Sql(
                "UPDATE tblWQSample_Sites SET State = c.Abbreviation FROM States c WHERE c.StateId = tblWQSample_Sites.State");
        }
    }
}
