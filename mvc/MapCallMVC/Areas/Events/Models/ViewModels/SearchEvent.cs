using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class SearchEvent : SearchSet<Event>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EventCategory))]
        public virtual int? EventCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EventSubcategory))]
        public virtual int? EventSubcategory { get; set; }
        public string EventSummary { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string RootCause { get; set; }
        public virtual string ResponseActions { get; set; }
        public IntRange EstimatedDurationHours { get; set; }
        public IntRange NumberCustomersImpacted { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }
        public string Owners { get; set; }

        #endregion
    }
}