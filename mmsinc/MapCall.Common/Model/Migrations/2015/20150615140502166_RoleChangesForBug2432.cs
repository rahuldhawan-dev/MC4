using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150615140502166), Tags("Production")]
    public class RoleChangesForBug2432 : Migration
    {
        public const int FIELD_SERVICES_ASSETS = 73;

        private const int FS_HYDRANTS = 4,
                          FS_HYDINSPECT = 5,
                          FS_VALVES = 6,
                          FS_VALVEINSPECT = 7,
                          WNP_SEWER = 40,
                          WNP_STORM = 41;

        private const int WATER_NON_POTABLE = 5;

        private const int NOTIFICATION_HYDRANT = 5,
                          NOTIFICATION_VALVE = 11;

        public override void Up()
        {
            // Create new role module

            this.EnableIdentityInsert("Modules");
            Insert.IntoTable("Modules").Row(new {ModuleId = FIELD_SERVICES_ASSETS, ApplicationID = 1, Name = "Assets"});
            this.DisableIdentityInsert("Modules");

            // Convert existing roles to the new one

            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_HYDRANTS});
            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_HYDINSPECT});
            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_VALVES});
            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_VALVEINSPECT});
            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = WNP_SEWER});
            Update.Table("Roles").Set(new {ApplicationID = 1, ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = WNP_STORM});

            // Convert notification purposes 
            Update.Table("NotificationPurposes").Set(new {ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_HYDRANTS});
            Update.Table("NotificationPurposes").Set(new {ModuleID = FIELD_SERVICES_ASSETS})
                  .Where(new {ModuleID = FS_VALVES});

            // Remove duplicate roles due to the update.

            Execute.Sql(@"
delete r1
from Roles r1 
left join Roles r2 on 
	r1.ApplicationID = r2.ApplicationID 
	and r1.ModuleID = r2.ModuleID 
	and r1.ActionID = r2.ActionID 
	and r1.UserID = r2.UserID 
	where r1.RoleID < r2.RoleID and IsNull(r1.OperatingCenterID, 0) = IsNull(r2.OperatingCenterID, 0)
");

            // Delete any UserAdmin roles for the new module. Doug said he'll add back the few users that require user admin.
            Delete.FromTable("Roles").Row(new {ModuleID = FIELD_SERVICES_ASSETS, ActionId = 1});

            // Delete old modules

            Delete.FromTable("Modules").Row(new {ModuleID = FS_HYDRANTS})
                  .Row(new {ModuleID = FS_HYDINSPECT})
                  .Row(new {ModuleID = FS_VALVES})
                  .Row(new {ModuleID = FS_VALVEINSPECT})
                  .Row(new {ModuleID = WNP_SEWER})
                  .Row(new {ModuleID = WNP_STORM});

            // Delete Water Non Potable application since it's no longer used.
            Delete.FromTable("Applications").Row(new {ApplicationID = WATER_NON_POTABLE}); // Water Non Potable

            // These two MCDistro roles need to be removed in order to let the MapCall proper regression tests
            // to continue working. MCDistro isn't supposted to have roles for assets.
            Delete.FromTable("Roles").Row(new {UserId = 1632, ModuleId = FIELD_SERVICES_ASSETS});
        }

        public override void Down()
        {
            // Recreate Application
            this.EnableIdentityInsert("Applications");
            Insert.IntoTable("Applications").Row(new {ApplicationID = WATER_NON_POTABLE, Name = "Water Non Potable"});
            this.DisableIdentityInsert("Applications");

            // Recreate modules
            this.EnableIdentityInsert("Modules");
            Insert.IntoTable("Modules").Row(new {ModuleId = FS_HYDRANTS, ApplicationID = 1, Name = "Hydrants"});
            Insert.IntoTable("Modules").Row(new
                {ModuleId = FS_HYDINSPECT, ApplicationID = 1, Name = "Hydrant Inspections"});
            Insert.IntoTable("Modules").Row(new {ModuleId = FS_VALVES, ApplicationID = 1, Name = "Valves"});
            Insert.IntoTable("Modules").Row(new
                {ModuleId = FS_VALVEINSPECT, ApplicationID = 1, Name = "Valve Inspections"});
            Insert.IntoTable("Modules").Row(new
                {ModuleId = WNP_SEWER, ApplicationID = WATER_NON_POTABLE, Name = "Sewer"});
            Insert.IntoTable("Modules").Row(new
                {ModuleId = WNP_STORM, ApplicationID = WATER_NON_POTABLE, Name = "Storm Water"});
            this.DisableIdentityInsert("Modules");

            // Set notification purposes back
            Update.Table("NotificationPurposes").Set(new {ModuleID = FS_HYDRANTS})
                  .Where(new {NotificationPurposeID = NOTIFICATION_HYDRANT});
            Update.Table("NotificationPurposes").Set(new {ModuleID = FS_VALVES})
                  .Where(new {NotificationPurposeID = NOTIFICATION_VALVE});

            // Break up the new role into the six modules roles. Users will have same access
            System.Action<int, int> convertRole = (moduleId, appId) => {
                const string format = @"
insert into [Roles] (OperatingCenterID, ApplicationID, ModuleID, ActionID, UserID)
select
	OperatingCenterID, ApplicationID = {1}, ModuleID = {0},	ActionID, UserID
from Roles 
where ModuleID = 73";
                Execute.Sql(string.Format(format, moduleId, appId));
            };

            convertRole(FS_HYDRANTS, 1);
            convertRole(FS_HYDINSPECT, 1);
            convertRole(FS_VALVES, 1);
            convertRole(FS_VALVEINSPECT, 1);
            convertRole(WNP_SEWER, WATER_NON_POTABLE);
            convertRole(WNP_STORM, WATER_NON_POTABLE);

            // Delete the roles with the new module id
            Delete.FromTable("Roles").Row(new {ModuleId = FIELD_SERVICES_ASSETS});

            // Delete the new module
            Delete.FromTable("Modules").Row(new {ModuleID = FIELD_SERVICES_ASSETS});
        }
    }
}
