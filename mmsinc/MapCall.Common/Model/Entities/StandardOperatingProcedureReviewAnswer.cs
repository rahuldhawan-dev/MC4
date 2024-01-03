using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StandardOperatingProcedureReviewAnswer : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Answer { get; set; }
        public virtual StandardOperatingProcedureQuestion Question { get; set; }
        public virtual StandardOperatingProcedureReview Review { get; set; }

        /// <summary>
        /// If this is null then the answer is not considered as correct OR incorrect.
        /// </summary>
        public virtual bool? IsCorrect { get; set; }

        #endregion
    }
}
