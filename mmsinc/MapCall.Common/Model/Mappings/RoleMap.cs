using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        #region Constructors

        public RoleMap()
        {
            Id(x => x.Id, "RoleID");

            References(x => x.User)
               .Not.Nullable();
            References(x => x.Application)
               .Not.Nullable();
            References(x => x.Module)
               .Not.Nullable();
            References(x => x.OperatingCenter)
               .Nullable();
            References(x => x.Action)
               .Not.Nullable();
        }

        #endregion
    }
}
