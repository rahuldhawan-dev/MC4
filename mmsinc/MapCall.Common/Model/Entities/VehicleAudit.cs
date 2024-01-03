using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class VehicleAudit : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int DECAL_NUMBER = 8,
                             PLATE_NUMBER = 8;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual int Mileage { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime AuditedOn { get; set; }

        public virtual string DecalNumber { get; set; }
        public virtual string PlateNumber { get; set; }

        #endregion
    }
}
