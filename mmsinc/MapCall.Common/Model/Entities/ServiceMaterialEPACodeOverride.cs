using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceMaterialEPACodeOverride : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual ServiceMaterial ServiceMaterial { get; set; }
        public virtual State State { get; set; }
        public virtual EPACode CustomerEPACode { get; set; }
        public virtual EPACode CompanyEPACode { get; set; }

        #endregion
    }
}
