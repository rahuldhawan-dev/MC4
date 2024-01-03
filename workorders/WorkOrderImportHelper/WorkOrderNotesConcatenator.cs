using System;
using System.Linq;
using System.Text;
using WorkOrderImportHelper.Model;

namespace WorkOrderImportHelper
{
    public class WorkOrderNotesConcatenator : WorkOrderProcessor
    {
        #region Constants

        private struct StatusStrings
        {
            public const string NEW_NOTES =
                "New notes for WorkOrder # {0}:\r\n{1}";
        }

        private struct StringFormats
        {
            public const string EMPLOYEE_NUMBER = "Employee # {0} ";
        }

        #endregion

        #region Private Members

        private WorkOrdersImportDataContext _dataContext;
        private IQueryable<tblWorkInputTable> _workOrders;

        #endregion

        #region Properties

        public WorkOrdersImportDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new WorkOrdersImportDataContext();
                return _dataContext;
            }
        }

        public IQueryable<tblWorkInputTable> WorkOrders
        {
            get
            {
                if (_workOrders == null)
                    _workOrders = GetWorkOrdersWithEmployeeWorkOrders();
                return _workOrders;
            }
        }

        #endregion

        #region Constructors

        public WorkOrderNotesConcatenator(Action<int> displayCurrentWorkOrderID, Action<string> outputFn)
            : base(displayCurrentWorkOrderID, outputFn)
        {
        }

        #endregion

        #region Private Methods

        private IQueryable<tblWorkInputTable> GetWorkOrdersWithEmployeeWorkOrders()
        {
            return (from wo in DataContext.tblWorkInputTables
                    where wo.tblEmployeeWorkOrders.Count > 0
                    orderby wo.Order_Number
                    select wo);
        }

        private string CompileNotes(tblEmployeeWorkOrder emp)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(emp.Employee_Assigned_to_Job))
                sb.AppendFormat(StringFormats.EMPLOYEE_NUMBER,
                    emp.Employee_Assigned_to_Job);

            if (emp.Date_Assigned != null)
                sb.Append(emp.Date_Assigned + " ");

            if (!string.IsNullOrEmpty(emp.Job_Notes))
                sb.AppendLine(emp.Job_Notes);

            return sb.ToString();
        }

        #endregion

        #region Exposed Methods

        override public int LoadWorkOrders()
        {
            return WorkOrders.Count();
        }

        public void ProcessNotes()
        {
            foreach (var order in WorkOrders)
            {
                _displayCurrentWorkOrderID(order.Order_Number);

                // prevent any null errors when concatenating later
                order.Notes = (string.IsNullOrEmpty(order.Notes)) ?
                    string.Empty : order.Notes + "\r\n";

                var employeeWorkOrders =
                    (from emp in order.tblEmployeeWorkOrders
                     orderby emp.Date_Assigned
                     select emp);

                foreach (var emp in employeeWorkOrders)
                {
                    order.Notes += CompileNotes(emp);
                }

                _outputFn(string.Format(StatusStrings.NEW_NOTES,
                    order.Order_Number, order.Notes));

                DataContext.SubmitChanges();
            }
        }

        #endregion
    }
}
