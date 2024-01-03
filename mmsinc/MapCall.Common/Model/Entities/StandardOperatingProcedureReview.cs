using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StandardOperatingProcedureReview : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual User AnsweredBy { get; set; }
        public virtual DateTime AnsweredAt { get; set; }
        public virtual User ReviewedBy { get; set; }
        public virtual DateTime? ReviewedAt { get; set; }
        public virtual IList<StandardOperatingProcedureReviewAnswer> Answers { get; set; }
        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }

        #endregion

        #region Constructor

        public StandardOperatingProcedureReview()
        {
            Answers = new List<StandardOperatingProcedureReviewAnswer>();
        }

        #endregion
    }
}
