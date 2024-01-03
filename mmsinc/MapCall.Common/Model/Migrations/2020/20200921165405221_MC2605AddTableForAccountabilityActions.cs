using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200921165405221), Tags("Production")]
    public class MC2605AddTableForAccountabilityActions : Migration
    {
        public const int APPLICATION_ID = 3,
                         MODULE_ID = 84,
                         ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION = 255;

        public const string TABLE_NAME_ACCOUNTABILITY_TAKEN_TYPES = "AccountabilityActionTakenTypes",
                            DATA_TYPE_ACTION = "Employee Accountability Action",
                            TABLE_NAME_ACCOUNTABILITY_ACTIONS = "EmployeeAccountabilityActions";

        public override void Up()
        {
            this.CreateLookupTableWithValues(TABLE_NAME_ACCOUNTABILITY_TAKEN_TYPES, "Verbal Counseling", "Written warning",
                "Suspension", "Suspension with last chance agreement", "Termination", "Other");

            Create.Table(TABLE_NAME_ACCOUNTABILITY_ACTIONS)
                   //Original
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", nullable: false)
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId", nullable: false)
                  .WithForeignKeyColumn("DisciplineAdministeredById", "tblEmployee", "tblEmployeeId", nullable: false)
                  .WithForeignKeyColumn("AccountabilityActionTakenTypeId", "AccountabilityActionTakenTypes",
                       nullable: false)
                  .WithColumn("AccountabilityActionTakenDescription").AsAnsiString(ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION).NotNullable()
                  .WithColumn("DateAdministered").AsDateTime().NotNullable()
                  .WithColumn("StartDate").AsDateTime().Nullable()
                  .WithColumn("EndDate").AsDateTime().Nullable()
                  .WithColumn("NumberOfWorkDays").AsInt64().Nullable()
                  .WithForeignKeyColumn("IncidentId", "Incidents", "Id", nullable: true)

                   //Modified
                  .WithColumn("HasModifiedDiscipline").AsBoolean()
                  .WithForeignKeyColumn("ModifiedDisciplineAdministeredById", "tblEmployee", "tblEmployeeId",
                       nullable: true)
                  .WithForeignKeyColumn("ModifiedAccountabilityActionTakenTypeId", TABLE_NAME_ACCOUNTABILITY_TAKEN_TYPES,
                       nullable: true)
                  .WithColumn("ModifiedAccountabilityActionTakenDescription").AsAnsiString(ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION).Nullable()
                  .WithColumn("DateModified").AsDateTime().Nullable()
                  .WithColumn("ModifiedStartDate").AsDateTime().Nullable()
                  .WithColumn("ModifiedEndDate").AsDateTime().Nullable()
                  .WithColumn("ModifiedNumberOfWorkDays").AsInt64().Nullable()
                  .WithColumn("BackPayRequired").AsBoolean();

            //Notes Docs
            this.AddDataType(TABLE_NAME_ACCOUNTABILITY_ACTIONS);
            this.AddDocumentType(DATA_TYPE_ACTION, TABLE_NAME_ACCOUNTABILITY_ACTIONS);

            //Role -- 'Employee Accountability Actions'
            Execute.Sql(@"SET IDENTITY_INSERT [Modules] ON;
                        INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES ({0}, {1}, 'Employee Accountability Actions');
                        SET IDENTITY_INSERT [Modules] OFF;", APPLICATION_ID, MODULE_ID);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TABLE_NAME_ACCOUNTABILITY_ACTIONS, "AccountabilityActionTakenTypeId", TABLE_NAME_ACCOUNTABILITY_TAKEN_TYPES, "Id");
            Delete.Table(TABLE_NAME_ACCOUNTABILITY_ACTIONS);
            Delete.Table(TABLE_NAME_ACCOUNTABILITY_TAKEN_TYPES);

            this.RemoveDocumentTypeAndAllRelatedDocuments(DATA_TYPE_ACTION, TABLE_NAME_ACCOUNTABILITY_ACTIONS);
            this.RemoveDataType(TABLE_NAME_ACCOUNTABILITY_ACTIONS);
            //Role
            Execute.Sql($"DELETE FROM Roles WHERE ModuleID = {MODULE_ID};" +
                        $"DELETE FROM Modules WHERE ModuleID = {MODULE_ID}");
        }
    }
}