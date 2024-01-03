using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180612100106890), Tags("Production")]
    public class PremiseStatusCodeNormalization : Migration
    {
        public override void Up()
        {
            //Creates table
            Create.Table("PremiseStatusCodes")
                  .WithIdentityColumn("Id")
                  .WithColumn("Description").AsString();
            //adds values to new table
            Execute.Sql("INSERT INTO PremiseStatusCodes (Description) VALUES ('Active')");
            Execute.Sql("INSERT INTO PremiseStatusCodes (Description) VALUES ('Inactive')");
            Execute.Sql("INSERT INTO PremiseStatusCodes (Description) VALUES ('Killed')");
            //Adds foreign key to premise
            Alter.Table("Premises").AddForeignKeyColumn("StatusCodeId", "PremiseStatusCodes");
            //Normalizes exisiting data, deletes old column
            Execute.Sql("UPDATE Premises SET StatusCodeId = 1 WHERE StatusCode = 'Active'");
            Execute.Sql("UPDATE Premises SET StatusCodeId = 2 WHERE StatusCode = 'Inactive'");
            Execute.Sql("UPDATE Premises SET StatusCodeId = 3 WHERE StatusCode = 'Killed'");

            Execute.Sql(
                "IF EXISTS (select 1 from sysindexes where Name = '_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28') DROP INDEX [_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON [dbo].[Premises]");

            Delete.Column("StatusCode").FromTable("Premises");

            Execute.Sql(@"SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON [dbo].[Premises]
(
	[ConnectionObject] ASC,
	[PremiseNumber] ASC,
	[DeviceLocation] ASC,
	[Installation] ASC,
	[ServiceStateId] ASC
)
INCLUDE ( 	[Id],
	[DeviceCategory],
	[Equipment],
	[DeviceSerialNumber],
	[ServiceAddressHouseNumber],
	[ServiceAddressFraction],
	[ServiceAddressApartment],
	[ServiceAddressStreet],
	[ServiceZip],
	[StatusCodeId],
	[MeterSerialNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
");
        }

        public override void Down()
        {
            Execute.Sql(
                "DROP INDEX [_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON [dbo].[Premises]");

            Alter.Table("Premises").AddColumn("StatusCode").AsString(10).Nullable();

            Execute.Sql(@"SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON [dbo].[Premises]
(
	[ConnectionObject] ASC,
	[PremiseNumber] ASC,
	[DeviceLocation] ASC,
	[Installation] ASC,
	[ServiceStateId] ASC
)
INCLUDE ( 	[Id],
	[DeviceCategory],
	[Equipment],
	[DeviceSerialNumber],
	[ServiceAddressHouseNumber],
	[ServiceAddressFraction],
	[ServiceAddressApartment],
	[ServiceAddressStreet],
	[ServiceZip],
	[StatusCode],
	[MeterSerialNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
");

            Execute.Sql("UPDATE Premises SET StatusCode = 'Active' WHERE StatusCodeId = 1");
            Execute.Sql("UPDATE Premises SET StatusCode = 'Inactive' WHERE StatusCodeId = 2");
            Execute.Sql("UPDATE Premises SET StatusCode = 'Killed' WHERE StatusCodeId = 3");
            Delete.ForeignKeyColumn("Premises", "StatusCodeId", "PremiseStatusCodes");
            Delete.Table("PremiseStatusCodes");
        }
    }
}
