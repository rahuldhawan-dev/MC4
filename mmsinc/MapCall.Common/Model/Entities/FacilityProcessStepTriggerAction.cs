using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityProcessStepTriggerAction : IEntity
    {
        #region Consts

        public const int MAX_ACTION_LENGTH = 255,
                         MAX_ACTION_RESPONSE_LENGTH = 255;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual FacilityProcessStepTrigger Trigger { get; set; }
        public virtual int Sequence { get; set; }
        public virtual string Action { get; set; }
        public virtual string ActionResponse { get; set; }

        #endregion
    }
}
