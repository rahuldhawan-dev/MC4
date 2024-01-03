using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class EditChemicalStorageLocation : ChemicalStorageLocationViewModel
    {
        #region Constructors

        public EditChemicalStorageLocation(IContainer container) : base(container) { }

        #endregion
    }

}
