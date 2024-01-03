using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140825082335423), Tags("Production")]
    public class AddLockOutTagTableForBug1991 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string CREATE_SUPPORTING_DATA_TYPE_DATA = @"
                declare @dataTypeId int
                select @dataTypeId = (Select max(datatypeID) from dataType)+1

                SET IDENTITY_INSERT [DataType] ON

                INSERT INTO [DataType] (DataTypeID, Data_Type, Table_Name) VALUES (@dataTypeId, 'LockoutForms', 'LockoutForms')

                SET IDENTITY_INSERT [DataType] OFF

                INSERT INTO [DocumentType] (Document_Type, DataTypeID) VALUES ('LockoutForms', @dataTypeId)

                declare @iconId int
                declare @iconSetId int
                set @iconSetId = 9

                insert into MapIcon ([iconUrl], [height], [width], [OffsetId]) VALUES('MapIcons/lo.png', 24, 24, 5)

                set @iconId = (SELECT @@IDENTITY)

                SET IDENTITY_INSERT [IconSets] ON;
                insert into IconSets ([Id], [Name], [DefaultIconId]) VALUES(@iconSetId, 'Lockout Forms', @iconId)
                SET IDENTITY_INSERT [IconSets] OFF;

                insert into MapIconIconSets ([IconId], [IconSetId]) VALUES(@iconId, @iconSetId)

                insert into NotificationPurposes select top 1 ModuleId, 'New Lockout Form' from Modules where name = 'Health and Safety'
                insert into NotificationPurposes select top 1 ModuleId, 'Updated Lockout Form' from Modules where name = 'Health and Safety'",
                                ROLLBACK_SUPPORTING_DATA_TYPE_DATA = @"
                    DELETE FROM NotificationPurposes WHERE ModuleId = (SELECT ModuleId FROM Modules WHERE name = 'Health and Safety') AND Purpose = 'New Lockout Form'
                    DELETE FROM NotificationPurposes WHERE ModuleId = (SELECT ModuleId FROM Modules WHERE name = 'Health and Safety') AND Purpose = 'Updated Lockout Form'
                    DELETE FROM MapIconIconSets		 WHERE IconSetId = (SELECT Id FROM IconSets where Name = 'Lockout Forms')
                    DELETE FROM IconSets			 WHERE Name = 'Lockout Forms'
                    DELETE FROM MapIcon				 WHERE iconUrl = 'MapIcons/lo.png'
                    DELETE FROM DocumentType		 WHERE DataTypeID = (Select DataTypeID from DataType			 WHERE Table_Name = 'LockoutForms')
                    DELETE FROM DataType			 WHERE Table_Name = 'LockoutForms'
                ";
        }

        private struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        public struct TableNames
        {
            public const string LOCKOUT_FORMS = "LockoutForms",
                                LOCKOUT_REASONS = "LockoutReasons",
                                LOCKOUT_DEVICE_LOCATIONS = "LockoutDeviceLocations",
                                LOCKOUT_DEVICES = "LockoutDevices",
                                LOCKOUT_DEVICE_COLORS = "LockoutDeviceColors";
        }

        public struct ForeignKeys
        {
            public const string FK_LOCKOUT_FORMS_OPERATINGCENTERS =
                                    "FK_LockoutForms_OperatingCenters_OperatingCenterId",
                                FK_LOCKOUT_FORMS_FACILITIES = "FK_LockoutForms_tblFacilities_FacilityId",
                                FK_LOCKOUT_FORMS_EQUIPMENT = "FK_LockoutForms_Equipment_EquipmentId",
                                FK_LOCKOUT_FORMS_LOCKOUT_REASONS = "FK_LockoutForms_LockoutReasons_LockoutReasonId",
                                FK_LOCKOUT_FORMS_LOCKOUT_DEVICE_LOCATIONS =
                                    "FK_LockoutForms_LockoutDeviceLocations_LockoutDeviceLocationId",
                                FK_LOCKOUT_FORMS_LOCKOUT_DEVICES = "FK_LockoutForms_LockoutDevices_LockoutDeviceId",
                                FK_LOCKOUT_FORMS_COORDINATES = "FK_LockoutForms_Coordinates_CoordinateId",
                                FK_LOCKOUT_DEVICES_EMPLOYEES = "FK_LockoutDevices_tblPermissions_PersonId",
                                FK_LOCKOUT_DEVICES_OPERATINGCENTERS =
                                    "FK_LockoutDevices_OperatingCenters_OperatingCenterId",
                                FK_LOCKOUT_DEVICES_LOCKOUT_DEVICE_COLORS =
                                    "FK_LockoutDevices_LockoutDeviceColors_ColorId",
                                FK_LOCKOUT_FORMS_TBLEMPLOYEE_OUT =
                                    "FK_LockoutForms_tblEmployee_OutOfServiceAuthorizedEmployeeId",
                                FK_LOCKOUT_FORMS_TBLEMPLOYEE_RETURN =
                                    "FK_LockoutForms_tblEmployee_ReturnToServiceAuthorizedEmployeeId";
        }

        #endregion

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.LOCKOUT_REASONS, "Inspection", "Maintenance", "Repair",
                "Replacement");
            this.CreateLookupTableWithValues(TableNames.LOCKOUT_DEVICE_LOCATIONS, "Circuit Breaker", "Valve",
                "Mechanical");
            this.CreateLookupTableWithValues(TableNames.LOCKOUT_DEVICE_COLORS, "Red", "Green", "Blue", "Yellow");

            // Create Tables
            Create.Table(TableNames.LOCKOUT_DEVICES)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("OperatingCenterId").AsInt32().ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_OPERATINGCENTERS,
                       "OperatingCenters", "OperatingCenterId").NotNullable()
                  .WithColumn("PersonId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_EMPLOYEES, "tblPermissions", "RecID").NotNullable()
                  .WithColumn("SerialNumber").AsAnsiString(StringLengths.DESCRIPTION).Nullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn("ColorId").AsInt32().ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_LOCKOUT_DEVICE_COLORS,
                       "LockoutDeviceColors", "Id").Nullable();

            Create.Table(TableNames.LOCKOUT_FORMS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("LockoutDateTime").AsDateTime().NotNullable()
                  .WithColumn("OperatingCenterId").AsInt32().ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_OPERATINGCENTERS,
                       "OperatingCenters", "OperatingCenterId").NotNullable()
                  .WithColumn("FacilityId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_FACILITIES, "tblFacilities", "RecordId").NotNullable()
                  .WithColumn("EquipmentId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_EQUIPMENT, "Equipment", "EquipmentId").Nullable()
                  .WithColumn("EquipmentIdOther").AsAnsiString(StringLengths.DESCRIPTION).Nullable()
                  .WithColumn("Address").AsCustom("text").NotNullable()
                  .WithColumn("CoordinateId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_COORDINATES, "Coordinates", "CoordinateID").NotNullable()
                  .WithColumn("SAPWorkOrderNumber").AsAnsiString(StringLengths.DESCRIPTION).Nullable()
                  .WithColumn("LockoutReasonId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_REASONS, "LockoutReasons", "Id").NotNullable()
                  .WithColumn("ReasonForLockout").AsCustom("text").NotNullable()
                  .WithColumn("LockoutDeviceLocationId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_DEVICE_LOCATIONS, "LockoutDeviceLocations", "Id")
                  .NotNullable()
                  .WithColumn("LocationOfLockoutNotes").AsCustom("text").NotNullable()
                  .WithColumn("LockoutDeviceId").AsInt32()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_DEVICES, "LockoutDevices", "Id").Nullable()
                  .WithColumn("AffectedEmployeesNotified").AsBoolean().NotNullable()
                  .WithColumn("AffectedEquipmentShutdown").AsBoolean().NotNullable()
                  .WithColumn("IsolatesEnergySources").AsBoolean().NotNullable()
                  .WithColumn("AdditionalLockoutNotes").AsCustom("text").Nullable()
                  .WithColumn("ClearlyIndicatesProhibited").AsBoolean().NotNullable()
                  .WithColumn("RenderedSafeUntilComplete").AsBoolean().NotNullable()
                  .WithColumn("CannotBeOperated").AsBoolean().NotNullable()
                  .WithColumn("OutOfServiceAuthorizedEmployeeId").AsInt32().NotNullable()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_TBLEMPLOYEE_OUT, "tblEmployee", "tblEmployeeID")
                  .WithColumn("OutOfServiceDateTime").AsDateTime().NotNullable()
                  .WithColumn("ParkedInHomeSafe").AsBoolean().Nullable()
                  .WithColumn("RemovedDeviceAndNotified").AsBoolean().Nullable()
                  .WithColumn("ReturnedToServiceNotes").AsCustom("text").Nullable()
                  .WithColumn("ReturnToServiceAuthorizedEmployeeId").AsInt32().Nullable()
                  .ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_TBLEMPLOYEE_RETURN, "tblEmployee", "tblEmployeeID")
                  .WithColumn("ReturnedToServiceDateTime").AsDateTime().Nullable();

            Execute.Sql(Sql.CREATE_SUPPORTING_DATA_TYPE_DATA);
        }

        public override void Down()
        {
            // Remove FK Constraints
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_OPERATINGCENTERS).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_FACILITIES).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_EQUIPMENT).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_REASONS).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_DEVICE_LOCATIONS).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_LOCKOUT_DEVICES).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_COORDINATES).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_TBLEMPLOYEE_OUT).OnTable(TableNames.LOCKOUT_FORMS);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_FORMS_TBLEMPLOYEE_RETURN).OnTable(TableNames.LOCKOUT_FORMS);

            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_EMPLOYEES).OnTable(TableNames.LOCKOUT_DEVICES);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_OPERATINGCENTERS).OnTable(TableNames.LOCKOUT_DEVICES);
            Delete.ForeignKey(ForeignKeys.FK_LOCKOUT_DEVICES_LOCKOUT_DEVICE_COLORS).OnTable(TableNames.LOCKOUT_DEVICES);

            // Remove Tables
            Delete.Table(TableNames.LOCKOUT_REASONS);
            Delete.Table(TableNames.LOCKOUT_DEVICE_COLORS);
            Delete.Table(TableNames.LOCKOUT_DEVICES);
            Delete.Table(TableNames.LOCKOUT_DEVICE_LOCATIONS);
            Delete.Table(TableNames.LOCKOUT_FORMS);

            Execute.Sql(Sql.ROLLBACK_SUPPORTING_DATA_TYPE_DATA);
        }
    }
}
