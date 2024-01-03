using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving Employee objects from persistence.
    /// </summary>
    public class EmployeeRepository : WorkOrdersRepository<Employee>, IEmployeeRepository
    {
        #region Exposed Static Methods

        public static Employee GetEmployeeByNumber(string employeeNumber )
        {
            return
                (from e in DataTable where e.EmpNum == employeeNumber select e).
                    FirstOrDefault();
        }

        public static Employee SelectEmployeeByUserName(string userName)
        {
            return
                (from e in DataTable where e.UserName == userName select e).
                    FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the employee in the repository with the given name.
        /// </summary>
        /// <param name="name">Name of the employee to search for.</param>
        /// <returns>The employee with the given name, if found.</returns>
        public static Employee SelectByName(string name)
        {
            return
                (from e in DataTable where e.FullName == name select e).
                    FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all of the employee objects from persistence.
        /// </summary>
        /// <returns>All of the employee objects currently stored.</returns>
        public static new List<Employee> SelectAllAsList()
        {
            return SelectAllAsList(0);
        }

        /// <summary>
        /// Retrieves all of the employee objects from persistence, limited
        /// to a given number.
        /// </summary>
        /// <param name="count">The number of employee objects to bring back.
        /// If 0, all employee objects will be retrieved.</param>
        /// <returns>A List of employee objects.</returns>
        public static List<Employee> SelectAllAsList(int count)
        {
            if (count <= 0)
                return
                    (from e in DataTable
                     where (e.EmpNum != null && e.EmpNum != String.Empty)
                     orderby e.FullName
                     select e).ToList();
            return
                (from e in DataTable
                 where (e.EmpNum != null && e.EmpNum != String.Empty)
                 orderby e.FullName
                 select e).Take(count).
                    ToList();
        }

        public static IEnumerable<Employee> GetEmployeesByOperatingCenterID(int operatingCenterID)
        {
            var opCode = OperatingCenterRepository.GetOpCodeByOperatingCenterID(
                    operatingCenterID);
            return GetEmployeesByOperatingCenterText(opCode);
        }

        public static IEnumerable<Employee> GetEmployeesByOperatingCenterText(string opCode)
        {
            return GetEmployeesByOperatingCenter(
                OperatingCenterRepository.GetOperatingCenterByOpCntr(opCode));
        }

        public static IEnumerable<Employee> GetEmployeesByOperatingCenter(OperatingCenter center)
        {
            return (from e in DataTable
                where e.DefaultOperatingCenter.OperatingCenterID ==
                      center.OperatingCenterID ||
                      e.DefaultOperatingCenter.OperatedByOperatingCenter
                       .OperatingCenterID == center.OperatingCenterID ||
                      e.AggregateRoles.Any(r =>
                          r.ModuleId == (int)RoleModules
                             .FieldServicesWorkManagement &&
                          (r.OperatingCenterId == null ||
                           r.OperatingCenterId ==
                           center.OperatingCenterID))
                orderby e.FullName
                select e);
        }

        public static IEnumerable GetEmployeeCounts(int employeeID, int operatingCenterID, DateTime dateStart, DateTime dateEnd)
        {
            var opcntrs =
                OperatingCenterRepository.SelectAll271OperatingCenters().Where(o =>
                    (operatingCenterID > 0 && employeeID == 0)
                        ? o.OperatingCenterID == operatingCenterID : true);
            
            foreach (var opcntr in opcntrs)
            {
                var employees = GetEmployeesByOperatingCenterText(opcntr.OpCntr).Where(e =>
                              (employeeID > 0)
                                  ? e.EmployeeID == employeeID : true);

                foreach (var employee in employees)
                {
                    yield return new {
                        Employee = employee,
                        Created = employee.CreatedWorkOrders.Count(wo =>
                            wo.CreatedOn >= dateStart &&
                            wo.CreatedOn < dateEnd &&
                            wo.OperatingCenterID == opcntr.OperatingCenterID
                            ),
                        Completed = employee.WorkOrdersCompleted.Count(wo =>
                            wo.DateCompleted >= dateStart &&
                            wo.DateCompleted < dateEnd &&
                            wo.OperatingCenterID == opcntr.OperatingCenterID
                            ),
                        OperatingCenter = opcntr
                    };
                }
            }
        }
    

        /// <summary>
        /// Retrieves the employee objects in the repository whose name
        /// contains the given string.
        /// </summary>
        /// <param name="namePart">Part of the employee name to search for.
        /// </param>
        /// <returns>A List of the employee objects found.</returns>
        public static List<Employee> SelectByNamePart(string namePart)
        {
            return SelectByNamePart(namePart, 0);
        }

        /// <summary>
        /// Retrieves the employee objects in the repository whose name
        /// the given string, limited to a count.
        /// </summary>
        /// <param name="namePart">Part of the employee name to search for.
        /// </param>
        /// <param name="count">The number of employee objects to bring back.
        /// If 0, all employee objects whose name matched will be retrieved.</param>
        /// <returns>A List of the employee objects found.</returns>
        public static List<Employee> SelectByNamePart(string namePart, int count)
        {
            if (String.IsNullOrEmpty(namePart))
                return SelectAllAsList(count);
            namePart = namePart.ToLower();
            if (count <= 0)
                return
                    (from e in DataTable
                     orderby e.FullName
                     where e.FullName.ToLower().Contains(namePart) select e).ToList();
            return
                (from e in DataTable
                 orderby e.FullName
                 where e.FullName.ToLower().Contains(namePart)
                 select e).Take(count).ToList();
        }

        public static IEnumerable<string> SelectAllCompanies()
        {
            return (from e in DataTable
                    where e.Company != null && e.Company != String.Empty
                    select e.Company).Distinct().OrderBy(s => s);
        }

        #endregion

        #region Exposed Methods

        public Employee GetEmployeeByUserName(string userName)
        {
            return SelectEmployeeByUserName(userName);
        }

        #endregion
    }

    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetEmployeeByUserName(string userName);
    }
}
