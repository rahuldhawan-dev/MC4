using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FEMAFloodRatingMap : ClassMap<FEMAFloodRating>
    {
        #region Constructors

        public FEMAFloodRatingMap()
        {
            Id(x => x.Id, "FEMAFloodRatingID");

            Map(x => x.Description);

            HasMany(x => x.Facilities)
               .KeyColumn("FEMAFloodRatingID").LazyLoad();
        }

        #endregion
    }
}
