using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SpoilFinalProcessingLocationMap : ClassMap<SpoilFinalProcessingLocation>
    {
        #region Constructors

        public SpoilFinalProcessingLocationMap()
        {
            Id(x => x.Id, "SpoilFinalProcessingLocationID");

            Map(x => x.Name);

            References(x => x.OperatingCenter);
            References(x => x.Town);
            References(x => x.Street);

            HasMany(x => x.SpoilRemovals).KeyColumn("FinalDestinationID");
        }

        #endregion
    }
}
