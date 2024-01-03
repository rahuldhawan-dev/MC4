using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateLockoutDevice : CreateLockoutDevice
    {
        #region Constructors

        public MyCreateLockoutDevice(IContainer container) : base(container) { }

        #endregion
    }
}
