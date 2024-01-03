using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorContactMap : ClassMap<ContractorContact>
    {
        #region Constructors

        public ContractorContactMap()
        {
            Id(x => x.Id, "ContractorContactID");

            References(x => x.Contractor);
            References(x => x.Contact);
            References(x => x.ContactType);
        }

        #endregion
    }
}
