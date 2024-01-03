using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class EditEventType : EventTypeViewModel
    {
        #region Constructor

        public EditEventType(IContainer container) : base(container) { }

        #endregion
    }
}