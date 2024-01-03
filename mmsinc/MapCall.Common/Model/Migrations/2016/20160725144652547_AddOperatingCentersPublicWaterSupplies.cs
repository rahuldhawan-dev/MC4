using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160725144652547), Tags("Production")]
    public class AddOperatingCentersPublicWaterSupplies : Migration
    {
        public override void Up()
        {
            Create.Table("OperatingCentersPublicWaterSupplies")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID")
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies");
            Execute.Sql(
                "INSERT INTO OperatingCentersPublicWaterSupplies SELECT OperatingCenterId, Id from PublicWaterSupplies WHERE OperatingCenterId is not null");
            Alter.Table("WaterQualityComplaints")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID");
            Execute.Sql(
                "UPDATE WaterQualityComplaints SET operatingCenterID = (SELECT pws.operatingCenterId FROM PublicWaterSupplies pws where pws.Id = WaterQualityComplaints.PWSID)" +
                "UPDATE WaterQualityComplaints SET operatingCenterID = (SELECT top 1 oct.operatingCenterId FROM OperatingCentersTowns oct where oct.TownId = WaterQualityComplaints.TownId) where WaterQualityComplaints.OperatingCenterId is null and WaterQualityComplaints.TOwnId Is not null");
            Delete.ForeignKeyColumn("PublicWaterSupplies", "OperatingCenterId", "OperatingCenters",
                "OperatingCenterID");
            Alter.Table("BacterialWaterSamples")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID");
            Execute.Sql(@"
                            update 
                                BacterialWaterSamples
                            set
                                OperatingCenterId = (select ss.OperatingCenterId from tblWQSample_Sites ss where ss.SampleSiteID = BacterialWaterSamples.SampleSiteID)
                            where
                                BacterialSampleTypeId = 1
                            update
                                BacterialWaterSamples
                            set
                                OperatingCenterId = (select top 1 oct.OperatingCenterId from OperatingCentersTowns oct where oct.TownID = BacterialWaterSamples.TownID) 
                            where
                                BacterialSampleTypeId in (2,5);
                            --update 
	                        --    BacterialWaterSamples 
                            --set
	                        --    OperatingCenterId = (select oc.OperatingCenterID from BacterialWaterSamples OS join tblWQSample_Sites SS on SS.SampleSiteID = os.SampleSiteID join PublicWaterSupplies PP on PP.Id = SS.PWSID join OperatingCenters oc on oc.OperatingCenterId = PP.OperatingCenterId WHERE OS.Id = BacterialWaterSamples.OriginalBacterialWaterSampleId)
                            --where 
	                        --    BacterialWaterSamples.BacterialSampleTypeId = 4 AND BacterialWaterSamples.OriginalBacterialWaterSampleId is not null;
                            update 
	                            BacterialWaterSamples 
                            set
	                            OperatingCenterId = (select OperatingCenterId from tblWQSample_Sites ss where ss.SampleSiteID = BacterialWaterSamples.SampleSiteID)
                            where 
	                            BacterialSampleTypeId = 4 AND OriginalBacterialWaterSampleId is null;
                            ");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WaterQualityComplaints", "OperatingCenterId", "OperatingCenters",
                "OperatingCenterID");
            Delete.ForeignKeyColumn("BacterialWaterSamples", "OperatingCenterId", "OperatingCenters",
                "OperatingCenterID");
            Alter.Table("PublicWaterSupplies")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID");
            Execute.Sql("UPDATE PublicWaterSupplies SET OperatingCenterID = " +
                        "(SELECT TOP 1 OperatingCenterID from OperatingCentersPublicWaterSupplies WHERE PublicWaterSupplyId = PublicWaterSupplies.Id)");
            Delete.Table("OperatingCentersPublicWaterSupplies");
        }
    }
}
