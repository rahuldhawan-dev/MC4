using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    ///     Entity which mirrors values returned from the Water Loss Management Report.
    /// </summary>
    [Serializable]
    public class NonRevenueWaterDetail : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual NonRevenueWaterEntry NonRevenueWaterEntry { get; set; }

        public virtual string Month { get; set; }

        public virtual string Year { get; set; }

        public virtual string BusinessUnit { get; set; }

        public virtual string WorkDescription { get; set; }

        public virtual long TotalGallons { get; set; }

        #endregion
    }
}
