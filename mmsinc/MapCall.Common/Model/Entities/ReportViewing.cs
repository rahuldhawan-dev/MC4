using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReportViewing : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime DateViewed { get; set; }
        public virtual string ReportName { get; set; }

        #endregion
    }
}
