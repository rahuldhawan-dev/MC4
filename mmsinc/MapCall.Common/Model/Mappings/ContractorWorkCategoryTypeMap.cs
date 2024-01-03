using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorWorkCategoryTypeMap : ClassMap<ContractorWorkCategoryType>
    {
        #region Constructors

        public ContractorWorkCategoryTypeMap()
        {
            Id(x => x.Id, "ContractorWorkCategoryTypeID");

            Map(x => x.Description, "Name");
        }

        #endregion
    }
}
