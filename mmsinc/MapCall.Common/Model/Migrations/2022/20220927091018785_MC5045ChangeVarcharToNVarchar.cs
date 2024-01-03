using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220927091018785), Tags("Production")]
    public class MC5045ChangeVarcharToNVarchar : Migration
    {
        public const string DROP_INDEXES_STATISTICS =
                                @"DROP INDEX[_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON[dbo].[Premises]
                                    DROP INDEX[IX_PREMISES_PREMISE_NUMBER_CONNECTION_OBJECT_DEVICE_LOCATION_INSTALLATION_ID] ON[dbo].[Premises]

                                    DROP STATISTICS[dbo].[Premises].[_dta_stat_131635662_16_27_6_3]
                                    DROP STATISTICS[dbo].[Premises].[_dta_stat_131635662_16_3_6_4]
                                    DROP STATISTICS[dbo].[Premises].[_dta_stat_131635662_27_6_3]
                                    DROP STATISTICS[dbo].[Premises].[_dta_stat_131635662_3_6]
                                    ",
                            CREATE_INDEXES_STATISTICS =
                                @"CREATE STATISTICS [_dta_stat_131635662_3_6] ON [dbo].[Premises]([PremiseNumber], [DeviceLocation])
                                    CREATE STATISTICS [_dta_stat_131635662_27_6_3] ON [dbo].[Premises]([Installation], [DeviceLocation], [PremiseNumber])
                                    CREATE STATISTICS [_dta_stat_131635662_16_3_6_4] ON [dbo].[Premises]([ServiceStateId], [PremiseNumber], [DeviceLocation], [ConnectionObject])
                                    CREATE STATISTICS [_dta_stat_131635662_16_27_6_3] ON [dbo].[Premises]([ServiceStateId], [Installation], [DeviceLocation], [PremiseNumber])

                                    CREATE NONCLUSTERED INDEX [IX_PREMISES_PREMISE_NUMBER_CONNECTION_OBJECT_DEVICE_LOCATION_INSTALLATION_ID] ON [dbo].[Premises] (
	                                    [Installation] ASC,
	                                    [DeviceLocation] ASC,
	                                    [ConnectionObject] ASC,
	                                    [PremiseNumber] ASC,
	                                    [Id] ASC
                                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]

                                    CREATE NONCLUSTERED INDEX [_dta_index_Premises_5_131635662__K4_K3_K6_K27_K16_1_5_7_9_11_12_13_14_17_22_28] ON [dbo].[Premises] (
	                                    [ConnectionObject] ASC,
	                                    [PremiseNumber] ASC,
	                                    [DeviceLocation] ASC,
	                                    [Installation] ASC,
	                                    [ServiceStateId] ASC
                                    )
                                    INCLUDE([Id],[DeviceCategory],[Equipment],[DeviceSerialNumber],[ServiceAddressHouseNumber],[ServiceAddressFraction],[ServiceAddressApartment],[ServiceAddressStreet],[ServiceZip],[StatusCodeId],[MeterSerialNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]";

        public override void Up()
        {
            Execute.Sql(DROP_INDEXES_STATISTICS);
            Alter.Column("ConnectionObject").OnTable("Premises").AsString(30).Nullable();
            Alter.Column("DeviceCategory").OnTable("Premises").AsString(18).Nullable();
            Alter.Column("DeviceLocation").OnTable("Premises").AsString(30).Nullable();
            Alter.Column("Equipment").OnTable("Premises").AsString(18).Nullable();
            Alter.Column("DeviceSerialNumber").OnTable("Premises").AsString(18).Nullable();
            Alter.Column("ServiceAddressHouseNumber").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("ServiceAddressFraction").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("ServiceAddressApartment").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("ServiceAddressStreet").OnTable("Premises").AsString(60).Nullable();
            Alter.Column("ServiceZip").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("MeterLocationId").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("Installation").OnTable("Premises").AsString(10).Nullable();
            Alter.Column("MeterSerialNumber").OnTable("Premises").AsString(30).Nullable();
            Execute.Sql(CREATE_INDEXES_STATISTICS);
        }

        public override void Down()
        {
            Execute.Sql(DROP_INDEXES_STATISTICS);
            Alter.Column("ConnectionObject").OnTable("Premises").AsAnsiString(30).Nullable();
            Alter.Column("DeviceCategory").OnTable("Premises").AsAnsiString(18).Nullable();
            Alter.Column("DeviceLocation").OnTable("Premises").AsAnsiString(30).Nullable();
            Alter.Column("Equipment").OnTable("Premises").AsAnsiString(18).Nullable();
            Alter.Column("DeviceSerialNumber").OnTable("Premises").AsAnsiString(18).Nullable();
            Alter.Column("ServiceAddressHouseNumber").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("ServiceAddressFraction").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("ServiceAddressApartment").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("ServiceAddressStreet").OnTable("Premises").AsAnsiString(60).Nullable();
            Alter.Column("ServiceZip").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("MeterLocationId").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("Installation").OnTable("Premises").AsAnsiString(10).Nullable();
            Alter.Column("MeterSerialNumber").OnTable("Premises").AsAnsiString(30).Nullable();
            Execute.Sql(CREATE_INDEXES_STATISTICS);
        }
    }
}

