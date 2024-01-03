using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels.Users;

namespace MapCallMVC.Models.ViewModels.Users
{
    public class SearchUser : SearchSet<User>, ISearchUser
    {
        #region Properties

        public SearchString FullName { get; set; }

        // Business is used to LastName being an exact string match, so leave this as string instead of SearchString.
        public string LastName { get; set; }
        public SearchString UserName { get; set; }
        [View("Employee ID")]
        public string EmployeeId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? DefaultOperatingCenter { get; set; }
        public SearchString Address { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsUserAdmin { get; set; }
        public bool? HasAccess { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(UserType))]
        public int? UserType { get; set; }

        #endregion
    }
}