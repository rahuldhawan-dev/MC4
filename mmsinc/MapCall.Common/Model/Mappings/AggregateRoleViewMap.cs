using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class AggregateRoleViewMap : AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(Dialect dialect, NHibernate.Engine.IMapping p, string defaultCatalog, string defaultSchema)
        {
            // NOTE: Be wary if you change this in another migration. The formatting for
            // concat is entirely different in sqlite than it is in sql server.
            var viewDef = @"
select 
    Roles.RoleId || '-' as CompositeId,
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
    RoleGroupRoles.Id || '-' || RoleGroupsUsers.UserId as CompositeId,
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
                ";

            return $"CREATE VIEW [AggregateRoles] AS {viewDef.ToSqlite()}";
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return "DROP VIEW [AggregateRoles]";
        }
    }
}
