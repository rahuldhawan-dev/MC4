using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211005073514477), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3159_RemoveLicenseOperatorFields : Migration
    {
        public override void Up()
        {
            /*
             * These key constraints are named differently so we have to manually remove the
             * key from the column instead of using `RemoveForeignKeyColumn`. That method makes
             * some assumptions about the name of the key based on the current name of the table.
             *
             * Also - per hte business in this story there is no need to migrate any of the employees
             * associated in these fields... they've all been migrated previously so nothing to see here.
             */
            Delete.ForeignKey("FK_tblPWSID_tblEmployee_C_LOR").OnTable("PublicWaterSupplies");
            Delete.ForeignKey("FK_tblPWSID_tblEmployee_N_LOR").OnTable("PublicWaterSupplies");
            Delete.ForeignKey("FK_tblPWSID_tblEmployee_S_LOR").OnTable("PublicWaterSupplies");
            Delete.ForeignKey("FK_tblPWSID_tblEmployee_T_LOR").OnTable("PublicWaterSupplies");
            Delete.ForeignKey("FK_tblPWSID_tblEmployee_W_LOR").OnTable("PublicWaterSupplies");
            
            Delete.Column("C_LOR").FromTable("PublicWaterSupplies");
            Delete.Column("N_LOR").FromTable("PublicWaterSupplies");
            Delete.Column("S_LOR").FromTable("PublicWaterSupplies");
            Delete.Column("T_LOR").FromTable("PublicWaterSupplies");
            Delete.Column("W_LOR").FromTable("PublicWaterSupplies");
        }

        public override void Down()
        {
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("C_LOR", "tblEmployee", "tblEmployeeID");
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("N_LOR", "tblEmployee", "tblEmployeeID");
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("S_LOR", "tblEmployee", "tblEmployeeID");
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("T_LOR", "tblEmployee", "tblEmployeeID");
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("W_LOR", "tblEmployee", "tblEmployeeID");
        }
    }
}

