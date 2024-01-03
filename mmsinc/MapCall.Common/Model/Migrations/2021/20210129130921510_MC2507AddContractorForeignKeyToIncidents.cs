using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210129130921510), Tags("Production")]
    public class MC2507AddContractorForeignKeyToIncidents : Migration
    {
        #region Consts

        private const string INCIDENTS_TABLE = "Incidents";

        #endregion

        public override void Up()
        {
            Alter.Table(INCIDENTS_TABLE)
                 .AddForeignKeyColumn("ContractorObservedById", "tblEmployee", "tblEmployeeId").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(INCIDENTS_TABLE, "ContractorObservedById", "tblEmployee");
        }
    }
}