using System.Linq;

namespace WorkOrders.Model
{
    public class LeakReportingSourceRepository : WorkOrdersRepository<LeakReportingSource>
    {
        #region Contstants

        public struct Indices
        {
            public const short FIELD_SERVICE_REP = 1,
                               MLOG = 2,
                               SURVEY = 3,
                               VILADE = 4;
        }

        public struct Descriptions
        {
            public const string FIELD_SERVICE_REP = "Field Service Rep.",
                                MLOG = "MLOG",
                                SURVEY = "Survey",
                                VILADE = "VILADE";
        }

        #endregion

        #region Private Static Members

        private static LeakReportingSource _fieldServiceRep,
                                           _mLOG,
                                           _survey,
                                           _vILADE;

        #endregion

        #region Static Properties

        public static LeakReportingSource FieldServiceRep
        {
            get
            {
                _fieldServiceRep = RetrieveAndAttach(Indices.FIELD_SERVICE_REP);
                return _fieldServiceRep;
            }
        }

        public static LeakReportingSource MLOG
        {
            get
            {
                _mLOG = RetrieveAndAttach(Indices.MLOG);
                return _mLOG;
            }
        }

        public static LeakReportingSource Survey
        {
            get
            {
                _survey = RetrieveAndAttach(Indices.SURVEY);
                return _survey;
            }
        }

        public static LeakReportingSource VILADE
        {
            get
            {
                _vILADE = RetrieveAndAttach(Indices.VILADE);
                return _vILADE;
            }
        }

        #endregion

        #region Private Static Methods

        private static LeakReportingSource RetrieveAndAttach(int index)
        {
            var source = GetEntity(index);
            if (!DataTable.Contains(source))
                DataTable.Attach(source);
            return source;
        }

        #endregion
    }
}