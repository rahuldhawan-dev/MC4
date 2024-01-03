using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class SearchEventType : SearchSet<EventType>
    {
        #region Properties

        [StringLength(EventType.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [StringLength(EventType.StringLengths.CREATED_BY)]
        public virtual string CreatedBy { get; set; }

        #endregion
    }
}