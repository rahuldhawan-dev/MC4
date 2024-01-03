using FluentMigrator;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210723112355879), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3241AddPermitTypesAsNotificationPurposes : Migration
    {
        public override void Up()
        {
            // 1.Many to many table
            Create.Table("NotificationConfigurationsNotificationPurposes")
                  .WithForeignKeyColumn("NotificationConfigurationId", "NotificationConfigurations", "NotificationConfigurationId")
                  .WithForeignKeyColumn("NotificationPurposeId", "NotificationPurposes", "NotificationPurposeId");

            // 2. Module
            this.CreateModule("Permit Types", "Environmental", 93);

            // 3. Migrate all the data stuff
            Execute.Sql(@"
                     -- figure out our id for the notification purpose being migrated
                declare @notificationPurposeToMigrateId int
                 select @notificationPurposeToMigrateId = NotificationPurposeId  
                   from NotificationPurposes 
                  where Purpose = 'Environmental Permit Expiration';

                     -- figure out our most recent notification purpose id
                declare @existingNotificationPurposesMaxId int          
                 select @existingNotificationPurposesMaxId = max(NotificationPurposeId) 
                   from NotificationPurposes;

                     -- port permit types to notification purposes
                 insert into NotificationPurposes
                 select 93
                      , [Description]
                   from EnvironmentalPermitTypes;
            
                     -- for each current notification configuration, create a notification configurations notification purpose
                 insert into NotificationConfigurationsNotificationPurposes
                 select NotificationConfigurationID
                      , NotificationPurposeID
                   from NotificationConfigurations;
                 
                     -- migrate the 1-1 relationship of permit expiration to be n-n
                 insert into NotificationConfigurationsNotificationPurposes
                 select nc.NotificationConfigurationID
                      , np.NotificationPurposeId  
                   from NotificationConfigurations nc
                      , NotificationPurposes np
                  where nc.NotificationPurposeId = @notificationPurposeToMigrateId
                    and np.NotificationPurposeId > @existingNotificationPurposesMaxId;
                 
                     -- delete the records in the n-n table that were used to help in migrating
                 delete from NotificationConfigurationsNotificationPurposes
                  where NotificationPurposeId = @notificationPurposeToMigrateId
            ");

            // 4. Drop NotificationPurposeId column
            Delete.ForeignKeyColumn("NotificationConfigurations", "NotificationPurposeID", "NotificationPurposes", "NotificationPurposeId");

            // 5. Notification Purposes - boy scout rule
            Rename.Column("NotificationPurposeID").OnTable("NotificationPurposes").To("Id");
            Rename.Column("ModuleID").OnTable("NotificationPurposes").To("ModuleId");

            // 6. Notification Configurations - boy scout rule
            Rename.Column("NotificationConfigurationID").OnTable("NotificationConfigurations").To("Id");
            Rename.Column("ContactID").OnTable("NotificationConfigurations").To("ContactId");
            Rename.Column("OperatingCenterID").OnTable("NotificationConfigurations").To("OperatingCenterId");

            // 7. Finally - let's delete the original notification purpose
            Execute.Sql(@"
                     -- figure out our id for the notification purpose being migrated
                declare @notificationPurposeThatWasMigratedId int
                 select @notificationPurposeThatWasMigratedId = Id  
                   from NotificationPurposes 
                  where Purpose = 'Environmental Permit Expiration';

                 delete from NotificationPurposes
                  where Id = @notificationPurposeThatWasMigratedId;
            ");

            // 8. Boy scout rule - modules does not have an FK to Application, so let's add one
            Alter.Table("Modules").AlterForeignKeyColumn("ApplicationID", "Applications", "ApplicationID", false);
        }

        public override void Down()
        {
            // 8. Boy scout rule - modules does not have an FK to Application, so let's add one
            Delete.ForeignKey("FK_Modules_Applications_ApplicationID").OnTable("Modules");

            // 7. Finally - let's delete the original notification purpose
            //      Nothing needed here, this is covered in step #3

            // 6. Notification Configurations - boy scout rule
            Rename.Column("OperatingCenterId").OnTable("NotificationConfigurations").To("OperatingCenterID");
            Rename.Column("ContactId").OnTable("NotificationConfigurations").To("ContactID");
            Rename.Column("Id").OnTable("NotificationConfigurations").To("NotificationConfigurationID");

            // 5. Notification Purposes - boy scout rule
            Rename.Column("ModuleId").OnTable("NotificationPurposes").To("ModuleID");
            Rename.Column("Id").OnTable("NotificationPurposes").To("NotificationPurposeID");

            // 4. Add NotificationPurposeId column
            Alter.Table("NotificationConfigurations")
                 .AddForeignKeyColumn("NotificationPurposeID", "NotificationPurposes", "NotificationPurposeID");

            // 3. Migrate all the data stuff (this was not fun to figure out, many cookies were eaten after completing it)
            Execute.Sql(@"
                      -- figure out our ids for application and modules that we need
                 declare @environmentalApplicationId int
                  select @environmentalApplicationId = ApplicationId 
                    from Applications 
                   where Name = 'Environmental';

                 declare @generalModuleId int
                  select @generalModuleId = ModuleId 
                    from Modules 
                   where ApplicationId = @environmentalApplicationId 
                     and Name = 'General';

                      -- insert the notification purpose that was previously deleted
                  insert into NotificationPurposes 
                  values (@generalModuleId, 'Environmental Permit Expiration');

                      -- get the id of the newly created notification purpose
                 declare @permitExpirationNotificationPurposeId int          
                  select @permitExpirationNotificationPurposeId = max(NotificationPurposeId) 
                    from NotificationPurposes;

                      -- revert notification configurations purpose id - for the specific environment permits
                  update NotificationConfigurations
                     set NotificationPurposeID = @permitExpirationNotificationPurposeId
                    from NotificationConfigurationsNotificationPurposes ncnp
                    join NotificationConfigurations nc
                      on ncnp.NotificationConfigurationId = nc.NotificationConfigurationID
                    join NotificationPurposes np
                      on np.NotificationPurposeId = ncnp.NotificationPurposeId
                   where np.ModuleId = 93;

                      -- revert notification configurations purpose id - for all other module types
                  update NotificationConfigurations
                     set NotificationPurposeID = ncnp.NotificationPurposeId
                    from NotificationConfigurationsNotificationPurposes ncnp
                    join NotificationConfigurations nc
                      on ncnp.NotificationConfigurationId = nc.NotificationConfigurationID
                      join NotificationPurposes np
                        on np.NotificationPurposeId = ncnp.NotificationPurposeId
                     where np.ModuleId <> 93;
            
                      -- delete all the records in the n-n table
                  delete from NotificationConfigurationsNotificationPurposes;

                      -- update any notification purposes that were the old 'permit types' module back to the 'general' module
                  update NotificationPurposes
                     set ModuleID = @generalModuleId
                    from NotificationConfigurations nc
                    join NotificationPurposes np
                      on nc.NotificationPurposeID = np.NotificationPurposeID
                   where np.ModuleId = 93;

                      -- delete any of the notification purposes that were from the new module and no longer needed
                  delete from NotificationPurposes
                   where ModuleId = 93;
                 ");

            // 2. Module
            this.DeleteModuleAndAssociatedRoles("Environmental", "Permit Types");

            // 1. Many to many table
            Delete.Table("NotificationConfigurationsNotificationPurposes");
        }
    }
}

