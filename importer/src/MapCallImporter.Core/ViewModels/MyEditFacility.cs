using MapCallMVC.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyEditFacility : EditFacilityBase
    {
        public MyEditFacility(IContainer container) : base(container) { }
    }
}