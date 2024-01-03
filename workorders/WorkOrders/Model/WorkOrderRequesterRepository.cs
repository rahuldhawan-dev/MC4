using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving WorkOrderRequester objects from persistence.
    /// </summary>
    public class WorkOrderRequesterRepository : WorkOrdersRepository<WorkOrderRequester>
    {
        #region Constants

        public struct Indices
        {
            public const short CUSTOMER = 1,
                               EMPLOYEE = 2,
                               LOCAL_GOVERNMENT = 3,
                               CALL_CENTER = 4,
                               FRCC = 5,
                               ECHOLOGICS = 6;
        }
        public struct Descriptions
        {
            public const string CUSTOMER = "Customer",
                                EMPLOYEE = "Employee",
                                LOCAL_GOVERNMENT = "Local Government",
                                CALL_CENTER = "Call Center";
        }

        #endregion

        #region Private Static Members

        private static WorkOrderRequester _customer,
                                          _employee,
                                          _localGovernment,
                                          _callCenter;

        #endregion

        #region Static Properties

        public static WorkOrderRequester Customer
        {
            get
            {
                _customer = RetrieveAndAttach(Indices.CUSTOMER);
                return _customer;
            }
        }

        public static WorkOrderRequester Employee
        {
            get
            {
                _employee = RetrieveAndAttach(Indices.EMPLOYEE);
                return _employee;
            }
        }

        public static WorkOrderRequester LocalGovernment
        {
            get
            {
                _localGovernment = RetrieveAndAttach(Indices.LOCAL_GOVERNMENT);
                return _localGovernment;
            }
        }

        public static WorkOrderRequester CallCenter
        {
            get
            {
                _callCenter = RetrieveAndAttach(Indices.CALL_CENTER);
                return _callCenter;
            }
        }

        #endregion

        #region Private Static Methods

        private static WorkOrderRequester RetrieveAndAttach(int index)
        {
            var requester = GetEntity(index);
            if (!DataTable.Contains(requester))
                DataTable.Attach(requester);
            return requester;
        }

        #endregion
    }
}