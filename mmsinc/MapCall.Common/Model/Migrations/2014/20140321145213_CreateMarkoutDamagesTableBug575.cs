using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140321145213), Tags("Production")]
    public class CreateMarkoutDamagesTableBug575 : Migration
    {
        #region Consts

        public const int MAX_CREATEDBY = 50,
                         MAX_CROSS_STREET = 100,
                         MAX_EXCAVATOR = 50,
                         MAX_EXCAVATOR_ADDRESS = 120,
                         MAX_EXCAVATOR_PHONE = 20,
                         MAX_REQUEST_NUM = 9,
                         MAX_STREET = 100;

        #endregion

        #region Private Methods

        private void CreateLookupTable(string table, params string[] descriptions)
        {
            Create.Table(table)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(50).Unique().NotNullable();

            foreach (var d in descriptions)
            {
                Insert.IntoTable(table).Row(new {Description = d});
            }
        }

        #endregion

        public override void Up()
        {
            CreateLookupTable("MarkoutDamageToTypes", "Ours", "Other");

            Create.Table("MarkoutDamages")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("RequestNum").AsString(MAX_REQUEST_NUM).Nullable()
                  .WithColumn("OperatingCenterId").AsInt32().NotNullable()
                  .ForeignKey("FK_MarkoutDamages_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId")
                  .WithColumn("CreatedBy").AsString(MAX_CREATEDBY).NotNullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable()
                  .WithColumn("TownId").AsInt32().NotNullable()
                  .ForeignKey("FK_Towns_TownId", "Towns", "TownId")
                  .WithColumn("Street").AsString(MAX_STREET).NotNullable()
                  .WithColumn("NearestCrossStreet").AsString(MAX_CROSS_STREET).NotNullable()
                  .WithColumn("CoordinateId").AsInt32().NotNullable()
                  .ForeignKey("FK_MarkoutDamages_Coordinates_CoordinateId", "Coordinates", "CoordinateID")
                  .WithColumn("Excavator").AsString(MAX_EXCAVATOR).Nullable()
                  .WithColumn("ExcavatorPhone").AsString(MAX_EXCAVATOR_PHONE).Nullable()
                  .WithColumn("ExcavatorAddress").AsString(MAX_EXCAVATOR_ADDRESS).Nullable()
                  .WithColumn("DamageOn").AsDateTime().NotNullable()
                  .WithColumn("DamageComments").AsCustom("ntext").NotNullable()
                  .WithColumn("MarkoutDamageToTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_MarkoutDamages_MarkoutDamageToTypes_MarkoutDamageToTypeId", "MarkoutDamageToTypes",
                       "Id")
                  .WithColumn("UtilitiesDamaged").AsCustom("ntext").Nullable()
                  .WithColumn("EmployeesOnJob").AsCustom("ntext").Nullable()
                  .WithColumn("IsMarkedOut").AsBoolean().NotNullable()
                  .WithColumn("IsMismarked").AsBoolean().NotNullable()
                  .WithColumn("MismarkedByInches").AsInt32().NotNullable() // Defaulting to 0 is fine here.
                  .WithColumn("ExcavatorDiscoveredDamage").AsBoolean().NotNullable()
                  .WithColumn("ExcavatorCausedDamage").AsBoolean().NotNullable()
                  .WithColumn("Was911Called").AsBoolean().NotNullable()
                  .WithColumn("WerePicturesTaken").AsBoolean().NotNullable()
                  .WithColumn("ApprovedOn").AsDateTime().Nullable()
                  .WithColumn("SupervisorSignOffEmployeeId").AsInt32().Nullable()
                  .ForeignKey("FK_MarkoutDamages_tblEmployee_SupervisorSignOffEmployeeId", "tblEmployee",
                       "tblEmployeeId");

            Execute.Sql(@"
                declare @moduleId int
                set @moduleId = (select ModuleID from [Modules] where Name = 'Work Management')
                insert into [NotificationPurposes] ([ModuleID], [Purpose]) VALUES(@moduleId, 'Markout Damage')
            ");

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('MarkoutDamages', 'MarkoutDamages')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Markout Damages', @dataTypeId)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Photos', @dataTypeId)");
        }

        public override void Down()
        {
            this.DeleteDataType("MarkoutDamages");

            Execute.Sql(@"
                declare @purposeId int
                set @purposeId = (select top 1 NotificationPurposeID from [NotificationPurposes] where [Purpose] = 'Markout Damage')
                
                delete from [NotificationConfigurations] where [NotificationPurposeID] = @purposeId
                delete from [NotificationPurposes] where [NotificationPurposeID] = @purposeId 
            ");

            Delete.ForeignKey("FK_MarkoutDamages_tblEmployee_SupervisorSignOffEmployeeId").OnTable("MarkoutDamages");
            Delete.ForeignKey("FK_MarkoutDamages_MarkoutDamageToTypes_MarkoutDamageToTypeId").OnTable("MarkoutDamages");
            Delete.ForeignKey("FK_MarkoutDamages_Coordinates_CoordinateId").OnTable("MarkoutDamages");
            Delete.ForeignKey("FK_MarkoutDamages_OperatingCenters_OperatingCenterId").OnTable("MarkoutDamages");

            Delete.Table("MarkoutDamages");

            Delete.Table("MarkoutDamageToTypes");
        }
    }
}
