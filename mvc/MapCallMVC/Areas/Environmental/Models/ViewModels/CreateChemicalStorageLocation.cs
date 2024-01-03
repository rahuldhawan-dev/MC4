using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class CreateChemicalStorageLocation : ChemicalStorageLocationViewModel
    {
        #region Constructors

        public CreateChemicalStorageLocation(IContainer container) : base(container) { }

        #endregion
    }
}
