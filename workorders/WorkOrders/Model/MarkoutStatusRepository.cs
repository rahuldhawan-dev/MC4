
using System.Linq;

namespace WorkOrders.Model
{
    public class MarkoutStatusRepository : WorkOrdersRepository<MarkoutStatus>
    {
        #region Constants

        public struct Indices
        {
            public const short PENDING = 1,
                               READY = 2,
                               OVERDUE = 3;
        }

        public struct Descriptions
        {
            public const string PENDING = "Pending",
                                READY = "Ready",
                                OVERDUE = "Overdue";
        }

        #endregion

        #region Private Static Members

        private static MarkoutStatus _pending,
                                     _ready,
                                     _overdue;

        #endregion

        #region Static Properties

        public static MarkoutStatus Pending
        {
            get
            {
                _pending = RetrieveAndAttach(Indices.PENDING);
                return _pending;
            }
        }

        public static MarkoutStatus Ready
        {
            get
            {
                _ready = RetrieveAndAttach(Indices.READY);
                return _ready;
            }
        }

        public static MarkoutStatus Overdue
        {
            get
            {
                _overdue = RetrieveAndAttach(Indices.OVERDUE);
                return _overdue;
            }
        }

        #endregion

        #region Private Static Methods

        private static MarkoutStatus RetrieveAndAttach(int index)
        {
            var status = GetEntity(index);
            if (!Enumerable.Contains(DataTable, status))
                DataTable.Attach(status);
            return status;
        }

        #endregion
    }
}
