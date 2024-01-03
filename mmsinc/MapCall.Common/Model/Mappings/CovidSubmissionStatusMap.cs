using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CovidSubmissionStatusMap : EntityLookupMap<CovidSubmissionStatus>
    {
        public const string TABLE_NAME = "CovidSubmissionStatuses";

        public CovidSubmissionStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
