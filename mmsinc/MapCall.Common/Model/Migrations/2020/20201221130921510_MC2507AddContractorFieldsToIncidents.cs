using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201221130921510), Tags("Production")]
    public class MC2507AddContractorFieldsToIncidents : Migration
    {
        #region Consts

        private const string INCIDENTS_TABLE = "Incidents",
                             EMPLOYEE_TYPES_LOOKUP_TABLE = "EmployeeTypes";
        #endregion

        public override void Up()
        {
            this.CreateLookupTableWithValues(EMPLOYEE_TYPES_LOOKUP_TABLE, "Employee", "Contractor");

            Alter.Table(INCIDENTS_TABLE)
                 .AddForeignKeyColumn("EmployeeTypeId", EMPLOYEE_TYPES_LOOKUP_TABLE).NotNullable().WithDefaultValue(1)
                 .AddColumn("ContractorCompany").AsString(100).Nullable()
                 .AddColumn("ContractorName").AsString(100).Nullable()
                 .AlterColumn("EmployeeId").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ContractorCompany").FromTable(INCIDENTS_TABLE);
            Delete.Column("ContractorName").FromTable(INCIDENTS_TABLE);
            Delete.ForeignKeyColumn(INCIDENTS_TABLE, "EmployeeTypeId", EMPLOYEE_TYPES_LOOKUP_TABLE);
            Alter.Table(INCIDENTS_TABLE).AlterColumn("EmployeeId").AsInt32().NotNullable();
            Delete.Table(EMPLOYEE_TYPES_LOOKUP_TABLE);
        }
    }
}