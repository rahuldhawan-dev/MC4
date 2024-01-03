using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230419103228666), Tags("Production")]
    public class MC5496_CreateRoleGroups : Migration
    {
        public override void Up()
        {
            Create.Table("RoleGroups")
                  .WithIdentityColumn()
                  .WithColumn("Name").AsString(200).NotNullable().Unique();
            
            Create.Table("RoleGroupRoles")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("RoleGroupId", "RoleGroups", nullable: false)
                  .WithForeignKeyColumn("ModuleId", "Modules", "ModuleID", nullable: false)
                  .WithForeignKeyColumn("ActionId", "Actions", "ActionID", nullable: false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", nullable: true);

            Create.Table("RoleGroupsUsers")
                  .WithForeignKeyColumn("RoleGroupId", "RoleGroups", nullable: false)
                  .WithForeignKeyColumn("UserId", "tblPermissions", "RecId", nullable: false);

            Create.Index("IX_RoleGroupRoles_UniqueRoles").OnTable("RoleGroupRoles")
                  .WithOptions().Unique()
                  .OnColumn("RoleGroupId").Ascending()
                  .OnColumn("ModuleId").Ascending()
                  .OnColumn("ActionId").Ascending()
                  .OnColumn("OperatingCenterId").Ascending();

            Create.Index("IX_RoleGroupsUsers_UserId").OnTable("RoleGroupsUsers")
                  .OnColumn("UserId").Ascending();

            Execute.Sql(@"
create view dbo.[AggregateRoles]
as
select 
    concat(Roles.RoleId, '-') as CompositeId,
    Roles.RoleId as UserRoleId,
    null as RoleGroupId,
    null as RoleGroupRoleId,
    Roles.UserId as UserId, 
    Roles.ModuleId as ModuleId, 
    Roles.OperatingCenterId as OperatingCenterId, 
    Roles.ActionId as ActionId 
from Roles 
union
select
    concat(RoleGroupRoles.Id,'-', RoleGroupsUsers.UserId) as CompositeId,
    null as UserRoleId,
    RoleGroups.Id as RoleGroupId,
    RoleGroupRoles.Id as RoleGroupRoleId,
    RoleGroupsUsers.UserId as UserId,
    RoleGroupRoles.ModuleId as ModuleId,
    RoleGroupRoles.OperatingCenterId as OperatingCenterid,
    RoleGroupRoles.ActionId as ActionId
from RoleGroups 
inner join RoleGroupRoles on RoleGroupRoles.RoleGroupId = RoleGroups.Id
inner join RoleGroupsUsers on RoleGroupsUsers.RoleGroupId = RoleGroups.Id 
");
        }

        public override void Down()
        {
            Execute.Sql("DROP VIEW dbo.[AggregateRoles]");
            Delete.Index("IX_RoleGroupsUsers_UserId").OnTable("RoleGroupsUsers");
            Delete.Index("IX_RoleGroupRoles_UniqueRoles").OnTable("RoleGroupRoles");
            Delete.Table("RoleGroupsUsers");
            Delete.Table("RoleGroupRoles");
            Delete.Table("RoleGroups");
        }
    }
}

