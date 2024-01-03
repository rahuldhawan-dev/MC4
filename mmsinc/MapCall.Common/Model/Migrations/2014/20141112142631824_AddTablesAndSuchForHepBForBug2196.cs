using System;
using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141112142631824), Tags("Production")]
    public class AddTablesAndSuchForHepBForBug2196 : Migration
    {
        public const int EMPLOYEE_LIMITED = 71;
        public const string MODULE_NAME = "EmployeeLimited";

        public struct TableNames
        {
            public const string HEPATITIS_B_VACCINATIONS = "HepatitisBVaccinations",
                                HEPATITIS_B_VACCINE_STATUSES = "HepatitisBVaccineStatuses";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.HEPATITIS_B_VACCINE_STATUSES, "Accepted", "Declined",
                "Offered");

            Create.Table(TableNames.HEPATITIS_B_VACCINATIONS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("ResponseDate").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("HepatitisBVaccineStatusId", "HepatitisBVaccineStatuses").NotNullable();

            Execute.Sql("declare @applicationId int;" +
                        "select @applicationId = (select top 1 ApplicationId from Applications where Name = 'Human Resources');" +
                        "set identity_insert Modules on;" +
                        "INSERT INTO Modules(ModuleID, ApplicationID, Name) Values(" + EMPLOYEE_LIMITED +
                        ", @applicationId, '" + MODULE_NAME + "');" +
                        "set identity_insert Modules off;");
            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('HepatitisBVaccinations', 'HepatitisBVaccinations')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Decline-Affidavit', @dataTypeId)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Accept-Affidavit', @dataTypeId)");
        }

        public override void Down()
        {
            Delete.Table(TableNames.HEPATITIS_B_VACCINATIONS);
            Delete.Table(TableNames.HEPATITIS_B_VACCINE_STATUSES);

            Execute.Sql("Delete Roles where ModuleId = " + EMPLOYEE_LIMITED);

            Execute.Sql("Delete Modules where ModuleId = " + EMPLOYEE_LIMITED);
            this.DeleteDataType("HepatitisBVaccinations");
        }
    }
}
