using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230524160556436), Tags("Production")]
    public class MC4807_AddNewFieldsToDocument : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("DocumentStatuses", "Active", "Archived");
            Alter.Table("DocumentLink")
                 .AddForeignKeyColumn("DocumentStatusId", "DocumentStatuses");

            Alter.Table("DocumentLink")
                 .AddColumn("ReviewFrequency").AsInt32().Nullable()
                 .AddForeignKeyColumn("ReviewFrequencyUnitId", "RecurringFrequencyUnits");

            Execute.Sql(@"UPDATE 
    dl
SET 
    dl.DocumentStatusId = CASE
		WHEN d.File_Name LIKE '%Archived%'
				THEN (SELECT Id FROM DocumentStatuses WHERE Description = 'Archived')
		WHEN d.File_Name NOT LIKE '%Archived%'
			THEN (SELECT Id FROM DocumentStatuses WHERE Description = 'Active')
		END,
    dl.ReviewFrequency = 5,
    dl.ReviewFrequencyUnitId = (SELECT Id FROM RecurringFrequencyUnits WHERE Description = 'Year')
FROM 
    DocumentLink dl
    INNER JOIN Document d ON dl.DocumentID = d.documentID
WHERE 
    dl.DataTypeID IN (SELECT DataTypeID FROM DataType WHERE Table_Name = 'HelpTopics')");
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn("DocumentLink", "ReviewFrequencyUnitId", "RecurringFrequencyUnits");
            Delete.Column("ReviewFrequency").FromTable("DocumentLink");
            
            this.DeleteForeignKeyColumn("DocumentLink", "DocumentStatusId", "DocumentStatuses");
            Delete.Table("DocumentStatuses");
        }
    }
}

