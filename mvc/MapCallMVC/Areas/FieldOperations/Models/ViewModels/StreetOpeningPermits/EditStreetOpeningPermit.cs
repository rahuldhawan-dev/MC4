using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits
{
    public class EditStreetOpeningPermit : StreetOpeningPermitViewModel
    {
        #region Properties

        [Secured]
        public int? WorkOrder { get; set; }

        #endregion

        #region Constructors

        public EditStreetOpeningPermit(IContainer container) : base(container) { }

        #endregion
    }
}
