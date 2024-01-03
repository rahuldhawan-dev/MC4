using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180904105457038), Tags("Production")]
    public class AddCompletionActivityTypesForMC605 : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleWorkOrderCompletionsActivities")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderCompletionId", "ShortCycleWorkOrderCompletions")
                  .WithColumn("Description").AsAnsiString()
                  .NotNullable();
            Execute.Sql(
                "INSERT INTO ShortCycleWorkOrderCompletionsActivities SELECT Id, Activity1 FROM ShortCycleWorkOrderCompletions WHERE Activity1 is not null;" +
                "INSERT INTO ShortCycleWorkOrderCompletionsActivities SELECT Id, Activity2 FROM ShortCycleWorkOrderCompletions WHERE Activity2 is not null;" +
                "INSERT INTO ShortCycleWorkOrderCompletionsActivities SELECT Id, Activity3 FROM ShortCycleWorkOrderCompletions WHERE Activity3 is not null;"
            );
            Delete.Column("Activity1").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("Activity2").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("Activity3").FromTable("ShortCycleWorkOrderCompletions");
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("Activity1")
                 .AsAnsiString().Nullable();
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("Activity2")
                 .AsAnsiString().Nullable();
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("Activity3")
                 .AsAnsiString().Nullable();
            Execute.Sql(@"UPDATE
                            ShortCycleWorkOrderCompletions 
                        SET
                            Activity1 = (SELECT TOP 1 Description FROM ShortCycleWorkOrderCompletionsActivities A WHERE A.ShortCycleWorkOrderCompletionId = ShortCycleWorkOrderCompletions.Id ORDER BY A.Id)
                        WHERE
                            (SELECT COUNT(1) FROM ShortCycleWorkOrderCompletionsActivities scwoca where scwoca.ShortCycleWorkOrderCompletionId = ShortCycleWorkOrderCompletions.Id) > 0

                        UPDATE
                            ShortCycleWorkOrderCompletions 
                        SET
                            Activity2 = 
                            (
                                SELECT [Description] FROM
                                (
                                SELECT top 2 Description, ROW_NUMBER() OVER (Order by Id) as rownum 
                                FROM ShortCycleWorkOrderCompletionsActivities A 
                                WHERE A.ShortCycleWorkOrderCompletionId = SCWOC.Id
                                ) as Tbl WHERE rownum = 2
                            )
                        FROM
                            ShortCycleWorkOrderCompletions SCWOC
                        WHERE
                            (SELECT COUNT(1) FROM ShortCycleWorkOrderCompletionsActivities scwoca where scwoca.ShortCycleWorkOrderCompletionId = SCWOC.Id) > 1

                        UPDATE
                            ShortCycleWorkOrderCompletions 
                        SET
                            Activity3 = 
                            (
                                SELECT [Description] FROM
                                (
                                SELECT top 3 Description, ROW_NUMBER() OVER (Order by Id) as rownum 
                                FROM ShortCycleWorkOrderCompletionsActivities A 
                                WHERE A.ShortCycleWorkOrderCompletionId = SCWOC.Id
                                ) as Tbl WHERE rownum = 3
                            )
                        FROM
                            ShortCycleWorkOrderCompletions SCWOC
                        WHERE
                            (SELECT COUNT(1) FROM ShortCycleWorkOrderCompletionsActivities scwoca where scwoca.ShortCycleWorkOrderCompletionId = SCWOC.Id) > 2
");
            Delete.Table("ShortCycleWorkOrderCompletionsActivities");
        }
    }
}
