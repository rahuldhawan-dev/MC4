using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public abstract class EventDocumentsViewModel : ViewModel<EventDocument>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EventType))]
        public virtual int? EventType { get; set; }

        [Required, StringLength(EventDocument.StringLengths.DESCRIPTION), Multiline]
        public virtual string Description { get; set; }

        #endregion

        #region Constructor

        public EventDocumentsViewModel(IContainer container) : base(container) { }

        #endregion
    }
}