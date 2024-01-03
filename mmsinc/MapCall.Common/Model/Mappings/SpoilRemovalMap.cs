using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SpoilRemovalMap : ClassMap<SpoilRemoval>
    {
        #region Constructors

        public SpoilRemovalMap()
        {
            Id(x => x.Id, "SpoilRemovalID");

            Map(x => x.DateRemoved);
            Map(x => x.Quantity);

            References(x => x.FinalDestination);
            References(x => x.RemovedFrom);
        }

        #endregion
    }
}
