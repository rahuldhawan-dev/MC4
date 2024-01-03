
namespace WorkOrders.Library
{
    public static class Utilities
    {
        #region Constants

        /// <summary>
        /// EmployeeNumber of the employee record for the MapCallAdmin (McAdmin)
        /// user for MapCall.
        /// </summary>
        public const string DEFAULT_USER_EMPLOYEE_NUMBER = "00000000";
        private const int DEFAULT_OPERATING_CENTER_ID = 14;
        private const int DEFAULT_EMPLOYEE_ID = 1;
        private const string USER_ROLE_FORMAT = "{0}_Field Services_Work Management_User Administrator";
        #endregion

        #region Exposed Methods

        /// <summary>
        /// Uses the provided IPrincipal User object to derive the Employee Number
        /// of the currently logged in user, and then uses that to get the EmployeeID
        /// (RecID on that table).
        /// </summary>
        /// <param name="user">IPrincipal object, probably from the current page.</param>
        /// <returns>An Employee object representing the user's record in the
        /// Employees/Logins table (tblPermissions).</returns>
        //internal static Employee GetEmployeeFromPageUser(IUser user)
        //{
        //    var pb = ProfileBase.Create(user.Identity.Name);

        //    return
        //        GetEmployeeFromEmployeeNumber(
        //            new WebProfile(pb).EmployeeNumber);
        //}

        /// <summary>
        /// Uses the provided string to search for an Employee in the Employees table
        /// by their AWW EmployeeNumber.  Throws an ArgumentOutOfRange exception if the given
        /// EmployeeNumber is not found.
        /// </summary>
        /// <param name="employeeNumber">EmployeeNumber to search for.</param>
        /// <returns>An Employee object representing the user's record in the
        /// Employees/Logins table (tblPermissions).</returns>
        //internal static Employee GetEmployeeFromEmployeeNumber(string employeeNumber)
        //{
        //    #if DEBUG
        //    return new Employee {
        //        EmployeeID = DEFAULT_EMPLOYEE_ID,
        //        EmpNum = employeeNumber
        //    };
        //    #else
        //    var e = EmployeeRepository.GetEmployeeByNumber(employeeNumber);

        //    if (e == null)
        //        throw new System.ArgumentOutOfRangeException("employeeNumber",
        //           "Could not retrieve Employee by Employee number: " +
        //           employeeNumber);

        //    return e;
        //    #endif
        //}

//        public static bool UserIsInOperatingCenter(IUser user, OperatingCenter cntr)
//        {
////#if DEBUG
////            return true;
////#else
//            var emp = GetEmployeeFromPageUser(user);
//            return emp.DefaultOperatingCenter == cntr;
////#endif
//        }
        
        // TODO: Replace this with something similar to above.
        //public static int GetCurrentOperatingCenterID()
        //{
        //    return DEFAULT_OPERATING_CENTER_ID;
        //}

        #endregion
    }
}
