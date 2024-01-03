using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180212111720189), Tags("Production")]
    public class INC000000139160_RevertJobSiteCheckListChanges : Migration
    {
        public override void Up()
        {
            Delete.Column("AreOtherRestraintFieldsRequired").FromTable("JobSiteCheckLists");
            Delete.Column("AsBuiltInformationCheckedToReviewExistingRestraintMethod").FromTable("JobSiteCheckLists");
            Delete.Column("AllJointsWereAssembledUsingProperRestraintGlands").FromTable("JobSiteCheckLists");
            Delete.Column("IsLongHydrantInstallation").FromTable("JobSiteCheckLists");
            Delete.Column("ValveInstalledUsingAHydrantAnchorTee").FromTable("JobSiteCheckLists");
            Delete.Column("HydrantValveIsATappingValve").FromTable("JobSiteCheckLists");
            Delete.Column("EnoughPipeAndSoilToProvideRestraint").FromTable("JobSiteCheckLists");
            Delete.Column("ValveInstalledUsingRodsAndVisuallyConfirmed").FromTable("JobSiteCheckLists");
            Delete.Column("HasValveRestraintBeenVisuallyConfirmed").FromTable("JobSiteCheckLists");
            Delete.Column("IsValveRestraintSecure").FromTable("JobSiteCheckLists");
            Delete.Column("LongHydrantInstallationCalculation").FromTable("JobSiteCheckLists");
        }

        public override void Down()
        {
            Alter.Table("JobSiteCheckLists")
                 .AddColumn("AreOtherRestraintFieldsRequired").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("AsBuiltInformationCheckedToReviewExistingRestraintMethod").AsBoolean().Nullable()
                 .AddColumn("AllJointsWereAssembledUsingProperRestraintGlands").AsBoolean().Nullable()
                 .AddColumn("IsLongHydrantInstallation").AsBoolean().Nullable()
                 .AddColumn("ValveInstalledUsingAHydrantAnchorTee").AsBoolean().Nullable()
                 .AddColumn("HydrantValveIsATappingValve").AsBoolean().Nullable()
                 .AddColumn("EnoughPipeAndSoilToProvideRestraint").AsBoolean().Nullable()
                 .AddColumn("ValveInstalledUsingRodsAndVisuallyConfirmed").AsBoolean().Nullable()
                 .AddColumn("HasValveRestraintBeenVisuallyConfirmed").AsBoolean().Nullable()
                 .AddColumn("IsValveRestraintSecure").AsBoolean().Nullable()
                 .AddColumn("LongHydrantInstallationCalculation").AsInt32().Nullable();
        }
    }
}
