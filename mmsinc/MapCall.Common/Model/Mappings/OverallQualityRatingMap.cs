using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OverallQualityRatingMap : ClassMap<OverallQualityRating>
    {
        #region Constructors

        public OverallQualityRatingMap()
        {
            Id(x => x.Id, "Id");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
