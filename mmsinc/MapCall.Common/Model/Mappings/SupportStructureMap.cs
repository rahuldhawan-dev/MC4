using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SupportStructureMap : ClassMap<SupportStructure>
    {
        #region Constructors

        public SupportStructureMap()
        {
            Id(x => x.Id, "SupportStructureID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
