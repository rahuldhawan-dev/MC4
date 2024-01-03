using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateEquipment : CreateEquipmentBase
    {
        #region Constructors

        public MyCreateEquipment(IContainer container) : base(container) {}

        #endregion
    }
}