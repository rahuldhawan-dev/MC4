using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CrashTypeMap : EntityLookupMap<CrashType>
    {
        #region Constructors

        public CrashTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }

        #endregion
    }
}
