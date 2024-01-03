using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140826092314960), Tags("Production")]
    public class FixTrainingRolesForBug1738 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
				DECLARE @ModuleID int
				SELECT @ModuleID = 70
                UPDATE Modules SET Name = 'Training Modules' where Name = 'Training' and ApplicationID = 4
                SET IDENTITY_INSERT [Modules] ON;
                INSERT INTO Modules(ModuleID, ApplicationID, Name) Values(@ModuleID, 4, 'Training Records')
                SET IDENTITY_INSERT [Modules] OFF;
				INSERT INTO 
					Roles(OperatingCenterID, ApplicationID, ModuleID, ActionID, UserId)
				select 
					OperatingCenterID, ApplicationID, @ModuleID, ActionId, UserId from Roles where applicationID = 4 and ModuleId = 36
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
				DELETE FROM Roles where ModuleID = 70
				DELETE FROM Modules where ModuleID = 70
				UPDATE Modules SET Name = 'Training' where Name = 'Training Modules' and ApplicationID = 4
            ");
        }
    }
}
