using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OverallSafetyRatingMap : ClassMap<OverallSafetyRating>
    {
        #region Constructors

        public OverallSafetyRatingMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
