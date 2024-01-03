using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterType : EntityLookup
    {
        #region Constants

        public const int DESCRIPTION_LENGTH = 50;

        #endregion

        #region Properties

        [Required, StringLength(DESCRIPTION_LENGTH)]
        public override string Description
        {
            get => base.Description;
            set => base.Description = value;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
