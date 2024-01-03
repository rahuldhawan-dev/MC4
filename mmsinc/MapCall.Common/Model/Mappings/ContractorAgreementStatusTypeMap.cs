using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorAgreementStatusTypeMap : ClassMap<ContractorAgreementStatusType>
    {
        #region Constructors

        public ContractorAgreementStatusTypeMap()
        {
            Id(x => x.Id, "ContractorAgreementStatusTypeID");

            Map(x => x.Description, "Name");
        }

        #endregion
    }
}
