using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EchoshoreSite : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int DESCRIPTION = 255;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual Town Town { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{OperatingCenter.OperatingCenterCode} - {Town.ShortName} - {Description}";
        }
    }
}
