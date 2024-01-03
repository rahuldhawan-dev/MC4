using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SeriousInjuryOrFatalityTypeMap : EntityLookupMap<SeriousInjuryOrFatalityType>
    {
        #region Constructors

        public SeriousInjuryOrFatalityTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }

        #endregion
    }
}
