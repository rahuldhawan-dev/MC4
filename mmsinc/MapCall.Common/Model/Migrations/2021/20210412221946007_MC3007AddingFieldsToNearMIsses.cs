using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210412221946007), Tags("Production")]
    public class MC3007AddingFieldsToNearMIsses : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AddColumn("NotCompanyFacility").AsBoolean().Nullable()
                 .AddColumn("ReportAnonymously").AsBoolean().Nullable()
                 .AddColumn("ReportedToRegulator").AsBoolean().Nullable()
                 .AddColumn("EmployeeName").AsString().Nullable()
                 .AddColumn("ActionTaken").AsString().Nullable()
                 .AddColumn("ReviewedBy").AsString().Nullable()
                 .AddColumn("ReviewedAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("NotCompanyFacility").FromTable("NearMisses");
            Delete.Column("ReportAnonymously").FromTable("NearMisses");
            Delete.Column("ReportedToRegulator").FromTable("NearMisses");
            Delete.Column("EmployeeName").FromTable("NearMisses");
            Delete.Column("ActionTaken").FromTable("NearMisses");
            Delete.Column("ReviewedBy").FromTable("NearMisses");
            Delete.Column("ReviewedAt").FromTable("NearMisses");
        }
    }
}

