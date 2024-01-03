using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProcessStageMap : ClassMap<ProcessStage>
    {
        #region Constructors

        public ProcessStageMap()
        {
            Id(x => x.Id, "ProcessStageID");

            Map(x => x.Description);
        }

        #endregion
    }
}
