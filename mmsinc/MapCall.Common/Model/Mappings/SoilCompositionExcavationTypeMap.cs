using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SoilCompositionExcavationTypeMap : EntityLookupMap<SoilCompositionExcavationType>
    {
        #region Constructors

        public SoilCompositionExcavationTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
        }

        #endregion
    }
}
