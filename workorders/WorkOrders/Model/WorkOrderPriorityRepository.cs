using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving WorkOrderPriority objects from persistence.
    /// </summary>
    public class WorkOrderPriorityRepository : WorkOrdersRepository<WorkOrderPriority>
    {
        #region Constants

        public struct Indices
        {
            public const short EMERGENCY = 1,
                               HIGH_PRIORITY = 2,
                               ROUTINE = 4;
        }

        public struct Descriptions
        {
            public const string EMERGENCY = "Emergency",
                                HIGH_PRIORITY = "High Priority",
                                ROUTINE = "Routine";
        }

        #endregion

        #region Private Static Members

        private static WorkOrderPriority _emergency,
                                         _highPriority,
                                         _routine;

        #endregion

        #region Static Properties

        public static WorkOrderPriority Emergency
        {
            get
            {
                _emergency = RetrieveAndAttach(Indices.EMERGENCY);
                return _emergency;
            }
        }

        public static WorkOrderPriority HighPriority
        {
            get
            {
                _highPriority = RetrieveAndAttach(Indices.HIGH_PRIORITY);
                return _highPriority;
            }
        }

        public static WorkOrderPriority Routine
        {
            get
            {
                _routine = RetrieveAndAttach(Indices.ROUTINE);
                return _routine;
            }
        }

        #endregion

        #region Private Static Methods

        private static WorkOrderPriority RetrieveAndAttach(int index)
        {
            var priority = GetEntity(index);
            if (!DataTable.Contains(priority))
                DataTable.Attach(priority);
            return priority;
        }

        #endregion
    }
}