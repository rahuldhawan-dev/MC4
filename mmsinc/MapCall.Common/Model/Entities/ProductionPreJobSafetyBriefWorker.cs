using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionPreJobSafetyBriefWorker : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int CONTRACTOR = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ProductionPreJobSafetyBrief ProductionPreJobSafetyBrief { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual string Contractor { get; set; }
        public virtual DateTime SignedAt { get; set; }

        #endregion
    }
}
