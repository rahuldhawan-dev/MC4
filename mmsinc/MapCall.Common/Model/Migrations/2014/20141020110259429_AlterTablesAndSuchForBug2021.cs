using System;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141020110259429), Tags("Production")]
    public class AlterTablesAndSuchForBug2021 : Migration
    {
        public override void Up()
        {
            Create.Table("RegulationAgencies")
                  .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(10).NotNullable();

            Create.Table("RegulationStatuses")
                  .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(10).NotNullable();

            Execute.Sql("INSERT INTO RegulationAgencies (Description) SELECT Agency FROM Regulations;");

            Execute.Sql("INSERT INTO RegulationStatuses (Description) VALUES ('Active');");
            Execute.Sql("INSERT INTO RegulationStatuses (Description) VALUES ('Inactive');");

            Execute.Sql("UPDATE Regulations SET Agency = a.Id FROM RegulationAgencies a WHERE a.Description = Agency;");
            Execute.Sql("UPDATE Regulations SET Status = s.Id FROM RegulationStatuses s WHERE s.Description = Status");

            Rename.Column("Agency").OnTable("Regulations").To("AgencyId");
            Rename.Column("Status").OnTable("Regulations").To("StatusId");

            Alter.Column("AgencyId")
                 .OnTable("Regulations")
                 .AsInt32()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", "Regulations", "RegulationAgencies", "AgencyId"),
                      "RegulationAgencies", "Id")
                 .NotNullable();

            Alter.Column("StatusId")
                 .OnTable("Regulations")
                 .AsInt32()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", "Regulations", "RegulationStatuses", "StatusId"),
                      "RegulationStatuses", "Id")
                 .NotNullable();

            Alter.Table("TrainingRequirements")
                 .AddForeignKeyColumn("OSHAStandardId", "Regulations", "RegulationId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("TrainingRequirements", "OSHAStandardId", "Regulations", "RegulationId");

            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", "Regulations", "RegulationAgencies", "AgencyId"))
                  .OnTable("Regulations");
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", "Regulations", "RegulationStatuses", "StatusId"))
                  .OnTable("Regulations");

            Rename.Column("AgencyId").OnTable("Regulations").To("Agency");
            Rename.Column("StatusId").OnTable("Regulations").To("Status");

            Alter.Column("Agency").OnTable("Regulations").AsString(10).NotNullable();
            Alter.Column("Status").OnTable("Regulations").AsString(10).NotNullable();

            Execute.Sql("UPDATE Regulations SET Agency = a.Description FROM RegulationAgencies a WHERE Agency = a.Id");
            Execute.Sql("UPDATE Regulations SET Status = a.Description FROM RegulationStatuses a WHERE Status = a.Id");

            Delete.Table("RegulationAgencies");
            Delete.Table("RegulationStatuses");
        }
    }
}
