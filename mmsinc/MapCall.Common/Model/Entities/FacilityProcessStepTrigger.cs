using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityProcessStepTrigger : IEntity
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = 255;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Multiline]
        public virtual string Description { get; set; }

        public virtual FacilityProcessStep FacilityProcessStep { get; set; }
        public virtual int Sequence { get; set; }
        public virtual FacilityProcessStepTriggerType TriggerType { get; set; }
        public virtual FacilityProcessStepTriggerLevel TriggerLevel { get; set; }
        public virtual FacilityProcessStepAlarm Alarm { get; set; }

        public virtual IList<FacilityProcessStepTriggerAction> Actions { get; set; }

        #endregion

        #region Constructors

        public FacilityProcessStepTrigger()
        {
            Actions = new List<FacilityProcessStepTriggerAction>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Sequence + " - " + Description;
        }

        #endregion
    }
}
