using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public abstract class EventTypeViewModel : ViewModel<EventType>
    {
        #region Properties

        public override int Id { get; set; }

        [Required, StringLength(EventType.StringLengths.DESCRIPTION), Multiline]
        public virtual string Description { get; set; }

        [StringLength(EventType.StringLengths.CREATED_BY), Multiline]
        public virtual string CreatedBy { get; set; }

        #endregion

        #region Constructor

        public EventTypeViewModel(IContainer container) : base(container) { }

        #endregion
    }
}