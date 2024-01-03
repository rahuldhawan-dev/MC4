using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230502120023027), Tags("Production")]
    public class MC3977_IncreaseJobObservationAddressLength : Migration
    {
        public override void Up()
        {
            Alter.Table("tblJobObservations")
                 .AlterColumn("Location").AsAnsiString(100).Nullable();
        }

        public override void
            Down() { } // we don't do rollbacks for length changes because the length change would cause an error if anyone enters a value greater than length being rolled-back to.
    }
}