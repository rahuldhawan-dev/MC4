using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateSewerOpening : CreateSewerOpeningBase
    {
        #region Properties

        public int? SAPEquipmentId { get; set; }

        public int? OpeningSuffix { get; set; }

        public string OpeningNumber { get; set; }

        public string SAPErrorCode { get; set; }

        #endregion

        #region Constructors

        public MyCreateSewerOpening(IContainer container) : base(container) { }

        #endregion
    }
}