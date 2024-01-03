using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class EditEventDocument : EventDocumentsViewModel
    {
        #region Constructor

        public EditEventDocument(IContainer container) : base(container) { }

        #endregion
    }
}