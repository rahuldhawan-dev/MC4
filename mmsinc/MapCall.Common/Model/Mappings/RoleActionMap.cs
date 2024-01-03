using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RoleActionMap : ClassMap<RoleAction>
    {
        #region Constructors

        public RoleActionMap()
        {
            Table("Actions");
            Id(x => x.Id, "ActionID").GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable().Unique();
        }

        #endregion
    }
}
