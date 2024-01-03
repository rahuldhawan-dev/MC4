using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterChangeOutContract : IEntityWithCreationTimeTracking
    {
        #region Consts

        public struct StringLengths
        {
            public const int DESCRIPTION = 255; // 255 cause it's generally a file name.
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual IList<MeterChangeOut> MeterChangeOuts { get; set; }

        #endregion

        #region Constructor

        public MeterChangeOutContract()
        {
            MeterChangeOuts = new List<MeterChangeOut>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
