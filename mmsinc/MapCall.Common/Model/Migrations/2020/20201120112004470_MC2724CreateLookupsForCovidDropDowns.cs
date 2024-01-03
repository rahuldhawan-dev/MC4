using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201120112004470), Tags("Production")]
    public class MC2724CreateLookupsForCovidDropDowns : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("CovidAnswerTypes", "Yes", "No", "TBD", "Contact Tracer Must Complete");

            Alter.Table("CovidIssues")
                 .AddForeignKeyColumn("WorkExposureId", "CovidAnswerTypes")
                 .AddForeignKeyColumn("AvoidableCloseContactId", "CovidAnswerTypes")
                 .AddForeignKeyColumn("FaceCoveringWornId", "CovidAnswerTypes");

            Execute.Sql(@"UPDATE CovidIssues
           SET WorkExposureId = 1
           WHERE WorkExposure = 1;
           UPDATE CovidIssues
           SET WorkExposureId = 2
           WHERE WorkExposure = 0;
           UPDATE CovidIssues
           SET AvoidableCloseContactId = 1
           WHERE AvoidableCloseContact = 1;
           UPDATE CovidIssues
           SET AvoidableCloseContactId = 2
           WHERE AvoidableCloseContact = 0;
           UPDATE CovidIssues
           SET FaceCoveringWornId = 1
           WHERE FaceCoveringWorn = 1;
           UPDATE CovidIssues
           SET FaceCoveringWornId = 2
           WHERE FaceCoveringWorn = 0;");

            Delete.Column("WorkExposure").FromTable("CovidIssues");
            Delete.Column("AvoidableCloseContact").FromTable("CovidIssues");
            Delete.Column("FaceCoveringWorn").FromTable("CovidIssues");
        }

        public override void Down()
        {
            Alter.Table("CovidIssues")
                 .AddColumn("WorkExposure").AsBoolean().Nullable()
                 .AddColumn("AvoidableCloseContact").AsBoolean().Nullable()
                 .AddColumn("FaceCoveringWorn").AsBoolean().Nullable();

            Execute.Sql(@"UPDATE CovidIssues
            SET WorkExposure = 1
            WHERE WorkExposureId = 1;
            UPDATE CovidIssues
            SET WorkExposure = 0
            WHERE WorkExposureId = 2;
            UPDATE CovidIssues
            SET AvoidableCloseContact = 1
            WHERE AvoidableCloseContactId = 1;
            UPDATE CovidIssues
            SET AvoidableCloseContact = 0
            WHERE AvoidableCloseContactId = 2;
            UPDATE CovidIssues
            SET FaceCoveringWorn = 1
            WHERE FaceCoveringWornId = 1;
            UPDATE CovidIssues
            SET FaceCoveringWorn = 0
            WHERE FaceCoveringWornId = 2;");

            Delete.ForeignKeyColumn("CovidIssues", "WorkExposureId", "CovidAnswerTypes");
            Delete.ForeignKeyColumn("CovidIssues", "AvoidableCloseContactId", "CovidAnswerTypes");
            Delete.ForeignKeyColumn("CovidIssues", "FaceCoveringWornId", "CovidAnswerTypes");

            Delete.Table("CovidAnswerTypes");
        }
    }
}
