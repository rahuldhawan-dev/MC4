using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    ///     Entity captures any adjustments needed to supplement the total derived from the Water Loss Management Report
    /// </summary>
    [Serializable]
    public class NonRevenueWaterAdjustment : IEntity
    {
        #region Contants

        public struct StringLengths
        {
            public const int COMMENTS = 100;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual NonRevenueWaterEntry NonRevenueWaterEntry { get; set; }
        
        public virtual string BusinessUnit { get; set; }

        public virtual long TotalGallons { get; set; }

        public virtual string Comments { get; set; }

        #endregion
    }
}
