using System.Web.Mvc;
using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyEditEquipment : EditEquipmentBase
    {
        public MyEditEquipment(IContainer container) : base(container)
        {
            Form = new FormCollection();
        }
    }
}