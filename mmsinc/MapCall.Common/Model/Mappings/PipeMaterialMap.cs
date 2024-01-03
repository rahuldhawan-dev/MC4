using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PipeMaterialMap : ClassMap<PipeMaterial>
    {
        #region Constructors

        public PipeMaterialMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.RecurringProjects).KeyColumn("ProposedPipeMaterialID");
        }

        #endregion
    }
}
