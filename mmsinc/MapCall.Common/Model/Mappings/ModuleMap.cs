using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ModuleMap : ClassMap<Module>
    {
        #region Constructors

        public ModuleMap()
        {
            Id(x => x.Id, "ModuleID").GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable().Unique();

            References(x => x.Application).Not.Nullable();
        }

        #endregion
    }
}
