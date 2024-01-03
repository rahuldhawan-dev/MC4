using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityInspectionFormQuestion : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Weightage { get; set; }
        public virtual FacilityInspectionFormQuestionCategory Category { get; set; }
        public virtual string Question { get; set; }
        public virtual int DisplayOrder { get; set; }
    }
}
