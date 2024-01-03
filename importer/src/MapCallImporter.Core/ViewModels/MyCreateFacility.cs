using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateFacility : CreateFacilityBase
    {
        #region Constructors

        public MyCreateFacility(IContainer container) : base(container) { }

        #endregion
    }
}
