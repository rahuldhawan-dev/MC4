using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilitySubArea : IEntity
    {
        #region Constants

        public const int MAX_DESCRIPTION_LENGTH = 100;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual FacilityArea Area { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
