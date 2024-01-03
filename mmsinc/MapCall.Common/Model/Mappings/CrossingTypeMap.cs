using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CrossingTypeMap : ClassMap<CrossingType>
    {
        #region Constructors

        public CrossingTypeMap()
        {
            Id(x => x.Id, "CrossingTypeID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
