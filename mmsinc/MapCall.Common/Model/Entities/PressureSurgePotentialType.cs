using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PressureSurgePotentialType : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int DESCRIPTION = 100;
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        #endregion
    }
}
