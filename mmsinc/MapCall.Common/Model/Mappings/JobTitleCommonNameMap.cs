using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobTitleCommonNameMap : ClassMap<JobTitleCommonName>
    {
        #region Constructors

        public JobTitleCommonNameMap()
        {
            Id(x => x.Id, "JobTitleCommonNameID");

            Map(x => x.Description);
        }

        #endregion
    }
}
