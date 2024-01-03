using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201208142912138), Tags("Production")]
    public class FixIdColumnOnEventsForMC1776 : Migration
    {
        public override void Up()
        {
            Rename.Column("EventId").OnTable("Events").To("Id");
        }

        public override void Down()
        {
            Rename.Column("Id").OnTable("Events").To("EventId");
        }
    }
}
