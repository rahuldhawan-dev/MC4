using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankInspectionQuestionType : ReadOnlyEntityLookup
    {
        #region Properties

        public virtual TankInspectionQuestionGroup TankInspectionQuestionGroup { get; set; }

        #endregion
    }
}