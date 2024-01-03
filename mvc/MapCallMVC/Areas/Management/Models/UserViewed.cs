using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Management.Models
{
    public class SearchUserViewed : SearchSet<UserViewed>
    {
        #region Properties

        [Required]
        public RequiredDateRange ViewedAt { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(User))]
        public int? User { get; set; }

        #endregion
    }

    public class SearchUserViewedDailyRecordItem : SearchSet<UserViewedDailyRecordItem>, ISearchUserViewedDailyRecordItem
    {
        #region Properties

        public RequiredDateRange ViewedAt { get; set; }
        public string UserAddress { get; set; }

        #endregion
    }
}