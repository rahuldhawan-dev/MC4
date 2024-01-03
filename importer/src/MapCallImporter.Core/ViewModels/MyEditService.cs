using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyEditService : ViewModel<Service>
    {
        #region Properties

        public string Block { get; set; }

        #endregion

        public MyEditService(IContainer container) : base(container) { }
    }
}
