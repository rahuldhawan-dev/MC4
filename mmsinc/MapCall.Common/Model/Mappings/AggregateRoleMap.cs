using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AggregateRoleMap : ClassMap<AggregateRole>
    {
        public AggregateRoleMap()
        {
            ReadOnly();
            Table("AggregateRoles");

            // "Hey Ross, why aren't we mapping this as an NHibernate composite id
            // and/or why are we generating the composite id as part of the view?"
            // Because our actual composite key consists of nullable values, and because
            // they're nullable NHibernate just doesn't like it. It doesn't error. It just
            // doesn't return any objects even though the query itself returns the expected
            // results.
            // The only other option is to add three extra copies of the UserRoleId, UserId, and 
            // GroupRoleId values that can be mapped just as integers(rather than references)
            // which does work, but means we're returning duplicate data. No, you can't use
            // the existing column ids as Nhibernate will throw an error with trying to add
            // duplicate column names to its sql builder thing. -Ross 4/11/2023
            Id(x => x.CompositeId, "CompositeId");
            References(x => x.Action).Not.Nullable().Not.Insert().Not.Update();
            References(x => x.Module).Not.Nullable().Not.Insert().Not.Update();
            References(x => x.OperatingCenter).Nullable().Not.Insert().Not.Update();
            References(x => x.UserRole).Nullable().Not.Insert().Not.Update();
            References(x => x.RoleGroup).Nullable().Not.Insert().Not.Update();
            References(x => x.RoleGroupRole).Nullable().Not.Insert().Not.Update();
            References(x => x.User).Not.Nullable().Not.Insert().Not.Update();

            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
