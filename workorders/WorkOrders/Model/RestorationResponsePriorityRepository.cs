using System.Linq;

namespace WorkOrders.Model
{
    public class RestorationResponsePriorityRepository : WorkOrdersRepository<RestorationResponsePriority>
    {
        #region Constants

        public struct Indices
        {
            public const short STANDARD = 7;
        }

        public struct Descriptions
        {
            public const string STANDARD = "Standard (30 days)";
        }

        #endregion

        #region Private Static Members

        private static RestorationResponsePriority _standard;

        #endregion

        #region Static Properties

        public static RestorationResponsePriority Standard
        {
            get
            {
                _standard = RetrieveAndAttach(Indices.STANDARD);
                return _standard;
            }
        }

        #endregion

        #region Private Static Methods

        private static RestorationResponsePriority RetrieveAndAttach(int index)
        {
            var priority = GetEntity(index);
            if (!DataTable.Contains(priority))
            {
                DataTable.Attach(priority);
            }
            return priority;
        }

        #endregion
    }
}