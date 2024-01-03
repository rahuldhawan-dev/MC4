using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SoilConditionsWithinExcavationTypeMap : EntityLookupMap<SoilConditionsWithinExcavationType>
    {
        #region Constructors

        public SoilConditionsWithinExcavationTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
        }

        #endregion
    }
}
