using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterSize : EntityLookup
    {
        #region Constants

        public const int SIZE_LENGTH = 10;

        #endregion

        #region Properties

        [StringLength(SIZE_LENGTH)]
        public virtual string Size { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Size;
        }

        #endregion
    }
}
