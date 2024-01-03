using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210622161235750), Tags("Production")]
    public class MC3385CreatingPremiseUnavailableReasonLookupTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("PremiseUnavailableReasons",
                "Killed Premise",
                "New Installation");
            Alter.Table("Services")
                 .AddForeignKeyColumn("PremiseUnavailableReasonId", "PremiseUnavailableReasons").Nullable()
                 .AddColumn("PremiseNumberUnavailable").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PremiseNumberUnavailable").FromTable("Services");
            Delete.ForeignKeyColumn("Services", "PremiseUnavailableReasonId", "PremiseUnavailableReasons");
            Delete.Table("PremiseUnavailableReasons");
        }
    }
}

