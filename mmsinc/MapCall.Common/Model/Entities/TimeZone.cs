using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TimeZone : IEntity
    {
        #region Constants

        public struct Incides
        {
            public const int EST = 1, CST = 2, MST = 3, PST = 4, HST = 5;
        }

        public struct StringLengths
        {
            #region Constants

            public const int CODE = 5, DESCRIPTION = 255;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Zone { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal UTCOffSet { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{Zone} {Description} ({UTCOffSet})";
        }
    }
}
