using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ScadaReading : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual ScadaTagName TagName { get; set; }

        [DoesNotExport]
        public virtual ScadaSignal ScadaSignal { get; set; }

        [Required]
        public virtual DateTime Timestamp { get; set; }

        [Required]
        public virtual decimal Value { get; set; }

        #endregion
    }
}
