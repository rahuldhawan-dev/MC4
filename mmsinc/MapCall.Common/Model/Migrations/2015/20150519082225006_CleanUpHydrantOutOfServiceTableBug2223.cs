using System;
using FluentMigrator;
using System.Linq;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225006), Tags("Production")]
    public class CleanUpHydrantOutOfServiceTableBug2223 : Migration
    {
        #region Consts

        private static readonly DateTime BAD_1900_DATE_THAT_SHOULD_MEAN_NULL = new DateTime(1900, 1, 1);

        private const string NEW_TABLE_NAME = "HydrantsOutOfService",
                             NEW_BACK_IN_SERVICE_DATE = "BackInServiceDate",
                             NEW_OUT_OF_SERVICE_DATE = "OutOfServiceDate",
                             NEW_ID = "Id",
                             NEW_HYDRANT_ID = "HydrantId",
                             NEW_BACK_IN_SERVICE_BY_USER_ID = "BackInServiceByUserId",
                             NEW_OUT_OF_SERVICE_BY_USER_ID = "OutOfServiceByUserId";

        private const string OLD_TABLE_NAME = "tblNJAWHydOS",
                             OLD_BACK_IN_SERVICE_DATE = "DateCBS",
                             OLD_OUT_OF_SERVICE_DATE = "DateCOS",
                             OLD_DATE_CREATED = "DateCreated",
                             OLD_ID = "RecId",
                             OLD_OPERATING_CENTER = "OpCntr",
                             OLD_HYDNUM = "HydNum",
                             OLD_CROSS_STREET = "CrossStreet",
                             OLD_FULLSTNAME = "FullStName",
                             OLD_TOWN = "Town",
                             OLD_TOWN_ID = "TownId",
                             OLD_BIBY = "BIBy",
                             OLD_COBY = "COBy",
                             OLD_REMARKS = "Remarks",
                             OLD_FDCONTACT = "FDContact",
                             OLD_FDFAX = "FDFax",
                             OLD_FDPHONE = "FDPhone";

        #endregion

        #region Private methods

        private void CleanInvalidHydrantEntries()
        {
            // These are rows that can't match a hydrant because the hydrant's OPC switched from EW3 to NJ5
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMTH-125"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HESA-12"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HLUM-44"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMAF-108"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMTH-47"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HWEA-48"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HESA-38"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HLUM-42"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMAF-90"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMTH-185"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HHAS-61"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMAF-176"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HLUM-244"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ5"}).Where(new {HydNum = "HMAF-42"});

            // These are rows that can't match a hydrant because the hydrant's OPC switched from EW2 to NJ6
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HBED-26"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HBED-19"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HPEA-78"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HPEA-14"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HPEA-115"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HPEA-49"});
            Update.Table(NEW_TABLE_NAME).Set(new {OpCntr = "NJ6"}).Where(new {HydNum = "HPEA-76"});

            // These HIR hydrants were renamed to HIRV at some point in time.
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HIRV-122"}).Where(new {HydNum = "HIR-122"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HIRV-690"}).Where(new {HydNum = "HIR-690"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HIRV-235"}).Where(new {HydNum = "HIR-235"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HIRV-304"}).Where(new {HydNum = "HIR-304"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HIRV-104"}).Where(new {HydNum = "HIR-104"});

            // Bad hyd nums in general
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HLT-167"}).Where(new {HydNum = "HLT-167-R"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HLT-168"}).Where(new {HydNum = "HLT-168-R"});
            Update.Table(NEW_TABLE_NAME).Set(new {HydNum = "HHF-1"}).Where(new {HydNum = "HHF-"});

            // People whose names are wrong
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Tom Sesztak"}).Where(new {COBy = "Tom Sestak"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Tom Sesztak"}).Where(new {BIBy = "Tom Sestak"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Kevin J Maloney"}).Where(new {COBy = "Kevin Maloney"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Kevin J Maloney"}).Where(new {BIBy = "Kevin Maloney"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Robert Aumack_Jr"}).Where(new {COBy = "Robert Aumack Jr"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Robert Aumack_Jr"}).Where(new {BIBy = "Robert Aumack Jr"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Kenneth Kratzer_Jr"})
                  .Where(new {BIBy = "Kenneth Kratzer Jr"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Kenneth Kratzer_Jr"})
                  .Where(new {COBy = "Kenneth Kratzer Jr"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Kenneth Kratzer_Jr"}).Where(new {BIBy = "Kenneth Kratzer"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Kenneth Kratzer_Jr"}).Where(new {COBy = "Kenneth Kratzer"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Joseph Stankiewicz"}).Where(new {COBy = "Joe Stankiewicz"});
            // The spaces between Joshua and Gwyn are different characters. I KNOW RIGHT!
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Joshua Gwyn"}).Where(new {COBy = "Joshua Gwyn"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Robert M Urban"}).Where(new {COBy = "Robert Urban"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Kristin Bianco"}).Where(new {COBy = "Kristin Jenkins"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Kristin Bianco"}).Where(new {BIBy = "Kristin Jenkins"});
            Update.Table(NEW_TABLE_NAME).Set(new {COBy = "Patty Gannon"}).Where(new {COBy = "Patty Kaylor"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "Walter Howell_Jr"}).Where(new {BIBy = "Walter Howell Jr"});
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "tomshroba"}).Where(new {BIBy = "shrobatg"});

            // Alex said to set these to mcadmin. Only two records.
            Update.Table(NEW_TABLE_NAME).Set(new {BIBy = "mcadmin"}).Where(new {BIBy = "linterg"});
        }

        #endregion

        public override void Up()
        {
            // Renaming
            Rename.Table(OLD_TABLE_NAME).To(NEW_TABLE_NAME);
            Rename.Column(OLD_OUT_OF_SERVICE_DATE).OnTable(NEW_TABLE_NAME).To(NEW_OUT_OF_SERVICE_DATE);
            Rename.Column(OLD_BACK_IN_SERVICE_DATE).OnTable(NEW_TABLE_NAME).To(NEW_BACK_IN_SERVICE_DATE);
            Rename.Column(OLD_ID).OnTable(NEW_TABLE_NAME).To(NEW_ID);

            // Removing bad things
            Execute.Sql("ALTER TABLE [dbo].[HydrantsOutOfService] DROP CONSTRAINT [DF_tblNJAWHydOS_DateCBS]");
            Execute.Sql("ALTER TABLE [dbo].[HydrantsOutOfService] DROP CONSTRAINT [DF_tblNJAWHydOS_DateCreated]");

            // Fix columns
            Alter.Table(NEW_TABLE_NAME)
                 .AlterColumn(NEW_OUT_OF_SERVICE_DATE).AsDateTime().NotNullable()
                 .AlterColumn(NEW_BACK_IN_SERVICE_DATE).AsDateTime().Nullable()
                 .AlterColumn(OLD_DATE_CREATED).AsDateTime().NotNullable()
                 .AlterColumn(OLD_FDCONTACT).AsString(50).Nullable()
                 .AlterColumn(OLD_FDFAX).AsString(20).Nullable()
                 .AlterColumn(OLD_FDPHONE).AsString(20).Nullable()
                 .AddColumn(NEW_HYDRANT_ID).AsInt32().Nullable()
                 .ForeignKey("FK_Hydrants_HydrantId", "Hydrants", "Id")
                 .AddColumn(NEW_BACK_IN_SERVICE_BY_USER_ID).AsInt32().Nullable()
                 .ForeignKey("FK_tblPermissions_BackInServiceByUserId", "tblPermissions", "RecId")
                 .AddColumn(NEW_OUT_OF_SERVICE_BY_USER_ID).AsInt32().Nullable()
                 .ForeignKey("FK_tblPermissions_OutOfServiceByUserId", "tblPermissions", "RecId");

            // Cleaning up data
            Update.Table(NEW_TABLE_NAME).Set(new {BackInServiceDate = (DateTime?)null})
                  .Where(new {BackInServiceDate = BAD_1900_DATE_THAT_SHOULD_MEAN_NULL});
            Update.Table(NEW_TABLE_NAME).Set(new {FDContact = (string)null}).Where(new {FDContact = string.Empty});
            Update.Table(NEW_TABLE_NAME).Set(new {FDFax = (string)null}).Where(new {FDFax = string.Empty});
            Update.Table(NEW_TABLE_NAME).Set(new {FDPhone = (string)null}).Where(new {FDPhone = string.Empty});

            CleanInvalidHydrantEntries();

            // Get proper HydrantId values
            Execute.Sql(@"update HydrantsOutOfService 
set HydrantsOutOfService.HydrantId = Hydrants.Id 
from Hydrants 
where Hydrants.HydrantNumber = HydrantsOutOfService.HydNum
and HydrantsOutOfService.OpCntr = (select top 1 OperatingCenterCode from OperatingCenters where OperatingCenterId = Hydrants.OperatingCenterId)");

            Execute.Sql(
                @"UPDATE HydrantsOutOfService SET OutOfServiceByUserId = (select top 1 RecId from tblPermissions where FullName = COBy) where OutOfServiceByUserId is null");
            Execute.Sql(
                @"UPDATE HydrantsOutOfService SET OutOfServiceByUserId = (select top 1 RecId from tblPermissions where UserName = COBy) where OutOfServiceByUserId is null");
            Execute.Sql(
                @"UPDATE HydrantsOutOfService SET BackInServiceByUserId = (select top 1 RecId from tblPermissions where FullName = BIBy) where BackInServiceByUserId is null");
            Execute.Sql(
                @"UPDATE HydrantsOutOfService SET BackInServiceByUserId = (select top 1 RecId from tblPermissions where UserName = BIBy) where BackInServiceByUserId is null");

            // Alex said to delete any record where the Hydrant can't be matched. There's only 12 of these.
            Delete.FromTable(NEW_TABLE_NAME).Row(new {HydrantId = (int?)null});

            // This is a duplicate entry of an out of service record that hasn't been called back into service. It's the only one.
            Delete.FromTable(NEW_TABLE_NAME).Row(new {Id = 1256});

            // Convert remarks to notes for hydrant.
            Execute.Sql(@"declare @noteDataTypeId int
set @noteDataTypeId = (select top 1 DataTypeId from DataType where Table_Name = 'Hydrants');

insert into Note (Note, Date_Added, DataLinkID, DataTypeID, CreatedBy)
select
	'[From Out of Service Record #' + CONVERT(VARCHAR(MAX),hos.Id) + '] ' + CONVERT(VARCHAR(MAX),hos.Remarks) as Note,
	getdate() as Date_Added,
	hos.HydrantId as DataLinkID,
	@noteDataTypeId as DataTypeId,
	'mcadmin' as CreatedBy
from HydrantsOutOfService hos");

            // Make HydrantId not nullable
            Alter.Column(NEW_HYDRANT_ID).OnTable(NEW_TABLE_NAME).AsInt32().NotNullable();
            Alter.Column(NEW_OUT_OF_SERVICE_BY_USER_ID).OnTable(NEW_TABLE_NAME).AsInt32().NotNullable();

            // Remove junk columns
            Delete.Column(OLD_HYDNUM).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_OPERATING_CENTER).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_CROSS_STREET).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_FULLSTNAME).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_TOWN).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_BIBY).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_COBY).FromTable(NEW_TABLE_NAME);
            Delete.Column(OLD_REMARKS).FromTable(NEW_TABLE_NAME);

            Delete.ForeignKey("FK_tblNJAWHydOs_Towns_TownID").OnTable(NEW_TABLE_NAME);
            Delete.Column(OLD_TOWN_ID).FromTable(NEW_TABLE_NAME);

            // Kill OutOfService column from Hydrants table as it's now a formula.
            Delete.Column("OutOfService").FromTable("Hydrants");
        }

        public override void Down()
        {
            Alter.Table("Hydrants")
                 .AddColumn("OutOfService").AsBoolean().NotNullable().WithDefaultValue(false);

            Execute.Sql(
                @"UPDATE [Hydrants] SET [OutOfService] = 1 WHERE EXISTS (select null from HydrantsOutOfService hoos where hoos.HydrantId = Hydrants.Id and hoos.BackInServiceDate is null)");

            // This isn't a constraint before this, so we need to remove. We included it to set all the values.
            Execute.Sql("ALTER TABLE [dbo].[Hydrants] DROP CONSTRAINT [DF_Hydrants_OutOfService]");

            Update.Table(NEW_TABLE_NAME)
                  .Set(new {BackInServiceDate = BAD_1900_DATE_THAT_SHOULD_MEAN_NULL})
                  .Where(new {BackInServiceDate = (DateTime?)null});

            Alter.Table(NEW_TABLE_NAME)
                 .AlterColumn(NEW_BACK_IN_SERVICE_DATE).AsDateTime().Nullable()
                 .AlterColumn(NEW_OUT_OF_SERVICE_DATE).AsDateTime().Nullable()
                 .AddColumn(OLD_HYDNUM).AsString(10).Nullable()
                 .AddColumn(OLD_OPERATING_CENTER).AsString(4).Nullable()
                 .AddColumn(OLD_CROSS_STREET).AsString(30).Nullable()
                 .AddColumn(OLD_FULLSTNAME).AsString(50).Nullable()
                 .AddColumn(OLD_TOWN).AsString(50).Nullable()
                 .AddColumn(OLD_BIBY).AsString(25).Nullable()
                 .AddColumn(OLD_COBY).AsString(25).Nullable()
                 .AddColumn(OLD_REMARKS).AsCustom("ntext").Nullable()
                 .AddColumn(OLD_TOWN_ID).AsInt32().Nullable()
                 .ForeignKey("FK_tblNJAWHydOs_Towns_TownID", "Towns", "TownId");

            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET HydNum = (select HydrantNumber from Hydrants where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET OpCntr = (select OperatingCenterCode from Hydrants inner join OperatingCenters opc on opc.OperatingCenterId = Hydrants.OperatingCenterId where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET CrossStreet = (select s.FullStName from Hydrants inner join Streets s on s.StreetId = Hydrants.CrossStreetId where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET FullStName = (select s.FullStName from Hydrants inner join Streets s on s.StreetId = Hydrants.StreetId where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET Town = (select t.Town from Hydrants inner join Towns t on t.TownId = Hydrants.Town where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET TownId = (select Town from Hydrants where Hydrants.Id = HydrantId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET BIBy = (select top 1 UserName from tblPermissions where RecId = BackInServiceByUserId)");
            Execute.Sql(
                "UPDATE [HydrantsOutOfService] SET COBy = (select top 1 UserName from tblPermissions where RecId = OutOfServiceByUserId)");

            // Reset Remarks values. Note that if you migrate again it's going to end up with two [From out of Service Record] things.
            Execute.Sql(
                "update HydrantsOutOfService set Remarks = (select top 1 Note from Note where Note like '[[]From Out of Service Record #' + CONVERT(VARCHAR(MAX), Id) + ']%')");
            Execute.Sql("delete from Note where Note like '[[]From Out of Service Record%'");

            Delete.ForeignKey("FK_Hydrants_HydrantId").OnTable(NEW_TABLE_NAME);
            Delete.ForeignKey("FK_tblPermissions_BackInServiceByUserId").OnTable(NEW_TABLE_NAME);
            Delete.ForeignKey("FK_tblPermissions_OutOfServiceByUserId").OnTable(NEW_TABLE_NAME);
            Delete.Column(NEW_HYDRANT_ID).FromTable(NEW_TABLE_NAME);
            Delete.Column(NEW_BACK_IN_SERVICE_BY_USER_ID).FromTable(NEW_TABLE_NAME);
            Delete.Column(NEW_OUT_OF_SERVICE_BY_USER_ID).FromTable(NEW_TABLE_NAME);

            Rename.Column(NEW_ID).OnTable(NEW_TABLE_NAME).To(OLD_ID);
            Rename.Column(NEW_BACK_IN_SERVICE_DATE).OnTable(NEW_TABLE_NAME).To(OLD_BACK_IN_SERVICE_DATE);
            Rename.Column(NEW_OUT_OF_SERVICE_DATE).OnTable(NEW_TABLE_NAME).To(OLD_OUT_OF_SERVICE_DATE);

            // Readding bad things
            Execute.Sql(
                "ALTER TABLE [HydrantsOutOfService] ADD  CONSTRAINT [DF_tblNJAWHydOS_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]");
            Execute.Sql(
                "ALTER TABLE [HydrantsOutOfService] ADD  CONSTRAINT [DF_tblNJAWHydOS_DateCBS]  DEFAULT (((1)/(1))/(1900)) FOR [DateCBS]");

            Rename.Table(NEW_TABLE_NAME).To(OLD_TABLE_NAME);
        }
    }
}
