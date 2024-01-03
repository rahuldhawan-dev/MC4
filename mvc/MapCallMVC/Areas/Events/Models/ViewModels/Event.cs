using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public abstract class EventViewModel : ViewModel<Event>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EventCategory))]
        public virtual int? EventCategory { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EventSubcategory))]
        public virtual int? EventSubcategory { get; set; }

        [StringLength(Event.StringLengths.EVENT_SUMMARY), Multiline]
        public virtual string EventSummary { get; set; }

        public virtual bool? IsActive { get; set; }

        [StringLength(Event.StringLengths.ROOT_CAUSE), Multiline]
        public virtual string RootCause { get; set; }

        [StringLength(Event.StringLengths.RESPONSE_ACTIONS), Multiline]
        public virtual string ResponseActions { get; set; }

        public virtual int? EstimatedDurationHours { get; set; }

        public virtual int? NumberCustomersImpacted { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [StringLength(Event.StringLengths.OWNERS), Multiline]
        public virtual string Owners { get; set; }

        [Coordinate, View("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? Coordinate { get; set; }

        #endregion

        #region Constructor

        public EventViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
