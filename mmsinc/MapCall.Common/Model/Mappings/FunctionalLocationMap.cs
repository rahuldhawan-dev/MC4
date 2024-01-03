using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FunctionalLocationMap : ClassMap<FunctionalLocation>
    {
        #region Constructors

        public FunctionalLocationMap()
        {
            Id(x => x.Id, "FunctionalLocationID");

            Map(x => x.Description).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();

            References(x => x.Town);
            References(x => x.AssetType);
        }

        #endregion
    }
}
