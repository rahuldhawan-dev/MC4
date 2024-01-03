using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210820085320713), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3315_NewRequirementsOnStoppageTypes : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                set identity_insert SewerStoppageTypes on;
                insert into SewerStoppageTypes(SewerStoppageTypeID, Description) values(10, 'CSO Unapproved Location');
                set identity_insert SewerStoppageTypes off;
                update SewerStoppageTypes set Description = 'Blockage' where SewerStoppageTypeID = 3;                
                update SewerStoppageTypes set Description = 'Mechanical and Power Failure' where SewerStoppageTypeID = 4;
                update SewerStoppageTypes set Description = 'Wet Weather and I/I' where SewerStoppageTypeID = 5;
                update SewerStoppageTypes set Description = 'Line Break' where SewerStoppageTypeID = 6;
                update SewerStoppageTypes set Description = 'Other' where SewerStoppageTypeID = 7;
            ");
        }

        public override void Down()
        {
            Execute.Sql("delete from SewerStoppageTypes where SewerStoppageTypeID = 10;");
            
            /*
             * no need to update the others, this was only done as a
             * 'patch' migration, it will be handled in the original
             * Down() migration for this story.
             */
        }
    }
}

