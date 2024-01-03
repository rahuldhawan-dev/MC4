using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankInspectionQuestion : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual TankInspection TankInspection { get; set; }
        public virtual TankInspectionQuestionType TankInspectionQuestionType { get; set; }
        public virtual string ObservationAndComments { get; set; }
        public virtual bool? TankInspectionAnswer { get; set; }
        public virtual bool? RepairsNeeded { get; set; }
        public virtual DateTime? CorrectiveWoDateCreated { get; set; }
        public virtual DateTime? CorrectiveWoDateCompleted { get; set; }

        #endregion
    }
}