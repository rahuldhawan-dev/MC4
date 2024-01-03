using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConstructionTypeMap : ClassMap<ConstructionType>
    {
        #region Constructors

        public ConstructionTypeMap()
        {
            Id(x => x.Id, "ConstructionTypeID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
