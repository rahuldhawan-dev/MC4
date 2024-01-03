using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorFunctionalAreaTypeMap : ClassMap<ContractorFunctionalAreaType>
    {
        #region Constructors

        public ContractorFunctionalAreaTypeMap()
        {
            Id(x => x.Id, "ContractorFunctionalAreaTypeID");

            Map(x => x.Description);
        }

        #endregion
    }
}
