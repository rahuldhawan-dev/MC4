using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringProjectMain : IEntity
    {
        public virtual int Id { get; set; }
        public virtual RecurringProject RecurringProject { get; set; }

        [DisplayName("Final Grade")]
        public virtual string Layer { get; set; }

        public virtual string Guid { get; set; }
        public virtual decimal? TotalInfoMasterScore { get; set; }
        public virtual decimal Length { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual decimal? Diameter { get; set; }
        public virtual string Material { get; set; }
    }
}
