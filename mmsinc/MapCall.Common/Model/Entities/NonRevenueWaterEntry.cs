using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    ///     Entity for capturing Non-Revenue Water
    /// </summary>
    [Serializable]
    public class NonRevenueWaterEntry : IEntityWithChangeTracking<User>
    {
        #region Contants

        public struct DisplayName
        {
            public const string MONTH_NAME = "Month";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual bool HasBeenReportedToHyperion { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual User UpdatedBy { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual int? Year { get; set; }

        public virtual int? Month { get; set; }

        [DisplayName(DisplayName.MONTH_NAME)]
        public virtual string MonthName => DateTimeFormatInfo.CurrentInfo.GetMonthName(Month ?? 0);

        public virtual IList<NonRevenueWaterDetail> NonRevenueWaterDetails { get; set; } =
            new List<NonRevenueWaterDetail>();
        
        public virtual IList<NonRevenueWaterAdjustment> NonRevenueWaterAdjustments { get; set; } =
            new List<NonRevenueWaterAdjustment>();

        public virtual long NonRevenueWaterDetailSubTotal =>
            NonRevenueWaterDetails.Any()
                ? NonRevenueWaterDetails.Sum(x => x.TotalGallons)
                : 0;

        public virtual long NonRevenueWaterAdjustmentSubTotal =>
            NonRevenueWaterAdjustments.Any()
                ? NonRevenueWaterAdjustments.Sum(x => x.TotalGallons)
                : 0;

        public virtual long NonRevenueWaterTotal => NonRevenueWaterDetailSubTotal + NonRevenueWaterAdjustmentSubTotal;

        #endregion
    }
}
