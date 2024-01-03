using System;
using System.ComponentModel;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityInspectionFormAnswer : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual ApcInspectionItem ApcInspectionItem { get; set; }

        public virtual FacilityInspectionFormQuestion FacilityInspectionFormQuestion { get; set; }

        public virtual bool? IsSafe { get; set; }

        public virtual bool? IsPictureTaken { get; set; }

        public virtual string Comments { get; set; }

        #endregion

        #region Constructors

        public FacilityInspectionFormAnswer() { }

        #endregion
    }
}
