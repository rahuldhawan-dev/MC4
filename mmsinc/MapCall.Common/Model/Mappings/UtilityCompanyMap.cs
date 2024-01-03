using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class UtilityCompanyMap : EntityLookupMap<UtilityCompany>
    {
        public const string TABLE_NAME = "UtilityCompanies";

        public UtilityCompanyMap()
        {
            Table(TABLE_NAME);
            //Id(x => x.Id).GeneratedBy.Assigned();
            //Map(x => x.Description).Length(UtilityCompany.StringLengths.DESCRIPTION).Not.Nullable();
            References(x => x.State);
        }
    }
}
