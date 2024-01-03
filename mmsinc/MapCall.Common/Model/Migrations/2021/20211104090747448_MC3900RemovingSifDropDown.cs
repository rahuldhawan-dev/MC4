using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211104090747448), Tags("Production")]
    public class MC3900RemovingSifDropDown : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AddColumn("SeriousInjuryOrFatality").AsBoolean().Nullable();

            Delete.ForeignKeyColumn("NearMisses", "SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes");
            Delete.Table("SeriousInjuryOrFatalityTypes");
        }

        public override void Down()
        {
            Delete.Column("SeriousInjuryOrFatality").FromTable("NearMisses");

            this.CreateLookupTableWithValues("SeriousInjuryOrFatalityTypes", "SIF", "SIF Potential");
            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes");
        }
    }
}

