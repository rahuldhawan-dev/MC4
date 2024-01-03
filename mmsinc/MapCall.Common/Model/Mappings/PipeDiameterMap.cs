using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PipeDiameterMap : ClassMap<PipeDiameter>
    {
        #region Constructors

        public PipeDiameterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Diameter).Not.Nullable();

            HasMany(x => x.RecurringProjects).KeyColumn("ProposedDiameterID");
        }

        #endregion
    }
}
