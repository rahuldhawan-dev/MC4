using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231026105612315), Tags("Production")]
    public class MC6163_AddSIFPToTheSIFDropdown :
        Migration
    {
        public override void Up() 
        {
            this.CreateLookupTableWithValues("SeriousInjuryOrFatalityTypes", "SIF",
                "SIF-P", "Non SIF");
            Alter.Table("Incidents")
                 .AddForeignKeyColumn("SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes");
            Execute.Sql(@"
            UPDATE Incidents set SeriousInjuryOrFatalityTypeId = 3  where IsSeriousInjuryOrFatality = 0
            UPDATE Incidents set SeriousInjuryOrFatalityTypeId = 1  where IsSeriousInjuryOrFatality = 1
            ");
            Delete.Column("IsSeriousInjuryOrFatality").FromTable("Incidents");
        }

        public override void Down()
        {
            Alter.Table("Incidents")
                 .AddColumn("IsSeriousInjuryOrFatality").AsBoolean().Nullable();
            Execute.Sql(@"
            UPDATE Incidents set IsSeriousInjuryOrFatality = 0  where SeriousInjuryOrFatalityTypeId = 3
            UPDATE Incidents set IsSeriousInjuryOrFatality = 1  where SeriousInjuryOrFatalityTypeId = 1
            ");
            Delete.ForeignKeyColumn("Incidents", "SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes");
            Delete.Table("SeriousInjuryOrFatalityTypes");
        }
    }
}

