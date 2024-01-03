using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211028225812612), Tags("Production")]
    public class Mc3876RemovingSifFromNearMisses : Migration
    {
        public override void Up()
        {
            Delete.Column("SeriousInjuryOrFatality").FromTable("NearMisses");
        }

        public override void Down()
        {
            Alter.Table("NearMisses")
                 .AddColumn("SeriousInjuryOrFatality").AsBoolean().Nullable();
        }
    }
}

