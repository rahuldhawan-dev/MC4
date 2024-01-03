using FluentMigrator;
using FluentMigrator.Expressions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201009095101849), Tags("Production")]
    public class MC2689AddAdditionalColumnsToCOVID : Migration
    {
        public override void Up()
        {
            Alter.Table("CovidIssues")
                 .AddColumn("WorkExposure").AsBoolean().Nullable()
                 .AddColumn("AvoidableCloseContact").AsBoolean().Nullable()
                 .AddColumn("FaceCoveringWorn").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("WorkExposure").FromTable("CovidIssues");
            Delete.Column("AvoidableCloseContact").FromTable("CovidIssues");
            Delete.Column("FaceCoveringWorn").FromTable("CovidIssues");
        }
    }
}
