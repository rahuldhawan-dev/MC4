using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class UserViewedDailyRecordItem
    {
        #region Properties

        [View(FormatStyle.Date)]
        public DateTime ViewedAt { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserAddress { get; set; }
        public string EmployeeId { get; set; }
        public int TapImages { get; set; }
        public int ValveImages { get; set; }
        public int AsBuiltImages { get; set; }

        #endregion
    }

    public interface ISearchUserViewedDailyRecordItem : ISearchSet<UserViewedDailyRecordItem>
    {
        RequiredDateRange ViewedAt { get; set; }

        [SearchAlias("User", "user", "Address")]
        string UserAddress { get; set; }
    }
}
