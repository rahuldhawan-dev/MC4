using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SapCommunicationStatusMap : ClassMap<SapCommunicationStatus>
    {
        #region Constants

        public const string TABLE_NAME = "SAPCommunicationStatuses";

        #endregion

        #region Constructors

        public SapCommunicationStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Description).Not.Nullable().Length(50);
        }

        #endregion
    }
}
